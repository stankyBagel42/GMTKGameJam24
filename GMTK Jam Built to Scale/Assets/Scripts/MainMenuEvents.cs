using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{

    [SerializeField]
    private UIDocument levelSelect;

    private UIDocument _document;
    private Button _playBtn;
    private Button _settingsBtn;
    private Button _exitBtn;


    // Start is called before the first frame update
    void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        _playBtn = _document.rootVisualElement.Q("PlayBtn") as Button;
        _settingsBtn = _document.rootVisualElement.Q("SettingsBtn") as Button;
        _exitBtn = _document.rootVisualElement.Q("ExitBtn") as Button;
        _playBtn.RegisterCallback<ClickEvent>(OnPlayClick);
    }

    void OnDisable()
    {
        _playBtn.UnregisterCallback<ClickEvent>(OnPlayClick);   
    }

    private void OnPlayClick(ClickEvent evt){
        gameObject.SetActive(false);
        levelSelect.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
