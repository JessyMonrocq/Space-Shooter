using Unity.Cinemachine;
using UnityEngine;

public class SpaceshipManager : MonoBehaviour
{
    [SerializeField] private SpaceshipController spaceshipControllerPrefab;
    [SerializeField] private SpaceshipReference spaceshipReferencePrefab;
    [SerializeField] private SpaceshipCamera spaceshipCamera;
    [SerializeField] private Transform spaceshipSpawnPoint;

    private SpaceshipController spaceshipController;

    private void Awake()
    {
        spaceshipController = Instantiate(spaceshipControllerPrefab, spaceshipSpawnPoint);
        spaceshipController.transform.localPosition = Vector3.zero;
        spaceshipController.transform.localRotation = Quaternion.identity;

        spaceshipController.SpaceshipCamera = spaceshipCamera;
        
        spaceshipController.SpaceshipReferencePrefab = spaceshipReferencePrefab;
    }

    public void InitializeSpaceship()
    {
        spaceshipController.InitializeSpaceship();
    }
}
