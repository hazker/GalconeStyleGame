using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [HideInInspector] public static GameController instance;
    GameSetup gameSetup;

    bool gameEnded = false;
    string winner = "";

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

    void StartTheGame()
    {
        int playerPlanet = Random.Range(0, MapGenerator.instance.planets.Count);
        int enemyPlanet = Random.Range(0, MapGenerator.instance.planets.Count);

        while (enemyPlanet == playerPlanet)
        {
            enemyPlanet = Random.Range(0, MapGenerator.instance.planets.Count);
        }

        MapGenerator.instance.planets[playerPlanet].GetComponent<Planet>().GettingControl(ObjectOwner.ControlledByPlayer);
        MapGenerator.instance.planets[playerPlanet].GetComponent<Planet>().ShipsChanges(50);
        MapGenerator.instance.planets[enemyPlanet].GetComponent<Planet>().GettingControl(ObjectOwner.ControlledByCPU);
        MapGenerator.instance.planets[enemyPlanet].GetComponent<Planet>().ShipsChanges(50);

        ArtificialIntelligence.instance.StartTHINKING(playerPlanet, enemyPlanet);

    }

    public void EndOfTheGame(bool t)
    {
        gameEnded = true;
        if (t)
        {
            winner = "Player Has Won";
            Time.timeScale = 0;
        }
        else
        {
            winner = "CPU defeated";
            Time.timeScale = 0;
        }
        UIController.instance.winner(winner);
    }

    public void ReadyToBegin(GameSetup diff)
    {
        gameSetup = diff;
        StartCoroutine(Begininng());
    }

    public IEnumerator Begininng()
    {
        MapGenerator.instance.gameSetup = gameSetup;
        ArtificialIntelligence.instance.gameSetup = gameSetup;
        MapGenerator.instance.GenerateMap();
        yield return new WaitForFixedUpdate();
        StartTheGame();
    }

}
