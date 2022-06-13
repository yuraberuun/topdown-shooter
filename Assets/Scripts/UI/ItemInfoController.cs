using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoController : MonoBehaviour
{
    static public ItemInfoController instance;

    [SerializeField] private SubMenuController subMenu;

    [Header("Info Objects")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI cellName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI addition;
    [SerializeField] private TextMeshProUGUI price;
    [SerializeField] private GameObject upgrades;
    [SerializeField] private GameObject buyBtn;

    [Header("Ubgrade block color")]
    [SerializeField] private Color blockActiveColor;
    [SerializeField] private Color blockDefaultColor;

    private CellInfo focusCell;

    private bool isShow = false;

    void Start()
    {
        instance = this;
        buyBtn.GetComponent<Button>().onClick.AddListener(OnBuyPress);
    }

    public void OnCellPress(CellInfo info)
    {
        UpdateInfo(info);
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void UpdateInfo(CellInfo info)
    {
        GetComponent<VerticalLayoutGroup>().enabled = false;
        if (!isShow)
            Show();
        focusCell = info;
        SetInfoFromCell(info);

    }

    private void SetPowerUpInfo(CellInfo info)
    {
        var upgrBlocksCounter = 0;

        foreach (Transform child in upgrades.transform)
        {
            if (upgrBlocksCounter < info.upgradeLevelMax)
            {
                child.gameObject.SetActive(true);
                if (upgrBlocksCounter < info.upgradeLevel)
                    child.GetComponent<Image>().color = blockActiveColor;
                else
                    child.GetComponent<Image>().color = blockDefaultColor;
                upgrBlocksCounter++;
            }

            else
                child.gameObject.SetActive(false);
        }

        if (info.upgradeLevel == info.upgradeLevelMax)
        {
            buyBtn.SetActive(false);
            price.text = "MAX";
            return;
        }
        buyBtn.SetActive(true);
        price.text = info.costs[info.upgradeLevel].ToString();
    }

    private void SetInfoFromCell(CellInfo info)
    {
        if (info.upgradeLevelMax != 0)
            SetPowerUpInfo(info);
        


        image.sprite = info.sprite;
        image.color = Color.white;
        addition.text = info.addition;
        cellName.text = info.name;

        if (subMenu.currentGenType == UIGenerateType.Achievement && !info.open)
        {
            cellName.text = "Closed";
            image.color = Color.black;
            addition.text = "";
        }

        description.text = info.description;

        GetComponent<VerticalLayoutGroup>().enabled = true;
    }

    private void SetVisible(bool value)
    {
        isShow = value;
        GetComponent<Image>().enabled = value;
        image.transform.parent.gameObject.SetActive(value);
        upgrades.SetActive(value);
        cellName.gameObject.SetActive(value);
        description.gameObject.SetActive(value);
        addition.gameObject.SetActive(value);
    }

    private void Show()
    {
        SetVisible(true);
    }

    public void Hide()
    {
        SetVisible(false);
        SetPowerUpAddonsEnable(false);
    }

    public void SetPowerUpAddonsEnable(bool enable)
    {
        buyBtn.SetActive(enable);
        foreach (Transform item in upgrades.transform)
        {
            item.gameObject.SetActive(enable);
        } 

        price.gameObject.transform.parent.gameObject.SetActive(enable);
    }

    public void OnBuyPress()
    {
        SoundManager.Instance.MakeSound(SoundType.UIClick);

        if (focusCell.upgradeLevel == focusCell.upgradeLevelMax)
            return;

        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        int upgradeCost = focusCell.costs[focusCell.upgradeLevel];

        if (totalMoney < upgradeCost)
            return;

        focusCell.upgradeLevel += 1;
        totalMoney -= upgradeCost;
        PlayerPrefs.SetInt(focusCell.playerPrefsName, focusCell.upgradeLevel);
        UpdateInfo(focusCell);
        PlayerPrefs.SetInt("TotalMoney", totalMoney);
        GUIController.instance.UpdateMoneyText(totalMoney);

        subMenu.ShowPowerUp(focusCell);
    }
}
