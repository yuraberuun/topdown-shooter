using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class Cell : MonoBehaviour
{
    public CellInfo info;
    public List<Color> upgradeColorList;
    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = info.sprite;

        if (info.upgradeLevel == 0)
            return;

        var backgroundColorInd = (info.upgradeLevel == info.upgradeLevelMax) ? upgradeColorList.Count - 1: info.upgradeLevel;
        GetComponent<Image>().color = upgradeColorList[backgroundColorInd];
    }

    void Awake() => GetComponent<Button>().onClick.AddListener(CellClick);

    private void CellClick() => ItemInfoController.instance.OnCellPress(info);

    public GameObject SetInfo(CellInfo t_info)    
    {
        info = t_info;
        return gameObject;
    }
}
