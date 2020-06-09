using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndDrop : MonoBehaviour
{
    private Dialog dialogScript;
    private List<GameObject> roadObjectList;
    private PlayerController playerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        roadObjectList = new List<GameObject>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        MakeRoad();
    }

    private void MakeRoad() // if player press shift he put mushroom that he collected (1 instade 3) if he didnt collected any alert he can do that action 
    {
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            if (roadObjectList.Count == 0)
            { dialogScript.SaySentence(" I dont have any mushrooms"); }
            else
            {
                roadObjectList[0].transform.position = transform.position;
                GameObject littlemushroom1 = roadObjectList[0].transform.Find("Circle.002").gameObject;
                GameObject littlemushroom2 = roadObjectList[0].transform.Find("Circle.001").gameObject;
                littlemushroom2.SetActive(false);
                littlemushroom1.SetActive(false);
                roadObjectList[0].SetActive(true);
                roadObjectList.Remove(roadObjectList[0]);
            }
        }
    }

    private void OnTriggerEnter(Collider other) //do diffrent action when player collide with staff 
    {

        if (other.CompareTag("MarkRoad") && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")) && !playerControllerScript.IsGameOver()) //pick up the markroad object and put it in a list
        {
            roadObjectList.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }

    }

}


