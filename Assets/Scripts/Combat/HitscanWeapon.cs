using System.Threading;
using UnityEngine;

public class HitscanWeapon : WeaponBase
{
    #region Inspector Fields
    [Header("Hitscan Settings")]
    [SerializeField] private float weaponCooldownDuration;
    [SerializeField] private float weaponRechargeRate;

    private float weaponCooldownTimer;
    private float rechargeTimer;
    private bool isShooting;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        weaponCooldownTimer = weaponCooldownDuration;
        rechargeTimer = 0f;
        isShooting = false;

        weaponShootVFX.startSpeed = weaponSpeed;
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

        if (weaponState == WeaponState.Active)
        {
            if (canShoot && currentAmmunicationCount > 0)
            {
                canShoot = false;
                isShooting = true;
                weaponCooldownTimer = 0f;
                rechargeTimer = 0f;

                // TODO : Change forward direction by Raycast from shootingPoint to SpaceshipForwarrd at maxDistance !!!
                Physics.Raycast(weaponShootingPoint.position, weaponShootingPoint.forward, out RaycastHit hit, weaponMaxDistance);
                weaponShootVFX.Emit(1);

                currentAmmunicationCount--;
            }
            else
            {
                isShooting = false;
            }
        }
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
