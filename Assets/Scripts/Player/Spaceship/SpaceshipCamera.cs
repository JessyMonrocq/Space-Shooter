using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipCamera : MonoBehaviour
{
    #region Inspector Fields
    [Header("Cinemachine Camera Settings")]
    [SerializeField] private CinemachineCamera spaceshipCamera;
    [SerializeField] private CinemachineBasicMultiChannelPerlin noiseComponent;

    private Transform followTarget;
    private Vector3 cameraPositionOffset;
    private float cameraPositionDampTime = 0.1f;
    private float cameraRotationDampSmoothness = 8f;

    private NoiseSettings defaultNoiseSettings;
    private NoiseSettings boostNoiseSettings;
    private float defaultNoiseAmplitude = 0.5f;
    private float defaultNoiseFrequency = 0.5f;
    private float boostNoiseAmplitude = 1.0f;
    private float boostNoiseFrequency = 1.0f;

    private bool isReady;

    private Vector3 currentVelocity;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        isReady = false;
    }

    private void LateUpdate()
    {
        if (!isReady)
        {
            return;
        }

        Vector3 desiredPosition = followTarget.position + followTarget.TransformDirection(cameraPositionOffset);
        spaceshipCamera.transform.position = Vector3.SmoothDamp(spaceshipCamera.transform.position, desiredPosition, ref currentVelocity, cameraPositionDampTime);

        Quaternion desiredRotation = followTarget.rotation;
        spaceshipCamera.transform.rotation = Quaternion.Slerp(spaceshipCamera.transform.rotation, desiredRotation, Time.deltaTime * cameraRotationDampSmoothness);
    }
    #endregion

    #region Public Methods
    public void InitializeCameraValues(SpaceshipStatsSO spaceshipStats)
    {
        cameraPositionOffset = spaceshipStats.cameraPositionOffset;
        cameraPositionDampTime = spaceshipStats.cameraPositionDampTime;
        cameraRotationDampSmoothness = spaceshipStats.cameraRotationDampSmoothness;

        defaultNoiseSettings = spaceshipStats.defaultNoiseSettings;
        boostNoiseSettings = spaceshipStats.boostNoiseSettings;
        defaultNoiseAmplitude = spaceshipStats.defaultNoiseAmplitude;
        defaultNoiseFrequency = spaceshipStats.defaultNoiseFrequency;
        boostNoiseAmplitude = spaceshipStats.boostNoiseAmplitude;
        boostNoiseFrequency = spaceshipStats.boostNoiseFrequency;

        noiseComponent.NoiseProfile = defaultNoiseSettings;
        noiseComponent.AmplitudeGain = defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = defaultNoiseFrequency;
    }

    public void SetCameraTarget(Transform target)
    {
        followTarget = target;
        spaceshipCamera.transform.position = followTarget.position + followTarget.TransformDirection(cameraPositionOffset);
        spaceshipCamera.transform.rotation = followTarget.rotation;
        isReady = true;
    }

    public void SetBoostMode(bool isBoosting)
    {
        noiseComponent.NoiseProfile = isBoosting ? boostNoiseSettings : defaultNoiseSettings;
        noiseComponent.AmplitudeGain = isBoosting ? boostNoiseAmplitude : defaultNoiseAmplitude;
        noiseComponent.FrequencyGain = isBoosting ? boostNoiseFrequency : defaultNoiseFrequency;
    }
    #endregion
}
