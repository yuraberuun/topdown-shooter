using System.Collections.Generic;
using UnityEngine;
public enum UpgradeType {None, Weapon, Buff, HP, Money}

[System.Serializable]
public class UpgradeInfo
{
    [SerializeField] internal string name;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal List<string> description;
    [SerializeField] internal string playerPrefsName;
    [SerializeField] internal string achPlayerPrefsName;
    [SerializeField] internal bool isOpen;
    [SerializeField] internal int upgradeLevel;
    [SerializeField] internal int upgradeLevelMax;
    [SerializeField] internal UpgradeType type;

    public override string ToString()
    {
        return $"{name} lvl: {upgradeLevel}/{upgradeLevelMax}";
    }

    public UpgradeInfo(UpgradeInfo other)
    {
        sprite = other.sprite;
        name = other.name;
        description = other.description;
        playerPrefsName = other.playerPrefsName;
        achPlayerPrefsName = other.achPlayerPrefsName;
        isOpen = other.isOpen;
        upgradeLevel = other.upgradeLevel;
        upgradeLevelMax = other.upgradeLevelMax;
        type = other.type;
    }
}

