using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public event Action OnPause;
    public event Action OnResume;

    public static PauseGame Instance { get; private set; }

    [Header("Spaceship Pause Settings")]
    [SerializeField] private static float duration = 0.75f;
    [SerializeField] private Ease easeType = Ease.OutSine;

    private bool isPaused;

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
    }

    private void Start()
    {
        isPaused = false;
    }

    public void PauseCurrentGame(bool pause)
    {
        StopCoroutine(PauseGameCoroutine(!pause));
        StartCoroutine(PauseGameCoroutine(pause));
    }

    private IEnumerator PauseGameCoroutine(bool pause)
    {
        if (pause)
        {
            yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0f, duration).SetEase(easeType).SetUpdate(true).WaitForCompletion();

            isPaused = !isPaused;
            OnPause?.Invoke();
        }
        else
        {
            isPaused = !isPaused;
            OnResume?.Invoke();

            yield return DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, duration).SetEase(easeType).SetUpdate(true).WaitForCompletion();
        }

    }
}
