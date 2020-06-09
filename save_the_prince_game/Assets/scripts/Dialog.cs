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
    private bool sentence;
    private bool typing;
    private bool sentenceHasAppeared;
    private int index;
    private float typingSpeed;
    private string[] sentences;
    public void Start()
    {
       typing = false;
    }

    public void Update()
    {
        if (!sentence && textDisplay.text == sentences[index]) { sentenceHasAppeared = true; } //check that all the sentence has been written
        if (sentenceHasAppeared) { NextSentence(); } // if so you can continiue to the next one (even if another one is wirrten down)
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

    public IEnumerator Wait() // if only one sentence has been said it'll diappear in 1 sec 
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

    public void NextSentence() //if you press enter you can continue with the next sentence or finish the dialog 
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

    public void Skip() //allow player to skip dialogs 
    {
        textDisplay.text = "";
        DialogCanvas.SetActive(false);
        typing = false;
        sentenceHasAppeared = false;
        skipButton.gameObject.SetActive(false);
    }

    public bool IsWriting()
    {
        if (DialogCanvas.activeInHierarchy && !sentence)
        {
            Cursor.visible = true; 
            return true;
        }
        else
        {
            Cursor.visible = false;
            return false;
        }
    }

}
