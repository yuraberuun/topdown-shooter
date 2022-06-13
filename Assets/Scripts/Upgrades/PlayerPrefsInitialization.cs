using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsInitialization : MonoBehaviour
{
    private string[] _charactersBuffsLevels = {"BattleHeroMaxHP", "BattleHeroSpeed", "BattleHeroMagnet", "BattleHeroLucky", "BattleHeroArmor", "BattleHeroGrowth",
      "ShopEnemySpeed", "BattleEnemySpeed"};

    private string[] _charactersShopBuffsLevels = {"ShopHeroMaxHP", "ShopHeroSpeed", "ShopHeroMagnet", "ShopHeroLucky", "ShopHeroGrowth",
     "ShopHeroArmor", "ShopEnemySpeed"};

    private string[] _weaponsBuffsLevels = {"BattleWeaponCooldown", "BattleWeaponDamage", "BattleWeaponDuration", "BattleWeaponArea",
    "BattleWeaponBulletCount", "BattleWeaponSpeed"};

    private string[] _weaponsShopBuffsLevels = {"ShopWeaponCooldown", "ShopWeaponDamage", "ShopWeaponDuration", "ShopWeaponArea",
    "ShopWeaponBulletCount", "ShopWeaponSpeed"};

    private string[] _achievements = { "openHolyWater", "openStar", "openLightning", "openShield", "openShield", "openFireball", "openBone", "openBoomerang",
    "openSpeed", "openMegaBrain", "openWeaponDuration" };
        
    private string[] _heroes = { "Recruit", "Siege", "Spellcaster", "Assasin", "Fighter" };

    private string[] _collection = { "AxeCollection", "BibleCollection", "BoneCollection" , "BoomerangCollection" , "FireballCollection", "FreezeCollection",
    "HoneyCollection", "KnifeCollection","LightningCollection", "MagicCollection", "ShieldCollection", "StarCollection", "SwordCollection", "WaterCollection", "ArmorCollection",
    "BulletSpeedCollection", "DuplicatorCollection", "ClockCollection", "DamageCollection", "LightTomeCollection", "CloverCollection", "GloveCollection",
    "RangeCollection","BootsCollection", "LifeCollection", "BrainCollection", "FindHP", "FindMagnet", "FindChest", "FindKillAll","FindClover","FindFreeze"};

    private string[] _hideHeroes = { "FighterHide", "AssasinHide"};

    private string[] _totals = { "TotalKills", "TotalMoney" };

    void Awake()
    {
        foreach (var i in _charactersBuffsLevels)
            PlayerPrefs.SetInt(i, 0);

        foreach (var i in _charactersShopBuffsLevels)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        foreach (var i in _weaponsBuffsLevels)
            PlayerPrefs.SetInt(i, 0);

        foreach (var i in _weaponsShopBuffsLevels)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        foreach (var i in _achievements)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        foreach (var i in _collection)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        foreach (var i in _totals)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        foreach (var i in _hideHeroes)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);

        PlayerPrefs.SetInt("Recruit", 1); //first hero
        foreach (var i in _heroes)
            if (!PlayerPrefs.HasKey(i))
                PlayerPrefs.SetInt(i, 0);
    }
}
