using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class LevelUpgradesUI : SingletonComponent<LevelUpgradesUI>
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;

    [SerializeField] GameObject invCellPrefab;
    [SerializeField] Transform inventoryContainer;

    [SerializeField] UpgradeList upgrades;

    internal void OnUpgradeCellPress(UpgradeInfo info)
    {
        Time.timeScale = 1;
        HUDController.Instance.AddUpgradeItem(info);
        gameObject.SetActive(false);
        GameStatus.Instance.status = GameState.Play;
        GUIController.instance.MakePlayerDeathLess();

        SoundManager.Instance.MakeSound(SoundType.ChooseUpgrade);
    }

    public void Generate(List<UpgradeInfo> cardsInfo)
    {
        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);
        
        foreach (Transform child in inventoryContainer)
            Destroy(child.gameObject);

        foreach (var item in cardsInfo)
            Instantiate(cardPrefab.GetComponent<UpgradeCell>().SetInfo(item), cardContainer);

        foreach (var item in Inventory.Instance.GetWeapons())
            Instantiate(invCellPrefab.GetComponent<InventoryCell>().SetInfo(item), inventoryContainer);

        foreach (var item in Inventory.Instance.GetBuffs())
            Instantiate(invCellPrefab.GetComponent<InventoryCell>().SetInfo(item), inventoryContainer);
    }

    public IEnumerator ShowCards()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        foreach (Transform child in cardContainer)
        {
            child.GetComponent<UpgradeCell>().ShowCard();
            yield return new WaitForSecondsRealtime(0.3f);
        }
    }
}
