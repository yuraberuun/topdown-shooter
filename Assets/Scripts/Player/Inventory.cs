using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroWeapon
{
    public string name;
    public GameObject weapon;
}

[System.Serializable]
public class CardsCountChances
{
    public int cards;
    [Range(0, 100f)]
    public float chance;
}

public class Inventory : SingletonComponent<Inventory>
{
    [SerializeField] private UpgradeList items;

    [SerializeField] private List<HeroWeapon> heroWeapons = new List<HeroWeapon>();

    private int money = 0;

    private List<UpgradeInfo> _weapons = new List<UpgradeInfo>();
    private List<UpgradeInfo> _buffs = new List<UpgradeInfo>();

    private HeroController hero;

    [SerializeField] private HeroBuffsData _heroData;

    private Dictionary<string, int> _bonuses = new Dictionary<string, int>();

    private int maxItemsCount = 6;  // count of buffs & weapons

    private int _maxCountBuffItem = 0;
    private int _maxCountWeaponItem = 0;

    private int _weaponsFullUpgraded = 0;
    private int _buffsFullUpgraded = 0;

    private int _buffsOpen = 0;
    private int _weaponsOpen = 0;

    [SerializeField]
    private List<CardsCountChances> _upgradeLevelCards = new List<CardsCountChances>();
    [SerializeField]
    private List<CardsCountChances> _chestCards = new List<CardsCountChances>();

    private void Start()
    {
        _weapons.Clear();
        _buffs.Clear();

        foreach (var item in items.buffs)
            item.upgradeLevel = 0;

        foreach (var item in items.weapons)
            item.upgradeLevel = 0;

        hero = GetComponent<HeroController>();

        _buffsOpen = items.buffs.FindAll(x => x.isOpen).Count;
        _weaponsOpen = items.buffs.FindAll(x => x.isOpen).Count;

        _maxCountBuffItem = maxItemsCount > _buffsOpen ? _buffsOpen : maxItemsCount;
        _maxCountWeaponItem = maxItemsCount > _weaponsOpen ? _weaponsOpen : maxItemsCount;
    }

    public void TakeMoney(int value)
    {
        money += value;

        HUDController.Instance.UpdateMoneyText(money);
    }

    public int GetSessionMoney() => money;

    public void AddItem(UpgradeInfo item)
    {
        if (IsMaxUpgraded(item))
            return;

        if (item.type == UpgradeType.HP)
        {
            hero.AddHP(30);
            return;
        }

        if (item.type == UpgradeType.Money)
        {
            TakeMoney(25);
            return;
        }

        if (HasItem(item))
        {
            if (item.type == UpgradeType.Buff)
            {
                var existing = _buffs.Find(x => x.name == item.name);
                existing.upgradeLevel += 1;

                PlayerPrefs.SetInt(item.playerPrefsName, PlayerPrefs.GetInt(item.playerPrefsName) + 1);
                hero.ResetCharacteristic();

                if (IsMaxUpgraded(existing))
                    _buffsFullUpgraded += 1;

                UpdateWeapons();

                return;
            }

            if (item.type == UpgradeType.Weapon)
            {
                var existing = _weapons.Find(x => x.name == item.name);
                existing.upgradeLevel += 1;

                var inventoryItem = heroWeapons.Find(x => x.name == item.name);

                if (inventoryItem.name != "Shield")
                    inventoryItem.weapon.GetComponent<WeaponController>().Upgrade(1);
                else
                    inventoryItem.weapon.GetComponent<ShieldController>().Upgrade(1);

                if (IsMaxUpgraded(existing))
                    _weaponsFullUpgraded += 1;

                return;
            }
        }

        if (item.type == UpgradeType.Buff)
        {
            PlayerPrefs.SetInt(item.playerPrefsName, PlayerPrefs.GetInt(item.playerPrefsName) + 1);
            hero.ResetCharacteristic();
            item.upgradeLevel = 1;
            _buffs.Add(item);
            UpdateWeapons();
        }

        if (item.type == UpgradeType.Weapon)
        {
            heroWeapons.Find(x => x.name == item.name).weapon.SetActive(true);
            item.upgradeLevel = 8;
            _weapons.Add(item);
        }
    }

    private UpgradeInfo GetItem()
    {
        int listCount = 2;

        if (_buffs.Count == _maxCountBuffItem && IsAllMaxUpgraded(_buffs))
            listCount -= 1;

        if (_weapons.Count == _maxCountWeaponItem && IsAllMaxUpgraded(_weapons))
            listCount -= 2;

        if (listCount < 0)
            return null;

        if (listCount == 2)
        {
            int rand = Random.Range(0, listCount);

            if (rand == 0)
            {
                if (_weapons.Count == _maxCountWeaponItem)
                {
                    return GetRandomItemInList(_weapons.FindAll(x => !IsMaxUpgraded(x)));
                }

                return GetRandomItemInList(items.weapons.FindAll(x => !IsMaxUpgraded(x) && x.isOpen));
            }
            else
            {
                if (_buffs.Count == _maxCountBuffItem)
                {
                    return GetRandomItemInList(_buffs.FindAll(x => !IsMaxUpgraded(x)));
                }

                return GetRandomItemInList(items.buffs.FindAll(x => !IsMaxUpgraded(x) && x.isOpen));
            }
        }

        if (listCount == 1)
        {
            if (_weapons.Count == _maxCountWeaponItem)
            {
                return GetRandomItemInList(_weapons.FindAll(x => !IsMaxUpgraded(x)));
            }

            return GetRandomItemInList(items.weapons.FindAll(x => !IsMaxUpgraded(x) && x.isOpen));
        }

        if (listCount == 0)
        {
            if (_buffs.Count == _maxCountBuffItem)
            {
                return GetRandomItemInList(_buffs.FindAll(x => !IsMaxUpgraded(x)));
            }

            return GetRandomItemInList(items.buffs.FindAll(x => !IsMaxUpgraded(x) && x.isOpen));
        }

        return null;
    }

    private UpgradeInfo GetChestItem()
    {
        int listCount = 2;

        if (IsAllMaxUpgraded(_buffs))
            listCount -= 1;

        if (IsAllMaxUpgraded(_weapons))
            listCount -= 2;

        if (listCount < 0)
            return null;

        if (listCount == 2)
        {
            int rand = Random.Range(0, listCount);

            if (rand == 0)
            {
                return GetRandomItemInList(_weapons.FindAll(x => !IsMaxUpgraded(x)));
            }
            else
            {
                return GetRandomItemInList(_buffs.FindAll(x => !IsMaxUpgraded(x)));
            }
        }

        if (listCount == 1)
        {
            return GetRandomItemInList(_weapons.FindAll(x => !IsMaxUpgraded(x)));
        }

        if (listCount == 0)
        {
            return GetRandomItemInList(_buffs.FindAll(x => !IsMaxUpgraded(x)));
        }

        return null;
    }

    private bool IsAllMaxUpgraded(List<UpgradeInfo> list)
    {
        bool result = true;

        foreach (var item in list)
            if (!IsMaxUpgraded(item))
                result = false;

        return result;
    }

    private UpgradeInfo GetRandomItemInList(List<UpgradeInfo> list)
    {
        List<UpgradeInfo> localList = new List<UpgradeInfo>();

        foreach (var i in list)
            localList.Add(i);

        if (list.Count == 0)
            return null;

        int index = Random.Range(0, localList.Count);
        var item = localList[index];

        while (IsMaxUpgraded(item))
        {
            localList.RemoveAt(index);

            if (localList.Count == 0)
                return null;

            index = Random.Range(0, localList.Count);
            item = localList[index];
        }

        if (HasItem(item))
            item.upgradeLevel = GetActualInfo(item).upgradeLevel;

        return item;
    }

    public List<UpgradeInfo> GenerateItems()
    {
        int count = GetCountWithLucky(_upgradeLevelCards, _heroData.GetLuckyBuff());

        List<UpgradeInfo> generated = new List<UpgradeInfo>();

        for (int i = 0; i < count; i++)
        {
            var newItem = GetItem();
            if (newItem == null)
            {
                Debug.LogWarning("All items full upgraded");
                continue;
            }

            bool isNullItem = false;

            while (generated.Contains(newItem) || IsMaxUpgraded(newItem))
            {
                newItem = GetItem();

                if (i >= (_maxCountBuffItem - _buffsFullUpgraded) + (_maxCountWeaponItem - _weaponsFullUpgraded))
                {
                    Debug.LogWarning("Full items generation");
                    isNullItem = true;
                    break;
                }
            }

            if (isNullItem)
                continue;

            generated.Add(newItem);
        }

        if (generated.Count == 0)
        {
            Debug.LogWarning("Zero items adding HP + Money");
            generated.Add(items.HPItem);
            generated.Add(items.moneyItem);
        }

        return generated;
    }

    public List<UpgradeInfo> GenerateChestItems()
    {
        int count = GetCountWithLucky(_chestCards, _heroData.GetLuckyBuff());

        List<UpgradeInfo> generated = new List<UpgradeInfo>();

        Debug.Log("Chest cards: " + count);

        for (int i = 0; i < count; i++)
        {
            var newItem = GetChestItem();

            if (newItem == null)
                continue;

            var item = new UpgradeInfo(newItem);

            if (item != null && generated.Contains(item))
                item.upgradeLevel += generated.FindAll(x => x.name == item.name).Count;

            generated.Add(item);

            HUDController.Instance.AddUpgradeItem(newItem);
        }

        if (generated.Count == 0)
        {
            Debug.LogWarning("Zero items adding Money + Money in chest");

            for(int i = 0; i < count; i++)
                generated.Add(items.moneyItem);
                AddItem(items.moneyItem);
        }

        return generated;
    }

    public List<UpgradeInfo> GetWeapons() => _weapons;

    public List<UpgradeInfo> GetBuffs() => _buffs;

    public bool HasItem(UpgradeInfo item)
    {
        if (item.type == UpgradeType.Buff)
            return _buffs.Find(x => x.name == item.name) != null;

        if (item.type == UpgradeType.Weapon)
            return _weapons.Find(x => x.name == item.name) != null;

        return false;
    }

    private bool IsMaxUpgraded(UpgradeInfo item)
    {
        return item.upgradeLevel == item.upgradeLevelMax;
    }

    private UpgradeInfo GetActualInfo(UpgradeInfo item)
    {
        if (item.type == UpgradeType.Buff)
            return _buffs.Find(x => x.name == item.name);

        if (item.type == UpgradeType.Weapon)
            return _weapons.Find(x => x.name == item.name);

        return null;
    }

    public void AddBonus(string bonusName)
    {
        if (_bonuses.ContainsKey(bonusName))
        {
            _bonuses[bonusName] += 1;
            return;
        }

        _bonuses.Add(bonusName, 1);
    }

    private void UpdateWeapons()
    {
        foreach(var item in _weapons)
        {
            var heroWeapon = heroWeapons.Find(x => x.name == item.name).weapon;
            if (heroWeapon.GetComponent<WeaponController>())
                heroWeapon.GetComponent<WeaponController>().Upgrade();
            else
                heroWeapon.GetComponent<ShieldController>().Upgrade();
        }
    }

    private int GetCountWithLucky(List<CardsCountChances> chances, float coefficient)
    {
        List<CardsCountChances> _chances = new List<CardsCountChances>(chances);

        float totalChance = 0f;

        for (int i = 0; i < _chances.Count; i++)
        {
            if(i > 0)
            {
                _chances[i].chance *= coefficient;
            }

            totalChance += _chances[i].chance;
        }

        float randomChance = Random.Range(0f, totalChance);

        float currentChanceAmount = 0;
        return _chances.Find((x) =>
        {
            bool result = randomChance > currentChanceAmount && randomChance < currentChanceAmount + x.chance;
            currentChanceAmount += x.chance;
            return result;
        })?.cards ?? 0;
    }

    public Dictionary<string, int> GetBonuses() => _bonuses;
}
