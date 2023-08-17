using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaPresenter : MonoBehaviour
{
    [SerializeField]
    private GachaView _view;

    [SerializeField]
    private PlayFabLogin _model;

    private void Start()
    {
        _view.OnClickGachaAgainButtonDelegate += _model.DrawGachaAndGetResultName;
    }
}
