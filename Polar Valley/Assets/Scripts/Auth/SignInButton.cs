using System;
using Realms.Sync;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInButton : MonoBehaviour
{
    public GameObject notificationPanel;
    public TMP_InputField email, password;
    public TMP_Text errorText;
    
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick() {
        if (string.IsNullOrEmpty(email.text) || string.IsNullOrEmpty(password.text)) {
            ShowErrorDialog("Rellena todos los campos.");   
            return;
        }
        
        Credentials creadenciales = Credentials.EmailPassword(email.text, password.text);
        RealmManager.GetRealmApp().LogInAsync(creadenciales).ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                ShowErrorDialog("Error al iniciar sesión, verifica tus credenciales.");
                return;
            }

            if (task.IsCanceled)
            {
                ShowErrorDialog("Inicio de sesión cancelado.");
                return;
            }

            // Establecer una variable global con el usuario logueado y obtener usuario logueado y su id
            RealmManager.SetUser(task.Result);
            
            // Verificar si el usuario está baneado
            if (task.Result.GetCustomData<GamePlayer>().IsBanned ?? false) {
                ShowErrorDialog("Tu cuenta ha sido baneada. Tienes prohibido el acceso a Polar Valley.");
                
                RealmManager.SetUser(null); // Cerrar sesión
                return;
            }
                
            // Ir a la escena del menu
            UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            });
        });
    }
    
    void ShowErrorDialog(string error)
    {
        UnityMainThreadDispatcher.Dispatcher.Enqueue(() =>
        {
            errorText.text = error;
            notificationPanel.SetActive(true);
        });
    }
    
}
