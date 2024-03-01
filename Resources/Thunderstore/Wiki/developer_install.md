# Developer Install/Setup

This guide will help you setup your project to develop with Configurable Company.

> **WARNING**  
> To develop with this mod, or any other mod or plugin, you must need to understand the [basics of C#](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/), [developing with visual studio](https://visualstudio.microsoft.com/es/vs/getting-started/) and [method patching](https://harmony.pardeike.net/articles/patching.html).

## Seting up the project

The basics of how to create a project will not be covered here. If you need information on how to create a project [check this guide](https://lethal.wiki/dev/initial-setup).

### NuGet Setup

You can easily Configurable Company to your project by using NuGet to download and update the mod for you. If you wish to learn how to use NuGet [check this guide](https://learn.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio).

You can add the NuGet package by clicking **Manage NuGet packages** and searching for `Amrv.ConfigurableCompany`.

> **INFO**  
> If no package is found, make sure your `Package source` is set to **all** or **nuget.org**.

On your project file (`.csproj`) add the following entry manually.

```xml
<ItemGroup>
    <PackageReference Include="Amrv.ConfigurableCompany" Version="*" PrivateAssets="all"/>
</ItemGroup>
```

After adding the reference now you must add the dependency to the plugin itself, to do so just add the following line to your `Plugin class`.

```csharp
[BepInPlugin(/*Your plugin GUID*/, /*Your plugin name*/, /*Your plugin version*/)]
[BepInDependency(ConfigAPI.PLUGIN_GUID,BepInDependency.DependencyFlags.HardDependency)]
    public class MyPlugin : BaseUnityPlugin {
        // You code goes here ...
    }
```

Remember that if you are planning to publish your mod to [thunderstore](https://thunderstore.io/c/lethal-company/) you must include ConfigurableCompany as a dependency in your [manifest.json](https://thunderstore.io/package/create/docs/).

### Manual Setup

If you don't want to use NuGet for your project you can add the refecence to your project manually.

To add the project manually you must download the `Amrv.ConfigurableCompany.dll`. You can get the latest one from the [versions page](https://thunderstore.io/c/lethal-company/p/AMRV/ConfigurableCompany/versions/). After downloading it, open the ZIP file and move the `Amrv.ConfigurableCompany.dll` to any folder of your project where you wish to keep your dependencies, preferably in the same folder as the other `.dll`.

Once you have a copy of the `dll` within your project, you must add a reference to it in the project file (`.csproj`).

_For this example, the `dll` is located in a folder called **libs** inside the project folder._

```xml
<ItemGroup>
   <!--You can name the reference however you want as long as unique-->
   <Reference Include="ConfigurableCompany">
       <!--This is an example path, you must put the path to your file-->
       <HintPath>libs\ConfigurableCompany.dll</HintPath>
   </Reference>
</ItemGroup>
```

> **WARNING**  
> Adding the dll manually will require you to update the `dll` with each update of Configurable Company (or at least with those ones that break backwards compatibility).  
> It will also produce a `Amrv.ConfigurableCompany.dll` with your project `dll`. You should **not** share that file with your project.

After adding the reference now you must add the dependency to the plugin itself, to do so just add the following line to your `Plugin class`.

```csharp
[BepInPlugin(/*Your plugin GUID*/, /*Your plugin name*/, /*Your plugin version*/)]
[BepInDependency(ConfigAPI.PLUGIN_GUID,BepInDependency.DependencyFlags.HardDependency)]
    public class MyPlugin : BaseUnityPlugin {
        // You code goes here ...
    }
```

Remember that if you are planning to publish your mod to [thunderstore](https://thunderstore.io/c/lethal-company/) you must include ConfigurableCompany as a dependency in your [manifest.json](https://thunderstore.io/package/create/docs/).
