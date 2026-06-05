using DG.Tweening;
using UnityEngine;

public class HealthBarDisplay : MonoBehaviour
{
    [Header("Health Display Settings")]
    [SerializeField] private CustomSlider healthSlider;
    [SerializeField] private CanvasGroup healthSliderCG;
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private float displayDuration = 5f;
    [SerializeField] private float displaySpeed = 0.25f;
    [SerializeField] private float displayAlpha = 0.75f;

    private Transform spaceshipTransform;
    private float displayTimer = 0f;
    private bool requiresDisplay;

    private void Start()
    {
        healthSliderCG.alpha = 0f;
        displayTimer = 0f;
        requiresDisplay = false;

        spaceshipTransform = SpaceshipController.Instance.transform;
    }

    private void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - spaceshipTransform.position);

        HandleDisplayTimer();
    }

    public void UpdateHealthBar(int damageTaken ,int currentHealth, int maxHealth)
    {
        if (!requiresDisplay)
        {
            requiresDisplay = true;
            healthSliderCG.DOKill();
            healthSliderCG.DOFade(displayAlpha, displaySpeed).SetEase(Ease.Linear);
        }
        displayTimer = 0f;

        // Display floating damage numbers;

        float sliderValue = ((float)currentHealth / (float)maxHealth);
        healthSlider.SliderValue(sliderValue);
    }

    private void HandleDisplayTimer()
    {
        if (!requiresDisplay)
        {
            return;
        }

        displayTimer += Time.deltaTime;
        if (displayTimer >= displayDuration)
        {
            requiresDisplay = false;
            displayTimer = 0f;

            healthSliderCG.DOKill();
            healthSliderCG.DOFade(0f, displaySpeed).SetEase(Ease.Linear);
        }
    }
}
