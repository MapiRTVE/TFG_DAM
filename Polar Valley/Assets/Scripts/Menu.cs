using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu main;

    [Header("References")]
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] Transform buttonContainer;
    [SerializeField] Button sellButton;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;
    private Button selectedButton;
    private Tower[] towers;
    private ArrayList buttons = new ArrayList();


    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        GenerateTowerButtons();
        ToggleTurretButtons();
        sellButton.onClick.AddListener(SellTurret);      
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("ShopOpen", isMenuOpen);
    }


    private void GenerateTowerButtons()
    {
        towers = BuildingManager.main.GetAllTowers();

        foreach (Tower tower in towers)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
            buttonObj.name = tower.name;

            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = $"{tower.name} - {tower.cost}";

            Button button = buttonObj.GetComponent<Button>();

            if (tower == BuildingManager.main.GetSelectedTower())
            {
                button.image.color = Color.blue;
                selectedButton = button;
            }
            else
            {
                button.image.color = Color.white;
            }

            button.onClick.AddListener(() => OnTowerButtonClicked(button, tower));
            buttons.Add(button);
        }
    }

    private void OnTowerButtonClicked(Button button, Tower tower)
    {
        selectedButton.image.color = Color.white;
        button.image.color = Color.blue;
        selectedButton = button;

        Tower[] towers = BuildingManager.main.GetAllTowers();
        int towerIndex = System.Array.IndexOf(towers, tower);
        BuildingManager.main.SetSelectedTower(towerIndex);
        BuildingManager.main.SellModeOff();
        sellButton.image.color = Color.white;
    }

    private void SellTurret()
    { 
        selectedButton.image.color = Color.white;
        sellButton.image.color = Color.blue;
        BuildingManager.main.SellModeOn();
    }

    public void ToggleTurretButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = (Button)buttons[i];
            Tower tower = towers[i];
            if (LevelManager.main.CheckCurrency(tower.cost))
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}
