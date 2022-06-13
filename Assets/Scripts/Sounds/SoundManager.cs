using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    None,
    StartGame,
    GameOver,
    LevelUp,
    UIClick, 
    ChooseUpgrade,
    ChestOpen, 
    TakeDamage,
    GiveDamage,
    CollectHP,
    CollectMagnet,
    CollectMoney,
    CollectXP,
    WeaponAxeKnifeBoomerang,
    WeaponBone,
    WeaponLightning,
    WeaponMagic,
    WeaponSword,
    WeaponWater,
}

[System.Serializable]
public struct SoundEffectInfo
{
    public SoundType type;
    public AudioClip clip;
}

public class SoundManager : SingletonComponent<SoundManager>
{
    [SerializeField] private AudioMixer _mixer;

    [SerializeField] private List<SoundEffectInfo> _sounds = new List<SoundEffectInfo>();

    [Header("Pool info")]
    [SerializeField] private GameObject _parent;
    [SerializeField] private int _poolSize = 15;
    [SerializeField] private GameObject _prefab;

    private ObjectsPool _pool;

    private void Start()
    {
        _pool = new ObjectsPool(_poolSize, _prefab, _parent);
    }

    public void MakeSound(SoundType type)
    {
        var sound = _sounds.Find(x => x.type == type);

        var gameObject = _pool.GetObject();

        var audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = sound.clip;
        audioSource.Play();
        gameObject.GetComponent<SoundEffect>().SetInfo(sound.clip.length, _pool.AddObject);
    }

    public void SetMusicVolume(float value) => _mixer.SetFloat("MusicVolume", value);

    public void SetSoundVolume(float value) => _mixer.SetFloat("SoundsVolume", value);
}
