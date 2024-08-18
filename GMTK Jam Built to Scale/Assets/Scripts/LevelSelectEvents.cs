using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectEvents : MonoBehaviour
{
    [SerializeField]
    public UIDocument mainMenu;

    [SerializeField]
    public Scene[] levels;

    private Button[] buttons;
    private UIDocument _document;

    
    void OnEnable()
    {
        _document = GetComponent<UIDocument>();
        buttons = _document.rootVisualElement.Query<Button>().ToList().ToArray<Button>();
        for(int i = 0; i < buttons.Length; i ++){
            buttons[i].RegisterCallback<ClickEvent>(evt => OnClick(i));
        }
    }

    void OnClick(int button_number){
        if(button_number >= levels.Length){
            gameObject.SetActive(false);
            mainMenu.gameObject.SetActive(true);
        }else{
            SceneManager.LoadScene(levels[button_number].name);
        }
    }
}