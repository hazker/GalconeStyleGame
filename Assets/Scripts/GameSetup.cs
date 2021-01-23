using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Scriptable Object/GameSetup",fileName ="New Setup")]

public class GameSetup : ScriptableObject
{
    public int planetsCount;
    public List<Sprite> planetTextures;
    public int otherObjects;
    public List<GameObject> otherObjectsPrefabs;
    public int planetShipsCapacity;
    public int shipProduces;
    public int timeForShipProduce;   
    public int CPUThinkTime;

}
