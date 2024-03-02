# Configurable Company

Configurable Company provides an enhanced experience for both players and developers adding an in-game menu where you can change you gameplay settings.

> **WARNING**  
> This plugin does not add content on it's own, it's an API.  
> If you want content alongside it install [Lethal Company Variables](https://thunderstore.io/c/lethal-company/p/AMRV/LethalCompanyVariables/).

| [Player information](#player-guide) | [Developer guide](#developer-guide) |
| ----------------------------------- | ----------------------------------- |

# Player information

### Menu

Using the in-game menu allows you to set a specific setting for your current file, you can have your **first save** with `x10` enemy spawning and your second one with `x0` enemy spawning.

![A image of the in-game menu with some configurations from Lethal Company Variables](https://i.imgur.com/A2zn1vy.png)

As the image shows, configuration are split into pages (_Seen at the top left of the image_) and then into categories.

### Buttons

![A image of the in-game menu showing the buttons](https://i.imgur.com/FUwQ4zA.png)

-   **Save configurations**: Saves the settings you changed.
-   **Reset to default values**: Resets every configuration to the initial value.
-   **Restore saved values**: Reverts back your modifications to the last saved configuration.
-   **Copy to clipboard**: Allows you to share your current configurations by copying them to your clipboard.
-   **Paste from clipboard**: Tries to read your clipboard for configurations and sets them.

> **INFO**  
> You don't need to share configurations with your friends, they are automatically synchronized.

### Tooltip

![A image of the in-game menu showing how the tooltip is displayed](https://i.imgur.com/jbyo0Jx.png)

The tooltip shows the configuration name, a description on what it does and some tags with extra information. These tags can be:

-   **Experimental**: If a configuration maybe produce issues while playing this tag will be shown.
-   **Default**: Shows the initial value from the configuration.
-   **Synchronize with client**: If that configuration needs to be synchronized with other players to work (This means they need the mod, otherwise they don't).
-   _**Green tag**_: A green text represents what values does the configuration accept.

# Developer guide

> **INFO**  
> If you want an easy-to-read guide I recommend you looking [Lethal comapny modding wiki for configurable company](https://lethal.wiki/dev/apis/configurable-company). However you can also check the [thunderstore wiki](https://thunderstore.io/c/lethal-company/p/AMRV/ConfigurableCompany/wiki/) or the [mod's github page](https://github.com/TheAnsuz/Lethal-Company-Configurable-Company-API/wiki).

This API will allow you as a developer to add configurations to the game easily. These configurations will be displayed in an in-game menu where the user will be able to change them.

Configurations will change automatically so I suggest you [listen to changes with events](#listening-to-events) if you need the value automatically updated.

### How to create configurations

```csharp
public static CConfig FloatConfiguration = new CConfigBuilder()
{
    Category = MyPluginCategories.Normal,
    ID = "my-mod-id_configuration_float-configuration",
    Name = "Does something",
    Tooltip = "This is my cool description of the configuration",
    Value = 69.420f
};
```

And you can get the value easily too:
```csharp
    int intValue = FloatConfiguration<int>Get();
    float floatValue = FloatConfiguration<float>Get();
    float floatValueWithDefault = FloatConfiguration<float>Get(10f);
```

You can even use BepInEx configurations:

```csharp
    CConfig myConfig = ConfigAPI.ConfigFromBepInEx(config);
```

### Listening to events

The API includes a lot of events you can access and listen to. If you want to see the full list, navigate to the [wiki section about events](https://thunderstore.io/c/lethal-company/p/AMRV/ConfigurableCompany/wiki/948-events/).

Here is an example on how you could listen to configuration changes:

First we need to register the listener (you might want to do this when your plugin starts)
```csharp
CEvents.ConfigEvents.ChangeConfig.AddListener(MyListenerMethod);
```

Now we need the method to execute
```csharp
public static void MyListenerMethod(CEventChangeConfig myEvent) {
    CConfig changedConfig = myEvent.Config;
    ChangeReason reason = myEvent.Reason;
    bool succeeded = myEvent.Success;
    bool converted = myEvent.Converted;
    object oldValue = myEvent.OldValue;
    object requestedValue = myEvent.RequestedValue;
    object newValue = myEvent.NewValue;

    // Here you can do whatever you need with the information
    if (myEvent.Success) {
        // Configuration changed correctly
    }
}
```
