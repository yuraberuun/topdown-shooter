using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AchievementList : ScriptableObject
{
    [SerializeField] internal List<CellInfo> achievements;
}