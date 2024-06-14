using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplosiveTurret : MonoBehaviour
{
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private int explosionDamage = 10;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject range;
    [SerializeField] private AudioClip sound;
    
    private AudioSource soundEffectSource;

    private void Start()
    {
        // Inicia la corrutina que espera un segundo antes de explotar
        StartCoroutine(ExplodeAfterDelay(1f));
        soundEffectSource = GetComponentInChildren<AudioSource>();
    }

    private IEnumerator ExplodeAfterDelay(float delay)
    {
        // Espera el tiempo especificado
        yield return new WaitForSeconds(delay);

        // Llama al método Explode después de esperar
        Explode();
    }

    private void Explode()
    {
        // Obtiene todos los enemigos dentro del rango de explosión
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, targetingRange, enemyMask);

        foreach (Collider2D hit in hits)
        {
            // Aplica daño a cada enemigo
            EnemyStats enemy = hit.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }
        
        if (sound != null && soundEffectSource != null)
        {
            soundEffectSource.PlayOneShot(sound);
        }

        // Destruye la torreta
        Destroy(gameObject);
    }

    // Método para dibujar el rango de explosión en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetingRange);
    }
}
