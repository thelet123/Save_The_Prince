using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject key;
    private Dialog dialogScript;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject gameStartCanvas;
    [SerializeField] GameObject DialogCanvas;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Image[] hearts;
    [SerializeField] Image xpbar;
    [SerializeField] TextMeshProUGUI sentences;
    [SerializeField] string[] gameOverSentenceArray;
    [SerializeField] string[] endLevelSentences;
    private float XPpoint;
    // Start is called before the first frame update
    void Start() //set the level by active and diactive objects in the scene  
    {
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        key = GameObject.Find("key");
        key.SetActive(false);
        XPpoint = 0;
        xpbar.fillAmount = XPpoint;
        gameStartCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateXpAndLevel() // increase bloodlevel by 0.1, if youve riched the max the key will be active (to the next level)
    {
        XPpoint += 1f;
        xpbar.fillAmount = XPpoint;
        if (XPpoint == 1)
        {
            dialogScript.Talk(endLevelSentences);
            if (!dialogScript.IsWriting())
            {
                key.transform.position = GameObject.Find("Player").transform.position - new Vector3(4, 3, 0);
                key.SetActive(true);
            }
        }
    }

    public void UpdateHealth(int health) //gets health number from PlayerController script and active the sprites 
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (health > hearts.Length)
            { health = hearts.Length; }

            if (i <= health)
            { hearts[i].sprite = fullHeart; }
            if (i >= health)
            { hearts[i].sprite = emptyHeart; }
        }
    }

    public void GameOver() //active game over canvas 
    {
        if (gameOverSentenceArray != null)
        {
            int randsentence = Random.Range(0, gameOverSentenceArray.Length);
            sentences.text = gameOverSentenceArray[randsentence];
        }
        gameOverCanvas.gameObject.SetActive(true);
        gameStartCanvas.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame() //restart game with a press of a button 
    {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}

    public void QuitGame() //quit game with a press of a button
    {
        Application.Quit();
        Debug.Log("has quit game");
    }

}
