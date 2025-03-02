# TwitchConnectorMod - A basic Twitch IRC implementation for Audica

This mod connects to twitch chat and exposes two-way twitch chat communication to other Audica mods. 

## Prerequisites

This requires MelonLoader 0.5.5* (or maybe later version support too ?) be installed on your Audica installation.

Check out https://melonwiki.xyz for installation instructions. 

**Latest version v0.6.1 break my mod, 0.5.3 doesn't seem available anymore, 0.5.5 is what i currently using and seem fine*

## Installation

1. Download the latest TwitchConnectorMod.zip from the releases section of this repository and unzip it into your  Audica installation folder.

2. In your Audica installation folder, edit the file `UserData\MelonPreferences.cfg` with a text editor (notepad will work) and add the following at the end:

   ```
   [TwitchConnector]
   Username = "your_twitch_username" # Replace "your_twitch_username" with your twitch username.
   OAuthToken = "oauth:xxxxx" # Get your twitch oauth token by visiting https://twitchapps.com/tmi/ and paste the token you receive into here.
   Channel = "your_twitch_username" # The twitch channel to join - typically this is the same as your username
   
   ```

   *Optionally, which my main purpose on fork this Mod and modify it, you can add this line too for change the Port used to connect Twitch Chat server, which solve my issue as port 6667 is somehow blocked by my ISP.*

   ```
   [TwitchConnector]
   Port = 80 # Default port are 6667, change to 80 if you not able to connect twitch chat
   
   ```

3. Start Audica.  In the MelonLoader window, if the connection to Twitch was successful, you should see a message similar to this: 

   ```
   [19:51:21.241] [Twitch_Connector_Mod] Twitch connected
   ```

   

## Usage with Mods

This section applies to modders that want to send/receive messages from Twitch chat.



### Receiving Twitch messages

Create a method to process incoming messages:

```csharp
void OnChatMessage(Object sender, TwitchConnectorMod.ParsedTwitchMessage eventArgs)
{
    // process the message how you see fit.
}

```

See the ParsedTwitchMessage class for details on structure. 

Register the event handler with the TwitchConnectorMod:

```csharp
TwitchConnectorMod.TwitchConnectorMod.AddChatMsgReceivedEventHandler(OnChatMessage);
```

### Sending Twitch messages

Call the SendMessage method:

```csharp
TwitchConnectorMod.TwitchConnectorMod.SendMessage("Greetings!");
```

Note that this is a VERY basic IRC client, and does not currently use Twitch Pub/Sub or any other Twitch features.  
