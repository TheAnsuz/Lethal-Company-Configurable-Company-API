# Using the mod

```
I suggest modders that use this API to link to this page to users can understand how to use the menu easily.
```

## How to change configurations

Configurations will appear in-game once you press the `Host` button. If you are joining a friend then his configuration will be synchronized with yours.

Each configuration can have a different value for each **save file** and values will be automatically saved once you start the game.  
_If a value is not permitted it will be removed automatically._

## Menu usage

![Guide image](https://i.imgur.com/457DmHb.png)

-   **Page name and changer**  
    Some mods may use their own configuration page to display all their configurations, if there are multiple pages you will see arrows allowing you to switch between them.

-   **Configuration name**  
    This is part of the configuration tooltip. It will simply display the full name of the selected configuration.

-   **Configuration description**  
    As it's name says, some information on what does the configuration do and maybe how to use will be shown.

-   **[`TAG`] Default value**  
    Shows what was the default value that the configuration had.

-   **[`TAG`] Client synchronize**  
    If a configuration has this tag, it means all clients must have the mod installed as it will be synchronized between all of them. _If a configuration does not have the tag, it means only the host needs the mod installed for it to work_.

-   **[`TAG`] Type**  
    What kind of values does this configuration accept, maybe is a **whole number** or a **text**.

-   **Menu scroller**  
    A scrollbar that lets you move up and down to see configurations

-   **Configuration menu**  
    A panel that shows all configurations split between some categories that you can expand or contract for visibility.

-   **Reset configuration**  
    Clicking this button will reset the configurations to it's default values. _Even those configurations that are not loaded because the mod for them is missing._

-   **Information / Guide**  
    Clicking this button will open a browser window showing the image shown above.
