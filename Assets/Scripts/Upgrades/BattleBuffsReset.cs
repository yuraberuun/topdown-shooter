using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBuffsReset : MonoBehaviour
{
    private string[] _battleBuffsLevels = {"BattleHeroMaxHP", "BattleHeroSpeed", "BattleHeroMagnet", "BattleHeroLucky", "BattleHeroArmor",
    "ShopEnemySpeed","BattleEnemySpeed", "BattleWeaponCooldown", "BattleWeaponDamage", "BattleWeaponDuration", "BattleWeaponBulletCount",
    "BattleWeaponFireBulletCount"};

    void Awake()
    {
        foreach (var i in _battleBuffsLevels)
            PlayerPrefs.SetInt(i, 0);
    }
}
