using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [HideInInspector] public static UIController instance;
    public List<GameSetup> GameSetups;
    public GameObject Button;
    public GameObject ButtonHolder;

    public GameObject startScreen;
    public GameObject difficultySetup;
    public GameObject winScreen;
    public Text winnerText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
    }

    private void Start()
    {
        startScreen.SetActive(true);
        foreach (var setup in GameSetups)
        {
            GameObject button = Instantiate(Button,ButtonHolder.transform);
            button.GetComponentInChildren<Text>().text = setup.name;
            Button responceButton = button.gameObject.GetComponent<Button>();
            if (responceButton)
            {
                responceButton.onClick.AddListener(delegate { difficultyChoosen(setup); });
            }
        }
    }

    public void winner(string winner)
    {
        winScreen.SetActive(true);
        winnerText.text = winner;
    }

    public void difficultyChoosen(GameSetup diff)
    {
        GameController.instance.ReadyToBegin(diff);
        difficultySetup.gameObject.SetActive(false);
    }
}
