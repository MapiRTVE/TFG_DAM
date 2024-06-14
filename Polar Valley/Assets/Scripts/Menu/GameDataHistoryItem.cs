using System.Globalization;
using TMPro;
using UnityEngine;

public class GameDataHistoryItem : MonoBehaviour
{
    [SerializeField] private TMP_Text gameLevelText;
    [SerializeField] private TMP_Text gameDateText;
    [SerializeField] private TMP_Text gameDurationText;
    [SerializeField] private TMP_Text maxWaveText;
    [SerializeField] private TMP_Text spendMoneyText;
    
    public void SetGameData(GameData gameData)
    {
        gameLevelText.text = "Nivel " + (gameData.LevelId ?? "Tutorial");
        gameDateText.text = gameData.GameDate?.ToString("yyyy-MM-dd HH:mm") ?? "yyyy-MM-dd HH:mm";
        gameDurationText.text = FormatTime(gameData.GameDuration ?? 0);
        maxWaveText.text = gameData.MaxWave?.ToString() ?? "N/A";
        spendMoneyText.text = gameData.SpendMoney?.ToString("N0", CultureInfo.InvariantCulture) ?? "N/A";
    }
    
    public static string FormatTime(long totalSeconds)
    {
        long minutes = totalSeconds / 60;
        long seconds = totalSeconds % 60;

        return $"{minutes}m {seconds}s";
    }
}