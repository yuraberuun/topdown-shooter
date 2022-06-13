using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] protected int level = 1;
    [SerializeField] protected WeaponLevelsData shieldLevelsAsset;
    [SerializeField] protected WeaponBuffsData weaponBuffsData;

    [SerializeField] private List<GameObject> _allShields;
    
    [SerializeField] private GameObject _character;
    
    protected WeaponLevelData currentLevelData;


    private int _shieldsCount = 1;
    private float _cooldown;


    private int _currentActiveShield = 0;

    private float _cooldownTimer = 0;
    private bool _generate = false;

    void OnEnable()
    {   
        Upgrade();

        _cooldownTimer = _cooldown;
    }

    public void RemoveShield()
    {
        _currentActiveShield--;
        ReactivateShields();

        GenerateShield();
    }

    public void GenerateShield()
    {
        if (_shieldsCount == _currentActiveShield)
            _cooldownTimer = _cooldown;
            
        if (_currentActiveShield < _shieldsCount)
            _generate = true;
    }

    private void AddShield()
    {
        _currentActiveShield++;
        ReactivateShields();

        _character.GetComponent<HeroController>().AddShield();

        _generate = (_currentActiveShield == _shieldsCount) ? false : true;
        _cooldownTimer = _cooldown;
    }

    private void ReactivateShields() //Залише тільки _currentActiveShield щит, інші деактивує
    {
        for(int i = 0; i < 3; i++)
            _allShields[i].SetActive(false);

        if (_currentActiveShield != 0)
            _allShields[_currentActiveShield - 1].SetActive(true);
    }

    void Update()
    {
        if (_generate)
        {
            if (_cooldownTimer < 0)
                AddShield();

            _cooldownTimer -= Time.deltaTime;
        }
    }

    public void Upgrade(int upgradeBy = 0)
    {
        if (level > 8 && upgradeBy != 0)
            return;

        level += upgradeBy;
        
        ResetCharacteristic();
        ResetBuffs();
        GenerateShield();
    }

    private void ResetCharacteristic()
    {
        var currentLevel = shieldLevelsAsset.GetLevel(level);
        
        _shieldsCount = currentLevel.BulletCount;
        _cooldown = currentLevel.Cooldown;
    }

    private void ResetBuffs() => _cooldown *= weaponBuffsData.GetCooldownBuff();
}
