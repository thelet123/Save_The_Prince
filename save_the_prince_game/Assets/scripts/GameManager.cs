using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image xpbar;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] TextMeshProUGUI sentences;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject gameStartCanvas;
    [SerializeField] GameObject DialogCanvas;
    [SerializeField] Image[] hearts;
    [SerializeField] string[] sentenceArray;
    private float XPpoint;
    private GameObject key;
    // Start is called before the first frame update
    void Start()
    {
        key = GameObject.Find("key");
        key.SetActive(false);
        XPpoint = 0;
        xpbar.fillAmount = XPpoint;
        gameStartCanvas.gameObject.SetActive(true);
        gameOverCanvas.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    public void UpdateXpAndLevel() //if youve killed an enemy your xp wil increase by 0.1, if youve riched the max youll find the key next player
    {
        XPpoint += 0.1f;
        xpbar.fillAmount = XPpoint;
        if (XPpoint == 1)
        {
            key.transform.position = GameObject.Find("Player").transform.position - new Vector3(4, 3, 0);
            key.SetActive(true);
            XPpoint = 0;
            xpbar.fillAmount = XPpoint;
        }
    }

    public void UpdateHealth(int health)
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

    public void GameOver()
    {
        //sentences.text = sentenceArray[randsentence];
        gameOverCanvas.gameObject.SetActive(true);
        gameStartCanvas.gameObject.SetActive(false);
        Cursor.visible = true;
    }

    public void RestartGame()
    {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("has quit game");
    }

}
