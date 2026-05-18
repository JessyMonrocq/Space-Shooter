using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipFOF : MonoBehaviour
{
    public event Action<bool> OnFightModeActivated;

    public enum FOFState
    {
        Fight,
        Flight
    }

    public bool FOFAvailable { get {  return fofAvailable; } set { fofAvailable = value; } }

    private FOFState fofState;

    private float transitionDuration = 1f;
    private bool fofAvailable;
    private bool transitionStarted;

    private void Awake()
    {
        fofState = FOFState.Flight;

        fofAvailable = true;
        transitionStarted = false;
    }

    private void Start()
    {
        InputManager.Instance.SpaceshipFightOrFlight.started += OnSpaceshipFightOrFlightStarted;
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipFightOrFlight.started -= OnSpaceshipFightOrFlightStarted;
    }

    private void OnSpaceshipFightOrFlightStarted(InputAction.CallbackContext context)
    {
        if (fofAvailable && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(TransitionCoroutine());
        }
    }

    private IEnumerator TransitionCoroutine()
    {
        switch (fofState)
        {
            case FOFState.Fight:
                fofState = FOFState.Flight;
                break;
            case FOFState.Flight:
                fofState = FOFState.Fight;
                break;
        }

        Debug.Log("FOF State : " + fofState);

        bool fightMode = fofState == FOFState.Fight;
        OnFightModeActivated?.Invoke(fightMode);
        yield return new WaitForSeconds(transitionDuration);
        transitionStarted = false;
    }
}
