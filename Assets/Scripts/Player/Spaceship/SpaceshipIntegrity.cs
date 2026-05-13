using System;
using UnityEngine;

public class SpaceshipIntegrity : HealthComponent
{
    #region Inspector Fields
    private float collisionDamageToShieldMultiplier = 0.33f;
    private float collisionDamageToHullMultiplier = 0.5f;
    private float collisionCooldownDuration = 1f;

    private Rigidbody spaceshipRB;

    private float collisionCooldownTimer;
    private bool collisionCooldownDone;
    #endregion

    #region Unity Methods
    protected override void Start()
    {
        base.Start();

        spaceshipRB = GetComponent<Rigidbody>();

        collisionCooldownTimer = 0f;
        collisionCooldownDone = true;
    }

    private void Update()
    {
        if (collisionCooldownDone)
        {
            return;
        }

        collisionCooldownTimer += Time.deltaTime;
        if (collisionCooldownTimer >= collisionCooldownDuration)
        {
            collisionCooldownDone = true;
            collisionCooldownTimer = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO : Detect if collision is from a Projectile or Not
        // If projectile, directly apply TakeDamage();

        int impactSpeed = 0;
        if (collision.rigidbody == null)
        {
            impactSpeed = (int)Mathf.Abs(Vector3.Dot(transform.forward, spaceshipRB.linearVelocity));
        }
        else
        {
            Vector3 relativeVelocity = spaceshipRB.linearVelocity - collision.rigidbody.linearVelocity;
            impactSpeed = (int)relativeVelocity.magnitude;
        }

        HandleCollision(impactSpeed);
    }
    #endregion

    #region Public Methods
    public void InitializeIntegrityValues(SpaceshipStatsSO spaceshipStats)
    {
        MaxHealth = spaceshipStats.shieldMaxCapacity;
        MaxShield = spaceshipStats.hullMaxHP;
        CurrentHealth = MaxHealth;
        CurrentShield = MaxShield;
    }
    #endregion

    #region Private Methods
    private void HandleCollision(int collisionSpeed)
    {
        if (collisionCooldownDone)
        {
            if (CurrentShield > 0)
            {
                TakeDamage((int)(collisionSpeed * collisionDamageToShieldMultiplier));
            }
            else
            {
                TakeDamage((int)(collisionSpeed * collisionDamageToHullMultiplier));
            }
            collisionCooldownDone = false;
        }
    }
    #endregion
}
