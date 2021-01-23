using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ObjectOwner
{
    ControlledByPlayer,
    ControlledByCPU,
    Neutral
}

public class Planet : MonoBehaviour
{
    [HideInInspector]
    public int shipsCapacity;
    [HideInInspector]
    public int shipProduces;
    [HideInInspector]
    public int timeToproduce;

    public TextMesh shipsCountUI;

    public GameObject selectedPlanet;

    [HideInInspector]
    public bool selected = false;
    public ObjectOwner planetOwner;
    int shipsCount;

    private void Start()
    {
        shipsCount = Random.Range(0, shipsCapacity / 2);
        displayShipsCount();

        SelectObjects.unit.Add(gameObject);

    }

    void ProduceShip()
    {
        for (int i = 0; i < shipProduces; i++)
        {
            if (shipsCount < shipsCapacity)
            {
                shipsCount++;
                displayShipsCount();
            }

        }

    }


    public void GettingControl(ObjectOwner owner)
    {
        planetOwner = owner;
        switch (planetOwner)
        {
            case ObjectOwner.ControlledByCPU:
                GetComponent<SpriteRenderer>().color = Color.red;
                shipsCountUI.color = Color.magenta;
                break;
            case ObjectOwner.ControlledByPlayer:
                GetComponent<SpriteRenderer>().color = Color.blue;
                shipsCountUI.color = Color.green;
                break;
            case ObjectOwner.Neutral:
                GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }

    }

    void displayShipsCount()
    {
        shipsCountUI.text = shipsCount.ToString();
    }

    public int GetShipsCount()
    {
        return shipsCount;
    }

    public void ShipsChanges(int v)
    {

        shipsCount = v;
        displayShipsCount();
        InvokeRepeating("ProduceShip", 0, timeToproduce);
    }

    private void OnMouseOver()
    {
        selectedPlanet.SetActive(true);
        if (Input.GetMouseButtonDown(1))
        {
            if (PlayerController.instance.homePlanet.Count > 0 && (planetOwner == ObjectOwner.ControlledByCPU || planetOwner == ObjectOwner.Neutral))
            {
                PlayerController.instance.homePlanet = PlayerController.instance.homePlanet.Distinct().ToList();
                PlayerController.instance.planetToSendShips = this;
                foreach (var item in PlayerController.instance.homePlanet)
                {

                    int shipsToSpawn = Mathf.RoundToInt(item.shipsCount / 2);
                    PlayerController.instance.SendShips(shipsToSpawn, item);
                    item.displayShipsCount();
                }


            }
        }

    }

    private void OnMouseExit()
    {
        if (!selected)
        {
            selectedPlanet.SetActive(false);
        }

    }

    private void OnMouseDown()
    {
        if (planetOwner == ObjectOwner.ControlledByPlayer)
        {
            selected = true;
            if (PlayerController.instance.homePlanet.Count > 0)
            {
                foreach (var item in PlayerController.instance.homePlanet)
                {
                    if (item != null)
                        item.selectedPlanet.SetActive(false);

                }
            }
            PlayerController.instance.homePlanet.Clear();
            PlayerController.instance.homePlanet.Add(this);
            selectedPlanet.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 9)
        {
            //Debug.Log(other.name);
            if (shipsCount > 0 && this == other.GetComponent<Ship>().planetToSendShips && planetOwner != other.GetComponent<Ship>().shipOwner)
            {

                shipsCount--;
                displayShipsCount();
                Destroy(other.gameObject);
                return;
            }

            if (shipsCount == 0 && this == other.GetComponent<Ship>().planetToSendShips)
            {

                shipsCount++;
                if (planetOwner == ObjectOwner.Neutral)
                {
                    InvokeRepeating("ProduceShip", 0, timeToproduce);
                }
                GettingControl(other.GetComponent<Ship>().shipOwner);
                Destroy(other.gameObject);
                displayShipsCount();
                ArtificialIntelligence.instance.IntelUpdate(this);
                return;


            }
            if (shipsCount > 0 && this == other.GetComponent<Ship>().planetToSendShips && planetOwner == other.GetComponent<Ship>().shipOwner)
            {
                shipsCount++;
                Destroy(other.gameObject);
                displayShipsCount();
                return;
            }

        }

    }

    public void ShipsReduced(int v)
    {
        shipsCount -= v;
        displayShipsCount();
    }

}
