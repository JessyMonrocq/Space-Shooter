using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SpaceshipUI : MonoBehaviour
{
    [Header("Spaceship UI References")]
    [SerializeField] private CanvasGroup mainCanvasGroup;
    [SerializeField] private CanvasGroup startMenuCG;
    [SerializeField] private CanvasGroup selectMenuCG;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private SpaceshipInventoryUI inventoryUI;

    private void Start()
    {
        startMenuCG.gameObject.SetActive(false);
        startMenuCG.alpha = 0f;
        selectMenuCG.gameObject.SetActive(false);
        selectMenuCG.alpha = 0f;
    }

    public void OpenInventoryMenu(List<ComponentStack> list)
    {
        StartCoroutine(OpenInventoryMenuCoroutine(list));
    }

    public void CloseInventoryMenu()
    {
        StartCoroutine(CloseInventoryMenuCoroutine());
    }

    private IEnumerator OpenInventoryMenuCoroutine(List<ComponentStack> list)
    {
        selectMenuCG.gameObject.SetActive(true);
        inventoryUI.UpdateInventoryUI(list);
        yield return selectMenuCG.DOFade(1f, 0.25f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
    }

    private IEnumerator CloseInventoryMenuCoroutine()
    {
        yield return selectMenuCG.DOFade(0f, 0.25f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
        selectMenuCG.gameObject.SetActive(false);        
    }
}
