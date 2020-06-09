using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private string[] hurtsentence = {" ouch", " can you be more carful??", " come on hit him", " that one was hurt"};
    [SerializeField] float boundeSize = 1000;
    public float movespeed;
    public int jumpForce = 300;
    public int health;
    public int decreaseNumHealth;
    public float rotationSpeed = 10.0f;
    private bool isGameOver;
    private int checkdoublejump;
    private GameManager gameManagerScript;
    private Dialog dialogScript;
    private levelManager levelManagerScript;
    private Rigidbody playerRB;
    private Animator playerAnim;
    private int Randomsentence;
    private bool hasNotHappened; 
    void Start()
    {
        hasNotHappened = true;
        health = 5;
        decreaseNumHealth = 0;
        checkdoublejump = 0;
        isGameOver = false;
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<levelManager>();
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("waiting", true);
    }

    void FixedUpdate() // Update is called once per frame
    { 
        MovePlayer();
        KeepBounds();
        gameManagerScript.UpdateHealth(health);
    }

    public void MovePlayer() //manage the player movment  
    {
        float verticalinput = Input.GetAxis("Vertical"); //to move - forword or backward 
        float horizontalinput = Input.GetAxis("Horizontal"); //to rotate - right of left
        if (!isGameOver )  //if the game is not over let the player move
        {
            //move all the ways with animation, if you press roll youll run 
            transform.Translate(Vector3.forward * verticalinput * movespeed * Time.deltaTime); //move forword or backward 
            transform.Translate(Vector3.right * horizontalinput * movespeed * Time.deltaTime); //move right of left
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s")) //roll backwords
            {
                playerAnim.SetBool("walkB", true);
                movespeed = 7.0f;
            }
            else  
            {
                playerAnim.SetBool("walkB", false);
                movespeed = 9.0f;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"))
            { playerAnim.SetBool("runF", true);}
            else { playerAnim.SetBool("runF", false); }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey("a"))
            { playerAnim.SetBool("runL", true); }
            else { playerAnim.SetBool("runL", false); }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey("d"))
            { playerAnim.SetBool("runR", true); }
            else { playerAnim.SetBool("runR", false); }

            if (Input.GetKey(KeyCode.Mouse1) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w")))  
            {
                movespeed = 7.0f;
                playerAnim.SetBool("walkF", true);
            }
            else
            {
                movespeed = 9.0f;
                playerAnim.SetBool("walkF", false);
            }

            // if you press spacebar you jump with animation
            if (Input.GetKeyDown(KeyCode.Space) && checkdoublejump == 0) 
            {
                playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                checkdoublejump ++;
                playerAnim.SetTrigger("jump_trig");
            }

            // if you press f you punch e kick q slide with animation
            if (Input.GetKeyDown("q"))
            {playerAnim.SetTrigger("slide_trig");}

            //if uve met the kidnaper you can klick leftmouse to attack with sword
            if (!hasNotHappened)
            {
                if (Input.GetKeyUp(KeyCode.Mouse0))
                { playerAnim.SetBool("sword1", true); }
                else { playerAnim.SetBool("sword1", false); }
            }
        }
    } 
 
    void KeepBounds () //keeps the player on terrain bounderys
    {
        if (transform.position.z > boundeSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, boundeSize);
        }
        if (transform.position.z < -boundeSize)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -boundeSize);
        }
        if (transform.position.x > boundeSize)
        {
            transform.position = new Vector3(boundeSize, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -boundeSize)
        {
            transform.position = new Vector3(-boundeSize, transform.position.y, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ground"))  //on collishion with ground player can jump again
        {checkdoublejump = 0;}
    }

    private void OnTriggerEnter(Collider other) //do diffrent action when player collide with staff 
    {
        if (other.gameObject.CompareTag("lives") && !isGameOver) //on collishion with live, live diappear and increase live
        {
            health++;
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("enemy") && !isGameOver) //when collide with enemy youll get hurt of the enemy get hurt 
        {
              decreaseNumHealth++;
              if (decreaseNumHealth == 5)
              {
                  Randomsentence = UnityEngine.Random.Range(0, hurtsentence.Length);
                  dialogScript.SaySentence(hurtsentence[Randomsentence]);
                  health--;
                  decreaseNumHealth = 0;
                  if (health == 0)
                  {
                      gameManagerScript.GameOver();
                      isGameOver = true;
                      playerAnim.SetBool("waiting", false);
                      playerAnim.SetTrigger("die_trig");
                  }
              }
          
        }

        else if (other.gameObject.CompareTag("key") && !isGameOver) //when pickup key haskey = true
        { 
            levelManagerScript.HasKey("yes");
            dialogScript.SaySentence(" great now i can open the gate!");
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("NextLevel") && !isGameOver)//when you touch the nextlevelpoint youll transorm to the next level  //when you touch the nextlevelpoint youll transorm to the next level 
        { levelManagerScript.NextLevel();}

        else if (other.gameObject.CompareTag("kidnapertrig") && !isGameOver && hasNotHappened) 
        {levelManagerScript.KidnaperScene(); hasNotHappened = false; }
    }

    public bool IsGameOver()
    { return isGameOver; }
}
