using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class CellInfo 
{
    [SerializeField] internal string name;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal Sprite heroWeaponSprite;
    [SerializeField] internal string heroWeaponName;
    [SerializeField] internal string description;
    [SerializeField] internal string addition;
    [SerializeField] internal string playerPrefsName;
    [SerializeField] internal string achPlayerPrefsName;
    [SerializeField] internal bool open;
    [SerializeField] internal List<int> costs;
    [SerializeField] internal int upgradeLevel;
    [SerializeField] internal int upgradeLevelMax;
}
