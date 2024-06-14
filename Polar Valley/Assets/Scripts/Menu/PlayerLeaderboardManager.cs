using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerLeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject lederboardItemPrefab;
    [SerializeField] private Transform contentContainer;

    private void Start()
    {
        List<GamePlayer> playerLeaderboard = RealmManager.GetRealm().All<GamePlayer>().OrderByDescending(p => p.Level).ToList();
        
        int position = 0;
        
        foreach (GamePlayer player in playerLeaderboard)
        {
            GameData bestGameData = RealmManager.GetRealm().All<GameData>().Where(d => d.UserId == player.UserId).OrderByDescending(d => d.MaxWave).FirstOrDefault();
            var topWave = bestGameData?.MaxWave ?? 0;
            GameObject leaderboardItem = Instantiate(lederboardItemPrefab, contentContainer);
            // aumentar la posición en 1
            position++;
            leaderboardItem.GetComponent<PlayerLeaderboardItem>().SetPlayer(player, topWave, position);
        }
    }
}