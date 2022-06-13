using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatManager : MonoBehaviour
{
    bool activePanel = false;
    [SerializeField] GameObject panel;
    [SerializeField] HeroController hero;
    [SerializeField] InputField expValue;
    [SerializeField] InputField killValue;
    [SerializeField] InputField moneyValue;

    void Start()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        activePanel = true;
#endif

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && activePanel)
        {
            panel.SetActive(!panel.activeInHierarchy);
        }
    }

    public void SpawnChest()
    {
        DropController.Instance.SpawnChest(hero.transform.position + Vector3.right * 10);
    }

    public void SetDeathless()
    {
        hero.HP = 1000000;
    }

    public void SetTimeScale(int value)
    {
        Time.timeScale = value;
    }

    public void AddExp()
    {
        XPSystem.Instance.TakeXP(int.Parse(expValue.text));
    }

    public void SetKills()
    {
        HUDController.Instance.UpdateKilledEnemies(int.Parse(killValue.text));
    }

    public void AddMoney()
    {
        Inventory.Instance.TakeMoney(int.Parse(moneyValue.text));
    }

    public void UpLevel()
    {
        XPSystem.Instance.UpOneLevel();
    }


    public void Set()
    {
        XPSystem.Instance.UpOneLevel();
    }

    public void KillEnemies()
    {
        var enemiesOnScreen = EnemyContainer.Instance.GetEnemyOnCamera();

        foreach (var enemy in enemiesOnScreen)
            enemy.GetComponent<EnemyAI>().TakeDamage(9999);

        Inventory.Instance.AddBonus("Death");
    }

}
