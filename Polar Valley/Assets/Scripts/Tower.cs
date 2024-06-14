using System;
using UnityEngine;

//Clase Torre
[Serializable]
public class Tower
{
    public String name;
    public int cost;
    public GameObject prefab;

    public Tower(String _name, int _cost, GameObject _prefab)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
    }
    
}
