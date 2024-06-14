using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceholderUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] TextMeshProUGUI enemiesAliveUI;
    [SerializeField] TextMeshProUGUI healthUI;

    //Boton pausa UI
    [SerializeField] Button pauseButton;

    //Panel de ayuda
    [SerializeField] GameObject helpPanel;

    //Botones del panel de pausa
    [SerializeField] Button resumeButton;
    [SerializeField] Button exitButton;
    [SerializeField] Button helpButton;
    [SerializeField] Button backButton;
    


    private void Start()
    {
        // Asignar el método OpenPauseMenu al evento onClick del botón de pausa
        pauseButton.onClick.AddListener(OpenPauseMenu);
        resumeButton.onClick.AddListener(ClosePauseMenu);
        exitButton.onClick.AddListener(ExitLevel);
        helpButton.onClick.AddListener(OpenHelpPanel);
        backButton.onClick.AddListener(CloseHelpPanel);
    }

    private void OnGUI()
    {
        scoreUI.text = "Puntuacion: " + LevelManager.main.score.ToString();
        currencyUI.text = "Dinero: " + LevelManager.main.currency.ToString();
        waveUI.text = "Oleada: " + (LevelManager.main.currentWave + 1);
        enemiesAliveUI.text = "Enemigos vivos: " + LevelManager.main.enemiesAlive.ToString();

        if (PlayerHealth.main != null)
        {
            healthUI.text = PlayerHealth.main.currentHealth.ToString();
        }
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        Debug.Log("El juego ha sido pausado");
        UIManager.main.ShowPauseWindow();
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        Debug.Log("Juego reanudado");
        UIManager.main.HidePauseWindow();
    }

    public void ExitLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenHelpPanel()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        helpPanel.SetActive(false);
    }
}
