using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct KilledEnemies
{
    public int killEnemies;
    public string achPlayerPrefsName;
    public bool isTotal;
}

[System.Serializable]
struct ReachedLevel
{
    public string heroName;
    public int reachedLevel;
    public string achPlayerPrefsName;
    public bool isTotal;
}

[System.Serializable]
struct UpgradeWeapon
{
    public string weaponName;
    public string achPlayerPrefsName;
}

[System.Serializable]
struct LiveTime
{
    public string heroName;
    public float time;
    public string achPlayerPrefsName;
    public bool isTotal;
}

[System.Serializable]
struct TakeBonus
{
    public string bonusName;
    public int value;
    public string achPlayerPrefsName;
}

[System.Serializable]
struct OpenItem
{
    public string itemName;
    public string achPlayerPrefsName;
}

[CreateAssetMenu()]
public class AchievementsTask : ScriptableObject
{
    [SerializeField] internal List<KilledEnemies> killedEnemiesAch;
    [SerializeField] internal List<ReachedLevel> reachedLevelsAch;
    [SerializeField] internal List<UpgradeWeapon> upgradeWeaponsAch;
    [SerializeField] internal List<LiveTime> liveTimesAch;
    [SerializeField] internal List<TakeBonus> takeBonusAch;
    [SerializeField] internal List<OpenItem> openItems;
}
