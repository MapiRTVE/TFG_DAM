using TMPro;
using UnityEngine;

public class LevelBarManager : MonoBehaviour
{
    private UnityEngine.UI.Image progressBar;
    
    [SerializeField] private GameObject levelBar;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text experienceText;
    [SerializeField] private TMP_Text percentageText;
    
    // Start is called before the first frame update
    void Start()
    {
        progressBar = levelBar.GetComponent<UnityEngine.UI.Image>();
        
        GamePlayer player = RealmManager.GetPlayer(RealmManager.realmUser.Id);
        
        long exp = player.Exp ?? 0;
        long neededExp = CalculateXPForNextLevel(player.Level ?? 1);
        
        float percentage = (float) exp / neededExp;
        progressBar.fillAmount = percentage;
        
        nextLevelText.text = $"Nivel {player.Level + 1}";
        currentLevelText.text = $"Nivel {player.Level}";
        
        experienceText.text = $"{exp} / {neededExp} XP";
        percentageText.text = $"{(int) (percentage * 100)}%";
    }
    
    // TODO: Eliminar esto
    
    // Ajustes para el cálculo dinámico de XP
    public float baseXP = 100f; // XP base para el primer nivel
    public float exponent = 1.5f; // Exponente para el incremento de XP
    
    private long CalculateXPForNextLevel(int level)
    {
        // Asegurarse de que el nivel sea al menos 1
        level = Mathf.Max(level, 1);

        // Incremento exponencial de XP: puedes ajustar la base y el exponente según tu diseño de juego
        return (long)(baseXP * Mathf.Pow(level, exponent));
    }
}
