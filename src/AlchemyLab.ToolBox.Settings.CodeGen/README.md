# Settings.CodeGen

Generates strongly-typed constants for appsettings.json configuration keys.

## Usage

1. Install the package:
```bash
dotnet add package Settings.CodeGen
```
2. Build your project - constants will be generated automatically.
3. Use the generated keys:
```cs
string conn = configuration[AppSettingsKeys.ConnectionStrings.DefaultConnection];
```

## Configuration
```xml
<PropertyGroup>
  <SettingsNamespace>MyApp.Config</SettingsNamespace>
  <SettingsClassName>ConfigKeys</SettingsClassName>
  <SettingsCodeGenDisabled>true</SettingsCodeGenDisabled>
</PropertyGroup>
```
