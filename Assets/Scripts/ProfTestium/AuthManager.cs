using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    private string baseUrl = "http://xn----7sbpbfclakh1al9a7fxc.xn--p1ai:8000/";
    private string signInEndpoint = "users/sign_in";
    
    [SerializeField] public GameObject modulesCanvas ;
    [SerializeField] public GameObject authCanvas ;
    
    public IEnumerator LoginUser(string email, string password)
    {
        string url = baseUrl + signInEndpoint;

        Dictionary<string, string> bodyData = new Dictionary<string, string>
        {
            { "email", email },
            { "password", password }
        };
        string jsonData = JsonConvert.SerializeObject(bodyData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("ngrok-skip-browser-warning", "69420");
        
        yield return request.SendWebRequest();
        Debug.Log(request.responseCode);
        string responseText = request.downloadHandler.text;
        print(responseText);
        
        if (request.responseCode == 200)
        {
            Debug.Log("success");
            AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(request.downloadHandler.text);
            string accessToken = authResponse.tokenPair.accessToken;
            string refreshToken = authResponse.tokenPair.refreshToken;
            PlayerPrefs.SetString("AccessToken", accessToken);
            PlayerPrefs.SetString("RefreshToken", refreshToken);
            authCanvas.SetActive(false);
            modulesCanvas.SetActive(true);
        }
        else
        {
            Debug.LogError(request.responseCode);
        }
    }
    
    [System.Serializable]
    public class TokenPair
    {
        public string accessToken;
        public string refreshToken;
    }

    [System.Serializable]
    public class AuthResponse
    {
        public TokenPair tokenPair;
        public string role;
    }
    
    
    
    
}
