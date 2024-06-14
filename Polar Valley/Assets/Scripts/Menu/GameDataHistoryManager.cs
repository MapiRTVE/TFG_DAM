using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameDataHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject gameDataHistoryItemPrefab;
    [SerializeField] private Transform contentContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        List<GameData> gameDataHistory = RealmManager.GetRealm().All<GameData>().Where(d => d.UserId == RealmManager.realmUser.Id).ToList();
            
        foreach (GameData gameData in gameDataHistory)
        {
            GameObject gameDataHistoryItem = Instantiate(gameDataHistoryItemPrefab, contentContainer);
            gameDataHistoryItem.GetComponent<GameDataHistoryItem>().SetGameData(gameData);
        }
    }
}
