using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _survivedTimeText;
    //[SerializeField] private TextMeshProUGUI _earnedGoldText;
    [SerializeField] private TextMeshProUGUI _killedEnemiesText;
    //[SerializeField] private TextMeshProUGUI _reachedLevelText;

    private void Start()
    {
        ResultInfo info = HUDController.Instance.GetResults();
        _survivedTimeText.text = info.timeText;
        //_earnedGoldText.text = info.gold.ToString();
        _killedEnemiesText.text = info.killedEnemies.ToString();
        //_reachedLevelText.text = info.reachedLevel.ToString();
        PlayerPrefs.SetInt("TotalKills", PlayerPrefs.GetInt("TotalKills") + info.killedEnemies);
        //AchievementsManager.Instance.FindAchievements(info, GUIController.instance.currentHero.name);

    }

    public void RestartGame()
    {
        StartCoroutine(RestartWithDelay());
    }

    private IEnumerator RestartWithDelay()
    {
        SoundManager.Instance.MakeSound(SoundType.UIClick);
        yield return new WaitForSecondsRealtime(.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
