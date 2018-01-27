using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public Button startButton, quitGame;

	// Use this for initialization
	void Start ()
    {
        startButton.onClick.AddListener(StartGame);
        quitGame.onClick.AddListener(QuitGame);	
	}
	
    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
