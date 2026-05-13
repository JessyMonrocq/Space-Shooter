using UnityEngine;

public class SpaceshipVFX : MonoBehaviour
{
    #region Inspector Fields
    public float CurrentSpeed { set { currentSpeed = value; } }
    public float MaxSpeed { set { maxSpeed = value; } }

    private ParticleSystem[] thrusterParticleSystems;
    private Vector3[] initialParticlesScales;

    private float minSpeedThreshold = 0.1f;
    private float currentSpeed;
    private float maxSpeed;
    private float boostParticleMultiplier;

    private bool isMoving;
    private bool isReady;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        boostParticleMultiplier = 0f;
        isMoving = false;
        isReady = false;
    }

    private void LateUpdate()
    {
        if (!isReady)
        {
            return;
        }

        if (currentSpeed < 0f)
        {
            return;
        }

        if (currentSpeed < (maxSpeed * minSpeedThreshold))
        {
            if (isMoving)
            {
                foreach (var particle in thrusterParticleSystems)
                {
                    particle.Stop();
                }
            }
            isMoving = false;
            return;
        }
        else
        {
            if (!isMoving)
            {
                foreach (var particle in thrusterParticleSystems)
                {
                    particle.Play();
                }
            }
            isMoving = true;
        }

        boostParticleMultiplier = currentSpeed / maxSpeed;

        for (int i = 0; i < thrusterParticleSystems.Length; i++)
        {
            float x = initialParticlesScales[i].x;
            float y = initialParticlesScales[i].y * boostParticleMultiplier;
            float z = initialParticlesScales[i].z;
            thrusterParticleSystems[i].gameObject.transform.localScale = new Vector3(x, y, z);
        }
    }
    #endregion

    #region Public Methods
    public void InitializeVFXValues(SpaceshipStatsSO spaceshipStats)
    {
        minSpeedThreshold = spaceshipStats.minSpeedThreshold;
        maxSpeed = spaceshipStats.thrust;
    }

    public void InitializeBoostParticles(ParticleSystem[] boostParticles)
    {
        thrusterParticleSystems = boostParticles;
        initialParticlesScales = new Vector3[thrusterParticleSystems.Length];

        for (int i = 0; i < thrusterParticleSystems.Length; i++)
        {
            thrusterParticleSystems[i].Stop();
            initialParticlesScales[i] = thrusterParticleSystems[i].gameObject.transform.localScale;
        }

        isReady = true;
    }
    #endregion
}
