using Realms;
using Realms.Exceptions;
using Realms.Sync;
using System.Linq;
using UnityEngine;

public class RealmManager : MonoBehaviour
{
    // ID de la aplicación de Realm utilizado para inicializar la aplicación de Realm.
    private static string appId = "polarvalley-pafegay";

    // Instancia estática de la aplicación de Realm.
    private static App realm;

    // Instancia estática de la base de datos de Realm.
    private static Realm realmInstance;

    // Propiedad para mantener al usuario actual de Realm.
    public static User realmUser { get; set; }
    
    // Start se llama cuando se carga la instancia del script.
    private void Start()
    {
        // Evita que este objeto sea destruido al cargar una nueva escena.
        DontDestroyOnLoad(gameObject);

        // Inicializa la aplicación de Realm con el ID especificado.
        realm = App.Create(appId);

        /*
        ELIMINAR ANTES DE ENTREGAR, POR SI CAMBIAMOS LAS COSAS DE NUEVO :caritadiablo:
        
        */
        string realmPath = RealmConfiguration.DefaultConfiguration.DatabasePath;
        Debug.Log(realmPath);
        Realm.DeleteRealm(new RealmConfiguration(realmPath));

        // Inicializa la instancia de Realm con la configuración predeterminada.
        realmInstance = Realm.GetInstance();
    }

    // Devuelve la instancia estática de la aplicación de Realm.
    public static App GetRealmApp()
    {
        return realm;
    }

    // Establece el usuario actual e inicializa la instancia de Realm según corresponda.
    public static void SetUser(User user)
    {
        realmUser = user;

        if (realmUser != null)
        {
            UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
            {
                // Inicializa la instancia de Realm con la configuración del usuario actual.
                realmInstance = Realm.GetInstance(new FlexibleSyncConfiguration(realmUser));

                // Agrega las consultas de sincronización y crea un escuchador que escucha constantemente las actualizaciones de los player y la data
                realmInstance.Subscriptions.Update(() =>
                {
                    var allPlayers = realmInstance.All<GamePlayer>();
                    realmInstance.Subscriptions.Add(allPlayers);

                    var allData = realmInstance.All<GameData>();
                    realmInstance.Subscriptions.Add(allData);
                });
            });
        }
        else
        {
            // Inicializa la instancia de Realm con la configuración predeterminada.
            realmInstance = Realm.GetInstance();
        }
    }

    // Devuelve la instancia estática de la base de datos de Realm.
    public static Realm GetRealm()
    {
        return realmInstance;
    }

    // Recupera un GamePlayer por su ID de usuario, o crea uno nuevo si no se encuentra.
    public static GamePlayer GetPlayer(string id)
    {
        // Consulta a Realm para obtener un GamePlayer con el ID de usuario especificado.
        var player = realmInstance.All<GamePlayer>().FirstOrDefault(p => p.UserId == id);

        // Si no se encuentra ningún jugador, crea un nuevo GamePlayer.
        if (player == null)
        {
            player = new GamePlayer
            {
                UserId = id,
                PlayerName = string.Empty,
                Level = 1
            };
        }

        return player;
    }

    // Actualiza el GamePlayer especificado en la base de datos de Realm.
    public static void UpdatePlayer(GamePlayer player)
    {
        //Aunque la instruccion se esta lanzando desde un hilo principal esto se debe a que realm necesita que funcione en un solo hilo aun asi la tarea se ejecuta de tarea asincrona
        UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
        {
            try
            {
                // Ejecuta la actualización del jugador en el hilo principal de Unity.
                realmInstance.Write(() =>
                {
                    GetRealm().Add(player, update: true);
                });

            }
            catch (RealmException ex)
            {
                Debug.LogError($"RealmException en UpdatePlayer: {ex.Message}");
                Debug.LogError(ex.StackTrace);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Excepción en UpdatePlayer: {ex.Message}");
                Debug.LogError(ex.StackTrace);
            }
        });
    }

    // Guarda los datos de una partida en la base de datos de Realm.
    public static void SaveGameData(GameData gameData)
    {
        try
        {
            // Ejecuta la operación de escritura en el hilo principal de Unity.
            realmInstance.Write(() =>
            {
                GetRealm().Add(gameData, update: true);
            });
        }
        catch (RealmException ex)
        {
            Debug.LogError($"RealmException en SaveGameData: {ex.Message}");
            Debug.LogError(ex.StackTrace);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Excepción en SaveGameData: {ex.Message}");
            Debug.LogError(ex.StackTrace);
        }
    }
}
