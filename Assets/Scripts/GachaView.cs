using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class GachaView : MonoBehaviour
{
    public System.Func<UniTask<string>> OnClickGachaAgainButtonDelegate;

    [SerializeField]
    private Button _againButton;

    [SerializeField]
    private Button _closeButton;

    [SerializeField]
    private TMP_Text _text;

    private void Start()
    {
        _againButton.onClick.AddListener(OnClickGachaAgainButton);
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        OnClickGachaAgainButton();
    }

    private async void OnClickGachaAgainButton()
    {
        var result = await OnClickGachaAgainButtonDelegate.Invoke();

        Debug.Log(result.ToString());

        _text.text = result.ToString();
    }
}
