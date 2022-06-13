using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCell : MonoBehaviour
{
    public UpgradeInfo info;

    [SerializeField]
    private Image image;

    [SerializeField]
    private TextMeshProUGUI name;

    [SerializeField]
    private TextMeshProUGUI level;

    [SerializeField]
    private GameObject isNew;

    [SerializeField]
    private TextMeshProUGUI description;

    public void UpdateInfo()
    {
        image.sprite = info.sprite;
        name.text = info.name;
        level.text = "Level " + (info.upgradeLevel + 1).ToString();
        description.text = info.description[info.upgradeLevel];
    }

    public void DisplayNewText()
    {
        if (GameStatus.Instance.status == GameState.UpgradeScreen)
            isNew.SetActive(info.upgradeLevel == 0);
    }

    void Start() => GetComponent<Button>()?.onClick?.AddListener(CellClick);

    private void CellClick() => LevelUpgradesUI.Instance.OnUpgradeCellPress(info);

    public void ShowCard()
    {
        GetComponent<Animator>().Play("ShowCard");
    }

    public GameObject SetInfo(UpgradeInfo t_info)
    {
        info = t_info;
        UpdateInfo();
        return gameObject;
    }
}
