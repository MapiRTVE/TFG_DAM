using TMPro;
using UnityEngine;

public class PlayerLeaderboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text playerPositionText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerLevelText;
    [SerializeField] private TMP_Text playerTopWaveText;
    
    public void SetPlayer(GamePlayer player, long topWave, int position)
    {
        playerPositionText.text = "#" + position;
        playerNameText.text = player.PlayerName;
        playerLevelText.text = player.Level.ToString();
        playerTopWaveText.text = topWave.ToString();
        
        GetComponent<AdminActions>().setPlayerId(player);
    }
}