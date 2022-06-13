using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HeroBuffsData", menuName = "Hero buffs data", order = 53)]
public class HeroBuffsData : ScriptableObject
{
    [SerializeField] private List<float> _shopMaxHPBuff;
    [SerializeField] private List<float> _battleMaxHPBuff;
    [SerializeField] private List<float> _shopSpeedBuff;
    [SerializeField] private List<float> _battleSpeedBuff;
    [SerializeField] private List<float> _shopMagnetBuff;
    [SerializeField] private List<float> _battleMagnetBuff;
    [SerializeField] private List<float> _shopLuckyBuff;
    [SerializeField] private List<float> _battleLuckyBuff;
    [SerializeField] private List<float> _shopGrowthBuff;
    [SerializeField] private List<float> _battleGrowthBuff;
    [SerializeField] private List<int> _shopArmorBuff;
    [SerializeField] private List<int> _battleArmorBuff;

    public float GetMaxHPBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroMaxHP") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroMaxHP") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopMaxHPBuff.Count) ? _shopMaxHPBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleMaxHPBuff.Count) ? _battleMaxHPBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetSpeedBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroSpeed") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroSpeed") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopSpeedBuff.Count) ? _shopSpeedBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleSpeedBuff.Count) ? _battleSpeedBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetMagnetBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroMagnet") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroMagnet") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopMagnetBuff.Count) ? _shopMagnetBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleMagnetBuff.Count) ? _battleMagnetBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetLuckyBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroLucky") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroLucky") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopLuckyBuff.Count) ? _shopLuckyBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleLuckyBuff.Count) ? _battleLuckyBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public float GetGrowthBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroGrowth") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroGrowth") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopGrowthBuff.Count) ? _shopGrowthBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleGrowthBuff.Count) ? _battleGrowthBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }

    public int GetArmorBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopHeroArmor") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleHeroArmor") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopGrowthBuff.Count) ? _shopGrowthBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleGrowthBuff.Count) ? _battleGrowthBuff[battlBuffeLv] : 0.0f;

        return (int)coefficient;
    }
}

