using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PowerUpList : ScriptableObject
{
    [SerializeField] internal List<CellInfo> powerUps;

    public int GetPowerUpIndex(CellInfo info)
    {
        int counter = 0;

        if (info == null)
            return -1;

        foreach (var item in powerUps)
        {
            if (item.playerPrefsName == info.playerPrefsName)
                return counter;
            counter++;
        }
        return -1;
    }
}