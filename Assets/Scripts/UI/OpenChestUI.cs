using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpenChestUI : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform cardContainer;

    [SerializeField] Button doneButton;
    [SerializeField] TextMeshProUGUI moneyText;

    [Header("Gold Per Card")]
    [SerializeField] private int minGold = 50;
    [SerializeField] private int maxGold = 130;

    [SerializeField] private float goldMultiplyPerCard = 0.5f;

    private int generatedGold = 0;

    public void GenerateCards(List<UpgradeInfo> cardsInfo)
    {
        generatedGold = 0;
        doneButton.interactable = false;

        moneyText.text = Mathf.Round(generatedGold).ToString("0");

        GenerateGoldAmount(cardsInfo.Count);

        foreach (Transform child in cardContainer)
            Destroy(child.gameObject);

        foreach (var item in cardsInfo)
            Instantiate(cardPrefab.GetComponent<UpgradeCell>().SetInfo(item), cardContainer).GetComponent<UpgradeCell>().ShowCard();
    }

    public void OnDonePress()
    {
        Time.timeScale = 1;
        GameStatus.Instance.status = GameState.Play;
        gameObject.SetActive(false);

        ChestParticles.Instance.StopParticles();

        GUIController.instance.MakePlayerDeathLess();

        Inventory.Instance.TakeMoney(generatedGold);

        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    private void GenerateGoldAmount(int cardsCount)
    {
        int randomGold = Random.Range(minGold, maxGold);

        generatedGold = (int)((cardsCount * goldMultiplyPerCard) * randomGold) + randomGold;

        Debug.Log("Generated gold in chest: " + generatedGold);

        StartCoroutine(MakeGoldAnimation());
    }

    private IEnumerator MakeGoldAnimation()
    {
        float amount = 0;

        while (Mathf.Round(amount) != generatedGold)
        {
            amount += Time.unscaledDeltaTime * Mathf.Lerp(1f, generatedGold / 4f, 1f);

            moneyText.text = Mathf.Round(amount).ToString();

            yield return null;
        }

        doneButton.interactable = true;
    }
}
