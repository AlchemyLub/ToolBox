namespace AlchemyLab.ToolBox.Settings.CodeGen;

/// <summary>
/// MSBuild task для генерации констант из appsettings.json
/// </summary>
public class SettingsCodeGenTask : Task
{
    private static readonly HashSet<string> ReservedWords =
    [
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
        "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
        "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
        "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
        "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
        "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
        "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this",
        "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
        "using", "virtual", "void", "volatile", "while"
    ];

    /// <summary>
    /// Путь к папке проекта с конфигами (если не указан, используется текущий проект)
    /// </summary>
    public required string? SettingsProjectDir { get; set; }

    /// <summary>
    /// Имя пространства имен для сгенерированного класса
    /// </summary>
    public required string SettingsNamespaceName { get; set; } = "Generated";

    /// <summary>
    /// Имя класса для сгенерированного файла
    /// </summary>
    public required string SettingsClassName { get; set; } = "AppSettingsKeys";

    /// <inheritdoc />
    public override bool Execute()
    {
        SettingsProjectDir ??= Path.GetDirectoryName(BuildEngine.ProjectFileOfTaskNode);

        if (string.IsNullOrEmpty(SettingsProjectDir))
        {
            Log.LogError("SettingsProjectDir is required");
            return false;
        }

        if (!Directory.Exists(SettingsProjectDir))
        {
            Log.LogError($"SettingsProjectDir does not exist: {SettingsProjectDir}");
            return false;
        }

        try
        {
            string[] settingsFiles = Directory.GetFiles(SettingsProjectDir, "appsettings*.json");

            if (settingsFiles.Length is 0)
            {
                Log.LogMessage(MessageImportance.Low, "No appsettings*.json files found");
                return true;
            }

            string outputDir = Path.Combine(SettingsProjectDir, "Generated");
            string outputFile = Path.Combine(outputDir, $"{SettingsClassName}.g.cs");

            if (!ShouldRegenerate(settingsFiles, outputFile))
            {
                Log.LogMessage(MessageImportance.Low, "Generated file is up to date, skipping generation");
                return true;
            }

            SettingsNode root = new("Root");

            foreach (string file in settingsFiles)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    JsonDocumentOptions options = new()
                    {
                        CommentHandling = JsonCommentHandling.Skip,
                        AllowTrailingCommas = true
                    };
                    JsonDocument doc = JsonDocument.Parse(json, options);
                    ProcessElement(doc.RootElement, root);
                    Log.LogMessage(MessageImportance.Low, $"Processed {Path.GetFileName(file)}");
                }
                catch (Exception ex)
                {
                    Log.LogWarning($"Error processing {file}: {ex.Message}");
                }
            }

            string generatedClass = GenerateSettingsClass(root, SettingsNamespaceName, SettingsClassName);

            Directory.CreateDirectory(outputDir);
            File.WriteAllText(outputFile, generatedClass);

            Log.LogMessage(MessageImportance.Normal, $"Generated {SettingsClassName}.g.cs");
            return true;
        }
        catch (Exception ex)
        {
            Log.LogErrorFromException(ex, true);
            return false;
        }
    }

    private static void ProcessElement(JsonElement element, SettingsNode parent)
    {
        foreach (JsonProperty property in element.EnumerateObject())
        {
            string className = ToValidClassName(property.Name);

            switch (property.Value.ValueKind)
            {
                case JsonValueKind.Object:
                {
                    SettingsNode child = parent.GetOrCreateChild(className, property.Name);
                    ProcessElement(property.Value, child);
                    break;
                }
                case JsonValueKind.Array:
                {
                    for (int i = 0; i < property.Value.GetArrayLength(); i++)
                    {
                        parent.AddValue($"{className}_{i}", $"{property.Name}:{i}");
                    }

                    break;
                }
                default:
                    parent.AddValue(className, property.Name);
                    break;
            }
        }
    }

    private static string GenerateSettingsClass(SettingsNode root, string namespaceName, string className)
    {
        StringBuilder sb = new();

        sb.AppendLine("// <auto-generated />");
        sb.AppendLine("// This file was generated by Settings.CodeGen");
        sb.AppendLine();
        sb.AppendLine($"namespace {namespaceName};");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// Strongly-typed keys for application settings");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public static class {className}");
        sb.AppendLine("{");
        GenerateNode(sb, root, 1);
        sb.AppendLine("}");

        return sb.ToString();
    }

    private static void GenerateNode(StringBuilder sb, SettingsNode node, int indent)
    {
        string indentStr = new(' ', indent * 4);

        foreach (KeyValuePair<string, string> value in node.Values.OrderBy(x => x.Key))
        {
            sb.AppendLine($"{indentStr}/// <summary>");
            sb.AppendLine($"{indentStr}/// Configuration key: {value.Value}");
            sb.AppendLine($"{indentStr}/// </summary>");
            sb.AppendLine($"{indentStr}public const string {value.Key} = \"{value.Value}\";");
        }

        foreach (KeyValuePair<string, SettingsNode> child in node.Children.OrderBy(x => x.Key))
        {
            if (node.Values.Any())
            {
                sb.AppendLine();
            }

            sb.AppendLine($"{indentStr}/// <summary>");
            sb.AppendLine($"{indentStr}/// Configuration section: {child.Value.FullPath}");
            sb.AppendLine($"{indentStr}/// </summary>");
            sb.AppendLine($"{indentStr}public static class {child.Key}");
            sb.AppendLine($"{indentStr}{{");
            sb.AppendLine($"{indentStr}    public const string Section = \"{child.Value.FullPath}\";");

            if (child.Value.Values.Any() || child.Value.Children.Any())
            {
                sb.AppendLine();
            }

            GenerateNode(sb, child.Value, indent + 1);
            sb.AppendLine($"{indentStr}}}");
        }
    }

    private static string ToValidClassName(string name)
    {
        string result = name.Replace("-", "_").Replace(".", "_");

        result = new([.. result.Where(c => char.IsLetterOrDigit(c) || c is '_')]);

        if (char.IsDigit(result[0]))
        {
            result = "_" + result;
        }

        if (ReservedWords.Contains(result.ToLower()))
        {
            result = "@" + result;
        }

        return result;
    }

    private static bool ShouldRegenerate(string[] settingsFiles, string outputFile)
    {
        if (!File.Exists(outputFile))
        {
            return true;
        }

        DateTime outputTime = File.GetLastWriteTime(outputFile);
        return settingsFiles.Any(f => File.GetLastWriteTime(f) > outputTime);
    }
}
