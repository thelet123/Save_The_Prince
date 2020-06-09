using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    private string sceneName;
    [SerializeField] string[] levelSentences;
    [SerializeField] string[] kidnapeSentences;
    private Dialog dialogScript;
    private bool hasKey;
    private int scenenumber = 0;
    private GameObject MoonSword;
    // Start is called before the first frame update
    void Start()
    {
        MoonSword = GameObject.Find("MoonSword");
        MoonSword.SetActive(false);
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        dialogScript.Talk(levelSentences);
    }

    public bool HasKey(string yesorno)
    {
        if (yesorno == "yes") { hasKey = true; }
        else { hasKey = false; }
        return hasKey;
    }

    public void NextLevel()
    {
        if (hasKey == true)
        { 
            scenenumber++;
            SceneManager.LoadScene(scenenumber);
        }
        else { dialogScript.SaySentence("seems like it'd be the way back home, but how?"); }
    }

    public void KidnaperScene()
    {
        if (sceneName == "Level1")
        {
            dialogScript.Talk(kidnapeSentences);
            MoonSword.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
