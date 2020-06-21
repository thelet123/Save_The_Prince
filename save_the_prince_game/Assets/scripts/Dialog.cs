using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Dialog : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textDisplay;
    [SerializeField] TextMeshProUGUI skipButton;
    [SerializeField] GameObject DialogCanvas;
    private GameObject Player;
    private PlayerController playerControllerScript;
    private int index;
    private float typingSpeed;
    private string[] sentences;
    private bool sentence;
    private bool typing;
    private bool sentenceHasAppeared;
    public void Start()
    {
        Player = GameObject.Find("Player");
        playerControllerScript = Player.GetComponent<PlayerController>();
        typing = false;
    }

    public void Update()
    {
        if (!sentence && textDisplay.text == sentences[index]) { sentenceHasAppeared = true; } //check that all the sentence has been written in canvas
        if (sentenceHasAppeared) { NextSentence(); } // if so you can continiue to the next one (even if another sentence is wirrten)
    }
    
    public void SaySentence(string nowsentences) //gets and write one sentence 
    {
        if (typing == false)
        {
            typing = true;
            sentence = true;
            index = 0;
            sentences = new[] { nowsentences };
            DialogCanvas.SetActive(true);
            StartCoroutine(Type());
        }
    } 

    public IEnumerator Wait() // if only one sentence has been written it'll diappear after 1 sec 
    {
        yield return new WaitForSeconds(1);
        DialogCanvas.SetActive(false);
        textDisplay.text = "";
        typing = false;
    }

    public void Talk(string[] nowsentences) //gets and write a few sentences 
    {
        sentence = false;
        index = 0;
        DialogCanvas.SetActive(true);
        skipButton.gameObject.SetActive(true);
        sentences = nowsentences;
        StartCoroutine(Type()); 
    }    

   
    public IEnumerator Type()  //gets array of sentences, each sentence seperated to letters and display them one by one with wating of few seconds
    {
        typing = true;
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        if (index == sentences.Length -1 && sentence)
        {StartCoroutine(Wait());}
    }

    public void NextSentence() //if you press enter you can continue with the next sentence
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sentenceHasAppeared = false;
            if (index < sentences.Length - 1)
            {
                index++;
                textDisplay.text = "";
                StartCoroutine(Type());
            }
            else
            {
                textDisplay.text = "";
                DialogCanvas.SetActive(false);
                typing = false;
                skipButton.gameObject.SetActive(false);
            }
        }
        
    }

    public void Skip() //allow player to skip dialogs with a press on the "skip" button
    {
        textDisplay.text = "";
        DialogCanvas.SetActive(false);
        typing = false;
        sentenceHasAppeared = false;
        skipButton.gameObject.SetActive(false);
    }

    public bool IsWriting() //check if there is a written sentence in the game. if so, the curdor will appear (so you can press skip) and you wont be able to move the camera around
    {
        if (DialogCanvas.activeInHierarchy && !sentence)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return true;
        }
        else if(!playerControllerScript.IsGameOver())
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            return false;
        }
        return false;
    }

}
