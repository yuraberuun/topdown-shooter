using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestParticles : SingletonComponent<ChestParticles>
{
    [SerializeField]
    private GameObject oneTime;
    [SerializeField]
    private GameObject loopTime;

    private void Start()
    {
        StopParticles();
    }

    public void PlayParticles()
    {
        oneTime.gameObject.SetActive(true);
        loopTime.gameObject.SetActive(true);
    }

    public void StopParticles()
    {
        oneTime.gameObject.SetActive(false);
        loopTime.gameObject.SetActive(false);
    }
}
