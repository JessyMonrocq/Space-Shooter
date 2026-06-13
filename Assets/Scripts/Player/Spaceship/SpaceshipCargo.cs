using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipCargo : MonoBehaviour
{
    private int cargoMaxCapacity;

    public List<ComponentStack> CargoInventory { get { return cargoInventory; } }

    private List<ComponentStack> cargoInventory;
    private int currentCargoSpaceUsage;
    private ComponentItem currentDetectedItem;

    private void Start()
    {
        currentDetectedItem = null;

        InputManager.Instance.SpaceshipInteract.performed += OnInteractPerformed;
    }

    private void OnDestroy()
    {
        InputManager.Instance.SpaceshipInteract.performed -= OnInteractPerformed;
    }

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
        foreach(ComponentStack stack in cargoInventory)
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

    private void OnItemDetected(GameObject detectedItem, bool isDetected)
    {
        detectedItem.GetComponent<ComponentItem>().SetIconDisplay(isDetected);
        currentDetectedItem = isDetected ? detectedItem.GetComponent<ComponentItem>() : null;
    }
}
