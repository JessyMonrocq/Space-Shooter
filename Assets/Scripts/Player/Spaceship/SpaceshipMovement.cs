using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    #region Inspector Fields
    private Rigidbody spaceshipRigidbody;

    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 100f;
    [SerializeField] private float translationSpeed = 5f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private float rollSpeed = 0.1f;

    private float forwardMoveInput;
    private float horizontalMoveInput;
    private float verticalMoveInput;
    private float pitchInput;
    private float yawInput;
    private float rollInput;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        spaceshipRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        
    }
    #endregion

    #region Input Events
    
    #endregion

    #region Update Method
    private void Update()
    {
        forwardMoveInput = InputManager.Instance.SpaceshipForwardMove.ReadValue<float>();
        horizontalMoveInput = InputManager.Instance.SpaceshipHorizontalMove.ReadValue<float>();
        verticalMoveInput = InputManager.Instance.SpaceshipVerticalMove.ReadValue<float>();
        pitchInput = InputManager.Instance.SpaceshipPitch.ReadValue<float>();
        yawInput = InputManager.Instance.SpaceshipYaw.ReadValue<float>();
        rollInput = InputManager.Instance.SpaceshipRoll.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.forward) * forwardMoveInput * forwardSpeed, ForceMode.VelocityChange);
        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.right) * horizontalMoveInput * translationSpeed, ForceMode.VelocityChange);
        spaceshipRigidbody.AddForce(spaceshipRigidbody.transform.TransformDirection(Vector3.up) * verticalMoveInput * translationSpeed, ForceMode.VelocityChange);

        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.right * rotationSpeed * pitchInput, ForceMode.VelocityChange);
        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.up * rotationSpeed * yawInput, ForceMode.VelocityChange);

        spaceshipRigidbody.AddTorque(spaceshipRigidbody.transform.forward * rollSpeed * rollInput, ForceMode.VelocityChange);
    }
    #endregion
}
