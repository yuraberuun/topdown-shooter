using UnityEngine;
using System.Collections.Generic;
using TMPro;
using EnemySpawn;

[System.Serializable]
public struct HeroAnimator
{
    public string name;
    public AnimatorOverrideController animator;
}

public class GUIController : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject HUDPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Animator moneyAnimator;
    [SerializeField] TextMeshProUGUI playButtonText;
    [SerializeField] TextMeshProUGUI exitButtonText;

    [SerializeField] private OpenChestUI chestManager;

    public CellInfo currentHero; 

    static public GUIController instance;

    private GameStatus gamest;

    bool isMenuOpen = true;

    HUDController HUDcontrl;

    [SerializeField] private HeroController player;
    [SerializeField] private GameObject playerHealthBar;

    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private List<HeroAnimator> _heroesAnimators = new List<HeroAnimator>();

    public void Start()
    {
        instance = this;
        gamest = GameStatus.Instance;
        gamest.status = GameState.InMenu;

        Time.timeScale = 0;

        HUDcontrl = HUDPanel.GetComponent<HUDController>();

        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        UpdateMoneyText(totalMoney);
    }

    void Update()
    {
        bool canOpenPause = (gamest.status == GameState.Play) || (gamest.status == GameState.OnPause);
        if (Input.GetKeyDown(KeyCode.Escape) && canOpenPause)
            OpenOrCloseMenu();
    }

    public void StartGame(CellInfo hero)
    {
        currentHero = hero;
        OpenOrCloseMenu();
        gamest.status = GameState.Play;
        HUDcontrl.ShowKilledEnemies();
        HUDcontrl.AddStartWeapon(hero);
        player.SetSprite(hero.sprite);
        player.SetAnimatorController(_heroesAnimators.Find(x => x.name == hero.name).animator);
        playerHealthBar.SetActive(true);

        SoundManager.Instance.MakeSound(SoundType.StartGame);
    }

    public void EndGame()
    {
        gamest.status = GameState.GameOver;
        Time.timeScale = 0;
        AdvancedSpawner.Instance.StopAllCoroutines();
        gameOverPanel.SetActive(true);
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
        PlayerPrefs.SetInt("TotalMoney", totalMoney + Inventory.Instance.GetSessionMoney());

        SoundManager.Instance.MakeSound(SoundType.GameOver);
    }

    public void OpenOrCloseMenu()
    {
        if (GameStatus.Instance.status == GameState.Play)
            menuPanel.GetComponent<MenuController>().SetPauseMenu();

        isMenuOpen = !isMenuOpen;
        moneyAnimator.SetTrigger(isMenuOpen ? "Increase" : "Reduce");

        menuPanel.SetActive(isMenuOpen);
        HUDPanel.SetActive(!isMenuOpen);
        playButtonText.text = isMenuOpen ? "Resume" : "Start" ;
        exitButtonText.text = isMenuOpen ? "Back" : "Exit" ;
        gamest.status =  isMenuOpen ?  GameState.OnPause : GameState.Play;
        Time.timeScale = isMenuOpen ? 0 : 1;
    }

    public void ShowChestPanel()
    {
        Time.timeScale = 0;
        GameStatus.Instance.status = GameState.OpenChest;
        chestManager.gameObject.SetActive(true);
        chestManager.GenerateCards(Inventory.Instance.GenerateChestItems());

        ChestParticles.Instance.PlayParticles();
    }

    public void UpdateMoneyText(int money) => moneyText.text = money.ToString();

    public void MakePlayerDeathLess() => player.MakeDeathless();
}
