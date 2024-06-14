using MongoDB.Bson;
using Realms.Sync;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpButton : MonoBehaviour
{
    // Referencia al panel de notificación.
    public GameObject notificationPanel;

    // Campos de entrada para email, contraseña, confirmación de contraseña y nombre de usuario.
    public TMP_InputField email, password, passwordConfirm, username;

    // Texto para mostrar mensajes de error.
    public TMP_Text errorText;

    // Método Start se llama al inicio de la escena.
    private void Start()
    {
        // Agrega el método OnClick como listener al evento onClick del botón.
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Método que se llama cuando se hace clic en el botón.
    void OnClick()
    {
        // Verifica que todos los campos estén llenos.
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text) || string.IsNullOrEmpty(passwordConfirm.text))
        {
            ShowDialog("Rellena todos los campos.");
            return;
        }

        // Verifica que las contraseñas coincidan.
        if (password.text != passwordConfirm.text)
        {
            ShowDialog("Las contraseñas no coinciden.");
            return;
        }

        // Intenta registrar un nuevo usuario con email y contraseña.
        RealmManager.GetRealmApp().EmailPasswordAuth.RegisterUserAsync(email.text, password.text).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                string message = task.Exception?.InnerException?.Message ?? task.Exception?.Message;
                ShowDialog("Error interno al iniciar sesión, error: " + message);
                return;
            }

            if (task.IsCanceled)
            {
                ShowDialog("Registro de usuario cancelado.");
                return;
            }

            // Crea credenciales con el email y contraseña.
            Credentials credenciales = Credentials.EmailPassword(email.text, password.text);

            // Intenta iniciar sesión con las credenciales creadas.
            RealmManager.GetRealmApp().LogInAsync(credenciales).ContinueWith(loginTask =>
            {
                if (loginTask.IsFaulted)
                {
                    string message = loginTask.Exception?.InnerException?.Message ?? loginTask.Exception?.Message;
                    ShowDialog("Error interno al iniciar sesión, error: " + message);
                    return;
                }

                if (loginTask.IsCanceled)
                {
                    ShowDialog("Inicio de sesión cancelado.");
                    return;
                }

                // Establecer una variable global con el usuario logueado.
                RealmManager.SetUser(loginTask.Result);

                // Crear el usuario en la base de datos de mongo.
                RealmManager.UpdatePlayer(new GamePlayer
                {
                    Id = ObjectId.GenerateNewId(),
                    UserId = RealmManager.realmUser.Id,
                    IsAdmin = false,
                    IsBanned = false,
                    PlayerName = username.text,
                    Level = 1,
                    Exp = 0
                });

                RealmManager.realmUser.RefreshCustomDataAsync();

                // Ir a la escena del menú principal.
                UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                });
            });
        });
    }

    // Método para mostrar un mensaje de error en el panel de notificación.
    void ShowDialog(string error)
    {
        UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
        {
            // Actualiza el texto de error y muestra el panel de notificación.
            errorText.text = error;
            notificationPanel.SetActive(true);
        });
    }
}