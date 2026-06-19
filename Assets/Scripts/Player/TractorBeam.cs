using System;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
    /*
    public event Action<GameObject, bool> OnItemDetected;

    private GameObject currentDetectedObject;
    private List<GameObject> detectedObjectsList;

    private void Start()
    {
        currentDetectedObject = null;
        detectedObjectsList = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject detectedObject = other.gameObject;
        if (detectedObject.GetComponent<ComponentItem>() != null)
        {
            if (!detectedObjectsList.Contains(detectedObject))
            {
                detectedObjectsList.Add(detectedObject);

                if (detectedObjectsList.Count == 1)
                {
                    currentDetectedObject = detectedObject;
                    OnItemDetected?.Invoke(detectedObject, true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject lostObject = other.gameObject;
        if (lostObject.GetComponent<ComponentItem>() != null && detectedObjectsList.Contains(lostObject))
        {
            detectedObjectsList.Remove(lostObject);

            if (currentDetectedObject == lostObject)
            {
                OnItemDetected?.Invoke(lostObject, false);
                currentDetectedObject = null;
            }

            if (detectedObjectsList.Count > 0 && detectedObjectsList[0] != null)
            {
                currentDetectedObject = detectedObjectsList.First();
                OnItemDetected?.Invoke(currentDetectedObject, true);
            }
            else if (detectedObjectsList.Count == 0)
            {
                Debug.Log("TractorBeam: No more detected objects.");
                detectedObjectsList.Clear();
                detectedObjectsList = new List<GameObject>();
            }
        }
    }
    
    public void RemoveDetectObjectReference(GameObject detectedObject)
    {
        if (detectedObjectsList.Contains(detectedObject))
        {
            detectedObjectsList.Remove(detectedObject);
        }

        if (currentDetectedObject == detectedObject)
        {
            OnItemDetected?.Invoke(detectedObject, false);
            currentDetectedObject = null;
        }

        if (detectedObjectsList.Count > 0)
        {
            currentDetectedObject = detectedObjectsList[0];
            OnItemDetected?.Invoke(currentDetectedObject, true);
        }
    */

    public event Action<GameObject> OnItemDetected;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 50f;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private LayerMask itemLayer;

    public ComponentItem CurrentTarget { get; private set; }

    private readonly List<ComponentItem> availableItems = new();
    private ComponentItem previousTarget;

    private void Start()
    {
        previousTarget = null;
        InvokeRepeating(nameof(ScanItems), 0f, 0.1f);
    }

    private void ScanItems()
    {
        availableItems.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, itemLayer);

        float bestDot = -1f;
        CurrentTarget = null;

        foreach (Collider hit in hits)
        {
            ComponentItem item = hit.GetComponent<ComponentItem>();

            if (item == null)
            {
                continue;
            }

            Vector3 direction = (item.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, direction);

            if (angle > detectionAngle)
            {
                continue;
            }

            availableItems.Add(item);
            float dot = Vector3.Dot(transform.forward, direction);

            if (dot > bestDot)
            {
                bestDot = dot;
                CurrentTarget = item;
            }
        }

        if (CurrentTarget != previousTarget)
        {
            OnItemDetected?.Invoke(CurrentTarget != null ? CurrentTarget.gameObject : null);
            previousTarget = CurrentTarget;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle, 0) * transform.forward;

        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle, 0) * transform.forward;

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionRadius);

        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionRadius);
    }
}

