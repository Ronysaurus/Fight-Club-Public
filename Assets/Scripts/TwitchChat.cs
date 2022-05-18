using LitJson;
using System.Collections;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;
using UnityEngine.Networking;

public class TwitchChat : MonoBehaviour
{
    public static TwitchChat twitchChat;
    public string username, password, channelName;

    private Client client;
    private Api api;
    private PubSub pubSub;

    private void Start()
    {
        twitchChat = this;
        Application.runInBackground = true;

        ConnectionCredentials credentials = new ConnectionCredentials(username, Secret.bot_access_token);
        client = new Client();
        client.Initialize(credentials, channelName);
        client.Connect();

        api = new Api();
        api.Settings.AccessToken = Secret.bot_access_token;
        api.Settings.ClientId = Secret.client_id;

        pubSub = new PubSub();
        pubSub.ListenToVideoPlayback(channelName);
        pubSub.Connect();

        client.OnMessageReceived += ReadChat;
        StartCoroutine(GetAccessToken());
    }

    private void Newfollower(object sender, TwitchLib.PubSub.Events.OnFollowArgs e)
    {
        SendMsg($"Thank you for following {e.DisplayName}");
    }

    private void ReadChat(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
    {
        string msg = e.ChatMessage.Message.ToLower();
        string usr = e.ChatMessage.DisplayName;
        string id = e.ChatMessage.UserId;

        if (msg == "!join")
        {
            DataManager.dataManager.SetPlayerData(usr, id);
            QueueManager.queueManager.AddPlayer(usr, id);
            StartCoroutine(GetUrlbyId(id));
        }

        if (msg == "!hp")
            LevelUpManager.levelUpManager.ChooseStat(DataManager.dataManager.GetFighterByUserID(id), 0);
        if (msg == "!atk")
            LevelUpManager.levelUpManager.ChooseStat(DataManager.dataManager.GetFighterByUserID(id), 1);
        if (msg == "!def")
            LevelUpManager.levelUpManager.ChooseStat(DataManager.dataManager.GetFighterByUserID(id), 2);
        if (msg == "!spd")
            LevelUpManager.levelUpManager.ChooseStat(DataManager.dataManager.GetFighterByUserID(id), 3);
        if (msg == "!evd")
            LevelUpManager.levelUpManager.ChooseStat(DataManager.dataManager.GetFighterByUserID(id), 4);

        if (msg == "!info")
            SendMsg($"This is twitch interactive rpg developed by @ronysaurus. You can join de Queue using the command !join to be paired with another player. Gain xp, level up and upgrade your stats");

        if (msg == "!stats")
        {
            DataManager.dataManager.SetPlayerData(usr, id);
            SendMsg($"@{usr} stats are: {DataManager.dataManager.GetStatsbyId(id)}");
        }

        if (msg == "!lvlup")
        {
            DataManager.dataManager.SetPlayerData(usr, id);
            LevelUpManager.levelUpManager.LevelUp(DataManager.dataManager.GetFighterByUserID(id));
        }

        if (msg == "!nextlvl")
        {
            DataManager.dataManager.SetPlayerData(usr, id);
            SendMsg($"@{usr} you need {LevelUpManager.levelUpManager.NextLvl(DataManager.dataManager.GetFighterByUserID(id))}xp for the next level");
        }

        if (msg == "!commands")
            SendMsg("This are the commands: !info (get the basic info about the game), !join (join the queue to fight), !stats (get your character stats),!nextLvl (how close is your next level?),  !lvlUp (choose the stat you want to level up)");
    }

    private IEnumerator GetUrlbyId(string _id)
    {
        StartCoroutine(GetAccessToken());
        using (UnityWebRequest request = UnityWebRequest.Get($"https://api.twitch.tv/helix/users?id={_id}"))
        {
            request.SetRequestHeader("Authorization", $"Bearer {Secret.apiToken}");
            request.SetRequestHeader("Client-Id", Secret.bot_refresh_token);
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                JsonData jsonData = JsonMapper.ToObject(request.downloadHandler.text);
                DataManager.dataManager.SetPlayerImg(_id, jsonData["data"][0]["profile_image_url"].ToString());
            }
        }
    }

    private IEnumerator GetAccessToken()
    {
        WWWForm form = new WWWForm();
        form.AddField("client_id", Secret.bot_refresh_token);
        form.AddField("client_secret", Secret.client_secret);
        form.AddField("grant_type", "client_credentials");

        UnityWebRequest uwr = UnityWebRequest.Post("https://id.twitch.tv/oauth2/token", form);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            JsonData jsonData = JsonMapper.ToObject(uwr.downloadHandler.text);
            Secret.apiToken = jsonData["access_token"].ToString();
        }
    }

    public void SendMsg(string _msg)
    {
        client.SendMessage(client.JoinedChannels[0], _msg);
    }
}