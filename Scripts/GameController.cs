using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public static GameController instance;
    [Header("Transforms")]
    public Transform boyTransform;
    public Transform girlTransform;
    public Transform lavaTransform;
    public PlayerProperties boyProperties;
    public PlayerProperties girlProperties;

    [Header("Rooms")]
    public Transform[] roomsAnchors;


    ScoreManager scoreManager;
    

    public Transform roomCameraAnchor;

    

    [Header("Cinemacine")]
    public Cinemachine.CinemachineVirtualCamera cinemachineCamera;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    public bool lerpTest;

    public int nextSceneIndex;

    public static bool FinishedGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        FinishedGame = false;

        FinishedGame = false;

        SetInitialReferences();
    }

    private void OnLevelWasLoaded()
    {
        SetInitialReferences();
    }

    private void Update()
    {
        if(lerpTest)
        {
            lerpTest = false;
            EndRoom();
        }
    }

    private void SetInitialReferences()
    {
        instance = this;
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
    }

    private void CheckForLife()
    {
        if(!boyProperties.isAlive && !girlProperties.isAlive)
        {
            RestartLevel();
        }
        if((boyProperties.finishedLevel||!boyProperties.isAlive)
            && (girlProperties.finishedLevel||!girlProperties.isAlive))
        { 
            EndRoom();
        }
    }

    private void RestartLevel()
    {
        //TODO restart timer
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartLava()
    {
        LavaController.instance.MoveLavaPlane();
        LavaController.instance.ToggleLava();
    }
    
    public void EndRoom()
    {
        LavaController.instance.ToggleLava();
       
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        scoreManager.UpdateScore();
        SceneManager.LoadScene("TallieScore");
    }

    public void EndGame()
    {
        boyTransform.GetComponent<CharController>().enabled = false;
        girlTransform.GetComponent<CharController>().enabled = false;
        Debug.Log("About to end game");
        SceneManager.LoadScene("TallieScore");
    }
}