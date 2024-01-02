# Developing with ConfigurableCompany

_The guide assumes you already know the basics of programming, c# and the basic usage of visual studio._

## Listening to events

Events are the way the mod communicates to developers on what actions are occuring. There are multiple events you can listen to. All of them inside the `Events` class inside the `Amrv.ConfigurableCompany` namespace.

-   `ConfigurationChanged`: Listens to any change on a configuration value and holds the reason on why did it change among the configuration that changed and the old and new values.

-   `ConfigurationCreated`: Listens to any creation of configuration and holds the configuration that was created.

-   `PluginInitialized`: Called once, this event triggers when the **ConfigurableCompany** plugin ended initializating all the resources that needed.

-   `PluginSetup`: Called once, this event triggers when the **ConfigurableCompany** plugin ended setting up the content and patching methods.

-   `PluginEnabled`: Called once, this even triggers when the **ConfigurableCompany** plugin completed the boot up and is ready to work.

-   `BeforeMenuDisplay`: Called every time the in-game configuration menu starts to display.

-   `AfterMenuDisplay`: Called every time the in-game configuration menu is displayed.

To listen to any of the events you should use the [standard pattern](https://learn.microsoft.com/en-us/dotnet/standard/events/#event-handlers).
