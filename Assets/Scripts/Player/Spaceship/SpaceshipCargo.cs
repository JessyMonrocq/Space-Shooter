using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipCargo : MonoBehaviour
{
    #region Inspector Fields
    public event Action<GameObject> OnItemAdded;

    public List<ComponentStack> CargoInventory { get { return cargoInventory; } }
    private List<ComponentStack> cargoInventory;

    private int cargoMaxCapacity;
    private int currentCargoSpaceUsage;
    private ComponentItem currentDetectedItem;
    #endregion

    #region Unity Methods
    private void Start()
    {
        currentDetectedItem = null;

        InputManager.Instance.SpaceshipInteract.performed += OnInteractPerformed;
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipInteract.performed -= OnInteractPerformed;
    }
    #endregion

    #region Public Methods
    public void InitializeCargoValues(SpaceshipStatsSO spaceshipStats, SpaceshipModel spaceshipModel)
    {
        cargoMaxCapacity = spaceshipStats.cargoMaxCapacity;
        cargoInventory = new List<ComponentStack>();
        cargoInventory.Clear();
        currentCargoSpaceUsage = 0;

        if (spaceshipModel.UsesTractorBeam)
        {
            spaceshipModel.TractorBeam.OnItemDetected += OnItemDetected;
        }
    }
    #endregion

    #region Private Methods
    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        AddItemToCargo();
    }

    private void AddItemToCargo()
    {
        if (currentCargoSpaceUsage < cargoMaxCapacity && currentDetectedItem != null)
        {
            if (currentDetectedItem.AssignedComponent.ComponentSize + currentCargoSpaceUsage <= cargoMaxCapacity)
            {
                currentCargoSpaceUsage += currentDetectedItem.AssignedComponent.ComponentSize;
                CheckCargoForItem(currentDetectedItem.AssignedComponent);
                currentDetectedItem.SetIconDisplay(false);
                Destroy(currentDetectedItem.gameObject);
            }
        }
    }

    private void CheckCargoForItem(ComponentSO item)
    {
        bool itemFound = false;
        foreach (ComponentStack stack in cargoInventory)
        {
            if (stack.component.name == item.name)
            {
                stack.amount++;
                itemFound = true;
                break;
            }
        }

        if (!itemFound)
        {
            ComponentStack newStack = new ComponentStack(item, 1);
            cargoInventory.Add(newStack);
        }
    }

    private void OnItemDetected(GameObject detectedItem)
    {
        if (detectedItem != currentDetectedItem && currentDetectedItem != null)
        {
            currentDetectedItem.SetIconDisplay(false);
        }

        if (detectedItem == null)
        {
            currentDetectedItem = null;
            return;
        }
        currentDetectedItem = detectedItem.GetComponent<ComponentItem>();
        currentDetectedItem.SetIconDisplay(true);
    }
}
#endregion
