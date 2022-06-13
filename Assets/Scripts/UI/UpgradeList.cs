using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeList", menuName = "Upgrade List")]
public class UpgradeList : ScriptableObject
{
    [SerializeField] internal List<UpgradeInfo> weapons;
    [SerializeField] internal List<UpgradeInfo> buffs;

    [SerializeField] internal UpgradeInfo HPItem;
    [SerializeField] internal UpgradeInfo moneyItem;

    public UpgradeInfo GetUpgradeFromCell(CellInfo info)
    {
        if (info == null)
            return null;

        foreach (var item in weapons)
            if (item.name == info.heroWeaponName)
                return item;

        foreach (var item in buffs)
            if (item.name == info.heroWeaponName)
                return item;

        return null;
    }

}