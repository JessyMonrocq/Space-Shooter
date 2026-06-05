using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    #region Inspector Fields
    [Header("Hitscan Settings")]
    [SerializeField] private float weaponCooldownDuration;
    [SerializeField] private float weaponRechargeRate;
    [SerializeField] private Material weaponLaserMaterial;
    [SerializeField] private LayerMask hitLayerMask;

    private float weaponCooldownTimer;
    private float rechargeTimer;
    private float weaponRange;
    private bool isShooting;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        weaponCooldownTimer = weaponCooldownDuration;
        rechargeTimer = 0f;
        weaponRange = weaponSpeed * weaponShootVFX.startLifetime;
        isShooting = false;

        weaponShootVFX.startSpeed = weaponSpeed;
        weaponShootVFX.GetComponent<Renderer>().material = weaponLaserMaterial;
    }

    private void Update()
    {
        if (weaponState == WeaponState.Active)
        {
            if (!canShoot)
            {
                weaponFireRateTimer += Time.deltaTime;
                if (weaponFireRateTimer >= weaponFireRate)
                {
                    canShoot = true;
                    weaponFireRateTimer = 0;
                }
            }
        }

        RechargeWeapon();
    }
    #endregion

    #region Public Methods
    public override void Shoot()
    {
        base.Shoot();

        if (weaponState != WeaponState.Active)
        {
            return;
        }

        if (!canShoot || currentAmmunicationCount <= 0)
        {
            isShooting = false;
            return;
        }

        canShoot = false;
        isShooting = true;
        weaponCooldownTimer = 0f;
        rechargeTimer = 0f;

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(weaponShootingPoint.transform.position, weaponShootingPoint.transform.forward, out hit, weaponRange, hitLayerMask))
        {
            targetPoint = hit.point;
            hit.collider.GetComponent<HealthComponent>()?.TakeDamage(weaponDamage);
        } 
        else
        {
            targetPoint = weaponShootingPoint.transform.position + weaponShootingPoint.transform.forward * weaponRange;
        }

        float distance = Vector3.Distance(weaponShootingPoint.transform.position, targetPoint);
        weaponShootVFX.startLifetime = distance / weaponSpeed;
        weaponShootVFX.Emit(1);

        currentAmmunicationCount--;
    }
    #endregion

    #region Private Methods
    private void RechargeWeapon()
    {
        if (currentAmmunicationCount < maxAmmunitionCount && !isShooting)
        {
            weaponCooldownTimer += Time.deltaTime;
            if (weaponCooldownTimer >= weaponCooldownDuration)
            {
                rechargeTimer += Time.deltaTime;

                while (rechargeTimer >= 1f / weaponRechargeRate)
                {
                    currentAmmunicationCount++;
                    rechargeTimer -= 1f / weaponRechargeRate;
                }
            }
        }
    }
    #endregion
}
