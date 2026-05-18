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
    [SerializeField] private GameObject spaceshipReferenceParent;

    [HideInInspector]
    public SpaceshipCamera SpaceshipCamera { get => spaceshipCamera; set => spaceshipCamera = value; }
    [HideInInspector]
    public SpaceshipModel SpaceshipReferencePrefab;

    private SpaceshipCamera spaceshipCamera;
    private SpaceshipModel spaceshipReference;
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
        spaceshipFOF.OnFightModeActivated += spaceshipWeapons.SetWeaponsState;
    }

    private void OnDestroy()
    {
        spaceshipBoost.OnSpaceshipBoost -= spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipCamera.SetBoostMode;
        spaceshipBoost.OnSpaceshipDodge -= spaceshipMovement.Dodge;

        spaceshipIntegrity.OnSpaceshipImpact -= spaceshipBoost.InterruptBoost;

        spaceshipFOF.OnFightModeActivated -= spaceshipMovement.SetFightMode;
        spaceshipFOF.OnFightModeActivated -= spaceshipWeapons.SetWeaponsState;
    }

    private void Update()
    {
        spaceshipHUD.CurrentShield = spaceshipIntegrity.CurrentShield;
        spaceshipHUD.CurrentHealth = spaceshipIntegrity.CurrentHealth;
        spaceshipHUD.CurrentEnergy = spaceshipBoost.CurrentBoostEnergy;

        spaceshipVFX.CurrentSpeed = spaceshipMovement.CurrentSpeed;

        spaceshipReference.IsIdle = spaceshipMovement.CurrentSpeed <= 0.1f;
    }
    #endregion

    #region Public Methods
    public void InitializeSpaceship()
    {
        spaceshipStats = SpaceshipReferencePrefab.SpaceshipStats;
        if (spaceshipReferenceParent.GetComponentInChildren<SpaceshipModel>() != null)
        {
            foreach (Transform child in spaceshipReferenceParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        spaceshipReference = Instantiate(SpaceshipReferencePrefab, spaceshipReferenceParent.transform);

        spaceshipMovement.InitializeMovementValues(spaceshipStats);
        spaceshipBoost.InitializeBoostValues(spaceshipStats);
        spaceshipIntegrity.InitializeIntegrityValues(spaceshipStats);
        spaceshipWeapons.InitializeSpaceshipWeapons(spaceshipReference);
        spaceshipHUD.InitializeHUDValues(spaceshipStats);
        spaceshipVFX.InitializeVFXValues(spaceshipStats);
        spaceshipVFX.InitializeBoostParticles(spaceshipReference.ThrusterParticleSystem);

        spaceshipCamera.InitializeCameraValues(spaceshipStats);
        spaceshipCamera.SetCameraTarget(transform);

        spaceshipBoost.OnSpaceshipBoost += spaceshipCamera.SetBoostMode;
    }
    #endregion
}