using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{

    public static UserManager main;


    // Ajustes para el cálculo dinámico de XP
    public float baseXP = 100f; // XP base para el primer nivel
    public float exponent = 1.5f; // Exponente para el incremento de XP


    private void Awake()
    {
        main = this;
        DontDestroyOnLoad(this);
    }

    public void AddExperience(GamePlayer player, long exp)
    {
        RealmManager.GetRealm().WriteAsync(() =>
        {
            //Añado la exp al jugador
            player.Exp += exp;

            //Calculamos si tiene que subir de nivel
            var currentLevel = player.Level ?? 1;
            var currentExp = player.Exp ?? 0;
            var expToNextLevel = CalculateXPForNextLevel(currentLevel);

            if (currentExp >= expToNextLevel)
            {
                player.Level++;
                player.Exp = 0;
            }
        });
    }


    // Método para calcular la XP requerida para el siguiente nivel
    private long CalculateXPForNextLevel(int level)
    {
        // Asegurarse de que el nivel sea al menos 1
        level = Mathf.Max(level, 1);

        // Incremento exponencial de XP: puedes ajustar la base y el exponente según tu diseño de juego
        return (long)(baseXP * Mathf.Pow(level, exponent));
    }

    public GamePlayer getCurrentPlayer()
    {
        return RealmManager.GetPlayer(RealmManager.realmUser.Id);
    }

}
