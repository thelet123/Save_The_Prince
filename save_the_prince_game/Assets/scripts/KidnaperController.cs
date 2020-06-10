using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidnaperController : MonoBehaviour
{
    private Animator KidnapperAnim;
    private GameObject Player;
    public float distance;
    private Transform walkTo;
    public float speed = 10;
    public float MinDistFollow = 25f;
    public void Start()
    {
        Player = GameObject.Find("Player");
        KidnapperAnim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        MoveKidnapper();
    }

    void MoveKidnapper ()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance <= MinDistFollow && distance >10)
        {
            walkTo = Player.transform;
            KidnapperAnim.SetBool("walk", true);
            transform.LookAt(walkTo);
            transform.position += transform.forward * speed * Time.deltaTime;
           
        }
        if (distance < 10)
        {
            KidnapperAnim.SetBool("walk", false);
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }

}
