using UnityEngine;
using UnityEngine.Pool;

public class ProjectileWeapon : WeaponBase
{
    #region Inspector Fields
    [Header("Projectile Settings")]
    [SerializeField] private Projectile projectilePrefab;

    private IObjectPool<Projectile> projectilePool;
    private int poolDefaultCapacity = 20;
    private int pollMaxSize = 50;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseFromPool, OnDestroyPooledObject, false, poolDefaultCapacity, pollMaxSize);
    }

    private void Update()
    {
        if (weaponState == WeaponState.Active && !canShoot)
        {
            weaponFireRateTimer += Time.deltaTime;
            if (weaponFireRateTimer >= weaponFireRate)
            {
                canShoot = true;
                weaponFireRateTimer = 0;
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

                Projectile projectile = projectilePool.Get();
                projectile.transform.SetPositionAndRotation(weaponShootingPoint.position, weaponShootingPoint.rotation);
                // TODO : Change forward direction by Raycast from shootingPoint to SpaceshipForwarrd at maxDistance !!!
                projectile.GetComponent<Rigidbody>().linearVelocity = weaponShootingPoint.forward * weaponSpeed;
                projectile.SetDamage(weaponDamage);

                weaponShootVFX.Play();

                currentAmmunicationCount--;
            }
        }
    }
    #endregion

    #region Pooling Methods
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.ProjectilePool = projectilePool;
        return projectileInstance;
    }

    private void OnGetFromPool(Projectile pooledProjectile)
    {
        pooledProjectile.gameObject.SetActive(true);
    }

    private void OnReleaseFromPool(Projectile pooledProjectile)
    {
        pooledProjectile.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Projectile pooledProjectile)
    {
        Destroy(pooledProjectile.gameObject);
    }
    #endregion
}
