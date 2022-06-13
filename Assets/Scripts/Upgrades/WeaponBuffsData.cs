using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponBuffsData", menuName = "Weapon buffs data", order = 52)]
public class WeaponBuffsData : ScriptableObject
{
    [SerializeField] private List<float> _shopDamageBuff;
    [SerializeField] private List<float> _battleDamageBuff;
    [SerializeField] private List<float> _shopCooldownBuff;
    [SerializeField] private List<float> _battleCooldownBuff;
    [SerializeField] private List<float> _shopDurationBuff;
    [SerializeField] private List<float> _battleDurationBuff;
    [SerializeField] private List<float> _shopAreaBuff;
    [SerializeField] private List<float> _battleAreaBuff;
    [SerializeField] private List<float> _shopSpeedBuff;
    [SerializeField] private List<float> _battleSpeedBuff;
    [SerializeField] private List<int> _shopBulletCountBuff;
    [SerializeField] private List<int> _battleBulletCountBuff;

    public float GetDamageBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopWeaponDamage") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleWeaponDamage") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopDamageBuff.Count) ? _shopDamageBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleDamageBuff.Count) ? _battleDamageBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetCooldownBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopWeaponCooldown") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleWeaponCooldown") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopCooldownBuff.Count) ? _shopCooldownBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleCooldownBuff.Count) ? _battleCooldownBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetLifeTimeBuff()
    {
        var shopLv  = PlayerPrefs.GetInt("ShopWeaponDuration") - 1;
        var battleLv  = PlayerPrefs.GetInt("BattleWeaponDuration") - 1;

        float coefficient = 1f;
        coefficient += (shopLv >= 0 && shopLv < _shopDurationBuff.Count) ? _shopDurationBuff[shopLv] : 0.0f;
        coefficient += (battleLv >= 0 && battleLv < _battleDurationBuff.Count) ? _battleDurationBuff[battleLv] : 0.0f;

        return coefficient;
    }

    public float GetAttackAreaBuff()
    {
        var shopLv  = PlayerPrefs.GetInt("ShopWeaponArea") - 1;
        var battleLv  = PlayerPrefs.GetInt("BattleWeaponArea") - 1;

        float coefficient = 1.0f;
        coefficient += (shopLv >= 0 && shopLv < _shopAreaBuff.Count) ? _shopAreaBuff[shopLv] : 0.0f;
        coefficient += (battleLv >= 0 && battleLv < _battleAreaBuff.Count) ? _battleAreaBuff[battleLv] : 0.0f;

        return coefficient;
    }
    
    public int GetBulletCountBuff()
    {
        var shopLv  = PlayerPrefs.GetInt("ShopWeaponBulletCount") - 1;
        var battleLv  = PlayerPrefs.GetInt("BattleWeaponBulletCount") - 1;

        var count = (battleLv >= 0 && battleLv < _battleBulletCountBuff.Count) ? _battleBulletCountBuff[battleLv] : 0;

        return count;
    }

    public float GetSpeedBuff()
    {
        var shopLv  = PlayerPrefs.GetInt("ShopWeaponSpeed") - 1;
        var battleLv  = PlayerPrefs.GetInt("BattleWeaponSpeed") - 1;

        var count = (shopLv >= 0 && shopLv < _shopSpeedBuff.Count) ? _shopSpeedBuff[shopLv] : 0;
        count += (battleLv >= 0 && battleLv < _battleSpeedBuff.Count) ? _battleSpeedBuff[battleLv] : 0;

        return count + 1;
    }
}
