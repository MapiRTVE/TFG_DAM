using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Clase que gestiona la funcionalidad del menu
public class MenuController : MonoBehaviour
{

    public static MenuController main;

    [Header("Options")]
    //Slider de sonidos
    public Slider sonido;
    public Slider musica;
    public Slider master;

    //Elementos para gestionar el audio
    public AudioMixer mixer;
    public AudioSource fxSource;
    public AudioClip clickSound;

    [Header("Panels")]
    //Paneles de la interfaz
    public GameObject mainPanel;
    public GameObject optionsPanel;
    public GameObject levelsPanel;
    public GameObject historyPanel;
    public GameObject leaderboardPanel;

    //Metodo que a�ade escuchadores a los elementos de sonido
    private void Awake()
    {
        // Add listeners to the sliders
        master.onValueChanged.AddListener(ChangeVolumeMaster);
        musica.onValueChanged.AddListener(ChangeVolumeMusic);
        sonido.onValueChanged.AddListener(ChangeVolumeFx);
    }

    //Metodo que agrega los botones del menu
    private void Start()
    {
        // Set the range of the sliders
        master.minValue = -60;
        master.maxValue = 20;
        musica.minValue = -60;
        musica.maxValue = 20;
        sonido.minValue = -60;
        sonido.maxValue = 20;

        // Recuperar los valores guardados y establecer los sliders
        master.value = PlayerPrefs.GetFloat("volMaster", 0);
        musica.value = PlayerPrefs.GetFloat("volMusic", 0);
        sonido.value = PlayerPrefs.GetFloat("volFx", 0);

        // Establecer los valores en el AudioMixer
        mixer.SetFloat("volMaster", master.value);
        mixer.SetFloat("volMusic", musica.value);
        mixer.SetFloat("volFx", sonido.value);

        //Array con todos los botones
        List<Button> allButtons = new List<Button>();

        //Botones de la ventana principal
        allButtons.AddRange(mainPanel.GetComponentsInChildren<Button>(true));

        //Botones de la ventana de opciones
        allButtons.AddRange(optionsPanel.GetComponentsInChildren<Button>(true));

        //Botones de la ventana de niveles
        allButtons.AddRange(levelsPanel.GetComponentsInChildren<Button>(true));

        //Asigna un listener a todos los botones
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(PlaySoundButton);
        }
    }

    //Metodo que cierra el juego
    public void ExitGame()
    {
        Application.Quit();
    }

    //Metodo que carga el nivel seleccionado
    public void PlayLevel(String levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    //Metodo que abre un panel
    public void OpenPanel(GameObject panel)
    {
        //Desactiva todos los paneles
        mainPanel.SetActive(false);
        optionsPanel.SetActive(false);
        levelsPanel.SetActive(false);
        historyPanel.SetActive(false);
        leaderboardPanel.SetActive(false);

        //Activa el que queremos ver
        panel.SetActive(true);
    }

    //Metodo que cambia el volumen general
    public void ChangeVolumeMaster(float v)
    {
        mixer.SetFloat("volMaster", v);
        PlayerPrefs.SetFloat("volMaster", v);
        Debug.Log("Master Volume Set " + v);
    }

    //Metodo que cambia el volumen de la musica
    public void ChangeVolumeMusic(float v)
    {
        mixer.SetFloat("volMusic", v);
        PlayerPrefs.SetFloat("volMusic", v);
        Debug.Log("Music Volume Set: " + v);
    }

    //Metodo que cambia el volumen fx
    public void ChangeVolumeFx(float v)
    {
        mixer.SetFloat("volFx", v);
        PlayerPrefs.SetFloat("volFx", v);
        Debug.Log("Fx Volume Set: " + v);
    }

    //Metodo que reproduce un sonido al pulsar un boton del menu
    public void PlaySoundButton()
    {
        fxSource.PlayOneShot(clickSound);
        fxSource.loop = true;
    }
}
