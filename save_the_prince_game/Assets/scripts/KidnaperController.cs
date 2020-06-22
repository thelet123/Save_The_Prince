using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidnaperController : MonoBehaviour
{
    private GameObject MoonSword;
    private Animator KidnapperAnim;
    private GameObject Player;
    private Dialog dialogScript;
    private Transform walkTo;
    public float distance;
    public float speed = 10;
    public float MinDistFollow = 25f;
    public bool met = false;
    public bool turn = false;
    public void Start()
    {
        MoonSword = GameObject.Find("MoonSword");
        MoonSword.SetActive(false);
        Player = GameObject.Find("Player");
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        KidnapperAnim = GetComponent<Animator>();
        turn = false;
        met = false;
    }


    // Update is called once per frame
    void Update()
    {
        MoveKidnapper();
    }

    void MoveKidnapper ()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance <= MinDistFollow && !met) //when close, move torwords the player
        {
            walkTo = Player.transform;
            KidnapperAnim.SetBool("walk", true);
            transform.LookAt(walkTo);
            transform.position += transform.forward * speed * Time.deltaTime;
            if (distance < 10){met = true;}
        }
        if (met) // when finish the conversation kidnapper disappear 
        {
            KidnapperAnim.SetBool("walk", false);
            if (!dialogScript.IsWriting())
            { 
                gameObject.SetActive(false);
                MoonSword.SetActive(true);
                met = false;
            }
        }

    }

}
