# Creating your first configuration

This tutorial will guide you on how to create your first configuration using the mod.

## Project setup

For this tutorial we are going to use [NuGet to download Configurable Company](https://www.nuget.org/packages/Amrv.ConfigurableCompany/).

_Always make sure to download the latest version, it might not match the one shown in the image_.
![NuGet gallery page](https://i.imgur.com/5WbagLb.png)

On our visual studio project we are going to add the package using the **Manage NuGet packages** button.

![Configurable company from Visual studio](https://i.imgur.com/yveOFBM.png)

> **INFO**  
> If the API does not appear, make sure you have set your **package origins** to **ALL** or **nuget.org**.

Once we have downloaded the latest version we now need to mark it as a BepInEx dependency.

We should go to our `PluginClass` and add a new `BepInDependency` for Configurable company.

It should look something like this:

```csharp
[BepInPlugin("MY_PLUGIN_GUID", "my_plugin", "1.0")]
[BepInDependency(ConfigAPI.PLUGIN_GUID,BepInDependency.DependencyFlags.HardDependency)]
    public class MyPlugin : BaseUnityPlugin {

        public void Awake() {
            // Your plugin should do whatever you want
        }
    }
```

## Creating the configuration

If we correctly setup the project, we can now use and create configurations.

In this example, we are going to create a class called `PluginConfigs` (you can name it however you want) that will hold our configurations.

> **PRO TIP**  
> You don't even need this class, you can create your configurations wherever you want as long as you reference it.

```csharp

public class PluginConfigs {

}

```

Now we are going to add a new configuration that will allow the user to input any number.

```csharp

public class PluginConfigs {

    public CConfig NumberConfig = new CConfigBuilder() {
        ID = "my-mod_config_any-number",
        Value = 10, // Some configurations as this one will automatically detect their type
        Type = CTypes.DecimalNumber(), // Most of the times you wont need to specify the type unless you want something special
        Name = "Any number config",
        Tooltip = "Set any number, no limits",
        Synchronized = true // If you want this value to be synchronized between clients
    };

}

```

That's all we need to start, now lest create this object in our plugin.

```csharp
[BepInPlugin("MY_PLUGIN_GUID", "my_plugin", "1.0")]
[BepInDependency(ConfigAPI.PLUGIN_GUID,BepInDependency.DependencyFlags.HardDependency)]
    public class MyPlugin : BaseUnityPlugin {

        public static PluginConfigs Configs = new PluginConfigs();

        public void Awake() {

        }
    }
```

## Accesing the configuration

Once we have our configurations ready, we might as well use them for something. In this example we are going to display a message in the console with the value everytime the ship lands on a planet.

_This tutorial will not be covering the basics of method patching, pleasle refer to [Harmony patching](https://harmony.pardeike.net/articles/patching.html) to understand it's basics._

```csharp
[HarmonyPatch(typeof(StartOfRound))]
[HarmonyPatch("OnShipLandedMiscEvents")]
[HarmonyPostfix]
private static void OnShipLandedMiscEvents_Postfix(StartOfRound __instance)
{
    Console.WriteLine($"Value from my configuration: {MyPlugin.Configs.NumberConfig.Get<int>()}");
}
```

As easy as that, you can now start creating your own configurations.

## Advanced topics

Remember this is just a basic tutorial, ConfigurableCompany provides with a ton of options. If you need an in-depth tutorial I suggest reading the wiki pages individually depending on what would you want to do.

Feel free to [ask **the_ansuz** on discord](https://img.shields.io/badge/Discord_user-the__ansuz-5865F2?style=flat&logo=discord&logoColor=%237f8afa&link=https%3A%2F%2Fdiscordapp.com%2Fusers%2F341967365908594700) or [open an issue](https://github.com/TheAnsuz/Lethal-Company-Configurable-Company-API/issues) at the github page.
