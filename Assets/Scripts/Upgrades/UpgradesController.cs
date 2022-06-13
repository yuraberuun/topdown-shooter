using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpgradesController : SingletonComponent<UpgradesController>
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<GameObject> _weapons;

    [SerializeField] private GameObject _shield;
    private bool _shieldIsAvaible = false;

    public void AddWeaponToControll(GameObject obj) => _weapons.Add(obj);

    public void AddShieldControll(GameObject obj)
    {
        _shield = obj;
        _shieldIsAvaible = true;
    }

    public void UpgradeWeapon(string weaponName)
    {
        if (weaponName == "Shield" && _shieldIsAvaible)
            _shield.GetComponent<ShieldController>().Upgrade(1);

        else 
        {
            var weapon = _weapons.Find(x => x.name == weaponName);
             weapon.GetComponent<WeaponController>().Upgrade(1);
        }   
    }

    public void ResetPlayerBuffs() => _player.GetComponent<HeroController>().ResetCharacteristic();
        
    public void ResetWeaponsBuffs()
    {
        foreach(GameObject obj in _weapons)
            obj.GetComponent<WeaponController>().Upgrade();

        if (_shieldIsAvaible)
            _shield.GetComponent<ShieldController>().Upgrade();
    }
}
