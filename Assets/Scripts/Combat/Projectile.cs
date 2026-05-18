using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    #region Inspector Fields
    [SerializeField] private float projectileLifetime = 10f;

    private int projectileDamage;
    private float lifetimeTimer;

    private IObjectPool<Projectile> projectilePool;

    public IObjectPool<Projectile> ProjectilePool
    {
        set => projectilePool = value;
    }
    #endregion

    #region Unity Methods
    private void Start()
    {
        lifetimeTimer = 0f;
    }

    private void FixedUpdate()
    {
        lifetimeTimer += Time.fixedDeltaTime;
        if (lifetimeTimer >= projectileLifetime)
        {
            Deactivate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Deactivate();
    }
    #endregion

    #region Public Methods
    public int GetDamage()
    {
        return projectileDamage;
    }

    public void SetDamage(int damage)
    {
        projectileDamage = damage;
    }
    #endregion

    #region Pooling Methods
    public void Deactivate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        lifetimeTimer = 0;

        projectilePool.Release(this);
    }
    #endregion
}
