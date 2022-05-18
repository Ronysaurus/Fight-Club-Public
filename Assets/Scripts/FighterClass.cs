public class FighterClass
{
    public string MyName;
    public string id;
    public string img;
    public int lvl;
    public int hp;
    public int atk;
    public int def;
    public int spd;
    public int evd;
    public int xp;

    public bool canlvl;
    public bool hasGrow;
    public int hpGrow;
    public int atkGrow;
    public int defGrow;
    public int spdGrow;
    public int evdGrow;

    public FighterClass(string myName, string id, string img, int lvl, int hp, int atk, int def, int spd, int evd, int xp, bool canlvl, bool hasGrow, int hpGrow, int atkGrow, int defGrow, int spdGrow, int evdGrow)
    {
        MyName = myName;
        this.id = id;
        this.img = img;
        this.lvl = lvl;
        this.hp = hp;
        this.atk = atk;
        this.def = def;
        this.spd = spd;
        this.evd = evd;
        this.xp = xp;
        this.canlvl = canlvl;
        this.hasGrow = hasGrow;
        this.hpGrow = hpGrow;
        this.atkGrow = atkGrow;
        this.defGrow = defGrow;
        this.spdGrow = spdGrow;
        this.evdGrow = evdGrow;
    }
}