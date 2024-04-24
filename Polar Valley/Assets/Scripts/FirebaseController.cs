using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using System.Net.Mail;
using Firebase;
using UnityEngine.SceneManagement;

public class FirebaseController : MonoBehaviour
{


    public GameObject loginPanel, signUpPanel, notificationPanel;
    public TMP_InputField username, password, registerUsername, registerPassword, registerConfirmPassword;
    public TMP_Text errorText;
    private FirebaseAuth auth;
    private FirebaseUser user;


    public void openLoginPanel()
    { 
        loginPanel.SetActive(true);
        signUpPanel.SetActive(false);
    }

    public void openSignUpPanel()
    {
        loginPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(username.text) && string.IsNullOrEmpty(password.text)) {
            showErrorDialog("Rellena todos los campos.");   
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(username.text, password.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

        });
    }

    public void showErrorDialog(string error)
    { 
        errorText.text = error;
        notificationPanel.SetActive(true);
    }

    public void closeErrorDialog() { 
        errorText.text = string.Empty;
        notificationPanel.SetActive(false);
    }

    public void SignUpUser() {
        if (string.IsNullOrEmpty(registerUsername.text) && string.IsNullOrEmpty(registerPassword.text) && string.IsNullOrEmpty(registerConfirmPassword.text)) {
            showErrorDialog("Rellena todos los campos.");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(registerUsername.text, registerPassword.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                result.User.DisplayName, result.User.UserId);

        });
    }

    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null
                && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            InitializeFirebase();
        });
    }
}
