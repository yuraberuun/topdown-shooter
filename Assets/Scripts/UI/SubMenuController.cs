using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public enum UIGenerateType { None, Collection, Achievement, PowerUp, Heroes }

public class SubMenuController : MonoBehaviour
{
    [Header("Info lists")]
    [SerializeField] private CollectionList collectionList;
    [SerializeField] private AchievementList achievementList;
    [SerializeField] private PowerUpList powerUpList;
    [SerializeField] private UpgradeList upgrades;
    [SerializeField] private HeroList heroList;

    [Header("Grid controller")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject heroCellPrefab;
    [SerializeField] private GameObject closeCellPrefab;
    [SerializeField] private GameObject closeHeroCellPrefab;
    [SerializeField] private GameObject closeAchCellPrefab;
    [SerializeField] private Image scrollBar;
    [SerializeField] private Image scrollBarHeader;
    [SerializeField] private Transform cellGrid;
    [SerializeField] private Transform heroCellGrid;

    [Header("InfoZones")]
    [SerializeField] private GameObject optionsZone;
    [SerializeField] private GameObject heroesZone;
    [SerializeField] private GameObject otherZone;

    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI subMenuTitle;

    [HideInInspector] public UIGenerateType currentGenType = UIGenerateType.None;

    private void Start()
    {
        UpdateInventoryUpgrades();
    }
    private void Generate(UIGenerateType type, int focusItemInd = 0)
    {
        List<CellInfo> usesList = new List<CellInfo>();
        CellInfo firstElement = new CellInfo();
        var spawnCell = cellPrefab;
        var spawnCloseCell = closeCellPrefab;
        var spawnGrid = cellGrid;

        currentGenType = type;

        bool isPowerUpList = false;
        bool isHeroList = false;

        UpdateList(type);

        switch (type)
        {
            case UIGenerateType.None:
                break;
            case UIGenerateType.Collection:
                {
                    usesList = collectionList.collection;
                    firstElement = usesList[focusItemInd].open ? usesList[focusItemInd] : closeCellPrefab.GetComponent<Cell>().info;
                    break;
                }
            case UIGenerateType.Achievement:
                {
                    usesList = achievementList.achievements;
                    firstElement = usesList[focusItemInd];
                    spawnCloseCell = closeAchCellPrefab;
                    break;
                }
            case UIGenerateType.PowerUp:
                {
                    usesList = powerUpList.powerUps;
                    firstElement = usesList[Mathf.Max(focusItemInd, 0)];
                    isPowerUpList = true;
                    break;
                }
            case UIGenerateType.Heroes:
                {
                    foreach (var item in heroList.heroes)
                        if (item.upgradeLevel >= 0)
                            usesList.Add(item);

                    firstElement = usesList[Mathf.Max(focusItemInd, 0)];
                    spawnCell = heroCellPrefab;
                    spawnGrid = heroCellGrid;
                    spawnCloseCell = closeHeroCellPrefab;
                    isHeroList = true;
                    break;
                }
            default:
                break;
        }

        if (isHeroList)
            HeroInfoController.instance.UpdateInfo(firstElement);
        else
        {
            ItemInfoController.instance.SetPowerUpAddonsEnable(isPowerUpList);
            ItemInfoController.instance.UpdateInfo(firstElement); //set item start info
        }

        ResetGrid(usesList.Count, spawnGrid, type);

        foreach (var item in usesList)
        {
            if (item.open || isPowerUpList)
            {                var cellWithInfo = isHeroList ? spawnCell.GetComponent<HeroCell>().SetInfo(item) : spawnCell.GetComponent<Cell>().SetInfo(item);
                Instantiate(cellWithInfo, spawnGrid);
            }
            else
            {

                switch (type)
                {
                    case UIGenerateType.Achievement:
                        spawnCloseCell = spawnCloseCell.GetComponent<Cell>().SetInfo(item);
                        break;
                    case UIGenerateType.Heroes:
                        spawnCloseCell = spawnCloseCell.GetComponent<HeroCell>().SetInfo(item);
                        break;
                    default:
                        break;
                }
                Instantiate(spawnCloseCell, spawnGrid);
            }
        }
    }

    private void ResetGrid(int ListSize, Transform _grid, UIGenerateType type)
    {
        //remove preview items
        foreach (Transform child in _grid)
            Destroy(child.gameObject);

        //reset inertia
        var scroll = _grid.parent.parent.GetComponent<ScrollRect>();
        scroll.enabled = false;
        scroll.enabled = type == UIGenerateType.Heroes ? ListSize > 9 : ListSize > 16;
        scrollBar.enabled = scroll.enabled;
        scrollBarHeader.enabled = scroll.enabled;




        var gridLayout = _grid.GetComponent<GridLayoutGroup>();
        var gridRect = _grid.GetComponent<RectTransform>();

        var rectSize = new Vector2(gridRect.sizeDelta.x, ((gridLayout.cellSize.y ) * ListSize) / 4 /*column count*/);


        //reset scroll pos
        gridRect.anchoredPosition = new Vector2(gridRect.anchoredPosition.x, -rectSize.y / 2);

        //set scroll lenght
        gridRect.sizeDelta = rectSize;
    }


    private void ChangeActivePanelByName(string name)
    {
        bool activeOptionZone = name == "Options";
        //ItemInfoController.instance.Hide();

        optionsZone.SetActive(name == "Options");
        otherZone.SetActive(name == "Collections" || name == "Achievements" || name == "Power Up");
        heroesZone.SetActive(name == "Heroes");


        subMenuTitle.text = name;
    }

    public void MenuInPause()
    {
        optionsZone.SetActive(false);
        otherZone.SetActive(false);
        heroesZone.SetActive(false);
        subMenuTitle.text = "Pause";
    }

    public void ShowCollection()
    {
        ChangeActivePanelByName("Collections");
        Generate(UIGenerateType.Collection);
    }

    public void ShowAchievement()
    {
        ChangeActivePanelByName("Achievements");
        Generate(UIGenerateType.Achievement);
    }

    public void ShowPowerUp(CellInfo focusItemInfo = null)
    {
        ChangeActivePanelByName("Power Up");
        Generate(UIGenerateType.PowerUp, powerUpList.GetPowerUpIndex(focusItemInfo));
    }

    public void ShowHeroes(CellInfo focusItemInfo = null)
    {
        ChangeActivePanelByName("Heroes");
        Generate(UIGenerateType.Heroes, heroList.GetHeroIndex(focusItemInfo));
    }

    public void ShowOptions()
    {
        ChangeActivePanelByName("Options");
    }

    public void UpdateList(UIGenerateType listType)
    {
        List<CellInfo> usesList = new List<CellInfo>();

        switch (listType)
        {
            case UIGenerateType.None:
                break;
            case UIGenerateType.PowerUp:
                {
                    foreach (var item in powerUpList.powerUps)
                        if (PlayerPrefs.HasKey(item.playerPrefsName))
                            item.upgradeLevel = PlayerPrefs.GetInt(item.playerPrefsName);
                    return;
                }
            case UIGenerateType.Collection:
                {
                    usesList = collectionList.collection;
                    break;
                }
            case UIGenerateType.Achievement:
                {
                    usesList = achievementList.achievements;
                    break;
                }

            case UIGenerateType.Heroes:
                {
                    usesList = heroList.heroes;
                    break;
                }
            default:
                break;
        }

        foreach (var item in usesList) {
            if (listType == UIGenerateType.Heroes)
            {
                if (PlayerPrefs.GetInt(item.achPlayerPrefsName) == 1)
                    item.upgradeLevel = 0;
            }

            if (PlayerPrefs.HasKey(item.playerPrefsName))
                item.open = PlayerPrefs.GetInt(item.playerPrefsName) == 1;
        }
    }

    private void UpdateInventoryUpgrades()
    {
        foreach (var item in upgrades.weapons)
            if (PlayerPrefs.HasKey(item.achPlayerPrefsName))
                item.isOpen = PlayerPrefs.GetInt(item.achPlayerPrefsName) == 1;

        foreach (var item in upgrades.buffs)
            if (PlayerPrefs.HasKey(item.achPlayerPrefsName))
                item.isOpen = PlayerPrefs.GetInt(item.achPlayerPrefsName) == 1;
    }


}
