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
        await GetPlayerDisplayNameAsync();

        var jsonString = await GetTitleDataAsync();
        var monsterData = JsonUtility.FromJson<MonstersArray>(jsonString);

        if(monsterData != null)
        {
            for (int i = 0; i < monsterData.monsters.Length; i++)
            {
                Debug.Log(monsterData.monsters[i].name + " HP" + monsterData.monsters[i].hp);
            }
        }

        var jsonItems = await GetItemDataAsync();
        var itemData = JsonUtility.FromJson<ItemsArray>(jsonItems);

        if (itemData != null)
        {
            for (int i = 0; i < itemData.items.Length; i++)
            {
                Debug.Log(itemData.items[i].name + " Price" + itemData.items[i].price);
            }
        }

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

    private async UniTask<string> GetTitleDataAsync()
    {
        var request = new GetTitleDataRequest
        {
            Keys = new List<string> { "monster" }
        };

        var responce = await PlayFabClientAPI.GetTitleDataAsync(request);

        if(responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
        }
        else
        {
            return responce.Result.Data["monster"];
        }

        return "";
    }

    private async UniTask<string> GetItemDataAsync()
    {
        var request = new GetTitleDataRequest
        {
            Keys = new List<string> { "item" }
        };

        var responce = await PlayFabClientAPI.GetTitleDataAsync(request);

        if(responce.Error != null)
        {
            Debug.Log(responce.Error.GenerateErrorReport());
        }
        else
        {
            return responce.Result.Data["item"];
        }

        return "";
    }
}

[System.Serializable]
public class MonsterData
{
    public int id;
    public int hp;
    public string name;
    public bool isHuman;
}
public class MonstersArray
{
    public MonsterData[] monsters;
}

[System.Serializable]
public class ItemData
{
    public int id;
    public string name;
    public string efficacy;
    public int price;
}

public class ItemsArray
{
    public ItemData[] items;
}
