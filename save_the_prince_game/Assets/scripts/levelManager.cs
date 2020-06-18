using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelManager : MonoBehaviour
{
    private Dialog dialogScript;
    [SerializeField] string[] levelSentences;
    [SerializeField] string[] kidnapeSentences;
    private bool hasKey;
    private int scenenumber = 0;
    private string sceneName;

    // Start is called before the first frame update
    void Start() //active and diactive object by level, find the scene name ext, make differt entery dialog in every level (if there is any)
    {
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (levelSentences != null) { dialogScript.Talk(levelSentences); }
    }

    public bool HasKey(string yesorno) //set true or false from playercontroller if the player took the key 
    {
        if (yesorno == "yes") { hasKey = true; }
        else { hasKey = false; }
        return hasKey;
    }

    public void NextLevel() //if the player have the key he can enter the cave to the next level 
    {
        if (hasKey == true)
        { 
            scenenumber++;
            SceneManager.LoadScene(scenenumber);
        }
        else { dialogScript.SaySentence("seems like it'd be the way back home, but how?"); }
    }

    public void KidnaperScene() //make differnt dialog kidnaper scene in every level 
    {
        if (sceneName == "Level1")
        {dialogScript.Talk(kidnapeSentences);}
    }

   

}
