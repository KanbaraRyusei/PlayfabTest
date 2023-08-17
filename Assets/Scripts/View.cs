using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class View : MonoBehaviour
{
    public string DisplayName => _inputField.text;

    public bool IsMan => _isMan;

    public System.Action OnClickGachaButtonDelegate;

    [SerializeField]
    private Button _manButton;

    [SerializeField]
    private Button _womanButton;

    [SerializeField]
    private Image _manCheck;

    [SerializeField]
    private Image _womanCheck;

    [SerializeField]
    private TMP_InputField _inputField;

    [SerializeField]
    private Button _gachaButton;

    [SerializeField]
    private GameObject _gachaPanel;

    private bool _isMan = false;

    public void Init(bool flag, string name)
    {
        _manCheck.enabled = flag;
        _womanCheck.enabled = !flag;

        _isMan = flag;

        _inputField.text = name;

        _manButton.onClick.AddListener(OnClickManButton);
        _womanButton.onClick.AddListener(OnClickWomanButton);

        _gachaButton.onClick.AddListener(OnClickGachaButton);
    }

    private void OnClickManButton()
    {
        _isMan = true;

        _manCheck.enabled = _isMan;
        _womanCheck.enabled = !_isMan;
    }

    private void OnClickWomanButton()
    {
        _isMan = false;

        _manCheck.enabled = _isMan;
        _womanCheck.enabled = !_isMan;
    }

    private void OnClickGachaButton()
    {
        //OnClickGachaButtonDelegate?.Invoke();

        _gachaPanel.SetActive(true);
        gameObject.SetActive(false);
    }
}
