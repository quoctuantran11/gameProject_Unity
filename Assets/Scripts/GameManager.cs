using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int lives;
    public int defaultLevel = 1;

    int playerType;

    public int getType
    {
        get {return playerType; }
        set
        {
            playerType = value;
        }
    }

    private GameManager gameManager;
    void Awake()
    {
        if(gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        this.defaultLevel = PlayerPrefs.GetInt("level");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
