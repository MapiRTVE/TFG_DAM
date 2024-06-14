using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminActions : MonoBehaviour
{

    [SerializeField] private GameObject adminBanButton;

    private String userId;
    
    // Start is called before the first frame update
    void Start()
    {
        if (RealmManager.realmUser.GetCustomData<GamePlayer>().IsAdmin ?? false)
        {
            adminBanButton.SetActive(true);
            
            adminBanButton.GetComponent<Button>().onClick.AddListener(OnClick);
        }
        else
        {
            adminBanButton.SetActive(false);
        }
    }

    void OnClick()
    {
        // Banear al jugador
        GamePlayer player = RealmManager.GetPlayer(userId);
        
        RealmManager.GetRealm().WriteAsync(() =>
        {
            player.IsBanned = !(player.IsBanned ?? false);
            
            UpdateText(player);
        });
    }

    public void setPlayerId(GamePlayer player)
    {
        userId = player.UserId;
        
        UpdateText(player);
    }

    private void UpdateText(GamePlayer player)
    {
        var buttonText = adminBanButton.GetComponentInChildren<TMP_Text>();
        
        if (player.IsBanned ?? false)
        {
            buttonText.SetText("Eliminar bloqueo");
        }
        else
        {
            buttonText.SetText("Bloquear jugador");
        }
    }
}
