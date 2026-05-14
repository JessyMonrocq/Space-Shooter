using UnityEngine;

public class CourseManager : MonoBehaviour
{
    [Header("Course Settings")]
    [SerializeField] private CourseTime courseTime;
    [SerializeField] private Vector3 vector;
    [SerializeField] private WaypointRing[] courseRings;
}
