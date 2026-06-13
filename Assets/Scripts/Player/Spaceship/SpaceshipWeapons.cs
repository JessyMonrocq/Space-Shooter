using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipWeapons : MonoBehaviour
{
    #region Inspector Fields
    private WeaponBase primaryWeapon;
    private WeaponBase secondaryWeapon;

    private bool primaryWeaponPerformed;
    private bool secondaryWeaponPerformed;

    private bool fightMode;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        primaryWeaponPerformed = false;
        secondaryWeaponPerformed = false;

        fightMode = false;
    }

    private void Start()
    {
        InputManager.Instance.SpaceshipPrimaryWeapon.performed += OnPrimaryWeaponPerformed;
        InputManager.Instance.SpaceshipPrimaryWeapon.canceled += OnPrimaryWeaponCanceled;
        InputManager.Instance.SpaceshipSecondaryWeapon.performed += OnSecondaryWeaponPerformed;
        InputManager.Instance.SpaceshipPrimaryWeapon.canceled += OnSecondaryWeaponCanceled;
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipPrimaryWeapon.performed -= OnPrimaryWeaponPerformed;
        InputManager.Instance.SpaceshipPrimaryWeapon.canceled -= OnPrimaryWeaponCanceled;
        InputManager.Instance.SpaceshipSecondaryWeapon.performed -= OnSecondaryWeaponPerformed;
        InputManager.Instance.SpaceshipPrimaryWeapon.canceled -= OnSecondaryWeaponCanceled;
    }

    private void Update()
    {
        if (primaryWeaponPerformed)
        {
            primaryWeapon.Shoot();
        }

        if (secondaryWeaponPerformed)
        {
            secondaryWeapon.Shoot();
        }
    }
    #endregion

    #region Input Callbacks
    private void OnPrimaryWeaponPerformed(InputAction.CallbackContext context)
    {
        if (primaryWeapon == null || !fightMode)
        {
            return;
        }

        if (primaryWeapon.GetComponent<HitscanWeapon>())
        {
            primaryWeaponPerformed = true;
        }
        else if (primaryWeapon.GetComponent<ProjectileWeapon>())
        {
            primaryWeapon.Shoot();
        }
    }

    private void OnPrimaryWeaponCanceled(InputAction.CallbackContext context)
    {
        if (primaryWeapon == null || !fightMode)
        {
            return;
        }

        if (primaryWeapon.GetComponent<HitscanWeapon>())
        {
            primaryWeaponPerformed = false;
        }
    }

    private void OnSecondaryWeaponPerformed(InputAction.CallbackContext context)
    {
        if (secondaryWeapon == null || !fightMode)
        {
            return;
        }

        if (secondaryWeapon.GetComponent<HitscanWeapon>())
        {
            secondaryWeaponPerformed = true;
        }
        else if (secondaryWeapon.GetComponent<ProjectileWeapon>())
        {
            secondaryWeapon.Shoot();
        }
    }

    private void OnSecondaryWeaponCanceled(InputAction.CallbackContext context)
    {
        if (secondaryWeapon == null || !fightMode)
        {
            return;
        }

        if (secondaryWeapon.GetComponent<HitscanWeapon>())
        {
            secondaryWeaponPerformed = false;
        }
    }
    #endregion

    #region Public Methods
    public void InitializeSpaceshipWeapons(SpaceshipModel spaceshipModel)
    {
        if (!spaceshipModel.UsesWeapons)
        {
            return;
        }

        primaryWeapon = spaceshipModel.PrimaryWeapon;
        secondaryWeapon = spaceshipModel.SecondaryWeapon;

        primaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);
        secondaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);
    }

    public void SetFightMode(bool fightMode)
    {
        this.fightMode = fightMode;
        SetWeaponsState(fightMode);
    }
    #endregion

    #region Private Methods
    private void SetWeaponsState(bool state)
    {
        if (state)
        {
            primaryWeapon.SetWeaponState(WeaponBase.WeaponState.Active);
            secondaryWeapon.SetWeaponState(WeaponBase.WeaponState.Active);
        }
        else
        {
            primaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);
            secondaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);

            primaryWeaponPerformed = false;
            secondaryWeaponPerformed = false;
        }
    }
    #endregion
}
