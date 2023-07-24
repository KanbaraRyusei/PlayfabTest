using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Cysharp.Threading.Tasks;

public class PlayFabLogin : MonoBehaviour
{
    public bool WasLogin => _wasLogin;

    public bool IsMan => GetPlayerDataAsync().ToString().Contains("man");

    public string DisplayName => GetPlayerDisplayNameAsync().ToString();

    [SerializeField]
    private string _debugUser = "";

    private bool _wasLogin = false;

    async void Awake()
    {
        PlayFabSettings.staticSettings.TitleId = "38269";

        var request = new LoginWithCustomIDRequest
        {
            CustomId = _debugUser + SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        var result = await PlayFabClientAPI.LoginWithCustomIDAsync(request);
        var message = result.Error is null ? $"Login success! PlayFabID:{result.Result.PlayFabId}" : result.Error.GenerateErrorReport();
        Debug.Log(message);

        await GetPlayerDataAsync();
        var s = await GetPlayerDisplayNameAsync();

        Debug.Log(s);

        _wasLogin = true;
    }

    public async void UpdateDisplayName(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        var responce = await PlayFabClientAPI.UpdateUserTitleDisplayNameAsync(request);
        if (responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
        }

        Debug.Log(name);
    }

    public async void UpdatePlayerDataAsync(bool flag)
    {
        string gender = flag ? "man" : "woman";
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> { { "gender", gender } }
        };

        var responce = await PlayFabClientAPI.UpdateUserDataAsync(request);
        if (responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
        }

        Debug.Log(await GetPlayerDataAsync());
    }

    private async UniTask<string> GetPlayerDisplayNameAsync()
    {
        var request = new GetPlayerProfileRequest { };
        var responce = await PlayFabClientAPI.GetPlayerProfileAsync(request);
        if (responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
            return "";
        }
        return responce.Result.PlayerProfile.DisplayName;
    }

    private async UniTask<string> GetPlayerDataAsync()
    {
        var request = new GetUserDataRequest
        {
            Keys = new List<string> { "gender" }
        };
        var responce = await PlayFabClientAPI.GetUserDataAsync(request);
        if (responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
        }
        else
        {
            return (string)responce.Result.Data["gender"].Value;
        }
        return "";
    }
}
