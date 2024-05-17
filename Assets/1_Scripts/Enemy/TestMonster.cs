using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMonster : MonoBehaviour
{
    public Transform target;

    NavMeshAgent nmAgent;
    Collider attack;

    // Start is called before the first frame update
    void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
        attack = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        nmAgent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
    }
}
