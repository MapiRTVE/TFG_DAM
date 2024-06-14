using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script se encarga de gestionar la seleccion de torres para construirlas
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int selectedTower = 0;
    public bool sellMode = false;

    private void Awake()
    {
        main = this;
    }

    //Metodo para obtener la torre seleccionada
    public Tower GetSelectedTower() 
    {
        return towers[selectedTower];
    }

    //Metodo para establecer el indice de la torre seleccionada
    public void SetSelectedTower(int _selectedTower) 
    { 
        selectedTower = _selectedTower;
    }

    public Tower[] GetAllTowers()
    {
        return towers;
    }

    public bool SellModeOn()
    { 
        return sellMode = true;
    }

    public bool SellModeOff()
    {
        return sellMode = false;
    }

}
   
