using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CourseManager courseManager;
    [SerializeField] private SpaceshipManager spaceshipManager;

    private void Start()
    {
        InputManager.Instance.SetSpaceshipInputState(false);
        spaceshipManager.InitializeSpaceship();
        courseManager.InitializeCourse();
    }
}
