using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
public class GunData : ScriptableObject
{
    [Header("Art & Sound")]
    public Sprite gunTexture;
    public AudioClip gunShotSound;
    public AudioClip reloadSound;

    [Header("Gun Stats")]
    [Tooltip("The rate of fire in shots per second.")]
    [Range(0, 60)]
    public float fireRate;
    [Tooltip("The time it takes to reload the gun in seconds.")]
    public float reloadTime;
    [Tooltip("The size of the gun's magazine.")]
    public uint magazineSize;
    [Tooltip("The total amount of ammo the player has for this gun.")]
    public uint ammo;
}
