using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    [SerializeField] Toggle fullscreen;
    [SerializeField] Toggle showDamage;

    private void Start()
    {
        fullscreen.onValueChanged.AddListener(OnScreenTogglePress);
        showDamage.onValueChanged.AddListener(OnShowDamageTogglePress);

        fullscreen.isOn = SettingsLoader.Instance.IsFullscreen;
        showDamage.isOn = SettingsLoader.Instance.ShowEnemyDamageText;

        musicSlider.value = SettingsLoader.Instance.MusicVolume;
        soundSlider.value = SettingsLoader.Instance.SoundVolume;
    }

    public void OnScreenTogglePress(bool value)
    {
        if (value != SettingsLoader.Instance.IsFullscreen)
            SoundManager.Instance.MakeSound(SoundType.UIClick);

        Screen.fullScreen = value;
        SettingsLoader.Instance.IsFullscreen = value;

        PlayerPrefs.SetInt("FullScreen", value ? 1 : 0);
    }

    public void OnShowDamageTogglePress(bool value)
    {
        if (value != SettingsLoader.Instance.ShowEnemyDamageText)
            SoundManager.Instance.MakeSound(SoundType.UIClick);

        SettingsLoader.Instance.ShowEnemyDamageText = value;
       
        PlayerPrefs.SetInt("ShowEnemyDamageText", value ? 1 : 0);
    }

    public void OnSoundValueChanged()
    {
        Debug.Log("Sound value: " + soundSlider.value.ToString());

        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        SoundManager.Instance.SetSoundVolume(soundSlider.value);
    }

    public void OnMusicValueChanged()
    {
        Debug.Log("Music value: " + musicSlider.value.ToString());

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        SoundManager.Instance.SetMusicVolume(musicSlider.value);
    }
}
