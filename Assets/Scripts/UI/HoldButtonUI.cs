using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoldButtonUI : MonoBehaviour
{
    public event Action OnClickConfirmed;

    [Header("Hold Button References")]
    [SerializeField] private float holdDuration = 1;
    [SerializeField] private float resetSpeed = 2f;
    [Tooltip("Image Type must be set to Filled")]
    [SerializeField] private Image highlightImage;
    [SerializeField] private bool oneTimeUse = false;

    private float holdTimer;
    private bool isPressing;
    private bool isPressed;
    private bool isReseting;

    private void Start()
    {
        holdTimer = 0f;
        isPressing = false;
        isPressed = false;
        isReseting = false;

        highlightImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (isPressing && !isPressed)
        {
            holdTimer += Time.unscaledDeltaTime;
            highlightImage.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                holdTimer = 0f;
                highlightImage.fillAmount = 1f;
                OnClickConfirmed?.Invoke();
                isPressed = true;
                return;
            }
        }
        
        if (!isPressing && isReseting)
        {
            holdTimer -= Time.unscaledDeltaTime * resetSpeed;
            highlightImage.fillAmount = holdTimer / holdDuration;
            if (holdTimer <= 0f)
            {
                holdTimer = 0f;
                highlightImage.fillAmount = 0f;
                isReseting = false;
                return;
            }
        }
    }

    public void OnButtonPress()
    {
        if (isReseting)
        {
            return;
        }

        isPressing = true;
        holdTimer = 0f;
    }

    public void OnButtonRelease()
    {
        if (isPressed && oneTimeUse)
        {
            return;
        }

        isPressed = false;
        isPressing = false;
        isReseting = true;
    }
}
