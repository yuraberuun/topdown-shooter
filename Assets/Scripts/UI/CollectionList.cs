using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CollectionList : ScriptableObject
{
    [SerializeField] internal List<CellInfo> collection;
}

