using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    #region Inspector Fields
    public static SpaceshipController Instance { get; private set; }

    [Header("Spaceship Scripts References")]
    [SerializeField] private SpaceshipMovement spaceshipMovement;
    [SerializeField] private SpaceshipBoost spaceshipBoost;
    [SerializeField] private SpaceshipIntegrity spaceshipIntegrity;
    [SerializeField] private SpaceshipWeapons spaceshipWeapons;
    [SerializeField] private SpaceshipHUD spaceshipHUD;
    [SerializeField] private SpaceshipVFX spaceshipVFX;
    [SerializeField] private SpaceshipFOF spaceshipFOF;
    [SerializeField] private SpaceshipCargo spaceshipCargo;
    [SerializeField] private SpaceshipMenu spaceshipMenu;
    [SerializeField] private SpaceshipUI spaceshipUI;

    [Header("Others")]
    [SerializeField] private GameObject spaceshipModelParent;
    [SerializeField] private MeshCollider spaceshipMeshCollider;

    [HideInInspector] public SpaceshipCamera SpaceshipCamera { get => spaceshipCamera; set => spaceshipCamera = value; }
    [HideInInspector] public SpaceshipModel SpaceshipReferencePrefab;

    private SpaceshipCamera spaceshipCamera;
    private SpaceshipModel spaceshipModel;
    private SpaceshipStatsSO spaceshipStats;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Update()
    {
        if (spaceshipHUD != null && spaceshipIntegrity != null)
        {
            spaceshipHUD.CurrentShield = spaceshipIntegrity.CurrentShield;
            spaceshipHUD.CurrentHealth = spaceshipIntegrity.CurrentHealth;
        }

        if (spaceshipHUD != null && spaceshipBoost != null)
        {
            spaceshipHUD.CurrentEnergy = spaceshipBoost.CurrentBoostEnergy;
        }

        if (spaceshipModel.UsesWeapons)
        {
            if (spaceshipModel.PrimaryWeapon != null)
            {
                spaceshipHUD.CurrentPrimaryAmmo = spaceshipModel.PrimaryWeapon.WeaponCurrentAmmunition;
            }
            if (spaceshipModel.SecondaryWeapon != null)
            {
                spaceshipHUD.CurrentSecondaryAmmo = spaceshipModel.SecondaryWeapon.WeaponCurrentAmmunition;
            }
        }

        if (spaceshipVFX != null && spaceshipMovement != null)
        {
            spaceshipVFX.CurrentSpeed = spaceshipMovement.CurrentSpeed;
        }

        if (spaceshipModel != null && spaceshipMovement != null)
        {
            spaceshipModel.IsIdle = spaceshipMovement.CurrentSpeed <= 0.1f;
        }
    }
    #endregion

    #region Public Methods
    public void InitializeSpaceship()
    {
        spaceshipStats = SpaceshipReferencePrefab.SpaceshipStats;
        if (spaceshipModelParent.GetComponentInChildren<SpaceshipModel>() != null)
        {
            foreach (Transform child in spaceshipModelParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        spaceshipModel = Instantiate(SpaceshipReferencePrefab, spaceshipModelParent.transform);

        spaceshipMovement.InitializeMovementValues(spaceshipStats);
        spaceshipBoost.InitializeBoostValues(spaceshipStats);
        spaceshipIntegrity.InitializeIntegrityValues(spaceshipStats);
        spaceshipWeapons.InitializeSpaceshipWeapons(spaceshipModel);
        spaceshipHUD.InitializeHUDValues(spaceshipStats, spaceshipModel);
        spaceshipVFX.InitializeVFXValues(spaceshipStats, spaceshipModel);
        spaceshipFOF.InitializeFOFValues(spaceshipModel);
        spaceshipCamera.InitializeCameraValues(spaceshipStats);
        spaceshipCamera.SetCameraTarget(transform);
        spaceshipCargo.InitializeCargoValues(spaceshipStats, spaceshipModel);

        spaceshipMeshCollider.sharedMesh = spaceshipModel.CollisionMesh;

        SubscribeEvents();
    }
    #endregion

    #region Private Methods
    private void SubscribeEvents()
    {
        spaceshipBoost.OnSpaceshipBoost += spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost += spaceshipVFX.HandleStarStreakVFX;
        spaceshipBoost.OnSpaceshipDodge += spaceshipMovement.Dodge;
        spaceshipBoost.OnSpaceshipBoost += spaceshipCamera.SetBoostMode;

        spaceshipIntegrity.OnSpaceshipImpact += spaceshipBoost.InterruptBoost;

        spaceshipFOF.OnFightModeActivated += spaceshipMovement.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipBoost.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipWeapons.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipHUD.DisplayAmmoGroup;

        spaceshipMenu.OnMenu += spaceshipCamera.OnPause;
        spaceshipMenu.OnMenu += OnMenu;
    }

    private void OnMenu(bool state)
    {
        if (state)
        {
            spaceshipUI.OpenInventoryMenu(spaceshipCargo.CargoInventory);
        } else
        {
            spaceshipUI.CloseInventoryMenu();
        }
    }

    private void UnsubscribeEvents()
    {
        spaceshipBoost.OnSpaceshipBoost -= spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipVFX.HandleStarStreakVFX;
        spaceshipBoost.OnSpaceshipDodge -= spaceshipMovement.Dodge;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipCamera.SetBoostMode;

        spaceshipIntegrity.OnSpaceshipImpact -= spaceshipBoost.InterruptBoost;

        spaceshipFOF.OnFightModeActivated -= spaceshipMovement.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipBoost.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipWeapons.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipHUD.DisplayAmmoGroup;

        spaceshipMenu.OnMenu -= spaceshipCamera.OnPause;
    }
    #endregion
}