using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipFOF : MonoBehaviour
{
    #region Inspector Fields
    public event Action<bool> OnFightModeActivated;
    public bool FOFAvailable { get {  return fofAvailable; } set { fofAvailable = value; } }

    private float transitionDuration = 1f;
    private bool fightMode;
    private bool fofAvailable;
    private bool transitionStarted;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        fofAvailable = true;
        fightMode = false;
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
    #endregion

    #region Private Methods
    private void OnSpaceshipFightOrFlightStarted(InputAction.CallbackContext context)
    {
        if (fofAvailable && !transitionStarted)
        {
            transitionStarted = true;
            StartCoroutine(TransitionCoroutine());
        }
    }
    #endregion

    #region Coroutines Methods
    private IEnumerator TransitionCoroutine()
    {
        fightMode = !fightMode;
        OnFightModeActivated?.Invoke(fightMode);
        
        yield return new WaitForSeconds(transitionDuration);
        transitionStarted = false;
    }
    #endregion
}
