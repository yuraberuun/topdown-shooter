using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroInfoController : MonoBehaviour
{
    static public HeroInfoController instance;

    bool confirmHero = false;

    [SerializeField] private SubMenuController subMenu;

    [Header("Info Objects")]
    [SerializeField] private Image image;
    [SerializeField] private Image heroBackground;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI cellName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI addition;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject costImage;
    [SerializeField] private Button button;


    CellInfo currentInfo;

    private bool isShow = false;

    void Awake()
    {
        instance = this;
        button.onClick.AddListener(OnStartPress);
        transform.parent.gameObject.SetActive(false);
    }

    public void onHeroCellPress(CellInfo info)
    {
        UpdateInfo(info);
        Debug.Log("Item " + info.name.ToString());
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void UpdateInfo(CellInfo info)
    {
        confirmHero = false;
        currentInfo = info;
        if (!isShow)
            Show();

        SetInfoFromCell(info);
    }


    private void SetInfoFromCell(CellInfo info)
    {
        print(info.open);
        image.sprite = info.sprite;
        weaponImage.sprite = info.heroWeaponSprite;
        cellName.text = info.name;
        description.text = info.description;
        addition.text = info.addition;
        buttonText.text = info.open ? "Confirm" : "Buy";
        cost.gameObject.SetActive(!info.open);
        costImage.SetActive(!info.open);
        image.color = info.open ? Color.white : Color.black;
        weaponImage.color = info.open ? Color.white : Color.black;
        if (!info.open)
            cost.text = info.costs[0].ToString();
        
    }

    private void SetVisible(bool value)
    {
        isShow = value;
        GetComponent<Image>().enabled = value;
        image.transform.parent.gameObject.SetActive(value);
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
    }

    public void OnStartPress()
    {
        SoundManager.Instance.MakeSound(SoundType.UIClick);

        if (!currentInfo.open)
        {
            //buttonText.text = "Buy";
            int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);

            int heroCost = currentInfo.costs[currentInfo.upgradeLevel];

            if (totalMoney < heroCost)
                return;

            totalMoney -= heroCost;

            PlayerPrefs.SetInt(currentInfo.playerPrefsName, 1);
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
            GUIController.instance.UpdateMoneyText(totalMoney);
            subMenu.ShowHeroes(currentInfo);
            confirmHero = false;
            return;
        }

        if (confirmHero)
            GUIController.instance.StartGame(currentInfo);
        else
        {
            confirmHero = true;
            buttonText.text = "Go";
        }

        Debug.Log("Press First Time (Confirm)");
    }
}
