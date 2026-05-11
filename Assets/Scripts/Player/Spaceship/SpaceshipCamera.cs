using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipCamera : MonoBehaviour
{
    #region Inspector Fields
    [Header("Cinemachine Camera Settings")]
    [SerializeField] private CinemachineCamera spaceshipCamera;
    [SerializeField] private CinemachineThirdPersonFollow followComponent;
    [SerializeField] private CinemachineRotateWithFollowTarget rotateComponent;

    [Header("Cinemachine Noise Settings")]
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseComponent;
    [SerializeField] private NoiseSettings defaultNoiseSettings;
    [SerializeField] private NoiseSettings boostNoiseSettings;
    [SerializeField] private float defaultNoiseAmplitude = 0.5f;
    [SerializeField] private float defaultNoiseFrequency = 0.5f;
    [SerializeField] private float boostNoiseAmplitude = 1.0f;
    [SerializeField] private float boostNoiseFrequency = 1.0f;

    [Header("Camera Settings")]
    [SerializeField] private float cameraDistance = 25f;
    [SerializeField] private float cameraVerticalArmLength = 5f;
    [SerializeField] private Vector3 cameraPositionDampening = Vector3.zero;
    [SerializeField] private float cameraRotationDampening = 0f;
    #endregion

    #region Unity Methods
    private void Start()
    {
        followComponent.CameraDistance = cameraDistance;
        followComponent.VerticalArmLength = cameraVerticalArmLength;
        followComponent.Damping = cameraPositionDampening;

        rotateComponent.Damping = cameraRotationDampening;

        noiseComponent.NoiseProfile = defaultNoiseSettings;
        noiseComponent.AmplitudeGain = defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = defaultNoiseFrequency;
    }

    private void Update()
    {
        // Reduce dampening when boosting (and at full speed)
    }
    #endregion

    #region Public Methods
    public void SetBoostCameraSettings(bool isBoosting)
    {
        noiseComponent.NoiseProfile = isBoosting ? boostNoiseSettings : defaultNoiseSettings;
        noiseComponent.AmplitudeGain = isBoosting ? boostNoiseAmplitude : defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = isBoosting ? boostNoiseFrequency : defaultNoiseFrequency;
    }
    #endregion
}
