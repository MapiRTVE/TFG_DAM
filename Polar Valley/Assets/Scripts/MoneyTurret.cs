using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] private int baseUpgradeCost = 100;

    private int level = 1;

    private void Start()
    {
        // Llama al método GenerateMoney cada 5 segundos, empezando después de 5 segundos
        InvokeRepeating("GenerateMoney", 5f, 5f);
    }

    //Metodo para abrir la interfaz de mejora de la torre
    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    //Metodo para cerrar la interfaz de mejora de la torre
    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    //Metodo para mejorar la torre
    public void UpgradeTurret()
    {
        //Si no se tiene el suficiente dinero no hace nada
        if (CalculateCost() > LevelManager.main.currency) return;

        //Gasta el dinero necesario para la mejora e incrementa un nivel
        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        CloseUpgradeUI();
    }

    //Metodo para calcular el costo de la mejora (cada nivel aumenta)
    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    // Método para generar dinero cada 5 segundos
    private void GenerateMoney()
    {
        // Aumenta el dinero que se gana cada 5 segundos
        LevelManager.main.IncreaseCurrency(100);
    }
}
