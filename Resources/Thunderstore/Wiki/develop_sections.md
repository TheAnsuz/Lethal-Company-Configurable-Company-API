# Develop Sections

Sections are the way to split categories into different regions to organize configurations.

They are only used for organization and visualization purposes.

## Creating Sections

If you don't care about the section that much you can use the easy access method to create one section.

```csharp
CSection page = ConfigAPI.CreateSectionAuto("Name", myCategory);
```

However if this method does not fit your needs, you can use the section builder.

```csharp
CSection section = new CSectionBuilder() {
    ID = "my-mod-name_section_section-name",
    Name = "My section"
    Category = CCategory.Default,
}
```

You can also make use of the **disposable object** pattern:

```csharp
public static void CreatePage() {
    using CPageBuilder builder = new CSectionBuilder();
    ID = "my-mod-name_section_section-name",
    Name = "My section"
    Category = CCategory.Default,
}
```

## Parameters

These parameters are everything that you can choose to modify for the section.

-   `string` ID:  
    The **unique** identifier for this section definition, no other existing section should have the same value, I recommend using the ID convention **[mod-name]**\_section\_**[section-name]**.

-   `string` Name:  
    The name that will be shown in the in-game menu.

-   `string` Category / `CCategory` CCategory:  
    The category that this section belongs to (can be set to null to use the default one).

## Get Existing Sections

Some times you might just want to get sections provided by other mods. You can check if a section is registered by other mod calling:

```csharp
if (ConfigAPI.TryGetSection("other-mod_section_some-name", out CSection section)) {
    // The section exists and has been stored into "section"
} else {
    // The section does not exist
}
```
