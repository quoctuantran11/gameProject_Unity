using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;
    public float xSmooth = 8f;
    public float ySmooth = 8f;
    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    private Transform m_Player;
    CameraInputActions inputActions;
    //Show pause menu
    public GameObject pauseMenu;

    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    private bool CheckXMargin()
    {
        return (transform.position.x - m_Player.position.x) < xMargin;
    }
    // Start is called before the first frame update
    void Start()
    {
        inputActions = new CameraInputActions();
        inputActions.Camera.Enable();
        inputActions.Camera.Pause.performed += OnPauseGame;
    }

    // Update is called once per frame
    void Update()
    {
        TrackPlayer();
    }

    private void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if (CheckXMargin())
        {
            targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.deltaTime);
        }

        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        //targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    public void OnPauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MyGameManager.Instance.PauseGame();
            //Show pause menu
            pauseMenu.SetActive(true);
        }
    }
}
