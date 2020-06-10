using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spawn : MonoBehaviour
{
    private GameObject[] animalstospawn;
    private GameObject Player;
    private float MinDistFollowAD = 10;

    // Start is called before the first frame update
    void Start() //make an array by given objects that sctive in the game 
    {
        Player = GameObject.Find("Player");
        animalstospawn = GameObject.FindGameObjectsWithTag("animal");
        InactiveArray(animalstospawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (animalstospawn != null)
        { ActiveAndDiactivateObjects(animalstospawn); }
    }

    void InactiveArray(GameObject[] array) // make all the objects unactive
    {
        foreach (GameObject setInactiveThisObject in array)
        { setInactiveThisObject.SetActive(false); }
    }

    void ActiveAndDiactivateObjects(GameObject[] array) //active and diactive object 
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
