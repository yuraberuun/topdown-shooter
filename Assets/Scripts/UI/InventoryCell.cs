using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] Transform blocks;
    [SerializeField] Image image;

    [SerializeField] Color activeBlock;

    public GameObject SetInfo(UpgradeInfo info)
    {
        for (int i = 0; i < blocks.childCount; i++)
        {
            blocks.GetChild(i).GetComponent<Image>().color = i < info.upgradeLevel ? activeBlock : Color.white;
            blocks.GetChild(i).gameObject.SetActive(i <= (info.upgradeLevelMax - (info.type == UpgradeType.Buff ? 1 : 0)));
            
        }
            
        image.sprite = info.sprite;

        return gameObject;
    }
}
