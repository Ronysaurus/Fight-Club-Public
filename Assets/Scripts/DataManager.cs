using LitJson;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager dataManager;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        Application.runInBackground = true;
        dataManager = this;
    }

    public FighterClass GetFighterByUserID(string _user)
    {
        JsonData jsonData = JsonMapper.ToObject(PlayerPrefs.GetString(_user));

        return new FighterClass(jsonData["MyName"].ToString(), jsonData["id"].ToString(), jsonData["img"].ToString(), int.Parse(jsonData["lvl"].ToString()), int.Parse(jsonData["hp"].ToString()), int.Parse(jsonData["atk"].ToString()), int.Parse(jsonData["def"].ToString()), int.Parse(jsonData["spd"].ToString()), int.Parse(jsonData["evd"].ToString()), int.Parse(jsonData["xp"].ToString()), (bool)jsonData["canlvl"], (bool)jsonData["hasGrow"], int.Parse(jsonData["hpGrow"].ToString()), int.Parse(jsonData["atkGrow"].ToString()), int.Parse(jsonData["defGrow"].ToString()), int.Parse(jsonData["spdGrow"].ToString()), int.Parse(jsonData["evdGrow"].ToString()));
    }

    public void SetPlayerData(string _name, string _id)
    {
        if (PlayerPrefs.HasKey(_id))
        {
            JsonData jsonData = JsonMapper.ToObject(PlayerPrefs.GetString(_id));
            FighterClass player = new FighterClass(_name.ToString(), _id, jsonData["img"].ToString(), int.Parse(jsonData["lvl"].ToString()), int.Parse(jsonData["hp"].ToString()), int.Parse(jsonData["atk"].ToString()), int.Parse(jsonData["def"].ToString()), int.Parse(jsonData["spd"].ToString()), int.Parse(jsonData["evd"].ToString()), int.Parse(jsonData["xp"].ToString()), (bool)jsonData["canlvl"], (bool)jsonData["hasGrow"], int.Parse(jsonData["hpGrow"].ToString()), int.Parse(jsonData["atkGrow"].ToString()), int.Parse(jsonData["defGrow"].ToString()), int.Parse(jsonData["spdGrow"].ToString()), int.Parse(jsonData["evdGrow"].ToString()));
            JsonData pJson = JsonMapper.ToJson(player);
            PlayerPrefs.SetString(_id, pJson.ToString());
        }
        else
        {
            FighterClass player = new FighterClass(_name, _id, "", 1, 30, 10, 5, 1, 1, 0, false, false, 1, 1, 1, 1, 1);
            JsonData pJson = JsonMapper.ToJson(player);
            PlayerPrefs.SetString(_id, pJson.ToString());
        }
    }

    public void SavePlayerData(FighterClass _player)
    {
        JsonData pJson = JsonMapper.ToJson(_player);
        PlayerPrefs.SetString(_player.id, pJson.ToString());
    }

    public string GetStatsbyId(string _id)
    {
        JsonData jsonData = JsonMapper.ToObject(PlayerPrefs.GetString(_id));
        return $"lvl: {jsonData["lvl"].ToString()}, hp: {jsonData["hp"].ToString()}, atk:{jsonData["atk"].ToString()}, def: {jsonData["def"].ToString()}, spd: {jsonData["spd"].ToString()}, evd:{jsonData["evd"].ToString()}, xp:{jsonData["xp"].ToString()}";
    }

    public void SetPlayerImg(string _id, string _url)
    {
        FighterClass fighter = GetFighterByUserID(_id);
        fighter.img = _url;
        SavePlayerData(fighter);
    }
}