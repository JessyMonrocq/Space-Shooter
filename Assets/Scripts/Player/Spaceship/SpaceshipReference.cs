using UnityEngine;

[RequireComponent (typeof(Mesh))]
[RequireComponent (typeof(MeshRenderer))]
public class SpaceshipReference : MonoBehaviour
{
    [Header("Spaceship References")]
    [SerializeField] private string spaceshipName;
    [SerializeField] private SpaceshipStatsSO spaceshipStats;
    [SerializeField] private Mesh collisionMesh;
    [SerializeField] private MeshRenderer meshRenderer;
}
