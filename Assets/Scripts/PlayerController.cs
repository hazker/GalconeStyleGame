using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [HideInInspector] public static PlayerController instance;
    [HideInInspector] public List<Planet> homePlanet = new List<Planet>();
    [HideInInspector] public Planet planetToSendShips;

    public GameObject shipPrefab;

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

    public void SendShips(int shipsToSpawn, Planet planet)
    {

        Vector3 pos = Vector2.zero;

        for (int i = 0; i < shipsToSpawn; i++)
        {

            pos = new Vector3(Random.Range(-20, 20), Random.Range(-20, 20)) + planet.transform.position;

            GameObject ship = Instantiate(shipPrefab, pos, Quaternion.identity);
            ShipSetup(ship, planet);
        }

        planet.GetComponent<Planet>().ShipsReduced(shipsToSpawn);

    }

    void ShipSetup(GameObject ship, Planet planet)
    {
        ship.transform.up = planetToSendShips.transform.position - ship.transform.position;
        ship.GetComponent<Ship>().target = planetToSendShips;
        ship.GetComponent<Ship>().shipOwner = ObjectOwner.ControlledByPlayer;

        ship.GetComponent<Ship>().planetToSendShips = planetToSendShips;
        ship.GetComponent<Ship>().homePlanet = planet;
    }
}
