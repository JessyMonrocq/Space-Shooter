using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipMenu : MonoBehaviour
{
    public event Action<bool> OnMenu;

    private bool isPaused;
    private bool isPausing;
    private bool isSelectMenu;
    private bool isStartMenu;

    private void Start()
    {
        InputManager.Instance.SelectMenu.performed += SetSpaceshipMenu;

        isPaused = false;
        isPausing = false;
        isStartMenu = false;
        isSelectMenu = false;
    }

    private void SetSpaceshipMenu(InputAction.CallbackContext context)
    {
        if (isPausing)
        {
            return;
        }

        if (!isPaused)
        {
            isPaused = true;
            isPausing = true;
            isSelectMenu = true;

            InputManager.Instance.SetSpaceshipInputState(false);
            PauseGame.Instance.OnPause += DisplayMenu;
            PauseGame.Instance.PauseCurrentGame(true);
        }
        else if (isPaused && isSelectMenu)
        {
            isPaused = false;
            isPausing = true;
            isSelectMenu = false;

            InputManager.Instance.SetSpaceshipInputState(true);
            PauseGame.Instance.OnResume += HideMenu;
            PauseGame.Instance.PauseCurrentGame(false);
        }
    }

    private void DisplayMenu()
    {
        PauseGame.Instance.OnPause -= DisplayMenu;
        Cursor.lockState = CursorLockMode.None;
        OnMenu?.Invoke(true);
        isPausing = false;
    }

    private void HideMenu()
    {
        PauseGame.Instance.OnPause -= DisplayMenu;
        Cursor.lockState = CursorLockMode.Locked;
        OnMenu?.Invoke(false);
        isPausing = false;
    }
}
