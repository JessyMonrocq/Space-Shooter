using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    #region Inspector Fields
    [Header("Spaceship Scripts References")]
    [SerializeField] private SpaceshipMovement spaceshipMovement;
    [SerializeField] private SpaceshipBoost spaceshipBoost;
    [SerializeField] private SpaceshipCamera spaceshipCamera;
    [SerializeField] private SpaceshipIntegrity spaceshipIntegrity;
    [SerializeField] private SpaceshipHUD spaceshipHUD;
    [SerializeField] private SpaceshipVFX spaceshipVFX;

    [Header("Others")]
    [SerializeField] private GameObject spaceshipModelParent;

    public SpaceshipReference SpaceshipReferencePrefab;

    private SpaceshipReference spaceshipReference;
    private SpaceshipStatsSO spaceshipStats;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        spaceshipBoost.OnSpaceshipBoost += spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost += spaceshipCamera.SetBoostMode;
        spaceshipBoost.OnSpaceshipDodge += spaceshipMovement.Dodge;
    }

    private void OnDestroy()
    {
        spaceshipBoost.OnSpaceshipBoost -= spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipCamera.SetBoostMode;
        spaceshipBoost.OnSpaceshipDodge -= spaceshipMovement.Dodge;
    }

    private void Update()
    {
        spaceshipHUD.CurrentShield = spaceshipIntegrity.CurrentShield;
        spaceshipHUD.CurrentHealth = spaceshipIntegrity.CurrentHealth;
        spaceshipHUD.CurrentEnergy = spaceshipBoost.CurrentBoostEnergy;

        spaceshipVFX.CurrentSpeed = spaceshipMovement.CurrentSpeed;
    }
    #endregion

    #region Public Methods
    public void InitializeSpaceship()
    {
        spaceshipStats = SpaceshipReferencePrefab.SpaceshipStats;
        if (spaceshipModelParent.GetComponentInChildren<SpaceshipReference>() != null)
        {
            foreach (Transform child in spaceshipModelParent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        spaceshipReference = Instantiate(SpaceshipReferencePrefab, spaceshipModelParent.transform);

        spaceshipMovement.InitializeMovementValues(spaceshipStats);
        spaceshipBoost.InitializeBoostValues(spaceshipStats);
        spaceshipCamera.InitializeCameraValues(spaceshipStats);
        spaceshipHUD.InitializeHUDValues(spaceshipStats);
        spaceshipVFX.InitializeVFXValues(spaceshipStats);
        spaceshipVFX.InitializeBoostParticles(spaceshipReference.ThrusterParticleSystem);
    }
    #endregion
}