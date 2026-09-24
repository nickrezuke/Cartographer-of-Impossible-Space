using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

private Button _startButton;
    private Button _exitButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        //Gets references to the buttons from the UXML.
        _startButton = _document.rootVisualElement.Q<Button>("StartGameButton");
        _exitButton = _document.rootVisualElement.Q<Button>("ExitButton");

        //Connects button click events.
        if (_startButton != null)
            _startButton.clicked += OnStartClicked;

        if (_exitButton != null)
            _exitButton.clicked += OnExitClicked;
    }

    private void OnDestroy()
    {
        //Removes button events when object is destroyed.
        if (_startButton != null)
            _startButton.clicked -= OnStartClicked;

        if (_exitButton != null)
            _exitButton.clicked -= OnExitClicked;
    }

    private void OnStartClicked()
    {
        Debug.Log("Start clicked");

        //Loads the gameplay scene.
        SceneManager.LoadScene("SampleScene");
    }

    private void OnExitClicked()
    {
        Debug.Log("Exit clicked");

        Application.Quit();
    }

}
