using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsLoader : SingletonComponent<SettingsLoader>
{
    public bool ShowEnemyDamageText { get; set; }
    public bool IsFullscreen { get; set; }

    public float MusicVolume { get; set; }
    public float SoundVolume { get; set; }

    void Awake()
    {
        LoadSettings();

        Screen.fullScreen = IsFullscreen;
    }

    private void Start()
    {
        SoundManager.Instance.SetMusicVolume(MusicVolume);
        SoundManager.Instance.SetSoundVolume(SoundVolume);
    }

    void LoadSettings()
    {
        ShowEnemyDamageText = PlayerPrefs.GetInt("ShowEnemyDamageText", 1) == 1;
        IsFullscreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0f);
    }
}
