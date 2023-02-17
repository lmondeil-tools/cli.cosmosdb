# Introduction

This project is a CLI application with the following capacities

* Generic host with dependency injection
* Nested settings read from appSettings.json
* Logging
  * Colored console
  * Files (traces / errors)

# Getting started

## Try first

Try the cli first : view documentation

* Build the project
* Open a console and navigate to the generation folder
* run the cli

=> You should see the help text

# Adapt the project to your needs

## Commands

### How it works

The project uses [McMaster CommandLineUtils](https://natemcmaster.github.io/CommandLineUtils/)

Cli is organised in commands and subcommands. SubCommands are commands registered on their parents. In this project, registrations are done using attributes on parent commands.

A command class has the `Command` attribute and a `SubCommand` attribute to register those (see `HostedApp` for example)

```c#
[Command("MyCli")]
[Subcommand(
    typeof(ShowFullHelp),
    typeof(HelloMaster),
    typeof(ListMaster),
    typeof(ShowSettings)
)]
```

A command entry point is an `OnExecute` method. This method can be private.

```c#
    private void OnExecute(IConsole console)
    private async Task OnExecute(IConsole console)
```

Have a look at classes in [Commands] folder to see how [Options](https://natemcmaster.github.io/CommandLineUtils/docs/options.html?tabs=using-attributes), [Arguments](https://natemcmaster.github.io/CommandLineUtils/docs/arguments.html?tabs=using-attributes) and [nested commands](https://natemcmaster.github.io/CommandLineUtils/v3.0/api/McMaster.Extensions.CommandLineUtils.SubcommandAttribute.html) work.

### Adapt

* Delete all files in [Commands] folder except ShowFullHelp.cs
* Add your commands and subcommands
* Register your root commands in HostedApp class as SubCommands

## appSettings

### How it works

Settings come from appSettings.json. They are injected as `IOption<TSetting>`

In nested mode, settings are classic json stream - you have to match the json structure with the corresponding settings class and name this json stream with the class name

In colon mode, settings describe each property as a full line with its path

```c#
public class NestedConfigurationSettings
{
    public string DisplayName { get; set; }
    public CosmosDbSettings CosmosDb { get; set; }
    public ServiceBusSettings ServiceBus { get; set; }
}
```

Nested mode

```json
  "NestedConfigurationSettings": {
    "displayName": "Cli template - NestedStyle",
    "cosmosDb": {
      "connectionString": "cnx-to-cosmosdb",
      "throughputBounds": {
        "min": 0,
        "max": 10
      }
    },
    "serviceBus": {
      "connectionString": "cnx-to-servicebus"
    }
  }
```

Colon mode

```
  "nestedConfigurationSettings:displayName": "Cli template - Azure function style",
  "nestedConfigurationSettings:cosmosDb:connectionString": "cnx-to-cosmosdb",
  "nestedConfigurationSettings:cosmosDb:throughputBounds:min": 0,
  "nestedConfigurationSettings:cosmosDb:throughputBounds:max": 10,
  "nestedConfigurationSettings:serviceBus:connectionString": "cnx-to-servicebus"
```



### Adapt

* Remove all from appSettings.json
* Create your settings class (example : mySettings)
* Add json serialization representation of your settings object and make it a  named property (where the name is your classname - mySettings in the example) of appSettings json stream
* Register your setting injection in `program.cs`

```c#
services.AddOptionMatchingSection<NestedConfigurationSettings>();
```

* Use you injected settings in your commands

```c#
public ShowSettings(IOptions<NestedConfigurationSettings> nestedSettings)
```

## Logging

> Afin de pouvoir pousser un package sur Azure Artifact, quelques prérequis sont à vérifier
>
> * [Définition du flux du repo nuget](https://dev.azure.com/fp-lp/Datahub/_artifacts/feed/datahub-feed/connect/dotnet)
> * [Azure artifact credential provider](https://github.com/microsoft/artifacts-credprovider#azure-artifacts-credential-provider)

Logging uses Serilog. Registration is done by injection in `program.cs` by the following code

```bash
dotnet nuget push Franprix.Datahub.Cli.Template.<version>.nupkg `
	-s https://fp-lp.pkgs.visualstudio.com/_packaging/datahub-feed/nuget/v3/index.json `
	--api-key az `
	--interactive
```

The extension method is in `IServiceCollectionExtensions` class.

### Adapt

If you want to change logging folders and files, update the passed parameters to `AddConsoleAndFileLogging` method

If you need other logging destinations, change `IServiceCollectionExtensions` class.

## Cleaning

[Créer un modèle d’élément pour dotnet new - CLI .NET | Microsoft Learn](https://learn.microsoft.com/fr-fr/dotnet/core/tutorials/cli-templates-create-item-template)