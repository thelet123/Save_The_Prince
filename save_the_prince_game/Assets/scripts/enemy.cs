using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Analytics;

public class enemy : MonoBehaviour
{
    private GameManager gameManagerScript;
    private Dialog dialogScript;
    private PlayerController playerControllerScript;
    private GameObject Player;
    private Animator enemyAnim;
    private int killpoint;
    private int Randomsentence;
    public float enemyspeed = 10;
    private float MinDistFollow = 40;
    private float MinDistAttack = 10;
    private bool waiting = false;
    private readonly string[] killedEnemySentence = { " good", " ok it's gone", " one down", " keep going!" };
    private bool found;
    public bool enemyAttacked = false;

    // Start is called before the first frame update
    void Start()
    {
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        Player = GameObject.Find("Player");
        playerControllerScript = Player.GetComponent<PlayerController>();
        enemyAnim = GetComponent<Animator>();
        killpoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControllerScript.IsGameOver()){FollowAndAttackPlayer();}
    }

    void FollowAndAttackPlayer() //the enemy will follow the player and attack him with animation
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if ((distance <= MinDistFollow || found) && !waiting)
        {
            enemyAnim.SetBool("Run_F", true);
            transform.LookAt(Player.transform);
            transform.position += transform.forward * enemyspeed * Time.deltaTime;
            found = true;
        }
        if (distance < MinDistAttack && !waiting) { enemyAnim.SetTrigger("fight"); }
        if (distance > MinDistFollow) { enemyAnim.SetBool("Run_F", false); }
    }
    public void Wait()  //make enemy wait a little before another attack
    {
        waiting = true;
        Debug.Log("im wainting");
        enemyAnim.SetBool("Run_F", false);
        enemyAnim.SetTrigger("breath");
        Invoke("NotWait", 7);
    }

    private void OnTriggerEnter(Collider other) //if the player's sword touched the enemy 5 times he will dissapear and the bloodlevel of the player will increase
    {
        if(other.gameObject.CompareTag("sword")) //if player's sword touched enemy his health will decreese 
        {
            Debug.Log("enemy hurt");
            enemyAnim.SetTrigger("hurt");
            Wait();
            killpoint++;
            if (killpoint == 5) //if youve killed an enemy it will die and your blood will increase 
            {
                enemyAnim.SetBool("death", true);
                gameManagerScript.UpdateXpAndLevel();
                killpoint = 0;
                Randomsentence = Random.Range(0, killedEnemySentence.Length);
                dialogScript.SaySentence(killedEnemySentence[Randomsentence]);
                Destroy(gameObject);
            }
        }
    }

    public void NotWait()
    { waiting = false; }



}
