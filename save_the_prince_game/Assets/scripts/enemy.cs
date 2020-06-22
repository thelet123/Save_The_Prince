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
    private readonly string[] killedEnemySentence = { " good", " ok it's gone", " one down", " keep going!" };
    private int killpoint;
    private int Randomsentence;
    private float runTime;
    public float enemyspeed = 10;
    private float MinDistFollow = 40;
    private float MinDistAttack = 3;
    public float distance;
    private bool waiting = false;
    private bool isDead = false;
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
        if (!playerControllerScript.IsGameOver() && !isDead) { FollowAndAttackPlayer();}
    }

    void FollowAndAttackPlayer() //the enemy will follow the player and attack him with animation
    {
        distance = Vector3.Distance(transform.position, Player.transform.position);
        if ((distance <= MinDistFollow && distance > MinDistAttack || found && distance > MinDistAttack) && !waiting)
        {
            enemyAnim.SetBool("Run_F", true);
            transform.LookAt(Player.transform);
            transform.position += transform.forward * enemyspeed * Time.deltaTime;
            found = true;
            runTime += Time.deltaTime;
            if (runTime > 5)   // if the enemy run for 5 sec he will wait and gain energy 
            {
                Wait();
                runTime = 0;
            }
        }
        if (distance <= MinDistAttack && !waiting)
        {
            enemyAnim.SetBool("Run_F", false);
            enemyAnim.SetBool("fight", true);
        }
        else { enemyAnim.SetBool("fight", false); }
        if (distance > MinDistFollow) { enemyAnim.SetBool("Run_F", false); }
    }

    public void Wait()  //make enemy wait a little before another attack
    {
        waiting = true;
        enemyAnim.SetBool("Run_F", false);
        enemyAnim.SetTrigger("breath");
        Invoke("NotWait", 5);
    }

    private void OnTriggerEnter(Collider other) //if the player's sword touched the enemy 5 times he will dissapear and the bloodlevel of the player will increase
    {
        if(other.gameObject.CompareTag("sword") && Input.GetKeyUp(KeyCode.Mouse0)) //if player's sword touched enemy his health will decreese 
        {
            enemyAnim.SetBool("hurt", true);
            killpoint++;
            if (killpoint == 6) //if youve killed an enemy it will die and your blood will increase 
            {
                enemyAnim.SetBool("fight", false);
                enemyAnim.SetBool("Run_F", false);
                enemyAnim.SetBool("death", true);
                isDead = true;
                gameManagerScript.UpdateXpAndLevel();
                Randomsentence = Random.Range(0, killedEnemySentence.Length);
                dialogScript.SaySentence(killedEnemySentence[Randomsentence]);
                Invoke("Destroy", 20);
                killpoint = 0;
            }       
        }
        else { enemyAnim.SetBool("hurt", false); }
    }

    public void NotWait()
    { waiting = false; }

    void Destroy()
    { Destroy(gameObject); }


}
