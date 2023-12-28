# Developing with ConfigurableCompany

_The guide assumes you already know the basics of programming, c# and the basic usage of visual studio._

## Setting up environment

First of all you need a copy of the `.dll` file (wich you can download manually or copy an existing one if you have the mod installed).

Add the `.dll` to the library folder of your project. _Make sure you also include it as a reference in the project._

```xml
<ItemGroup>
    <!--You can name the reference however you want as long as unique-->
    <Reference Include="ConfigurableCompany">
        <!--This is an example path, use the one you have-->
        <HintPath>libs\ConfigurableCompany.dll</HintPath>
    </Reference>
</ItemGroup>
```

_Keep in mind that the mod needs [BepInEx](https://thunderstore.io/c/lethal-company/p/BepInEx/BepInExPack/) as a dependency._

You might as well mark the mod as a hard dependency of your project. ConfigurableCompany contains all the resources you need in a class called `LethalConfiguration` located at `Amrv.ConfigurableCompany`. There you can reference and declare everything you might need.

```cs
[BepInDependency(
    LethalConfiguration.PLUGIN_GUID,
    BepInDependency.DependencyFlags.HardDependency)]
```
