using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class CourseManager : MonoBehaviour
{
    [Header("Course Settings")]
    [SerializeField] private CourseTime courseTime;
    [SerializeField, Min(1)] private int lapAmounts = 1;
    [SerializeField] private WaypointRing[] courseWaypoints;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI waypointsText;
    [SerializeField] private TextMeshProUGUI lapsText;
    [SerializeField] private TextMeshProUGUI countdownText;

    private CourseTime courseTimer;

    private float courseTimeFloat;
    private float courseTimerFloat;

    private int countdownTimer;
    private int currentWaypoint;
    private int numberOfWaypoints;
    private int currentLap;

    private bool courseStarted;

    private void Start()
    {
        courseTimerFloat = 0f;

        countdownTimer = 3;
        currentWaypoint = 0;
        numberOfWaypoints = courseWaypoints.Length;
        currentLap = 0;

        courseStarted = false;
        courseTimeFloat = courseTime.GetCourseToFloat();

        timerText.text = "00:00:000";
        timerText.DOFade(0f, 0f);
        waypointsText.text = $"{currentWaypoint}/{numberOfWaypoints}";
        waypointsText.DOFade(0f, 0f);
        lapsText.text = $"{currentLap}/{lapAmounts}";
        lapsText.DOFade(0f, 0f);
        countdownText.text = null;
        countdownText.DOFade(0f, 0f);

        foreach (WaypointRing waypoint in courseWaypoints)
        {
            waypoint.SetRingState(false);
            waypoint.OnRingPassed += UpdateCourseStatus;
        }

        InitializeCourse(); // Remove later
    }

    private void OnDestroy()
    {
        foreach (WaypointRing waypoint in courseWaypoints)
        {
            if (waypoint != null)
            {
                continue;
            }
            waypoint.SetRingState(false);
            waypoint.OnRingPassed -= UpdateCourseStatus;
        }
    }

    private void Update()
    {
        if (courseStarted)
        {
            courseTimerFloat += Time.deltaTime;
            courseTimer.SetCourseFromFloat(courseTimerFloat);
            timerText.text = courseTimer.ChronoToString();
        }
    }

    public void InitializeCourse()
    {
        StartCoroutine(CourseCountdownCoroutine());
    }

    private void UpdateCourseStatus()
    {
        currentWaypoint++;

        if (currentWaypoint == numberOfWaypoints)
        {
            currentLap++;

            if (currentLap == lapAmounts)
            {
                // Course ends;
                Debug.Log("Course finished");
                courseStarted = false;
            }
            else
            {
                currentWaypoint = 0;
                SetNextRingState();
            }
        }
        else
        {
            SetNextRingState();
        }

        waypointsText.text = $"{currentWaypoint}/{numberOfWaypoints}";
        lapsText.text = $"{currentLap}/{lapAmounts}";
    }

    private void SetNextRingState()
    {
        courseWaypoints[currentWaypoint].SetRingState(true);
    }

    private IEnumerator CourseCountdownCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (countdownTimer > 0)
        {
            countdownText.text = countdownTimer.ToString();
            yield return countdownText.DOFade(1f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(0.25f);
            yield return countdownText.DOFade(0f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(0.25f);
            countdownTimer--;
        }

        countdownText.text = "GO!";
        timerText.DOFade(0.75f, 1f).SetEase(Ease.Linear);
        waypointsText.DOFade(0.8f, 1f).SetEase(Ease.Linear);
        lapsText.DOFade(0.8f, 1f).SetEase(Ease.Linear);
        yield return countdownText.DOFade(1f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
        yield return new WaitForSeconds(0.25f);
        yield return countdownText.DOFade(0f, 0.25f).SetEase(Ease.Linear).WaitForCompletion();
        yield return new WaitForSeconds(0.25f);

        courseStarted = true;
        SetNextRingState();
    }
}
