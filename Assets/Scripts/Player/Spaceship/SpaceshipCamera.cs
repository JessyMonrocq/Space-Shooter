using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipCamera : MonoBehaviour
{
    #region Inspector Fields
    [Header("Cinemachine Camera Settings")]
    [SerializeField] private CinemachineCamera spaceshipCamera;
    [SerializeField] private CinemachineThirdPersonFollow followComponent;
    [SerializeField] private CinemachineRotateWithFollowTarget rotateComponent;

    private float cameraDistance = 25f;
    private float cameraVerticalArmLength = 5f;
    private Vector3 cameraPositionDampening = Vector3.zero;
    private float cameraRotationDampening = 0f;

    [Header("Cinemachine Noise Settings")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseComponent;

    private NoiseSettings defaultNoiseSettings;
    private NoiseSettings boostNoiseSettings;
    private float defaultNoiseAmplitude = 0.5f;
    private float defaultNoiseFrequency = 0.5f;
    private float boostNoiseAmplitude = 1.0f;
    private float boostNoiseFrequency = 1.0f;
    #endregion

    #region Unity Methods
    private void Update()
    {
        // Reduce dampening when boosting (and at full speed)
    }
    #endregion

    #region Public Methods
    public void InitializeCameraValues(SpaceshipStatsSO spaceshipStats)
    {
        cameraDistance = spaceshipStats.cameraDistance;
        cameraVerticalArmLength = spaceshipStats.cameraVerticalArmLength;
        cameraPositionDampening = spaceshipStats.cameraPositionDampening;
        cameraRotationDampening = spaceshipStats.cameraRotationDampening;

        defaultNoiseSettings = spaceshipStats.defaultNoiseSettings;
        boostNoiseSettings = spaceshipStats.boostNoiseSettings;
        defaultNoiseAmplitude = spaceshipStats.defaultNoiseAmplitude;
        defaultNoiseFrequency = spaceshipStats.defaultNoiseFrequency;
        boostNoiseAmplitude = spaceshipStats.boostNoiseAmplitude;
        boostNoiseFrequency = spaceshipStats.boostNoiseFrequency;

        followComponent.CameraDistance = cameraDistance;
        followComponent.VerticalArmLength = cameraVerticalArmLength;
        followComponent.Damping = cameraPositionDampening;

        rotateComponent.Damping = cameraRotationDampening;

        noiseComponent.NoiseProfile = defaultNoiseSettings;
        noiseComponent.AmplitudeGain = defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = defaultNoiseFrequency;
    }

    public void SetBoostMode(bool isBoosting)
    {
        noiseComponent.NoiseProfile = isBoosting ? boostNoiseSettings : defaultNoiseSettings;
        noiseComponent.AmplitudeGain = isBoosting ? boostNoiseAmplitude : defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = isBoosting ? boostNoiseFrequency : defaultNoiseFrequency;
    }
    #endregion
}
