using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalController : MonoBehaviour
{
    [SerializeField] private GameObject magic;
    [SerializeField] private GameObject bible;
    [SerializeField] private GameObject star;

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Alpha1))
       {
                magic.SetActive(true);
                bible.SetActive(false);
                star.SetActive(false);
       }

       if (Input.GetKeyDown(KeyCode.Alpha2))
       {
                magic.SetActive(false);
                bible.SetActive(true);
                star.SetActive(false);
       }

       if (Input.GetKeyDown(KeyCode.Alpha3))
       {
                magic.SetActive(false);
                bible.SetActive(false);
                star.SetActive(true);
       }
    }

        public void SomeMagic()
        {
                bible.SetActive(!bible.activeInHierarchy);
                star.SetActive(!star.activeInHierarchy);
        }
}
