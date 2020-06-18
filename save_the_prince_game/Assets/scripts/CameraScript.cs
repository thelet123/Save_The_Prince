using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject Player;
    private PlayerController playerControllerScript;
    private Dialog dialogScript;
    private float speed = 10.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private float maxviewangel = 45;
    private float minviewangel = -20;
    // Start is called before the first frame update
    void Start()
    {
        dialogScript = GameObject.Find("DialogManager").GetComponent<Dialog>();
        Player = GameObject.Find("Player");
        playerControllerScript = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerControllerScript.IsGameOver() && !dialogScript.IsWriting())
        {     
            // the camera moves where the player move 
            transform.position = Player.transform.position;

            // if you move the mouse you rotate the camera and the player (only on y axis) as well
            if (Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0)
            {
                yaw += speed * Input.GetAxis("Mouse X");
                pitch -= speed * Input.GetAxis("Mouse Y");
                transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
                Quaternion playerNewRotation = Quaternion.Euler(0, yaw, 0);
                Player.transform.rotation = playerNewRotation;
            }

            //limit up down camera rotation by max and min angels 
            if (transform.rotation.eulerAngles.x > maxviewangel && transform.rotation.eulerAngles.x < 180f)
            {
                transform.rotation = Quaternion.Euler(maxviewangel, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
            if (transform.rotation.eulerAngles.x > 180 && transform.rotation.eulerAngles.x < 360f + minviewangel)
            {
                transform.rotation = Quaternion.Euler(360f + minviewangel, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            }
        }

        if (dialogScript.IsWriting() && !playerControllerScript.IsGameOver()) //if theres a dialog lock the camera in this position
        {transform.eulerAngles = new Vector3(0f,Player.transform.rotation.eulerAngles.y, 0.0f);}
        

    }
}
