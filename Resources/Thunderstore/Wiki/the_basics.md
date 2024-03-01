# Developing With Configurable Company

## How It Works

This API will provide you with configuration builders to allow you to create and organize different variables that the player will be able to chenge while in game.

The main features this API provides are:

-   Configurations are individual for each save file slot
-   Configurations will be synchronized automatically in real-time
-   Configurations can be modified while in-game and dynamically created
-   Easy organization for both developers and players
-   Easy implementation for developers
-   Robust and optmized abstraction layer to maintain compatibility
-   Access to configurations created by other mods

## The Basics Of Developing

Everything you need to create your own configurations is located under the namespace `Amrv.ConfigurableCompany.API`.

You can create [CPages](../1710-develop-pages), [CCategories](../1706-develop-categories) or [CSections](../1711-develop-sections) to organize your settings into different parts to make them easy to read in-game.

You can create [CConfigs](../1709-develop-configurations) to get and update your values in real time.

There are also some helper methods located under `ConfigAPI` to create configurations from **BepInEx.ConfigEntry** or easily create pages, sections or categories without effort or builders.

You can then get the values of your configurations with `config.Get<Type>()` with the updated value or listen to changes with [CEvents](../1713-event-listening).

> **PRO TIP**  
> You can create everything in different ways, these include using **Builder pattern**, **IDisposable usings** or **object initializers**.

## Custom Configuration Types

The API allows you to create your own types of configurations, they will only accept the values you choose and will be displayed the way you want.

To create your own configuration type just extend `CType` to create your own type. more information in [creating custom configuration types](1708-develop-custom-configuration-type).

Most of the times if you add a custom type you also want to [create a custom display](1707-develop-custom-configuration-display), you can do so by extending `ConfigDisplay` class.

## IDs And Declarations

To maintain compatibility and synchronization, configurations, sections, categories and pages, all need unique ID. These unique IDs are strings that can contain any text but is highly recommended to keep the next format:  
`my-mod-name`\_`page`\_`page-name`.  
This splits the ID into three sections, one identifiying the owner mod, another one identifiying what the ID belongs to (IDs are separated, for example a page and a category can both have the same ID) and a last one identifiying the specific entry information.

## Listening To Events

Events provide you a way to instantly execute a method once something happens, this includes changes in configuratios, creation of pages, sections or categories and many more.

You can see what events you can listen under the class `CEvents`. For more information check the [listening to events guide](../1713-event-listening).
