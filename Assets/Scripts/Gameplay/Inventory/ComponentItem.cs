using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ComponentItem : MonoBehaviour
{
    [Header("Component Item References")]
    [SerializeField] private ComponentsListSO componentsListSO;
    [SerializeField] private ComponentSO assignedComponent;

    public ComponentSO AssignedComponent { get { return assignedComponent; } }

    [Header("Interaction Display Settings")]
    [SerializeField] private Image interactionIcon;
    [SerializeField] private float screenEdgePadding = 50f;
    [SerializeField] private float waypointScaleMult = 1.2f;
    [SerializeField] private float waypointScaleDuration = 0.5f;
    private Camera mainCamera;
    private bool isDisplayed;

    private void Start()
    {
        interactionIcon.DOFade(0f, 0f);
        isDisplayed = false;

        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateInteractIcon();
    }

    public void SetIconDisplay(bool state)
    {
        interactionIcon.DOKill();
        if (state)
        {
            interactionIcon.DOFade(1f, 0.5f);
        } else
        {
            interactionIcon.DOFade(0f, 0.5f);
        }
        isDisplayed = state;
    }

    private void UpdateInteractIcon()
    {
        if (!isDisplayed)
        {
            return;
        }

        Transform target = this.transform;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(target.position);

        bool isBehind = screenPos.z < 0;
        bool isOffScreen = isBehind || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height;

        if (isOffScreen)
        {
            if (isBehind)
            {
                screenPos *= -1;
            }

            screenPos.x = Mathf.Clamp(screenPos.x, screenEdgePadding, Screen.width - screenEdgePadding);
            screenPos.y = Mathf.Clamp(screenPos.y, screenEdgePadding, Screen.height - screenEdgePadding);
        }

        Vector3 iconPosition = new Vector3(screenPos.x, screenPos.y, 0f);
        interactionIcon.rectTransform.position = iconPosition;
    }
}
