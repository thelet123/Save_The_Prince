using System.Collections;
using System.Collections.Generic;
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
    private float enemyspeed = 10;
    private float MinDistFollow = 40;
    private float MinDistAttack = 7;
    private string[] killedEnemySentence = { " good", " ok she's gone", " one down", " keep going!" };
    private bool found;

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
        if (!playerControllerScript.IsGameOver())
        { FollowAndAttackPlayer();}
    }

    void FollowAndAttackPlayer() //the enemy will follow the player and attack him with animation
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance <= MinDistFollow && distance > MinDistAttack || found)
        {
            enemyAnim.SetBool("Run_F", true);
            transform.LookAt(Player.transform);
            transform.position += transform.forward * enemyspeed * Time.deltaTime;
            found = true;
        }
        if (distance <= MinDistAttack)
        {
            string randomAttack = Random.Range(1,2).ToString();
            enemyAnim.SetTrigger(randomAttack);
            transform.position -= transform.forward * enemyspeed * Time.deltaTime;
        }
        if (distance > MinDistFollow) { enemyAnim.SetBool("Run_F", false); }
    }


    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
    }

    private void OnTriggerEnter(Collider other) //if the player's sword touched the enemy 5 times he will dissapear and the bloodlevel of the player will increase
    {
        if(other.gameObject.CompareTag("sword") && Input.GetKeyDown(KeyCode.Mouse0))
        {
            killpoint++;
            if (killpoint == 5) //if youve killed an enemy it will disappear
            {
                gameManagerScript.UpdateXpAndLevel();
                killpoint = 0;
                Randomsentence = Random.Range(0, killedEnemySentence.Length);
                dialogScript.SaySentence(killedEnemySentence[Randomsentence]);
                StartCoroutine(Wait());
                Destroy(gameObject);
            }
        }
    }



}
