using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    [Header("Circle Slider Settings")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField, Range(0f, 1f)] private float sliderValue = 1f;
    [SerializeField] private Image sliderFrameImage;
    [SerializeField] private Image sliderLogoImage;
    [SerializeField] private TextMeshProUGUI sliderText;

#if UNITY_EDITOR
    private void OnValidate()
    {
        sliderFrameImage.fillAmount = sliderValue;
    }
#endif

    public void SliderValue(float sliderValue, float currentValue)
    {
        sliderFrameImage.fillAmount = sliderValue;

        sliderText.text = currentValue.ToString();
    }
}
