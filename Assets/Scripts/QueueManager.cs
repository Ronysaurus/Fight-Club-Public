using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager queueManager;

    [SerializeField]
    private TextMeshProUGUI queue_txt;

    private List<playerData> players;

    private struct playerData
    {
        public string name;
        public string id;
    }

    private void Start()
    {
        Application.runInBackground = true;

        queueManager = this;
        players = new List<playerData>();
        UpdateQueue();
    }

    private void Update()
    {
        StartFight();
    }

    private void UpdateQueue()
    {
        string text = "\nQueue: !join\n\n";

        foreach (playerData player in players)
        {
            text += player.name + "\n";
        }

        queue_txt.text = text;
    }

    public bool AddPlayer(string _player, string _id)
    {

        if (DataManager.dataManager.GetFighterByUserID(_id).canlvl)
        {
            TwitchChat.twitchChat.SendMsg($"@{_player} you can't join a fight untill you level up. Please use the command !lvlUp");
            return false;
        }
        foreach (playerData player in players)
        {
            if (player.id == _id)
            {
                TwitchChat.twitchChat.SendMsg($"@{_player} you are already in the queue");
                return false;
            }
        }
        playerData myPlayer = new playerData
        {
            name = _player,
            id = _id
        };
        players.Add(myPlayer);
        UpdateQueue();

        return true;
    }

    public void RemovePlayer(string _player)
    {
        foreach (playerData player in players)
        {
            if (player.id == _player)
                players.Remove(player);
        }

        UpdateQueue();
    }

    private void StartFight()
    {
        if (FightManager.fightManager.inFight || players.Count < 2)
            return;

        FightManager.fightManager.NewFight(players[0].id, players[1].id);
        players.Remove(players[0]);
        players.Remove(players[0]);
        UpdateQueue();
    }
}