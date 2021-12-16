using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Master class for game characters
public class Actor : MonoBehaviour
{

    public Animator animator;

    public float speed = 2;
    protected Vector3 frontVector;

    public bool isGrounded;
    public bool isAlive = true;

    public float maxLife = 100.0f;
    public float currentLife = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
