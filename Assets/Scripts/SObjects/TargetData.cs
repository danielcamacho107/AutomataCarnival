using UnityEngine;

[CreateAssetMenu(fileName = "TargetData", menuName = "Scriptable Objects/TargetData")]
public class TargetData : ScriptableObject
{
    [Header("Metadata")]
    public string gameObjectName;

    [Header("Visual & Audio")]
    public Sprite targetSprite;
    public AudioClip breakSound;

    [Header("Gameplay")]
    public uint pointValue;
    public float movementSpeed;
}
