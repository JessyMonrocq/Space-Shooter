using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Inspector Fields
    public static InputManager Instance { get; private set; }

    [Header("Spaceship Inputs")]
    [SerializeField] private InputActionReference spaceshipForwardMoveAction;
    [SerializeField] private InputActionReference spaceshipHorizontalMoveAction;
    [SerializeField] private InputActionReference spaceshipVerticalMoveAction;
    [SerializeField] private InputActionReference spaceshipPitchAction;
    [SerializeField] private InputActionReference spaceshipYawAction;
    [SerializeField] private InputActionReference spaceshipRollAction;
    [SerializeField] private InputActionReference spaceshipBoostAction;
    [SerializeField] private InputActionReference spaceshipDodgeAction;
    [SerializeField] private InputActionReference spaceshipPrimaryWeapon;
    [SerializeField] private InputActionReference spaceshipSecondaryWeapon;
    [SerializeField] private InputActionReference spaceshipFightOrFlight;

    public InputAction SpaceshipForwardMove => spaceshipForwardMoveAction.action;
    public InputAction SpaceshipHorizontalMove => spaceshipHorizontalMoveAction.action;
    public InputAction SpaceshipVerticalMove => spaceshipVerticalMoveAction.action;
    public InputAction SpaceshipPitch => spaceshipPitchAction.action;
    public InputAction SpaceshipYaw => spaceshipYawAction.action;
    public InputAction SpaceshipRoll => spaceshipRollAction.action;
    public InputAction SpaceshipBoost => spaceshipBoostAction.action;
    public InputAction SpaceshipDodge => spaceshipDodgeAction.action;
    public InputAction SpaceshipPrimaryWeapon => spaceshipPrimaryWeapon.action;
    public InputAction SpaceshipSecondaryWeapon => spaceshipSecondaryWeapon.action;
    public InputAction SpaceshipFightOrFlight => spaceshipFightOrFlight.action;
    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSpaceshipInputState(bool state)
    {
        SetInputState(SpaceshipForwardMove, state);
        SetInputState(SpaceshipHorizontalMove, state);
        SetInputState(SpaceshipVerticalMove, state);
        SetInputState(SpaceshipPitch, state);
        SetInputState(SpaceshipYaw, state);
        SetInputState(SpaceshipRoll, state);
        SetInputState(SpaceshipBoost, state);
        SetInputState(SpaceshipDodge, state);
        SetInputState(SpaceshipPrimaryWeapon, state);
        SetInputState(SpaceshipSecondaryWeapon, state);
        SetInputState(SpaceshipFightOrFlight, state);
    }


    private void SetInputState(InputAction action, bool enabled)
    {
        if (enabled)
        {
            action.Enable();
        }
        else
        {
            action.Disable();
        }
    }
}