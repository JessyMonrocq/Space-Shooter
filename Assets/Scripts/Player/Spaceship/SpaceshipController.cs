using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    #region Inspector Fields
    [Header("Spaceship Scripts References")]
    [SerializeField] private SpaceshipMovement spaceshipMovement;
    [SerializeField] private SpaceshipBoost spaceshipBoost;
    [SerializeField] private SpaceshipIntegrity spaceshipIntegrity;
    [SerializeField] private SpaceshipWeapons spaceshipWeapons;
    [SerializeField] private SpaceshipHUD spaceshipHUD;
    [SerializeField] private SpaceshipVFX spaceshipVFX;
    [SerializeField] private SpaceshipFOF spaceshipFOF;

    [Header("Others")]
    [SerializeField] private GameObject spaceshipModelParent;

    [HideInInspector]
    public SpaceshipCamera SpaceshipCamera { get => spaceshipCamera; set => spaceshipCamera = value; }
    [HideInInspector]
    public SpaceshipModel SpaceshipReferencePrefab;

    private SpaceshipCamera spaceshipCamera;
    private SpaceshipModel spaceshipModel;
    private SpaceshipStatsSO spaceshipStats;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        spaceshipBoost.OnSpaceshipBoost += spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipDodge += spaceshipMovement.Dodge;

        spaceshipIntegrity.OnSpaceshipImpact += spaceshipBoost.InterruptBoost;

        spaceshipFOF.OnFightModeActivated += spaceshipMovement.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipBoost.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipWeapons.SetFightMode;
        spaceshipFOF.OnFightModeActivated += spaceshipHUD.DisplayAmmoGroup;
    }

    private void OnDestroy()
    {
        spaceshipBoost.OnSpaceshipBoost -= spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipCamera.SetBoostMode;
        spaceshipBoost.OnSpaceshipDodge -= spaceshipMovement.Dodge;

        spaceshipIntegrity.OnSpaceshipImpact -= spaceshipBoost.InterruptBoost;

        spaceshipFOF.OnFightModeActivated -= spaceshipMovement.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipBoost.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipWeapons.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipHUD.DisplayAmmoGroup;
    }

    private void Update()
    {
        spaceshipHUD.CurrentShield = spaceshipIntegrity.CurrentShield;
        spaceshipHUD.CurrentHealth = spaceshipIntegrity.CurrentHealth;
        spaceshipHUD.CurrentEnergy = spaceshipBoost.CurrentBoostEnergy;
        spaceshipHUD.CurrentPrimaryAmmo = spaceshipModel.PrimaryWeapon.WeaponCurrentAmmunition;
        spaceshipHUD.CurrentSecondaryAmmo = spaceshipModel.SecondaryWeapon.WeaponCurrentAmmunition;

        spaceshipVFX.CurrentSpeed = spaceshipMovement.CurrentSpeed;

        spaceshipModel.IsIdle = spaceshipMovement.CurrentSpeed <= 0.1f;
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
        spaceshipVFX.InitializeVFXValues(spaceshipStats);
        spaceshipVFX.InitializeBoostParticles(spaceshipModel.ThrusterParticleSystem);

        spaceshipCamera.InitializeCameraValues(spaceshipStats);
        spaceshipCamera.SetCameraTarget(transform);

        spaceshipBoost.OnSpaceshipBoost += spaceshipCamera.SetBoostMode;
    }
    #endregion
}