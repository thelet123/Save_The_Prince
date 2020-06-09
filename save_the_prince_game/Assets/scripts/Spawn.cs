using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{
    public GameObject[] enemytospawn;
    private GameObject[] animalstospawn;
    private GameObject[] livestospawn;
    private GameObject Player;
    public float MinDistFollowA = 40;
    public float MinDistFollowAD = 10;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        enemytospawn = GameObject.FindGameObjectsWithTag("enemy");
        InactiveArray(enemytospawn);
        animalstospawn = GameObject.FindGameObjectsWithTag("animal");
        InactiveArray(animalstospawn);
        livestospawn = GameObject.FindGameObjectsWithTag("lives");
        InactiveArray(livestospawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemytospawn != null)
        { ActivateObject(enemytospawn); }
        if (animalstospawn != null)
        { ActiveAndDiactivateObjects(animalstospawn); }
        if (livestospawn != null)
        { ActiveAndDiactivateObjects(livestospawn); }
    }


    void InactiveArray(GameObject[] array)
    {
        foreach (GameObject setInactiveThisObject in array)
        { setInactiveThisObject.SetActive(false); }
    }


    void ActivateObject(GameObject[] array)
    {
        foreach (GameObject setActiveThisObject in array)
        {
            float distance = Vector3.Distance(setActiveThisObject.transform.position, Player.transform.position);
            if (distance <= MinDistFollowA)
            {setActiveThisObject.SetActive(true); }
        }
    }
    void ActiveAndDiactivateObjects(GameObject[] array)
    {
        foreach (GameObject setActiveThisObject in array)
        {
            float distance = Vector3.Distance(setActiveThisObject.transform.position, Player.transform.position);
            if (distance <= MinDistFollowAD)
            { setActiveThisObject.SetActive(true); }
            else { setActiveThisObject.SetActive(false); }
        }
    }

}
