using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ProjectileWeapon : WeaponBase
{
    #region Inspector Fields
    [Header("Projectile Settings")]
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private LayerMask projectileLayer;

    private IObjectPool<Projectile> projectilePool;
    private Transform poolParent;
    private int poolDefaultCapacity = 10;
    private int pollMaxSize = 50;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        GameObject poolObject = new GameObject("PlayerProjectilePool");
        poolParent = poolObject.transform;

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
        projectileInstance.transform.SetParent(poolParent);
        projectileInstance.gameObject.layer = Mathf.RoundToInt(Mathf.Log(projectileLayer.value, 2));
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
