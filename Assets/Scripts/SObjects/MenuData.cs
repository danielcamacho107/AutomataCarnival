using UnityEngine;

[CreateAssetMenu(fileName = "MenuData", menuName = "Scriptable Objects/MenuData")]
public class MenuData : ScriptableObject
{
    // Audio clips for menu interactions
    public AudioClip hoverSound;
    public AudioClip pressedSound;
}
