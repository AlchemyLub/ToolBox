namespace AlchemyLab.ToolBox.Settings.CodeGen.UnitTests;

public class SettingsCodeGenTaskTests : IDisposable
{
    private readonly string testDirectory;
    private readonly MockBuildEngine buildEngine;

    public SettingsCodeGenTaskTests()
    {
        testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(testDirectory);
        buildEngine = new();
    }

    [Fact]
    public void Execute_WithValidAppSettings_GeneratesClass()
    {
        const string appSettingsContent = """
                                          {
                                            "ConnectionStrings": {
                                              "DefaultConnection": "Server=localhost;Database=Test"
                                            },
                                            "Logging": {
                                              "LogLevel": {
                                                "Default": "Information"
                                              }
                                            },
                                            "AppName": "TestApp"
                                          }
                                          """;
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.json"), appSettingsContent);
        SettingsCodeGenTask task = new()
        {
            ProjectDirectory = testDirectory,
            SettingsNamespaceName = "TestNamespace",
            SettingsClassName = "TestSettings",
            BuildEngine = buildEngine
        };

        bool result = task.Execute();

        Assert.True(result);
        string generatedFile = Path.Combine(testDirectory, "Generated", "TestSettings.cs");
        Assert.True(File.Exists(generatedFile));
        string generatedCode = File.ReadAllText(generatedFile);
        Assert.Contains("namespace TestNamespace;", generatedCode);
        Assert.Contains("public static class TestSettings", generatedCode);
        Assert.Contains("public const string AppName = \"AppName\";", generatedCode);
        Assert.Contains("public static class ConnectionStrings", generatedCode);
        Assert.Contains("public const string Section = \"ConnectionStrings\";", generatedCode);
    }

    [Fact]
    public void Execute_WithMultipleAppSettings_MergesCorrectly()
    {
        const string appSettings = """{"Database": {"Host": "localhost"}}""";
        const string appSettingsDev = """{"Database": {"Port": 5432}, "Debug": true}""";
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.json"), appSettings);
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.Development.json"), appSettingsDev);
        SettingsCodeGenTask task = new()
        {
            ProjectDirectory = testDirectory,
            BuildEngine = buildEngine
        };

        bool result = task.Execute();

        Assert.True(result);
        string generatedCode = File.ReadAllText(Path.Combine(testDirectory, "Generated", "AppSettingsKeys.cs"));
        Assert.Contains("public const string Host = \"Database:Host\";", generatedCode);
        Assert.Contains("public const string Port = \"Database:Port\";", generatedCode);
        Assert.Contains("public const string Debug = \"Debug\";", generatedCode);
    }

    [Fact]
    public void Execute_WithInvalidJson_ContinuesWithWarning()
    {
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.json"), "invalid json");
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.Development.json"), """{"Valid": "setting"}""");
        SettingsCodeGenTask task = new()
        {
            ProjectDirectory = testDirectory,
            BuildEngine = buildEngine
        };

        bool result = task.Execute();

        Assert.True(result); // Task должен продолжать работу
        Assert.Contains(buildEngine.LoggedWarnings, w => w.Contains("Error processing") && w.Contains("appsettings.json"));
        string generatedCode = File.ReadAllText(Path.Combine(testDirectory, "Generated", "AppSettingsKeys.cs"));
        Assert.Contains("public const string Valid = \"Valid\";", generatedCode);
    }

    [Fact]
    public void Execute_WithNoAppSettings_ReturnsTrue()
    {
        SettingsCodeGenTask task = new()
        {
            ProjectDirectory = testDirectory,
            BuildEngine = buildEngine
        };

        bool result = task.Execute();

        Assert.True(result);
        Assert.Contains(buildEngine.LoggedMessages, m => m.Contains("No appsettings*.json files found"));
        Assert.False(Directory.Exists(Path.Combine(testDirectory, "Generated")));
    }

    [Fact]
    public void Execute_WithSpecialCharacters_GeneratesValidClassNames()
    {
        const string appSettings = """
                                   {
                                     "some-key": "value",
                                     "another.key": "value2",
                                     "123numeric": "value3"
                                   }
                                   """;
        File.WriteAllText(Path.Combine(testDirectory, "appsettings.json"), appSettings);
        SettingsCodeGenTask task = new()
        {
            ProjectDirectory = testDirectory,
            BuildEngine = buildEngine
        };

        bool result = task.Execute();

        Assert.True(result);
        string generatedCode = File.ReadAllText(Path.Combine(testDirectory, "Generated", "AppSettingsKeys.cs"));
        Assert.Contains("public const string some_key = \"some-key\";", generatedCode);
        Assert.Contains("public const string another_key = \"another.key\";", generatedCode);
        Assert.Contains("public const string _123numeric = \"123numeric\";", generatedCode);
    }

    public void Dispose()
    {
        if (Directory.Exists(testDirectory))
        {
            Directory.Delete(testDirectory, true);
        }
    }
}
