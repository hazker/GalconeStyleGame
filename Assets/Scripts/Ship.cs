using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ship : MonoBehaviour
{
    public ObjectOwner shipOwner;

    //[HideInInspector]
    public Planet homePlanet;
    //[HideInInspector]
    public Planet planetToSendShips;

    public Planet target;
    NavMeshAgent agent;

    public float acceleration = 2f;
    public float deceleration = 60f;
    public float closeEnoughMeters = 4f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //InvokeRepeating("UpdateRoute", 0,5);
        UpdateRoute();
    }

    void UpdateRoute()
    {
        agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //agent.SetDestination(target.transform.position);
        if (agent.hasPath)
            agent.acceleration = (agent.remainingDistance < closeEnoughMeters) ? deceleration : acceleration;
        //transform.LookAt(target.transform.position);
        

        //Vector3 movement = transform.forward * Time.deltaTime;

        //agent.Move(movement);
    }


}
