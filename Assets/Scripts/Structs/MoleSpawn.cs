using UnityEngine;
using System;

[Serializable]
public struct MoleSpawn
{
    public MoleData moleData;

    [Range(0f, 1f)]
    public float spawnProbability;
}
