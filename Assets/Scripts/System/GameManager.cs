using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CourseManager courseManager;
    [SerializeField] private SpaceshipManager spaceshipManager;
    [SerializeField] private bool InitializeCourse;

    private void Start()
    {
        InputManager.Instance.SetSpaceshipInputState(false);
        spaceshipManager.InitializeSpaceship();

        if (InitializeCourse)
        {
            courseManager.InitializeCourse();
        }
        else
        {
            InputManager.Instance.SetSpaceshipInputState(true);
        }
    }
}
