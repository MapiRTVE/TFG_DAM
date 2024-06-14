using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Clase que representa el terreno de juego
public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    //Objeto torre construido en el mapa
    private GameObject towerObj;
    //Script de la torre
    public Turret turret;
    private Color startColor;
    private bool notUpgradeableTurret = false;


    //Metodo que guarda el color del terreno
    private void Start()
    {
        startColor = sr.color;
    }

    //Metodo que cambia el color del terreno a pasar el raton
    private void OnMouseEnter() 
    { 
        sr.color = hoverColor;  
    }

    //Metodo que cambia el color del terreno al pasar el raton
    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    //Metodo que detecta los clicks del raton
    private void OnMouseDown()
    {
        //Si el puntero esta sobre un elemento de la interfaz no hace nada (impide que pulsemos el terreno a traves de los elemntos de interfaz)
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //Si hay elementos de la interfaz superpuestos sobre el terreno no hace nada
        if (UIManager.main.IsHoveringUI()) return;

        //Si hay una torre en el terreno se abre el menu de mejora de la torre
        if (towerObj != null && !notUpgradeableTurret) {
            turret.OpenUpgradeUI();
            return;
        }

        if (!BuildingManager.main.sellMode)
        {
            //Obtiene la torre de la tienda seleccionada 
            Tower towerToBuild = BuildingManager.main.GetSelectedTower();

            //Si el precio de la torre es mayor que el dinero del jugador no hace nada 
            if (towerToBuild.cost > LevelManager.main.currency)
            {
                Debug.Log("You cant afford this tower");
                return;
            }

            //En caso contrario gasta el dinero del jugador
            LevelManager.main.SpendCurrency(towerToBuild.cost);

            //Coloca la torre donde hayamos pulsado del terreno
            towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
            turret = towerObj.GetComponent<Turret>();

            if (turret == null) { 
                notUpgradeableTurret = true;
            }
        }
    }
}
