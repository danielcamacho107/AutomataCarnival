using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Gunner : MonoBehaviour
{
    // Delegate for handling changes in gun data (score, ammo counts)
    public delegate void GunDataChanged(uint score, uint ammoInMagazine, uint magazineCapacity, uint ammoInReserve);
    public delegate void OutOfAmmo();

    private Vector2 m_MousePosition;

    [SerializeField]
    private GunData m_GunData;

    // Reference to the input actions class
    private ShootingMinigame m_InputActions;

    // Flag to track if the shoot action is currently active
    private bool m_IsShooting;

    // Flag to track if the shooting coroutine is currently running
    private bool m_IsShootingInCooldown;

    // Current ammo count in the magazine
    private uint m_CurrentAmmoInMagazine;

    // Current total ammo count available to the player
    private uint m_CurrentTotalAmmo;

    // Current score of the player
    private uint m_Score;

    // Event that is invoked whenever the gun data changes (score, ammo counts)
    private GunDataChanged m_OnGunDataChanged;

    // Event that is invoked when the player runs out of ammo
    private OutOfAmmo m_OnOutOfAmmo;

    // Accessor to get the current ammo in the magazine
    public uint ammoInMagazine { get { return m_CurrentAmmoInMagazine; } }

    // Accessor to get the maximum ammo capacity of the magazine
    public uint magazineCapacity { get { return m_GunData.magazineSize; } }

    // Accessor to get the current ammo in reserve
    public uint ammoInReserve { get { return m_CurrentTotalAmmo; } }

    // Accessor to get the current score
    public uint score { get { return m_Score; } }

    public void SubscribeToGunDataChanged(GunDataChanged callback)
    {
        m_OnGunDataChanged += callback;
    }

    public void UnsubscribeFromGunDataChanged(GunDataChanged callback)
    {
        m_OnGunDataChanged -= callback;
    }

    public void SubscribeToOutOfAmmo(OutOfAmmo callback)
    {
        m_OnOutOfAmmo += callback;
    }

    public void UnsubscribeFromOutOfAmmo(OutOfAmmo callback)
    {
        m_OnOutOfAmmo -= callback;
    }

    private bool CanShoot()
    {
        // Check if there is ammo in the magazine
        return m_CurrentAmmoInMagazine > 0;
    }

    private IEnumerator ShootCoorutine()
    {
        // Set the shooting cooldown flag to prevent multiple shots at once
        m_IsShootingInCooldown = true;

        // Play the shooting sound effect
        AudioSource.PlayClipAtPoint(m_GunData.gunShotSound, transform.position);

        // Decrease the ammo count in the magazine
        --m_CurrentAmmoInMagazine;

        // Scan for targets in range and hit them
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && hit.collider.TryGetComponent<Target>(out Target target))
        {
            target.Hit();
            m_Score += target.score;
        }

        // Invoke the gun data changed event to update any UI or other systems
        m_OnGunDataChanged?.Invoke(m_Score, m_CurrentAmmoInMagazine, m_GunData.magazineSize, m_CurrentTotalAmmo);

        // If the magazine is empty after shooting, invoke the out of ammo event
        if (m_CurrentAmmoInMagazine == 0u)
        {
            m_OnOutOfAmmo?.Invoke();
        }

        // Wait for the reload time to elapse
        yield return new WaitForSeconds(1.0f / m_GunData.fireRate);

        // Reset the shooting cooldown flag to allow shooting again
        m_IsShootingInCooldown = false;
    }

    private IEnumerator ReloadCoroutine()
    {
        // Play the reload sound effect
        AudioSource.PlayClipAtPoint(m_GunData.reloadSound, transform.position);

        // Wait for the reload time to elapse
        yield return new WaitForSeconds(m_GunData.reloadTime);

        // Calculate how much ammo to reload into the magazine
        uint ammoToReload = Math.Min(m_GunData.magazineSize - m_CurrentAmmoInMagazine, m_CurrentTotalAmmo);

        // Update the ammo counts
        m_CurrentAmmoInMagazine += ammoToReload;
        m_CurrentTotalAmmo -= ammoToReload;

        // Invoke the gun data changed event to update any UI or other systems
        m_OnGunDataChanged?.Invoke(m_Score, m_CurrentAmmoInMagazine, m_GunData.magazineSize, m_CurrentTotalAmmo);
    }

    private void SnapToMouse()
    {
        // Get the mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(m_MousePosition);

        // Set the z position to 0 to keep the gun on the same plane
        mousePos.z = 0;

        // Move the gun to the mouse position
        transform.position = mousePos;
    }

    private void Initialize()
    {
        // Set the shooting state to false
        m_IsShooting = false;

        // Initialize ammo counts based on the GunData
        m_CurrentAmmoInMagazine = m_GunData.magazineSize;
        m_CurrentTotalAmmo = m_GunData.ammo;

        // Initialize the score to 0
        m_Score = 0u;

        // Initialize the input actions
        m_InputActions = new ShootingMinigame();

        // Enable the input actions
        m_InputActions.Enable();

        // Subscribe to the mouse position action
        m_InputActions.Shooting.Aim.performed += ctx => m_MousePosition = ctx.ReadValue<Vector2>();

        // Subscribe to the shoot action
        m_InputActions.Shooting.Shoot.performed += ctx => m_IsShooting = true;
        m_InputActions.Shooting.Shoot.canceled += ctx => m_IsShooting = false;

        // Subscribe to the reload action
        m_InputActions.Shooting.Reload.performed += ctx =>
        {
            // Start the reload coroutine
            StartCoroutine(ReloadCoroutine());
        };

        // Get the SpriteRenderer component
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Validate it
        if (spriteRenderer is null)
        {
            Debug.LogError("Gun requires a SpriteRenderer component.");
            return;
        }

        // Validate the GunData
        if (m_GunData is null)
        {
            Debug.LogError("GunData is not assigned.");
            return;
        }

        // Assign the texture from the GunData to the SpriteRenderer
        spriteRenderer.sprite = m_GunData.gunTexture;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        // Invoke the gun data changed event to initialize any UI or other systems with the starting values
        m_OnGunDataChanged?.Invoke(m_Score, m_CurrentAmmoInMagazine, m_GunData.magazineSize, m_CurrentTotalAmmo);
    }

    private void FixedUpdate()
    {
        SnapToMouse();

        // Continuously shoot while the shoot action is active
        if (m_IsShooting && CanShoot() && !m_IsShootingInCooldown)
        {
            StartCoroutine(ShootCoorutine());
        }
    }
}
