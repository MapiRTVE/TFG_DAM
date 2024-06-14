using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth main;

    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    // Referencia al componente SpriteRenderer del jugador
    private Image playerSpriteRenderer;
    [SerializeField] private GameObject playerHealth;

    // Array de sprites de salud reducida
    [SerializeField] private Sprite[] healthSprites;
    
    [SerializeField] private GameObject audioManager;
    [SerializeField] private AudioClip playerDeathSound;
    
    private AudioSource audioSource;

    private void Start()
    {
        main = this;
        currentHealth = maxHealth;
        playerSpriteRenderer = playerHealth.GetComponent<Image>();
        
        audioSource = audioManager.GetComponent<AudioSource>();
    }

    // Metodo para actualizar el sprite del jugador seg�n su salud
    private void UpdatePlayerSprite()
    {
        // Calcular el �ndice del sprite basado en el porcentaje de salud
        float healthPercentage = Math.Max((float)currentHealth / maxHealth, 0);
        int spriteIndex = Mathf.Clamp(Mathf.RoundToInt(healthPercentage * (healthSprites.Length - 1)), 0, healthSprites.Length - 1);
        playerSpriteRenderer.sprite = healthSprites[spriteIndex];
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Math.Max(currentHealth, 0);

        // Actualizar el sprite del jugador
        UpdatePlayerSprite();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("has perdido");
        UIManager.main.ShowGameOverWindow();
        
        audioSource.PlayOneShot(playerDeathSound);
    }
}
