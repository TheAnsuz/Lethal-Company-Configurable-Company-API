# Developing Configurations

## Creating Configurations

To create a configuration you can make use of **builder pattern** with `CConfigBuilder`, **registering** with `BepInEx` or **disposable object** with `usings`.

Here is an example of the creation methods:

-   Configuration builder

```csharp
CConfig myConfig = new CConfigBuilder() {
    ID = "my-mod-name_config_config-name",
    Name = "My custom configuration",
    Value = 12,
    Type = CTypes.WholeNumber()
}.Build();
```

> **PRO TIP**  
> If you assign the value directly to a `CConfig`, you don't need to call the `.Build()` method.

-   BepInEx ConfigEntry

To create a configuration using a BepInEx ConfigEntry you just need to call

```csharp
ConfigEntry<int> entry = ...

CConfig config = ConfigAPI.ConfigFromBepInEx<T>(entry);
```

> **WARNING**  
> Keep in mind beepInEx configurations are global while CConfigs are split between save files and will hold an arbitrary value that might change at any point.

-   Disposable object

```csharp
public static void CreateConfigs() {

    using CConfigBuilder builder = new CConfigBuilder();
    builder.ID = "my-mod-name_config_config-name",
    builder.Name = "My custom configuration",
    builder.Value = 12,
    builder.Type = CTypes.WholeNumber()
}
```

This method is a bit tricky, you might only want to use this method if you don't need to store the value in any variable.

Configurations have a lot of parameters that you can set, [click here to see them](#parameters).

When building the configuration, the builder will check the `Value` and assign a type to the configuration so most of the times you don't need to explicitly set the `Type`. For example if you set `Value = 12f` if will detect that the configuration is a `DecimalNumber`.

## Parameters

These parameters are everything that you can choose to modify for the configuration.

-   `string` ID:  
    The **unique** identifier for this configuration definition, no other existing configuration should have the same value, I recommend using the ID convention **[mod-name]**\_config\_**[config-name]**.

-   `string` Name:  
    The name that will be shown in the in-game menu.

-   `string` Section / `CSection` CSection:  
    The section that this configuration belongs to (can be null if you set a **category**).

-   `string` Category / `CCategory` CCategory:  
    The category ID that this configuration belongs to (can be null).

-   `string` Tooltip / `string[]` Tooltips:  
    A description about what this configuration does. You can use rich text.

-   `CType` Type:  
    The type of values that this configuration can accept. It will try to be automatically asigned according to the provided value.

-   `object` DefaultValue:  
    The default value, used when the configuration is reset or the current value is not valid.

-   `object` Value:  
    The initial value of the configuration, this can be modified later by the developer or the player in-game.

-   `bool` Enabled:  
    True if the configuration can be changed, false if the value should be locked and not let be modified (This status can be changed by the player).

-   `bool` Experimental:  
    An informative **tag** that will be shown in-game to notify players that this configuration may cause conflicts.

-   `bool` Synchronized:  
    A **tag** that informs the player that every player needs the mod for it to work. Any configuration with this tag will be synchronized in real time between clients (only the host can change the value).

-   `bool` Toogleable:  
    A **tag** that determines whenever this configuration can be enabled and disabled at any point.

## Getting Value

To get the current value of the configuration you just need to access the `Get` method of the configuration.

```csharp
// This is an example configuration
CConfig myConfig = new CConfigBuilder() {
    ID = "my-mod-name_config_weather-name",
    Value = LevelWeatherType.DustClouds,
    Tooltip = "A weather name configuration"
};

// Use the CConfig.Get method to retrieve the value as any type
LevelWeatherType value = myConfig.Get<LevelWeatherType>();

// You can also specify a fallback value in case the configuration can't convert the value
LevelWeatherType value = myConfig.Get<LevelWeatherType>(LevelWeatherType.DustClouds);
```

However you can get the value as other types (only the most used types are supported).

```csharp
// This is an example configuration
CConfig myConfig = new CConfigBuilder() {
    ID = "my-mod-name_config_some-number",
    Value = 12,
    Type = CTypes.WholeNumber()
};
int valueInt = myConfig.Get<int>();
float valueFloat = myConfig.Get<float>();
byte valueByte = myConfig.Get<byte>();
string valueString = myConfig.Get<string>();
// It will try it's best to get the value as your desired type, however this can fail if a type is not convertable, specially with custom configurations.
// In these cases, I recommend to always set a fallback value
int[] valueArray = myConfig.Get<int[]>(new int[0]);
```

## Get Existing Configurations

Some times you might just want to get configurations provided by other mods. You can check if a configuration is registered by other mod calling:

```csharp
if (ConfigAPI.TryGetConfig("other-mod_config_some-name", out CConfig config)) {
    // The configuration exists and has been stored into "config"
} else {
    // The configuration does not exist
}
```

If you want to `Build` your configuration or `Get` the existing one, you can do so by:

```csharp
CConfig myConfig = new CConfigBuilder() {
    ID = "other-mod_config_name",
    Value = 12,
    Type = CTypes.WholeNumber()

// This is the important part
}.GetOrCreate();
```

> **WARNING**  
> Keep in mind that if a configuration already exists, there is no guarantee that the `Type` of the configuration is wha you want, it might be from another different type.
