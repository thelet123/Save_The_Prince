using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidnaperController : MonoBehaviour
{
    private Animator KidnapperAnim;
    private Vector3 distance new Vector3(5, 0, 5);;
    public void Start()
    {
        KidnapperAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveKidnapper (Vector3 PlayerPosition)
    {
    }
}
