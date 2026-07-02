using UnityEngine;

[CreateAssetMenu(fileName = "MoleData", menuName = "Scriptable Objects/MoleData")]
public class MoleData : ScriptableObject
{
    [Header("Metadata")]
    public string gameObjectName;

    [Header("Visual & Audio")]
    public Sprite moleSprite;
    public AudioClip hitSound;

    [Header("Gameplay")]
    public uint pointValue;
    [Tooltip("The time in seconds that the mole will be active before disappearing.")]
    public float activeTime;
}
