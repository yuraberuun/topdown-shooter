using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPSystem : MonoBehaviour
{
    private static XPSystem instance;

    public static XPSystem Instance => instance;

    private int xpToNextLevel = 100;
    private float allXP = 0;
    private float currentXP = 0;
    private int currentLevel = 1;

    [SerializeField]
    private HeroBuffsData heroData;

    [SerializeField]
    private List<int> xpToLevels = new List<int>();

    void Start()
    {
        if (instance)
            Destroy(gameObject);

        instance = this;

        xpToNextLevel = xpToLevels[currentLevel];
    }

    public void TakeXP(int value)
    {
        float upgradedValue = value * heroData.GetGrowthBuff();

        currentXP += upgradedValue;
        allXP += upgradedValue;

        if (currentXP >= xpToNextLevel)
        {
            int upgradesAmount = 0;
            while(currentXP >= xpToNextLevel)
            {
                currentXP = currentXP - xpToNextLevel;
                upgradesAmount++;
                if (currentLevel + upgradesAmount >= xpToLevels.Count)
                    xpToNextLevel = xpToLevels[xpToLevels.Count - 1];
                else
                    xpToNextLevel = xpToLevels[currentLevel + upgradesAmount];
            }

            StartCoroutine(WaitToNextUpgrade(upgradesAmount));
        }

        HUDController.Instance.UpdateXPBar((float)currentXP / xpToNextLevel);
    }

    public void UpOneLevel()
    {
        TakeXP(xpToNextLevel);
    }

    private IEnumerator WaitToNextUpgrade(int upgradesAmount)
    {
        for (int i = 0; i < upgradesAmount; i++)
        {
            currentLevel += 1;
            SoundManager.Instance.MakeSound(SoundType.LevelUp);

            HUDController.Instance.UpdateLevel(currentLevel);

            while (GameStatus.Instance.status != GameState.Play)
            {
                yield return null;
            }
        }
    }
}
