using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected GameObject weaponModel;
    [SerializeField] protected Vector3 weaponActivePosition;
    [SerializeField] protected Vector3 weaponInactivePosition;
    [SerializeField] protected Transform weaponShootingPoint;
    [SerializeField] protected ParticleSystem weaponShootVFX;
    [SerializeField] protected int weaponDamage;
    [SerializeField] protected int maxAmmunitionCount;
    [SerializeField] protected float weaponFireRate;
    [SerializeField] protected float weaponMaxDistance;
    [SerializeField] protected float weaponSpeed;

    [SerializeField] protected float activationDuration;

    public enum WeaponState
    {
        Inactive,
        Active,
        Disabled
    }

    protected WeaponState weaponState;

    protected int currentAmmunicationCount;
    protected float weaponFireRateTimer;
    protected bool canShoot;

    protected void Start()
    {
        weaponState = WeaponState.Inactive;
        transform.localPosition = weaponInactivePosition;

        currentAmmunicationCount = maxAmmunitionCount;
        weaponFireRateTimer = weaponFireRate;
        canShoot = true;
    }

    public virtual void Shoot()
    {
        if (weaponState != WeaponState.Active)
        {
            return;
        }
    }

    public void SetWeaponState(WeaponState state)
    {
        switch (state)
        {
            case WeaponState.Inactive:
                StartCoroutine(WeaponPositionCoroutine(false));
                break;
            case WeaponState.Active:
                StartCoroutine(WeaponPositionCoroutine(true));
                break;
            case WeaponState.Disabled:
                weaponState = state;
                //...
                break;
        }
    }

    private IEnumerator WeaponPositionCoroutine(bool isActive)
    {
        Vector3 finalPos = isActive ? weaponActivePosition : weaponInactivePosition;
        transform.DOKill();
        yield return transform.DOLocalMove(finalPos, activationDuration).SetEase(Ease.Linear).WaitForCompletion();

        canShoot = isActive;
        weaponState = isActive ? WeaponState.Active : WeaponState.Inactive;
    }
}
