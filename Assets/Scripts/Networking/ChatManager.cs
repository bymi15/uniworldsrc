using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;
using System;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    [Serializable]
    public class ClientMessage
    {
        public bool statusText;
        public string message;

        public ClientMessage(string message, bool statusText = false){
            this.message = message;
            this.statusText = statusText;
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        if (!connected)
        {
            Debug.Log("Successfully connected to server!");
            connected = true;
        }
    }

    public void OnDisconnected()
    {
        connected = false;
        Debug.Log("Disconnected from server.");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if(ChatUI.Instance == null || channelName != this.channel)
        {
            return;
        }

        for(int i = 0; i < senders.Length; i++)
        {
            ClientMessage clientMessage = JsonUtility.FromJson<ClientMessage>(messages[i].ToString());
            if (clientMessage.statusText)
            {
                ChatUI.DisplayMessage(clientMessage.message, true);
            }
            else
            {
                ChatUI.DisplayMessage(clientMessage.message, senders[i]);
            }
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("Received private message: " + sender + ":: " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        if (ChatUI.Instance == null || OnlineUsersUI.Instance == null)
        {
            return;
        }
        if (results[0])
        {
            onlineUsers = CurrentChannel().Subscribers;
            OnlineUsersUI.SetChannelLabel(channel);
            OnlineUsersUI.SetOnlineUsers(onlineUsers);
            ChatUI.SetChannelLabel(channel);
            ChatUI.SetOnlineUsersLabel(onlineUsers.Count);
            ChatUI.ClearMessages();
            Send(user + " has joined the room.", true);
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Left channel " + channels[0] + "!");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        if (ChatUI.Instance == null || channel != this.channel)
        {
            return;
        }
        onlineUsers.Add(user);
        OnlineUsersUI.SetOnlineUsers(onlineUsers);
        ChatUI.SetOnlineUsersLabel(onlineUsers.Count);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        if (ChatUI.Instance == null || channel != this.channel)
        {
            return;
        }
        onlineUsers.Remove(user);
        OnlineUsersUI.SetOnlineUsers(onlineUsers);
        ChatUI.SetOnlineUsersLabel(onlineUsers.Count);
        ChatUI.DisplayMessage(user + " has left the room.", true);
        Debug.Log(user + " left the channel: " + channel);
    }

    public static ChatManager Instance;
    private ChatClient chatClient;

    public HashSet<string> onlineUsers;

    [SerializeField]
    public string user;
    public string channel;
    public bool connected;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            onlineUsers = new HashSet<string>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Connect(string user)
    {
        if (!string.IsNullOrWhiteSpace(user))
        {
            Debug.Log("Connecting to Photon Network Chat...");
            Instance.user = user;
            Instance.chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(user));
        }
    }

    public ChatChannel CurrentChannel()
    {
        if(!connected || chatClient == null || channel == null)
        {
            return null;
        }
        return chatClient.PublicChannels[channel];
    }

    public void JoinChannel(string channel)
    {
        if (connected)
        {
            // Leave current channel before joining a new one
            if(!string.IsNullOrWhiteSpace(this.channel))
            {
                chatClient.Unsubscribe(new string[] { this.channel });
            }
            this.channel = channel;
            chatClient.Subscribe(channel, creationOptions: new ChannelCreationOptions { PublishSubscribers = true });
            Debug.Log("Connecting to channel: " + channel + "...");
        }
        else
        {
            Debug.LogError("Cannot join a channel. Connection with server not yet established.");
        }
    }

    public void Send(string message, bool statusText = false)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            chatClient.PublishMessage(channel, JsonUtility.ToJson(new ClientMessage(message, statusText)));
        }
    }

    public void Disconnect()
    {
        chatClient.Disconnect();
        SceneManager.LoadSceneAsync("Connect");
    }

    void Start()
    {
        chatClient = new ChatClient(this);
    }

    void Update()
    {
        if(chatClient != null)
        {
            chatClient.Service();
        }
    }

    void OnDestroy()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    void OnApplicationQuit()
    {
        if (chatClient != null)
        {
            chatClient.Disconnect();
        }
    }
}
