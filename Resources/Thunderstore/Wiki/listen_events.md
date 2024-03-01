# Using Events

**Configurable company** provides some events that can help you updating configuration values and many other things.

The events are all registered under `CEvents` class.

## Add An Event Listener

To start listening a new event you need to attach a listener. I recommend to add all listeners when your mod starts.

```csharp
// This method should be called when you plugin starts
public static void AttachListeners() {

    // In this example we will listen to configuration changes
    CEvents.ConfigEvents.ChangeConfig.AddListener(MyEventListener);

}

public static void MyEventListener(CEventChangeConfig evt) {

    if (evt.Config.ID == myConfig.ID) {
        // The configuration is the one I want to check
    }
}
```

A brief explanation about what is happening. When you attach a listener, you are telling the event what method(s) should it call.

All events have one parameter that contains all the information about the event. In the example above this paramter is a `CEventChangeConfig` that contains information about what value changed in a configuration.

> **INFO**  
> Some events have a `CEvent` parameter. This is the default one and does not contain any information, however it should be added as a parameter anyways.

## Existing Events

There a lot of events you can listen to. These are divided into multiples sections depending on the type of event.

<table>
    <thead>
        <tr>
            <th>Event Type</th>
            <th>Event Subtype</th>
            <th>Parameter Type</th>
            <th>Information</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td rowspan=11>Menu Events</td>
            <td>Save</td>
            <td>CEvent</td>
            <td>Called every time the save button is pressed</td>
        </tr>
        <tr>
            <td>Reset</td>
            <td>CEvent</td>
            <td>Called every time the reset button is pressed</td>
        </tr>
        <tr>
            <td>Restore</td>
            <td>CEvent</td>
            <td>Called every time the restore button is pressed</td>
        </tr>
        <tr>
            <td>Copy</td>
            <td>CEvent</td>
            <td>Called every time the copy button is pressed</td>
        </tr>
        <tr>
            <td>Paste</td>
            <td>CEvent</td>
            <td>Called every time the paste button is pressed</td>
        </tr>
        <tr>
            <td>Prepare</td>
            <td>CEvent</td>
            <td>Called before the in-game menu is created</td>
        </tr>
        <tr>
            <td>Create</td>
            <td>CEvent</td>
            <td>Called when the in-game menu is created</td>
        </tr>
        <tr>
            <td>Destroy</td>
            <td>CEvent</td>
            <td>Called when the in-game menu is deleted</td>
        </tr>
        <tr>
            <td>Toggle</td>
            <td>CEventMenuToggle</td>
            <td>Called when the menu is open or closed (using the in-game button)</td>
        </tr>
        <tr>
            <td>Visible</td>
            <td>CEventMenuVisible</td>
            <td>Called when the menu is hidden or shown (when even the in-game show button is hidden/shown)</td>
        </tr>
        <tr>
            <td>ChangePage</td>
            <td>CEventChangePage</td>
            <td>Called when the currently shown page is changed</td>
        </tr>
        <tr>
            <td rowspan=6>Config Events</td>
            <td>CreatePage</td>
            <td>CEventCreatePage</td>
            <td>Called every time a new page is registered</td>
        </tr>
        <tr>
            <td>CreateCategory</td>
            <td>CEventCreateCategory</td>
            <td>Called every time a new category is registered</td>
        </tr>
        <tr>
            <td>CreateSection</td>
            <td>CEventCreateSection</td>
            <td>Called every time a new section is registered</td>
        </tr>
        <tr>
            <td>CreateConfig</td>
            <td>CEventCreateConfig</td>
            <td>Called every time a new config is registered</td>
        </tr>
        <tr>
            <td>ChangeConfig</td>
            <td>CEventChangeConfig</td>
            <td>Called every time the value of a configuration tries to be changed</td>
        </tr>
        <tr>
            <td>ToggleConfig</td>
            <td>CEventToggleConfig</td>
            <td>Called every time a configuration is enabled or disabled</td>
        </tr>
        <tr>
            <td rowspan=1>Lifecycle Events</td>
            <td>PluginStart</td>
            <td>CEvent</td>
            <td>Called after Configurable Company finishes loading</td>
        </tr>
        <tr>
            <td rowspan=2>IO Events</td>
            <td>CopyToClipboard</td>
            <td>CEventCopyClipboard</td>
            <td>Called after the configuration has been copied to clipboard</td>
        </tr>
        <tr>
            <td>PasteFromClipboard</td>
            <td>CEventPasteClipboard</td>
            <td>Called after the configuration has been pasted from the clipboard</td>
        </tr>
    </tbody>
</table>
