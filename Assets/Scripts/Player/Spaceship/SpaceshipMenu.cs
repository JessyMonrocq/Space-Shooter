using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipMenu : MonoBehaviour
{
    public event Action<bool> OnMenu;

    private enum Menu
    {
        Start,
        Select
    }

    private Menu currentMenu;

    private void Start()
    {
        InputManager.Instance.SelectMenu.performed += SetSelectMenu;
        InputManager.Instance.StartMenu.performed += SetStartMenu;
    }

    private void SetSelectMenu(InputAction.CallbackContext context)
    {
        if (SpaceshipGameManager.Instance.CurrentGameState == SpaceshipGameManager.GameState.Play)
        {
            SpaceshipGameManager.Instance.OnPause += DisplaySelectMenu;

            InputManager.Instance.SetGameInputState(false);
            SpaceshipGameManager.Instance.PauseGame(true);
            currentMenu = Menu.Select;
        }
        else if (SpaceshipGameManager.Instance.CurrentGameState == SpaceshipGameManager.GameState.Pause && currentMenu == Menu.Select)
        {
            SpaceshipGameManager.Instance.OnResume += (() => InputManager.Instance.SetGameInputState(true)); 

            HideSelectMenu();
            InputManager.Instance.SetGameInputState(false);
            SpaceshipGameManager.Instance.PauseGame(false);
        }
    }

    private void SetStartMenu(InputAction.CallbackContext context)
    {
        // ...
    }

    private void DisplaySelectMenu()
    {
        SpaceshipGameManager.Instance.OnPause -= DisplaySelectMenu;
        Cursor.lockState = CursorLockMode.None;
        InputManager.Instance.SetGameInputState(true);
        OnMenu?.Invoke(true);
    }

    private void HideSelectMenu()
    {
        SpaceshipGameManager.Instance.OnPause -= DisplaySelectMenu;
        Cursor.lockState = CursorLockMode.Locked;
        OnMenu?.Invoke(false);
    }
}
