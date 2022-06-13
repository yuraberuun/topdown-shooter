using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    private static DropController instance;

    public static DropController Instance => instance;

    [SerializeField]
    private Transform _player;
    [SerializeField]
    private float _xpMagnetSpeed = 5f;

    private bool isMagnetUsing = false;

    [SerializeField]
    private Transform _parent;

    [SerializeField]
    private DropInfo _xp3Drop; // 3 level of xp
    [SerializeField]
    private DropInfo _chestDrop; 

    [SerializeField]
    private List<DropInfo> xpPrefabs = new List<DropInfo>();
    [SerializeField]
    private List<DropInfo> lanternDrop = new List<DropInfo>();

    private List<GameObject> xpOnLevel = new List<GameObject>();
    private List<GameObject> additionalXP = new List<GameObject>();

    private List<Transform> magnetXPTransfroms = new List<Transform>();

    private List<Transform> _chests = new List<Transform>();
    void Awake()
    {
        if (instance)
            Destroy(gameObject);

        instance = this;
    }

    private void Update()
    {
        if (isMagnetUsing)
            UseMagnet();
    }

    private GameObject GetPrefabWithChance(List<DropInfo> list)
    {
        float totalChance = 0;
        foreach (var i in list)
            totalChance += i.chance;

        float randomChance = Random.Range(0, totalChance);

        float currentChanceAmount = 0;
        var item = list.Find((x) =>
        {
            bool result = randomChance > currentChanceAmount && randomChance < currentChanceAmount + x.chance;
            currentChanceAmount += x.chance;
            return result;
        });

        return item?.prefab;
    }

    public void SpawnXP(Vector2 pos)
    {
        var prefab = GetPrefabWithChance(xpPrefabs);

        if (prefab == null)
            return;

        var item = Instantiate(prefab, pos, Quaternion.identity, _parent);

        if (isMagnetUsing)
        {
            additionalXP.Add(item);
            return;
        }

        xpOnLevel.Add(item);
    }

    public void SpawnDropToLantern(Vector2 pos)
    {
        var prefab = GetPrefabWithChance(lanternDrop);

        if (prefab == null)
            return;

        Instantiate(prefab, pos, Quaternion.identity, _parent);
    }

    public void SpawnDropSpecialEnemy(Vector2 pos)
    {
        float randomChanceDropChest = Random.Range(0, 100f);
        float randomChanceDropXP3 = Random.Range(0, 100f);

        bool droppedChest = false;

        if(randomChanceDropChest < _chestDrop.chance)
        {
            SpawnChest(pos);
            droppedChest = true;
        }

        if (randomChanceDropXP3 < _xp3Drop.chance || !droppedChest)
        {
            GameObject item = Instantiate(_xp3Drop.prefab, pos, Quaternion.identity, _parent);
            if (isMagnetUsing)
            {
                additionalXP.Add(item);
                return;
            }

            xpOnLevel.Add(item);
        }
    }
    public void SpawnChest(Vector2 pos)
    {
        GameObject chest = Instantiate(_chestDrop.prefab, pos, Quaternion.identity, _parent);
        _chests.Add(chest.transform);
    }

    public List<Transform> AllActiveChests() => _chests;

    public void TakeXPItem(GameObject gm)
    {
        if(isMagnetUsing)
            magnetXPTransfroms.Remove(gm.transform);

        var result = xpOnLevel.Remove(gm);
        if (!result)
            additionalXP.Remove(gm);
    }

    private void UseMagnet()
    {
        if (xpOnLevel.Count == 0)
        {
            isMagnetUsing = false;
            xpOnLevel.AddRange(additionalXP);
            additionalXP.Clear();
            return;
        }

        foreach (var item in magnetXPTransfroms)
            item.position = Vector2.MoveTowards(item.position, _player.position, _xpMagnetSpeed * Time.deltaTime);
    }

    public void MakeXPMagnet()
    {
        isMagnetUsing = true;

        foreach (var item in xpOnLevel)
        {
            item.GetComponent<DropItem>().SetAnimated(true);
            magnetXPTransfroms.Add(item.transform);
        } 
    }
}
