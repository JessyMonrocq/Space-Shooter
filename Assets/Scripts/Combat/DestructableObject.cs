using System.Collections;
using UnityEngine;

public class DestructableObject : HealthComponent
{
    [Header("Destructable Object Settings")]
    [SerializeField] private Collider objectCollider;
    [SerializeField] private GameObject objectVisuals;
    [SerializeField] private ParticleSystem destructionVFX;
    [SerializeField] private GameObject unlockedItem;
    [SerializeField] private float timeToDestruction = 1f;

    [Header("Health Display")]
    [SerializeField] private HealthBarDisplay healthBarDisplay;

    protected override void OnDeath()
    {
        StartCoroutine(DestructionCoroutine());
    }

    protected override void Start()
    {
        base.Start();

        OnHealthDamageTaken += healthBarDisplay.UpdateHealthBar;
    }

    private IEnumerator DestructionCoroutine()
    {
        objectCollider.enabled = false;
        destructionVFX.Play();
        yield return new WaitForSeconds(timeToDestruction);
        objectVisuals.SetActive(false);

        if (unlockedItem != null)
        {
            Instantiate(unlockedItem);
            unlockedItem.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
