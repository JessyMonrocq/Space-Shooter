using System;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeam : MonoBehaviour
{
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
            detectedObjectsList.Add(detectedObject);
            if (detectedObjectsList.Count == 1)
            {
                currentDetectedObject = detectedObject;
                OnItemDetected?.Invoke(detectedObject, true);
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
            }

            if (detectedObjectsList.Count > 0)
            {
                currentDetectedObject = detectedObjectsList[0];
                OnItemDetected?.Invoke(currentDetectedObject, true);
            }
        }
    }
}
