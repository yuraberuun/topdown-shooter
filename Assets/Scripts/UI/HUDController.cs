using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class HUDController : MonoBehaviour
{
    private static HUDController instance;

    public static HUDController Instance => instance;

    [SerializeField]
    private Image xpImage;

    [SerializeField]
    private TextMeshProUGUI levelText;

    [SerializeField]
    private TextMeshProUGUI moneyText;

    [SerializeField]
    private TextMeshProUGUI killedEnemiesText;

    [SerializeField]
    private Animator killedEnemiesPanel;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Upgrades")]
    [SerializeField] private LevelUpgradesUI upgradeMenu;
    [SerializeField] private Transform buffsContainer;
    [SerializeField] private Transform weaponContainer;
    [SerializeField] private GameObject HUDCellPrefab;

    [SerializeField]
    private UpgradeList upgrades;

    private float _time;

    void Start()
    {
        if (instance)
            Destroy(gameObject);

        instance = this;

        UpdateLevel(1);
        UpdateXPBar(0);
        UpdateMoneyText(0);
        UpdateKilledEnemies(0);
    }

    private void Update()
    {
        UpdateTimer();
    }

    public void UpdateXPBar(float value) => xpImage.fillAmount = value;

    public void AddStartWeapon(CellInfo info)
    {
        AddUpgradeItem(upgrades.GetUpgradeFromCell(info));
    }

    public void AddUpgradeItem(UpgradeInfo info)
    {
        if (!Inventory.Instance.HasItem(info))
        {
            HUDCellPrefab.transform.GetChild(0).GetComponent<Image>().sprite = info.sprite;

            if(info.type == UpgradeType.Weapon)
                Instantiate(HUDCellPrefab, weaponContainer);
            else if (info.type == UpgradeType.Buff)
                Instantiate(HUDCellPrefab, buffsContainer);

        }

        Inventory.Instance.AddItem(info);
    }
    public void UpdateLevel(int value)
    {
        levelText.text = $"Lvl {value}";
        if (value == 1)
            return;

        GameStatus.Instance.status = GameState.UpgradeScreen;
        Time.timeScale = 0;

        upgradeMenu.gameObject.SetActive(true);
        upgradeMenu.Generate(Inventory.Instance.GenerateItems());
    }

    public void UpdateMoneyText(int value) => moneyText.text = value.ToString();

    public void ShowKilledEnemies() => killedEnemiesPanel.SetTrigger("Show");
    public void UpdateKilledEnemies(int value) => killedEnemiesText.text = value.ToString();

    private void UpdateTimer()
    {
        _time += Time.deltaTime;

        int minutes = (int)_time / 60;
        int seconds = (int)_time - minutes * 60;

        timerText.text = $"{minutes}:{(seconds >= 10 ? seconds.ToString() : $"0{seconds}")}";
    }

    public ResultInfo GetResults()
    {
        return new ResultInfo(_time ,timerText.text, moneyText.text, killedEnemiesText.text, levelText.text.Substring(3));
    }

}
