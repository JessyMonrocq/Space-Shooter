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
    [SerializeField] private ParticleSystem[] thrusterParticleSystems;

    public string SpaceshipName { get { return spaceshipName; } }
    public SpaceshipStatsSO SpaceshipStats { get { return spaceshipStats; } }
    public Mesh CollisionMesh { get { return collisionMesh; } }
    public MeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ParticleSystem[] ThrusterParticleSystem { get { return thrusterParticleSystems; } }
}
