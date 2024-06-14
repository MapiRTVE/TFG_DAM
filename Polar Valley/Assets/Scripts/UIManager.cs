using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Clase que maneja la interfaz
public class UIManager : MonoBehaviour
{
    public static UIManager main;

    //Ventana de gameOver
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private GameObject pauseWindow;
    [SerializeField] private Button loseButton;
    [SerializeField] private GameObject winWindow;
    [SerializeField] private Button winButton;

    private bool isHoveringUI;
    private GameObject range;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        loseButton.onClick.AddListener(ExitLevel);
        winButton.onClick.AddListener(ExitLevel);
    }

    //Metodo que establece el estado del hover
    public void SetHoveringState(bool state)
    { 
        isHoveringUI = state;
        HideRange();
    }

    //Metodo que comprueba que el puntero este sobre la interfaz
    public bool IsHoveringUI()
    { 
        return isHoveringUI;
    }

    //Metodo que muestra la pantalla de gameOver
    public void ShowGameOverWindow()
    { 
        gameOverWindow.SetActive(true);
    }

    public void ShowPauseWindow()
    { 
        pauseWindow.SetActive(true);
    }

    public void HidePauseWindow()
    {
        pauseWindow.SetActive(false);
    }

    public void HideRange()
    {
        if (!IsHoveringUI())
        { 
            this.range.SetActive(false);
            this.range = null;
        }
    }

    public void SetRange(GameObject range)
    { 
        range.gameObject.SetActive(true);
        this.range = range; 
    }

    public void ExitLevel() {
        SceneManager.LoadScene("MainMenu");
    }

    public void WinLevel()
    { 
        
        winWindow.SetActive(true);
    }
}
