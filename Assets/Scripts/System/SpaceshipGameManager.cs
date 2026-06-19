using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class SpaceshipGameManager : MonoBehaviour
{
    #region Inspector Fields
    public static SpaceshipGameManager Instance { get; private set; }
    public GameState CurrentGameState { get {  return currentGameState; }  }

    public event Action OnPause;
    public event Action OnResume;

    public enum GameState
    {
        Setup,
        Play,
        Pause
    }

    public enum MissionType
    {
        Course,
        Scavenge,
        FreeRoam
    }

    private GameState currentGameState;

    [Header("Mission Settings")]
    [SerializeField] private MissionType missionType;

    [Header("Game Manager")]
    [SerializeField] private CourseManager courseManager;

    [Header("Pause Manager")]
    [SerializeField] private float pauseDuration = 0.75f;
    [SerializeField] private Ease pauseEaseType = Ease.OutSine;
    private bool isPausing;

    [Header("Spaceship Manager")]
    [SerializeField] private SpaceshipController spaceshipControllerPrefab;
    [SerializeField] private SpaceshipModel spaceshipReferencePrefab;
    [SerializeField] private SpaceshipCamera spaceshipCamera;
    [SerializeField] private Transform spaceshipSpawnPoint;

    private SpaceshipController spaceshipController;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        currentGameState = GameState.Setup;
        isPausing = false;
        InputManager.Instance.SetSpaceshipInputState(false);

        spaceshipController = Instantiate(spaceshipControllerPrefab, spaceshipSpawnPoint);
        spaceshipController.transform.localPosition = Vector3.zero;
        spaceshipController.transform.localRotation = Quaternion.identity;
        spaceshipController.SpaceshipCamera = spaceshipCamera;
        spaceshipController.SpaceshipReferencePrefab = spaceshipReferencePrefab;

        if (missionType == MissionType.Course)
        {
            courseManager.InitializeCourse();
        }
    }

    private void Start()
    {
        // Fade in screen;
        // Set Timer before start;

        spaceshipController.InitializeSpaceship();

        if (missionType == MissionType.Course)
        {
            courseManager.StartCourse();
        }
        else
        {
            InputManager.Instance.SetSpaceshipInputState(true);
            currentGameState = GameState.Play;
        }
    }
    #endregion

    #region Public Methods
    public void PauseGame(bool pause)
    {
        if (isPausing)
        {
            return;
        }

        isPausing = true;
        if (currentGameState == GameState.Play && pause)
        {
            StartCoroutine(PauseGameCoroutine(true));
        }
        else if (currentGameState == GameState.Pause && !pause)
        {
            StartCoroutine(PauseGameCoroutine(false));
        }

    }
    #endregion

    #region Coroutine Methods
    private IEnumerator PauseGameCoroutine(bool pause)
    {
        if (pause)
        {
            yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, pauseDuration).SetEase(pauseEaseType).SetUpdate(true).WaitForCompletion();
            InputManager.Instance.SetSpaceshipInputState(false);
            currentGameState = GameState.Pause;
            OnPause?.Invoke();
        }
        else
        {
            InputManager.Instance.SetSpaceshipInputState(true);
            yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, pauseDuration).SetEase(pauseEaseType).SetUpdate(true).WaitForCompletion();
            currentGameState = GameState.Play;
            OnResume?.Invoke();
        }

        isPausing = false;
    }
    #endregion
}
