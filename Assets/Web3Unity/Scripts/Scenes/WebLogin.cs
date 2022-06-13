using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    [SerializeField]
    private Button loginButton;

    private void Start()
    {
        loginButton.onClick.AddListener(OnLogin);
    }

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");

        loginButton.onClick.RemoveAllListeners();
        loginButton.onClick.AddListener(SignMessage);
        loginButton.GetComponentInChildren<TextMeshProUGUI>().text = "Sign";
    }

    async private void SignMessage()
    {
        try
        {
            string message = "Treasure Arcade validating your account";
            string response = await Web3GL.Sign(message);
            Debug.Log(response);

            PlayerPrefs.SetString("Signature", response);

            // load next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}
#endif
