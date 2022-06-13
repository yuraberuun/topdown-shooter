using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropInfo 
{
    [Range(0, 100)]
    public float chance;
    public GameObject prefab;
}
