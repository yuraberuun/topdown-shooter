using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class HeroList : ScriptableObject
{
    [SerializeField] internal List<CellInfo> heroes;

    public int GetHeroIndex(CellInfo info)
    {
        int counter = 0;
        
        if (info == null)
            return -1;

        foreach (var item in heroes)
        {
            if (item.playerPrefsName == info.playerPrefsName)
                return counter;
            counter++;
        }
        return -1;
    }
}

