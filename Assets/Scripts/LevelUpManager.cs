using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager levelUpManager;

    private void Start()
    {
        levelUpManager = this;
    }

    public void AddXp(FighterClass _winner, FighterClass _looser, float _winnerhp)
    {
        int winnerXpToLevel = (int)(100 * ((_winner.lvl + 1) * 1.5f));
        int looserXpToLevel = (int)(100 * ((_looser.lvl + 1) * 1.5f));

        int winnerXp = (int)(winnerXpToLevel * 0.1) + (int)(looserXpToLevel * 0.1);
        int looserXp = (int)(winnerXpToLevel * _winnerhp * 0.05) + (int)(looserXpToLevel * 0.05);

        _winner.xp += winnerXp;
        _looser.xp += looserXp;

        if (_winner.xp >= winnerXpToLevel)
        {
            _winner.canlvl = true;
            LevelUp(_winner);
        }
        if (_looser.xp >= looserXpToLevel)
        {
            _looser.canlvl = true;
            LevelUp(_looser);
        }
        DataManager.dataManager.SavePlayerData(_winner);
        DataManager.dataManager.SavePlayerData(_looser);
    }

    public void LevelUp(FighterClass _fighter)
    {
        string msg;
        if (!_fighter.canlvl)
        {
            msg = $"@{_fighter.MyName} you can't level up at this moment. Gain more xp by battleing players using !join";
            TwitchChat.twitchChat.SendMsg(msg);
            return;
        }

        if (!_fighter.hasGrow)
        {
            _fighter.hpGrow = Random.Range(5, 11);
            _fighter.atkGrow = Random.Range(1, 6);
            _fighter.defGrow = Random.Range(1, 6);
            _fighter.spdGrow = Random.Range(1, 4);
            _fighter.evdGrow = Random.Range(1, 4);
            _fighter.hasGrow = true;
        }

        msg = $"Congratulations @{_fighter.MyName}! You can level up  to lvl {_fighter.lvl + 1}. Choose from this list wich stat you want\n!hp:+{_fighter.hpGrow}. !atk:+{_fighter.atkGrow}. !def:+{_fighter.defGrow}. !spd:+{_fighter.spdGrow}. !evd:+{_fighter.evdGrow}";
        TwitchChat.twitchChat.SendMsg(msg);

    }

    public int NextLvl(FighterClass _fighter)
    {
        return ((int)(100 * ((_fighter.lvl + 1) * 1.5f))) - _fighter.xp;
    }

    public void ChooseStat(FighterClass _fighter, int _stat)
    {
        string msg = "";

        if (!_fighter.canlvl)
        {
            msg = $"@{_fighter.MyName} you can't level up now";
            TwitchChat.twitchChat.SendMsg(msg);
            return;
        }

        _fighter.lvl++;
        _fighter.xp = 0;
        _fighter.hasGrow = false;
        _fighter.canlvl = false;

        switch (_stat)
        {
            case 0:
                _fighter.hp += _fighter.hpGrow;
                msg = $"@{_fighter.MyName} your hp is now {_fighter.hp}";
                break;

            case 1:
                _fighter.atk += _fighter.atkGrow;
                msg = $"@{_fighter.MyName} your atk is now {_fighter.atk}";
                break;

            case 2:
                _fighter.def += _fighter.defGrow;
                msg = $"@{_fighter.MyName} your def is now {_fighter.def}";
                break;

            case 3:
                _fighter.spd += _fighter.spdGrow;
                msg = $"@{_fighter.MyName} your spd is now {_fighter.spd}";
                break;

            case 4:
                _fighter.evd += _fighter.evdGrow;
                msg = $"@{_fighter.MyName} your evd is now {_fighter.evd}";
                break;
        }

        DataManager.dataManager.SavePlayerData(_fighter);
        TwitchChat.twitchChat.SendMsg(msg);
    }
}