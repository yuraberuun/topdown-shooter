using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyBuffsData", menuName = "Enemy buffs data", order = 54)]
public class EnemyBuffsData : ScriptableObject
{
    [SerializeField] private List<float> _shopSpeedBuff;
    [SerializeField] private List<float> _battleSpeedBuff;

    public float GetSpeedBuff()
    {
        var shopBuffLv  = PlayerPrefs.GetInt("ShopEnemySpeed") - 1;
        var battlBuffeLv  = PlayerPrefs.GetInt("BattleEnemySpeed") - 1;

        var coefficient = 1f;

        coefficient += (shopBuffLv >= 0 && shopBuffLv < _shopSpeedBuff.Count) ? _shopSpeedBuff[shopBuffLv] : 0.0f;
        coefficient += (battlBuffeLv >= 0 && battlBuffeLv < _battleSpeedBuff.Count) ? _battleSpeedBuff[battlBuffeLv] : 0.0f;

        return coefficient;
    }
}