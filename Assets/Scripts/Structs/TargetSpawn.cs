using UnityEngine;
using System;

[Serializable]
public struct TargetSpawn
{
    public TargetData targetData;

    [Range(0f, 1f)]
    public float spawnProbability;
}
