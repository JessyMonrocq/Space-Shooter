using DG.Tweening;
using System;
using UnityEngine;

public class WaypointRing : MonoBehaviour
{
    #region Inspector Fields
    public event Action OnRingPassed;

    [Header("Waypoint Ring Settings")]
    [SerializeField] private Transform ringTransform;
    [SerializeField] private MeshRenderer ringMeshRenderer;
    [SerializeField] private Color ringActiveEmissionColor;

    [Header("Animation Settings")]
    [SerializeField] private float ringScaleMult = 1.1f;
    [SerializeField] private float ringScaleDuration = 1f;

    public bool IsActive { set { isActive = value; } }
    private bool isActive;

    private MaterialPropertyBlock materialPropertyBlock;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        isActive = false;
    }

    private void Start()
    {
        SetRingEmission();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive)
        {
            return;
        }

        if (other.gameObject.GetComponent<SpaceshipController>() != null)
        {
            isActive = false;
            SetRingEmission();

            OnRingPassed?.Invoke();
        }
    }
    #endregion

    #region Private Methods
    private void SetRingEmission()
    {
        ringMeshRenderer.GetPropertyBlock(materialPropertyBlock);

        Color ringColor = isActive ? ringActiveEmissionColor * 2f : Color.black;
        materialPropertyBlock.SetColor("_EmissionColor", ringColor);
        ringMeshRenderer.SetPropertyBlock(materialPropertyBlock);

        if (isActive)
        {
            ringTransform.DOScale(ringScaleMult, ringScaleDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            ringTransform.DOScale(Vector3.one, 1f);
        }
    }
    #endregion
}
