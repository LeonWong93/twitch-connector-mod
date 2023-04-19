﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace TwitchConnectorMod
{
    public class TwitchConnectorMod : MelonMod
    {
        internal static TwitchIRC IRC = new TwitchIRC();

        private MelonPreferences_Category twitchConnectorPrefs;
        private MelonPreferences_Entry<string> oauthToken;
        private MelonPreferences_Entry<string> channel;
        private MelonPreferences_Entry<string> username;
        private MelonPreferences_Entry<string> server;
        private MelonPreferences_Entry<int> port;
        private MelonPreferences_Entry<bool> logTwitchMessages;
        private MelonPreferences_Entry<bool> logRawTwitchMessages;

        public override void OnInitializeMelon()
        {
            twitchConnectorPrefs = MelonPreferences.CreateCategory("TwitchConnector");

            username = twitchConnectorPrefs.CreateEntry<string>("Username", "");
            username.Comment = "Your twitch username";

            oauthToken = twitchConnectorPrefs.CreateEntry<string>("OAuthToken", "");
            oauthToken.Comment = "Get your twitch oauth token by visiting https://twitchapps.com/tmi/";

            // FIXME: Consider supporting multiple channels? 
            channel = twitchConnectorPrefs.CreateEntry<string>("Channel", "");
            channel.Comment = "The twitch channel to join - typically the same as your username";

            server = twitchConnectorPrefs.CreateEntry<string>("Server", "irc.twitch.tv");
            server.Comment = "Default irc chat server to connect, change it only if you know what you are doing";

            port = twitchConnectorPrefs.CreateEntry<int>("Port", 6667);
            port.Comment = "Default port are 6667, change to 80 if you not able to connect twitch chat";

            logTwitchMessages = twitchConnectorPrefs.CreateEntry<bool>("LogTwitchMessages", false);
            logTwitchMessages.Comment = "If set to true, all received twitch messages are written to the console log.";

            logRawTwitchMessages = twitchConnectorPrefs.CreateEntry<bool>("LogRawTwitchMessages", false);
            logRawTwitchMessages.Comment = "If set to true, all raw received twitch messages are written to the console log.";

            if (oauthToken.Value == "")
            {
                LoggerInstance.Msg("Twitch OAuth token not present, not connecting to twitch. (Add to MelonPreferences.cfg)");
                return;
            }

            if (username.Value == "")
            {
                LoggerInstance.Msg("Twitch username not present.  (Set you Username in MelonPreferences.cfg under TwitchConnector");
                return;
            }

            if (channel.Value == "")
            {
                LoggerInstance.Msg("Twitch channel not set, defaulting to twitch username.");
                channel.Value = username.Value;
            }

            if (server.Value == "")
            {
                LoggerInstance.Msg("Twitch channel not set, defaulting to twitch username.");
                server.Value = "irc.twitch.tv";
            }

            if (port.Value == 0)
            {
                LoggerInstance.Msg("Custom Port is not defined, default port 6667 will be used.");
                port.Value = 6667;
            }

            Melon<TwitchConnectorMod>.Logger.Msg("Starting Connection");
            IRC.oauth = oauthToken.Value;
            IRC.channelName = channel.Value;
            IRC.nickName = username.Value;
            IRC.server = server.Value;
            IRC.port = port.Value;

            AddChatMsgReceivedEventHandler(OnChatMsgReceived);
            if (logRawTwitchMessages.Value == true)
            {
                IRC.logRawMessages = true;
            }
            IRC.Enable();
        }

        public static void SendMessage(string message)
        {
            if (IRC != null)
            {
                IRC.SendMsg(message);
            }
        }

        public static void AddChatMsgReceivedEventHandler(TwitchIRC.MessageReceivedEventHandler eventHandler)
        {
            if (IRC != null)
            {
                IRC.MessageReceived += eventHandler;
            }
        }

        private void OnChatMsgReceived(Object sender, ParsedTwitchMessage eventArgs)
        {
            if (logTwitchMessages.Value == true) {
                Melon<TwitchConnectorMod>.Logger.Msg($"Twitch Message: {eventArgs.Message}");
            }
        }

        public override void OnUpdate()
        {
            IRC.Update();
        }
    }
}
