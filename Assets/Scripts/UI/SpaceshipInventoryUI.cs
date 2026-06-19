using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SpaceshipInventoryUI : MonoBehaviour
{
    #region Inspector Fields
    [Header("Inventory Menu References")]
    [SerializeField] private TextMeshProUGUI inventoryTitle;
    [SerializeField] private GameObject inventoryContentParent;
    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private GameObject emptyInventoryMessage;

    [Header("Details Panel References")]
    [SerializeField] private CanvasGroup inventoryDetailsPanelCG;
    [SerializeField] private Image detailsIcon;
    [SerializeField] private TextMeshProUGUI detailsItemName;
    [SerializeField] private TextMeshProUGUI detailsItemType;
    [SerializeField] private TextMeshProUGUI detailsItemSize;
    [SerializeField] private TextMeshProUGUI detailsItemTotalSize;
    [SerializeField] private TextMeshProUGUI detailsDescription;

    private IObjectPool<InventoryItem> inventoryItemPool;
    private int poolDefaultCapacity = 15;
    private int pollMaxSize = 50;

    private HashSet<InventoryItem> activeInventoryItems = new HashSet<InventoryItem>();
    private int poolCurrentCapacity;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        inventoryItemPool = new ObjectPool<InventoryItem>(CreateInventoryItem, OnGetFromPool, OnReleaseFromPool, OnDestroyPooledObject, false, poolDefaultCapacity, pollMaxSize);

        foreach (Transform child in inventoryContentParent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < poolDefaultCapacity; i++)
        {
            var it = inventoryItemPool.Get();
            inventoryItemPool.Release(it);
        }

        poolCurrentCapacity = poolDefaultCapacity;

        inventoryDetailsPanelCG.alpha = 0f;
        emptyInventoryMessage.SetActive(false);
    }
    #endregion

    #region Public Methods
    public void UpdateInventoryUI(List<ComponentStack> inventory)
    {
        if (poolCurrentCapacity < inventory.Count)
        {
            int additionalItemsCount = inventory.Count - poolCurrentCapacity;
            for (int i = 0; i < additionalItemsCount; i++)
            {
                var it = inventoryItemPool.Get();
                inventoryItemPool.Release(it);
            }

            poolCurrentCapacity += additionalItemsCount;
        }

        foreach (InventoryItem item in activeInventoryItems)
        {
            item.ResetItemValues();
            inventoryItemPool.Release(item);
        }
        activeInventoryItems.Clear();

        if (inventory.Count == 0)
        {
            emptyInventoryMessage.SetActive(true);
            return;
        }
        else
        {
            emptyInventoryMessage.SetActive(false);
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            InventoryItem item = inventoryItemPool.Get();
            item.InitializeItemValues(inventory[i]);

            // ajouter à l'ensemble des items actifs
            activeInventoryItems.Add(item);

            // capturer la référence locale pour éviter les pièges de fermeture
            var captured = item;
            captured.OnItemClick += () => UpdateDetailsPanel(captured.ComponentStackRef.component, captured.ComponentStackRef.amount);
        }

        inventoryDetailsPanelCG.alpha = 0f;
    }
    #endregion

    #region Private Methods
    private void UpdateDetailsPanel(ComponentSO component, int amount)
    {
        if (inventoryDetailsPanelCG.alpha != 1f)
        {
            inventoryDetailsPanelCG.alpha = 1f;
        }

        detailsIcon.sprite = component.ComponentIcon;
        detailsItemName.text = component.ComponentName;
        detailsItemType.text = component.ComponentType.ToString();
        detailsItemSize.text = component.ComponentSize.ToString();
        detailsItemTotalSize.text = (component.ComponentSize * amount).ToString();
        detailsDescription.text = component.ComponentDescription;
    }
    #endregion

    #region Pooling Methods
    private InventoryItem CreateInventoryItem()
    {
        InventoryItem itemInstance = Instantiate(inventoryItemPrefab);
        itemInstance.transform.SetParent(inventoryContentParent.transform);
        itemInstance.ItemPool = inventoryItemPool;
        return itemInstance;
    }

    private void OnGetFromPool(InventoryItem pooledItem)
    {
        pooledItem.gameObject.SetActive(true);
    }

    private void OnReleaseFromPool(InventoryItem pooledItem)
    {
        pooledItem.ResetItemValues();
        pooledItem.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(InventoryItem pooledItem)
    {
        Destroy(pooledItem.gameObject);
    }
    #endregion
}
