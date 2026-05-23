using UnityEngine;

public class SpaceshipVFX : MonoBehaviour
{
    #region Inspector Fields
    public float CurrentSpeed { set { currentSpeed = value; } }
    public float MaxSpeed { set { maxSpeed = value; } }

    private ParticleSystem[] thrusterParticleSystems;
    private Vector3[] initialThrusterParticlesScales;

    private ParticleSystem starStreakEffect;
    private float initialStarStreakSpeedMultiplier;

    private float thrusterMinSpeedThreshold = 0.1f;
    private float starStreakMinSpeedThreshold = 0.6f;
    private float currentSpeed;
    private float maxSpeed;
    private float boostParticleMultiplier;

    private bool isMoving;
    private bool isStarStreaking;
    private bool isReady;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        boostParticleMultiplier = 0f;
        isMoving = false;
        isStarStreaking = false;
        isReady = false;
    }

    private void LateUpdate()
    {
        boostParticleMultiplier = currentSpeed / maxSpeed;

        HandleThrusterVFX();
        HandleStarStreakVFX();
    }
    #endregion

    #region Public Methods
    public void InitializeVFXValues(SpaceshipStatsSO spaceshipStats, SpaceshipModel spaceshipModel)
    {
        thrusterMinSpeedThreshold = spaceshipStats.minSpeedThreshold;
        maxSpeed = spaceshipStats.thrust;

        thrusterParticleSystems = spaceshipModel.ThrusterParticleSystem;
        starStreakEffect = spaceshipModel.WarpSpeedEffect;
        initialThrusterParticlesScales = new Vector3[thrusterParticleSystems.Length];

        for (int i = 0; i < thrusterParticleSystems.Length; i++)
        {
            thrusterParticleSystems[i].Stop();
            initialThrusterParticlesScales[i] = thrusterParticleSystems[i].gameObject.transform.localScale;
        }

        ParticleSystem.MainModule starStreakMain = starStreakEffect.main;
        initialStarStreakSpeedMultiplier = starStreakMain.startSpeedMultiplier;

        isReady = true;
    }

    public void InitializeVFXParticles(SpaceshipModel spaceshipModel)
    {

    }
    #endregion

    #region Private Methods
    private void HandleThrusterVFX()
    {
        if (!isReady || currentSpeed < 0f)
        {
            return;
        }

        if (currentSpeed < (maxSpeed * thrusterMinSpeedThreshold))
        {
            if (isMoving)
            {
                foreach (var particle in thrusterParticleSystems)
                {
                    particle.Stop();
                }
                isMoving = false;
            }
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
                isMoving = true;
            }
        }

        for (int i = 0; i < thrusterParticleSystems.Length; i++)
        {
            float x = initialThrusterParticlesScales[i].x;
            float y = initialThrusterParticlesScales[i].y * boostParticleMultiplier;
            float z = initialThrusterParticlesScales[i].z;
            thrusterParticleSystems[i].gameObject.transform.localScale = new Vector3(x, y, z);
        }
    }

    private void HandleStarStreakVFX()
    {
        if (!isReady || currentSpeed < 0f)
        {
            return;
        }

        if (currentSpeed < (maxSpeed * starStreakMinSpeedThreshold))
        {
            if (isStarStreaking)
            {
                starStreakEffect.Stop();
                isStarStreaking = false;
            }
            return;
        }
        else
        {
            if (!isStarStreaking)
            {
                starStreakEffect.Play();
                isStarStreaking = true;
            }
        }

        ParticleSystem.MainModule starStreakMain = starStreakEffect.main;
        starStreakMain.startSpeedMultiplier = initialStarStreakSpeedMultiplier * boostParticleMultiplier;
    }
    #endregion
}
