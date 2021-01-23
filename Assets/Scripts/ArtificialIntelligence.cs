using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialIntelligence : MonoBehaviour
{
    List<Planet> allPlanets;
    List<Planet> humanPlanets = new List<Planet>();
    List<Planet> myPlanets = new List<Planet>();

    public GameObject shipPrefab;

    [HideInInspector]
    public GameSetup gameSetup;

    [HideInInspector] public static ArtificialIntelligence instance;

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

    // Start is called before the first frame update
    public void StartTHINKING(int p, int m)
    {
        allPlanets = MapGenerator.instance.planets;
        humanPlanets.Add(allPlanets[p]);
        myPlanets.Add(allPlanets[m]);
        allPlanets.Remove(allPlanets[p]);
        allPlanets.Remove(allPlanets[m]);
        InvokeRepeating("TimeToConquer", 5, gameSetup.CPUThinkTime);
    }

    public void IntelUpdate(Planet planet)
    {
        switch (planet.planetOwner)
        {
            case ObjectOwner.ControlledByCPU:
                myPlanets.Add(planet);
                humanPlanets.Remove(planet);
                break;
            case ObjectOwner.ControlledByPlayer:
                humanPlanets.Add(planet);
                myPlanets.Remove(planet);
                break;
        }
        if (myPlanets.Count == 0)
        {
            GameController.instance.EndOfTheGame(true);
        }
        if (humanPlanets.Count == 0)
        {
            GameController.instance.EndOfTheGame(false);
        }
        allPlanets.Remove(planet);
    }

    // Update is called once per frame
    void TimeToConquer()
    {
        Debug.Log("DUMAYU");
        foreach (var myPlanet in myPlanets)
        {
            //Debug.Log(myPlanets.Count);
            if (CanICapturedHumanPlanet(myPlanet))
            {
                Debug.Log("DIE HUMAN");
                return;
            }
            if (CanICapturedNeutralPlanet(myPlanet))
            {
                Debug.Log("I AM ASSUMING DIRECT CONTROL");
                return;
            }
        }
        if (ApathyIsDeath(myPlanets[Random.Range(0, myPlanets.Count)]))
        {
            Debug.Log("APATHY IS DEATH");
            return;
        }
    }

    bool CanICapturedHumanPlanet(Planet myPlanet)
    {

        foreach (var enemy in humanPlanets)
        {
            if (myPlanet.GetShipsCount() / 2 > enemy.GetShipsCount())
            {
                SendShips(Mathf.RoundToInt(myPlanet.GetShipsCount() / 2), myPlanet, enemy);
                return true;
            }
        }
        return false;
    }

    bool CanICapturedNeutralPlanet(Planet myPlanet)
    {

        foreach (var enemy in allPlanets)
        {
            if (myPlanet.GetShipsCount() / 2 > enemy.GetShipsCount())
            {
                SendShips(Mathf.RoundToInt(myPlanet.GetShipsCount() / 2), myPlanet, enemy);
                return true;
            }
        }
        return false;
    }

    bool ApathyIsDeath(Planet myPlanet)
    {
        SendShips(Mathf.RoundToInt(myPlanet.GetShipsCount() / 2), myPlanet, humanPlanets[Random.Range(0, humanPlanets.Count)]);
        return true;
    }

    public void SendShips(int shipsToSpawn, Planet homePlanet, Planet planetToSendShips)
    {
        Vector3 pos = Vector2.zero;
        for (int i = 0; i < shipsToSpawn; i++)
        {

            pos = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20)) + homePlanet.transform.position;

            GameObject ship = Instantiate(shipPrefab, pos, Quaternion.identity);
            ShipSetup(ship, homePlanet, planetToSendShips);

        }
        homePlanet.GetComponent<Planet>().ShipsReduced(shipsToSpawn);
    }

    void ShipSetup(GameObject ship, Planet homePlanet, Planet planetToSendShips)
    {
        ship.transform.up = planetToSendShips.transform.position - ship.transform.position;
        ship.GetComponent<Ship>().target = planetToSendShips;
        ship.GetComponent<Ship>().shipOwner = ObjectOwner.ControlledByCPU;

        ship.GetComponent<Ship>().planetToSendShips = planetToSendShips;
        ship.GetComponent<Ship>().homePlanet = homePlanet;
    }

}
