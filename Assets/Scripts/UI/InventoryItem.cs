using System;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    #region Inspector Fields
    public event Action OnItemClick;

    [Header("Inventory Item References")]
    [SerializeField] private Button itemButton;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amount;

    public ComponentStack ComponentStackRef { get { return componentStackRef; } set { componentStackRef = value; } }
    public IObjectPool<InventoryItem> ItemPool { set => itemPool = value; }

    private ComponentStack componentStackRef;
    private IObjectPool<InventoryItem> itemPool;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        itemButton.onClick.AddListener(() => OnItemClick?.Invoke());
    }
    #endregion

    #region Public Methods
    public void InitializeItemValues(ComponentStack stack)
    {
        componentStackRef = stack;
        icon.sprite = stack.component.ComponentIcon;
        amount.text = stack.amount.ToString();
    }

    public void ResetItemValues()
    {
        OnItemClick = null;
        componentStackRef = null;
        icon.sprite = null;
        amount.text = string.Empty;
    }
    #endregion

    #region Pooling Methods
    public void Deactivate()
    {
        itemPool.Release(this);
    }
    #endregion
}
