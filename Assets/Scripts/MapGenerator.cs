using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    [HideInInspector] public static MapGenerator instance;
    [HideInInspector]
    public GameSetup gameSetup;
    public GameObject planetPrefab;
    public List<Planet> planets;
    public Transform parent;

    public NavMeshSurface surface;

    int MapWidth = 1000;
    int MapHeight = 500;

    public int radius;


    float currentRadius;

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

    public void GenerateMap()
    {
        for (int i = 0; i < gameSetup.planetsCount; i++)
        {
            PlacePlanet(planetPrefab);
        }
        for (int i = 0; i < gameSetup.otherObjects; i++)
        {
            PlacePlanet(gameSetup.otherObjectsPrefabs[Random.Range(0, gameSetup.otherObjectsPrefabs.Count)]);
        }
        surface.BuildNavMesh();
    }

    private bool PreventSpawnOverlap(Vector2 pos)
    {
        if (Physics.OverlapSphere(new Vector3(pos.x, 0, pos.y), radius, 1 << 8).Length > 0)
        {
            return false;
        }

        return true;
    }

    void PlacePlanet(GameObject objectToSpawn)
    {
        bool canSpawnHere = false;
        Vector2 pos = Vector2.zero;
        int breakCond = 0;
        while (!canSpawnHere)
        {
            currentRadius = Random.Range(0.8f, 7.5f);
            pos = new Vector2(Random.Range(-MapWidth, MapWidth), Random.Range(-MapHeight, MapHeight));
            canSpawnHere = PreventSpawnOverlap(pos);
            breakCond++;
            if (canSpawnHere)
            {
                GameObject planet = Instantiate(objectToSpawn, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
                if (planet.GetComponent<Planet>())
                    PlanetSetup(planet);
                else
                    ObjectSetup(planet);
                break;
            }
            if (breakCond > 100)
            {
                break;
            }
        }

        void ObjectSetup(GameObject planet)
        {
            planet.transform.localScale = new Vector3((float)planet.transform.localScale.x * currentRadius, (float)planet.transform.localScale.y * currentRadius, planet.transform.localScale.z * currentRadius);
        }

        void PlanetSetup(GameObject planet)
        {
            planets.Add(planet.GetComponent<Planet>());
            planet.transform.Rotate(new Vector3(90, 0, 0));
            planet.GetComponent<Planet>().GettingControl(ObjectOwner.Neutral);
            planet.transform.localScale = new Vector3((float)planet.transform.localScale.x * currentRadius, (float)planet.transform.localScale.y * currentRadius, planet.transform.localScale.z);
            planet.GetComponent<SpriteRenderer>().sprite = gameSetup.planetTextures[Random.Range(0, gameSetup.planetTextures.Count)];
            planet.GetComponent<Planet>().shipsCapacity = gameSetup.planetShipsCapacity;
            planet.GetComponent<Planet>().shipProduces = gameSetup.shipProduces;
            planet.GetComponent<Planet>().timeToproduce = gameSetup.timeForShipProduce;
        }

    }
}
