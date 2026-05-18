using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipWeapons : MonoBehaviour
{
    #region Inspector Fields
    private WeaponBase primaryWeapon;
    private WeaponBase secondaryWeapon;
    #endregion

    #region Unity Methods
    private void Start()
    {
        InputManager.Instance.SpaceshipPrimaryWeapon.started += OnPrimaryWeaponStarted;
        InputManager.Instance.SpaceshipPrimaryWeapon.performed += OnPrimaryWeaponPerformed;
        InputManager.Instance.SpaceshipSecondaryWeapon.started += OnSecondaryWeaponStarted;
        InputManager.Instance.SpaceshipSecondaryWeapon.performed += OnSecondaryWeaponPerformed;
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipPrimaryWeapon.started -= OnPrimaryWeaponStarted;
        InputManager.Instance.SpaceshipPrimaryWeapon.performed -= OnPrimaryWeaponPerformed;
        InputManager.Instance.SpaceshipSecondaryWeapon.started -= OnSecondaryWeaponStarted;
        InputManager.Instance.SpaceshipSecondaryWeapon.performed -= OnSecondaryWeaponPerformed;
    }
    #endregion

    #region Input Callbacks
    private void OnPrimaryWeaponStarted(InputAction.CallbackContext context)
    {
        if (primaryWeapon != null && primaryWeapon.GetComponent<ProjectileWeapon>())
        {
            primaryWeapon.Shoot();
        }
    }

    private void OnPrimaryWeaponPerformed(InputAction.CallbackContext context)
    {
        if (primaryWeapon != null && primaryWeapon.GetComponent<HitscanWeapon>())
        {
            primaryWeapon.Shoot();
        }
    }

    private void OnSecondaryWeaponStarted(InputAction.CallbackContext context)
    {
        if (secondaryWeapon != null && secondaryWeapon.GetComponent<ProjectileWeapon>())
        {
            secondaryWeapon.Shoot();
        }
    }

    private void OnSecondaryWeaponPerformed(InputAction.CallbackContext context)
    {
        if (secondaryWeapon != null && secondaryWeapon.GetComponent<HitscanWeapon>())
        {
            secondaryWeapon.Shoot();
        }
    }
    #endregion

    #region Public Methods
    public void InitializeSpaceshipWeapons(SpaceshipModel spaceshipReference) 
    {
        primaryWeapon = spaceshipReference.PrimaryWeapon;
        secondaryWeapon = spaceshipReference.SecondaryWeapon;

        primaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);
        secondaryWeapon.SetWeaponState(WeaponBase.WeaponState.Inactive);
    }
    #endregion
}
