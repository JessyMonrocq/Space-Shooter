using UnityEngine;

[RequireComponent(typeof(Mesh))]
[RequireComponent(typeof(MeshRenderer))]
public class SpaceshipModel : MonoBehaviour
{
    [Header("Spaceship References")]
    [SerializeField] private string spaceshipName;
    [SerializeField] private SpaceshipStatsSO spaceshipStats;
    [SerializeField] private Mesh collisionMesh;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem[] thrusterParticleSystems;
    [SerializeField] private ParticleSystem starStreaksEffect;

    public string SpaceshipName { get { return spaceshipName; } }
    public SpaceshipStatsSO SpaceshipStats { get { return spaceshipStats; } }
    public Mesh CollisionMesh { get { return collisionMesh; } }
    public MeshRenderer MeshRenderer { get { return meshRenderer; } }
    public ParticleSystem[] ThrusterParticleSystem { get { return thrusterParticleSystems; } }
    public ParticleSystem WarpSpeedEffect { get { return starStreaksEffect; } }

    [Header("Spaceship Weapons References")]
    [SerializeField] private bool usesWeapons = true;
    [SerializeField] private WeaponBase primaryWeapon;
    [SerializeField] private WeaponBase secondaryWeapon;

    public bool UsesWeapons { get { return usesWeapons; } }
    public WeaponBase PrimaryWeapon { get { return primaryWeapon; } }
    public WeaponBase SecondaryWeapon { get {return secondaryWeapon; } }

    [Header("Spaceship Tractor Beam References")]
    [SerializeField] private bool usesTractorBeam = false;
    [SerializeField] private TractorBeam tractorBeam;

    public bool UsesTractorBeam { get { return usesTractorBeam; } }
    public TractorBeam TractorBeam { get {return tractorBeam; } }

    [Header("Spaceship Idle Animation")]
    [SerializeField] private float idleAnimAmplitude = 0.15f;
    [SerializeField] private float idleAnimFrequency = 1.5f;
    [SerializeField] private float smoothTime = 0.2f;

    public bool IsIdle { get { return isIdle; } set { isIdle = value; } }
    private bool isIdle;

    private Vector3 currentIdleOffset;
    private Vector3 currentVelocity;

    private void Start()
    {
        isIdle = true;
    }

    private void Update()
    {
        IdleAnimation();
    }

    private void IdleAnimation()
    {
        Vector3 targetOffset = Vector3.zero;

        if (isIdle)
        {
            float hover = Mathf.Sin(Time.time * idleAnimFrequency) * idleAnimAmplitude;
            targetOffset = new Vector3(0f, hover, 0f);
        }

        currentIdleOffset = Vector3.SmoothDamp(currentIdleOffset, targetOffset, ref currentVelocity, smoothTime);
        if (!isIdle && currentIdleOffset.magnitude < 0.01f)
        {
            currentIdleOffset = Vector3.zero;
        }
        transform.localPosition = currentIdleOffset;
    }
}
