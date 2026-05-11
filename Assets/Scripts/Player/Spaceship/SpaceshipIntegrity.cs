using System;
using UnityEngine;

public class SpaceshipIntegrity : HealthComponent
{
    [Header("Damage Settings")]
    [SerializeField] private float collisionDamageToShieldMultiplier = 0.33f;
    [SerializeField] private float collisionDamageToHullMultiplier = 0.5f;
    [SerializeField] private float collisionCooldownDuration = 1f;

    private Rigidbody spaceshipRB;

    private float collisionCooldownTimer;
    private bool collisionCooldownDone;

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

    private void HandleCollision(int collisionSpeed)
    {
        if (collisionCooldownDone)
        {
            Debug.Log("Speed : " + collisionSpeed);
            if (CurrentShield > 0)
            {
                Debug.Log("Damage : " + (int)(collisionSpeed * collisionDamageToShieldMultiplier));
                TakeDamage((int)(collisionSpeed * collisionDamageToShieldMultiplier));
            }
            else
            {
                TakeDamage((int)(collisionSpeed * collisionDamageToHullMultiplier));
            }
            collisionCooldownDone = false;
        }
    }
}
