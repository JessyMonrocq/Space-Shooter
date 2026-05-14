using UnityEngine;

public class CustomSlider : MonoBehaviour
{
    [Header("Custom Slider Settings")]
    [SerializeField] private RectTransform sliderFill;
    [SerializeField] private float slidingOffset;
    [SerializeField] private bool slideToLeft = true;
    [SerializeField, Range(0f, 1f)] private float sliderValue;

#if UNITY_EDITOR
    private void OnValidate()
    {
        float PosX = (sliderValue * slidingOffset) - slidingOffset;
        sliderFill.localPosition = new Vector3(PosX, 0f, 0f);
    }
#endif

    public void SliderValue(float val)
    {
        sliderValue = val;
        float PosX = (sliderValue * slidingOffset) - slidingOffset;
        sliderFill.localPosition = new Vector3(PosX, 0f, 0f);
    }
}
