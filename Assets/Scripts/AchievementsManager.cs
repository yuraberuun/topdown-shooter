using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsManager : SingletonComponent<AchievementsManager>
{
    [SerializeField] AchievementsTask achievementList;

    public void FindAchievements(ResultInfo info, string heroName)
    {

        foreach (var item in achievementList.killedEnemiesAch)
        {
            if (item.isTotal)
            {
                if (PlayerPrefs.GetInt("TotalKills") >= item.killEnemies)
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                continue;
            }

            if (info.killedEnemies >= item.killEnemies)
                PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
        }

        foreach (var item in achievementList.reachedLevelsAch)
        {
            if (!item.isTotal)
            {
                if (heroName == item.heroName && info.reachedLevel >= item.reachedLevel)
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                continue; 
            }

            if (info.reachedLevel >= item.reachedLevel)
                PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
        }

        foreach (var item in achievementList.liveTimesAch)
        {
            if (!item.isTotal)
            {
                if (heroName == item.heroName && info.time >= item.time)
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                continue;
            }

            if (info.time >= item.time)
                PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
        }

        foreach (var item in achievementList.upgradeWeaponsAch)
        {
            foreach (var weapon in Inventory.Instance.GetWeapons())
            {
                if (item.weaponName == weapon.name && weapon.upgradeLevel >= weapon.upgradeLevelMax)
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
            }
        }


        foreach (var item in achievementList.takeBonusAch)
        {
            foreach (var bonuses in Inventory.Instance.GetBonuses())
            {
                if (item.bonusName == bonuses.Key && item.value >= bonuses.Value)
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
            }
        }

        foreach (var item in achievementList.openItems)
        {
            bool skip = false;
            foreach (var weapon in Inventory.Instance.GetWeapons())
            {
                if (item.itemName == weapon.name)
                {
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                    skip = true;
                    break;
                }
            }

            if (skip)
                continue;

            foreach (var buff in Inventory.Instance.GetBuffs())
            {
                if (item.itemName == buff.name)
                {
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                    skip = true;
                    break;
                }
            }

            if (skip)
                continue;

            foreach (var bonuses in Inventory.Instance.GetBonuses())
            {
                if (item.itemName == bonuses.Key)
                {
                    PlayerPrefs.SetInt(item.achPlayerPrefsName, 1);
                    skip = true;
                    break;
                }
            }

        }


    }
}
