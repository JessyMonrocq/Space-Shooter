using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    [SerializeField] private SpaceshipStatsSO spaceshipStatisticsSO;
    [SerializeField] private SpaceshipMovement spaceshipMovement;
    [SerializeField] private SpaceshipBoost spaceshipBoost;
    [SerializeField] private SpaceshipCamera spaceshipCamera;

    private void Awake()
    {
        spaceshipBoost.InitializeBoost += spaceshipMovement.SetBoostValues;
        spaceshipBoost.InitializeDodge += spaceshipMovement.SetDodgeValues;

        spaceshipBoost.OnSpaceshipBoost += spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost += spaceshipCamera.SetBoostCameraSettings;
        spaceshipBoost.OnSpaceshipDodge += spaceshipMovement.Dodge;
    }

    private void OnDestroy()
    {
        spaceshipBoost.InitializeBoost -= spaceshipMovement.SetBoostValues;
        spaceshipBoost.InitializeDodge -= spaceshipMovement.SetDodgeValues;

        spaceshipBoost.OnSpaceshipBoost -= spaceshipMovement.SetBoostMode;
        spaceshipBoost.OnSpaceshipBoost -= spaceshipCamera.SetBoostCameraSettings;
        spaceshipBoost.OnSpaceshipDodge -= spaceshipMovement.Dodge;
    }
}