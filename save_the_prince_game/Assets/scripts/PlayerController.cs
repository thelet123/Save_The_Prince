﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameManager gameManagerScript;
    private Dialog dialogScript;
    private levelManager levelManagerScript;
    private Rigidbody playerRB;
    private Animator playerAnim;
    private KidnaperController kidnaperScript;
    private bool isGameOver;
    private bool hasNotHappened;
    private int jumpForce = 300;
    private int health;
    private int decreaseNumHealth;
    private int checkdoublejump;
    private int Randomsentence;
    private float movespeed;
    private readonly string[] hurtsentence = {" ouch", " can you be more carful??", " come on hit him", " that one was hurt"};

    void Start() //set objecs for the start
    {
        hasNotHappened = true;
        health = 5;
        decreaseNumHealth = 0;
        checkdoublejump = 0;
        isGameOver = false;
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        levelManagerScript = GameObject.Find("LevelManager").GetComponent<levelManager>();
        kidnaperScript = GameObject.Find("Kidnapper").GetComponent<KidnaperController>();
        playerRB = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAnim.SetBool("waiting", true);
    }

    void FixedUpdate() // Update is called once per frame
    { 
        MovePlayer();
        gameManagerScript.UpdateHealth(health);
    }

    public void MovePlayer() //manage the player movment with animation  
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

    private void OnCollisionEnter(Collision collision) //on collishion with ground player can jump again
    {
        if (collision.gameObject.CompareTag("ground"))  
        {checkdoublejump = 0;}
    }

    private void OnTriggerEnter(Collider other) //do diffrent action when player collide with objects 
    {
        if (other.gameObject.CompareTag("lives") && !isGameOver)  //on collishion with live, live diappear and increase live bar
        {
            health++;
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("enemy") && !isGameOver) //when collide with enemy youll get hurt and live decreas if theyll reach 0 game over 
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

        else if (other.gameObject.CompareTag("NextLevel") && !isGameOver)//when you touch the nextlevelpoint youll transorm to the next level (if you have the key)
        { levelManagerScript.NextLevel();}

        else if (other.gameObject.CompareTag("kidnapertrig") && !isGameOver && hasNotHappened) //when you reach a certain point the kidnaper and the kidnaper dialog will be active 
        {
            levelManagerScript.KidnaperScene();
            hasNotHappened = false;
        }
    }

    public bool IsGameOver() //return game over bool 
    { return isGameOver; }
}
