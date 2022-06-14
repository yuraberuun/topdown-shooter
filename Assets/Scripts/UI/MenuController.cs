using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] internal SubMenuController subMenu;
    [SerializeField] internal GameObject powerUpBtn;

    public void OnStartPress()
    {
        if (GameStatus.Instance.status == GameState.OnPause)
            GUIController.instance.OpenOrCloseMenu();
        else
            subMenu.ShowHeroes();
        Debug.Log("Press Start");

        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void OnCollectionPress()
    {
        subMenu.ShowCollection();
        Debug.Log("Press Clollection");
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void OnAchievementsPress()
    {
        subMenu.ShowAchievement();
        Debug.Log("Press Achievements");
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void OnPowerUpPress()
    {
        subMenu.ShowPowerUp();
        Debug.Log("Press PowerUp");
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void OnOptionsPress()
    {
        subMenu.ShowOptions();
        Debug.Log("Press Options");
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void OnExitPress()
    {
        if (GameStatus.Instance.status == GameState.OnPause)
        {
            GUIController.instance.OpenOrCloseMenu();
            GUIController.instance.EndGame();
        }
        else
            Application.Quit();

        Debug.Log("Press Exit");
        
        SoundManager.Instance.MakeSound(SoundType.UIClick);
    }

    public void SetPauseMenu()
    {
        //powerUpBtn.SetActive(false);
        subMenu.MenuInPause();
    }

}
