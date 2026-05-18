using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    #region Inspector Fields
    [Header("Hitscan Settings")]
    [SerializeField] private float weaponCooldownDuration;
    [SerializeField] private float weaponRechargeRate;

    private float weaponCooldownTimer;
    private bool isShooting;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        weaponCooldownTimer = weaponCooldownDuration;
        isShooting = false;
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

            if (currentAmmunicationCount < maxAmmunitionCount && !isShooting)
            {
                weaponCooldownTimer += Time.deltaTime;
                if (weaponCooldownTimer >= weaponCooldownDuration)
                {
                    currentAmmunicationCount += (int)(weaponRechargeRate * Time.deltaTime);
                    currentAmmunicationCount = Mathf.Min(currentAmmunicationCount, maxAmmunitionCount);
                }
            }
        }
    }
    #endregion

    #region Public Methods
    public override void Shoot()
    {
        base.Shoot();

        if (weaponState == WeaponState.Active)
        {
            if (canShoot && currentAmmunicationCount > 0)
            {
                canShoot = false;
                isShooting = true;

                // TODO : Change forward direction by Raycast from shootingPoint to SpaceshipForwarrd at maxDistance !!!
                Physics.Raycast(weaponShootingPoint.position, weaponShootingPoint.forward, out RaycastHit hit, weaponMaxDistance);
                weaponShootVFX.Play();

                currentAmmunicationCount--;

            }
            else
            {
                isShooting = false;
            }
        }
    }
    #endregion
}
