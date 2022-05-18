using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    public static FightManager fightManager;
    public bool inFight;

    private FighterClass fighter1, fighter2;
    private bool turn;
    private bool firstTurn;
    private float cd;

    [SerializeField]
    private Slider hp1, hp2;

    [SerializeField]
    private TextMeshProUGUI narrator;

    [SerializeField]
    private TextMeshProUGUI p1Name, p2Name, stats1, stats2, life1, life2;

    [SerializeField]
    private RawImage player1Img, player2Img;

    private void Start()
    {
        Application.runInBackground = true;

        fightManager = this;
        inFight = false;
    }

    private IEnumerator SetImages1(string _url)
    {
        WWW www = new WWW(_url);
        yield return www;
        player1Img.texture = www.texture;
        player1Img.SetNativeSize();
    }

    private IEnumerator SetImages2(string _url)
    {
        WWW www = new WWW(_url);
        yield return www;
        player2Img.texture = www.texture;
        player2Img.SetNativeSize();
    }

    private void Update()
    {
        if (!inFight)
            return;

        cd -= Time.deltaTime;
        if (cd <= 0)
        {
            NextTurn();
        }
    }

    public void NewFight(string _player1, string _player2)
    {
        fighter1 = DataManager.dataManager.GetFighterByUserID(_player1);
        fighter2 = DataManager.dataManager.GetFighterByUserID(_player2);
        hp1.maxValue = fighter1.hp;
        hp2.maxValue = fighter2.hp;
        hp1.value = fighter1.hp;
        hp2.value = fighter2.hp;
        p1Name.text = $"{fighter1.MyName} lvl{fighter1.lvl}";
        p2Name.text = $"{fighter2.MyName} lvl{fighter2.lvl}";
        stats1.text = $"atk: {fighter1.atk}\ndef: {fighter1.def}\nspd: {fighter1.spd}\nevd: {fighter1.evd}";
        stats2.text = $"atk: {fighter2.atk}\ndef: {fighter2.def}\nspd: {fighter2.spd}\nevd: {fighter2.evd}";
        life1.text = $"{fighter1.hp}/{fighter1.hp}";
        life2.text = $"{fighter2.hp}/{fighter2.hp}";
        inFight = true;
        firstTurn = true;
        StartCoroutine(SetImages1(fighter1.img));
        StartCoroutine(SetImages2(fighter2.img));
        cd = 5;
    }

    private void NextTurn()
    {
        cd = 3;
        if (firstTurn)
        {
            turn = fighter1.spd >= fighter2.spd;
            firstTurn = false;
        }

        if (fighter1.hp > 0 && fighter2.hp > 0)
        {
            if (turn)
                Turn1();
            else
                Turn2();
            life1.text = $"{fighter1.hp}/{hp1.maxValue}";
            life2.text = $"{fighter2.hp}/{hp2.maxValue}";
        }
        else
        {
            EndFight();
        }
    }

    private void Turn1()
    {
        int dmg = 1 + (Random.Range(0, fighter1.atk - fighter2.def));
        dmg = dmg <= 0 ? 1 : dmg;

        bool crit = Random.Range(0f, 1f) < (0.05f + ChanceCalc(fighter1.atk, fighter2.atk));
        dmg = crit ? Mathf.RoundToInt(dmg * 1.5f) : dmg;

        if (Random.Range(0.0f, 1.0f) >= ChanceCalc(fighter2.evd, fighter1.evd) + 0.025f)
        {
            fighter2.hp = (fighter2.hp - dmg < 0 ? 0 : fighter2.hp - dmg);
            hp2.value = fighter2.hp;
            if (!crit)
            {
                IOmanager.IO.Talk();
                narrator.text = $"{fighter1.MyName} attacked {fighter2.MyName} for {dmg} damage";
            }
            else
            {
                IOmanager.IO.Angry();
                narrator.text = $"{fighter1.MyName} attacked {fighter2.MyName} with a critical hit for {dmg} damage";
            }
            if (Random.Range(0.0f, 1.0f) <= ChanceCalc(fighter1.def, fighter2.def) + 0.0125f)
            {
                Debug.Log($"{Random.Range(0.0f, 1.0f)}, {ChanceCalc(fighter1.def, fighter2.def) + 0.0125f}");
                fighter1.hp += (int)(dmg * 0.5f);
                fighter1.hp = fighter1.hp > (int)hp1.maxValue ? (int)hp1.maxValue : fighter1.hp;
                narrator.text = $"{narrator.text}, {fighter1.MyName} healed themselves for {(int)(dmg * 0.5f)}";
            }
        }
        else
        {
            IOmanager.IO.Shocked();
            narrator.text = $"{fighter2.MyName} evaded {fighter1.MyName}'s attack for {dmg} damage";
        }

        if (Random.Range(0.0f, 1.0f) >= ChanceCalc(fighter1.spd, fighter2.spd) + 0.0125f)
        {
            turn = !turn;
            return;
        }

        narrator.text = $"{narrator.text}, {fighter1.MyName} got an extra turn";
    }

    private void Turn2()
    {
        int dmg = 1 + (Random.Range(0, fighter2.atk - fighter1.def));
        dmg = dmg <= 0 ? 1 : dmg;

        bool crit = Random.Range(0f, 1f) < (0.05f + ChanceCalc(fighter2.atk, fighter1.atk));
        dmg = crit ? Mathf.RoundToInt(dmg * 1.5f) : dmg;

        if (Random.Range(0.0f, 1.0f) >= ChanceCalc(fighter1.evd, fighter2.evd) + 0.025f)
        {
            fighter1.hp = (fighter1.hp - dmg < 0 ? 0 : fighter1.hp - dmg);
            hp1.value = fighter1.hp;
            if (!crit)
            {
                IOmanager.IO.Talk();
                narrator.text = $"{fighter2.MyName} attacked {fighter1.MyName} for {dmg} damage";
            }
            else
            {
                IOmanager.IO.Angry();
                narrator.text = $"{fighter2.MyName} attacked {fighter1.MyName} with a critical hit for {dmg} damage";
            }
            if (Random.Range(0.0f, 1.0f) <= ChanceCalc(fighter2.def, fighter1.def) + 0.0125f)
            {
                fighter2.hp += (int)(dmg * 0.5f);
                fighter2.hp = fighter2.hp > (int)hp2.maxValue ? (int)hp2.maxValue : fighter2.hp;
                narrator.text = $"{narrator.text}, {fighter2.MyName} healed themselves for {(int)(dmg * 0.5f)}";
            }
        }
        else
        {
            IOmanager.IO.Shocked();
            narrator.text = $"{fighter1.MyName} evaded {fighter2.MyName}'s attack for {dmg} damage";
        }

        if (Random.Range(0.0f, 1.0f) >= ChanceCalc(fighter2.spd, fighter1.spd) + 0.0125f)
        {
            turn = !turn;
            return;
        }

        narrator.text = $"{narrator.text}, {fighter2.MyName} got an extra turn";
    }

    private void EndFight()
    {
        inFight = false;
        FighterClass winner = fighter1.hp == 0 ? fighter2 : fighter1;
        FighterClass looser = fighter1.hp == 0 ? fighter1 : fighter2;
        float hpLeft;

        if (fighter1 == winner)
        {
            hpLeft = (hp1.maxValue - winner.hp) / hp1.maxValue;
            winner.hp = (int)hp1.maxValue;
            looser.hp = (int)hp2.maxValue;
        }
        else
        {
            hpLeft = (hp2.maxValue - winner.hp) / hp2.maxValue;
            winner.hp = (int)hp2.maxValue;
            looser.hp = (int)hp1.maxValue;
        }

        LevelUpManager.levelUpManager.AddXp(winner, looser, hpLeft);
        IOmanager.IO.Happy();
        narrator.text = $"{winner.MyName} won this encounter!";
    }

    private float ChanceCalc(int atacker, int defender)
    {
        float res = (atacker - 1) / defender;

        res = res > 1 ? 1 : res;

        return res * 0.5f;
    }
}