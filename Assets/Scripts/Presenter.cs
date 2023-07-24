using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class Presenter : MonoBehaviour
{
    [SerializeField]
    private PlayFabLogin _pl;

    [SerializeField]
    private View _view;

    private async void Start()
    {
        await UniTask.WaitUntil(() => _pl.WasLogin);

        _view.Init(_pl.IsMan, _pl.DisplayName);

        _view.ObserveEveryValueChanged(x => x.IsMan)
            .Subscribe(x => _pl.UpdatePlayerDataAsync(x));

        _view.ObserveEveryValueChanged(x => x.DisplayName).Skip(1)
            .Subscribe(x => _pl.UpdateDisplayName(x));
    }
}
