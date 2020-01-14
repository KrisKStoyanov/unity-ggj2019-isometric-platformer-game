using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int m_player1PointsValue;
    public static int m_player2PointsValue;
    public static int m_player1BankedScore;
    public static int m_player2BankedScore;
    public static int m_lavaTimerValue;

    public static int currentLevelIndex;

    public TextMeshProUGUI m_TMPPlayer1Score;
    public TextMeshProUGUI m_TMPPlayer2Score;
    public TextMeshProUGUI m_LavaTimerText;

    public static GameObject inGameUI;

    float currentTime;

    public static bool gameOver;
    public static bool inTallieScreen;

    public static bool earlyAdvanceLevel;

    bool inAttic;

    void Start()
    {
        m_lavaTimerValue = 30;
        inTallieScreen = false;
        DontDestroyOnLoad(this);
        inGameUI = GameObject.Find("UI");
        inAttic = false;
        
    }

    private void OnLevelWasLoaded()
    {
        //Player Starting Points
        if(SceneManager.GetActiveScene().name != "TallieScore")
        {
            gameOver = false;

            m_player1PointsValue = 0;
            m_player2PointsValue = 0;
            // Initial Lava timer setup
            m_lavaTimerValue = 30;
            currentTime = Time.time;
            m_LavaTimerText.text = "Time: " + m_lavaTimerValue;
            earlyAdvanceLevel = false;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Attic" && !GameController.FinishedGame)
        {
            Debug.Log("In Attic, Loading Final Scene");
            inAttic = true;
            inGameUI.SetActive(false);
            inTallieScreen = true;
            GameController.FinishedGame = true;
            StartCoroutine(DelayLevelLoad());

        }
        else if (!inAttic)
        {
            if (!gameOver)
            {
                UpdateScore();
                UpdateLavaTimer();
            }

            if (SceneManager.GetActiveScene().name != "Attic")
            {
                if (earlyAdvanceLevel || Input.GetKeyDown(KeyCode.Backspace))
                {

                    inGameUI.SetActive(false);
                    inTallieScreen = true;
                    GameController.instance.EndRoom();

                }
            }
            else
            {
                UpdateLavaTimer();
            }
        }
        
        
    }

    public void UpdateScore()
    {
        // TODO: Needs to be changed to set points after player enters endzone
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_player1PointsValue = m_lavaTimerValue;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_player2PointsValue = m_lavaTimerValue;
        }

        // TODO: Player score will decrease when they fall in lava
        // TODO: Players score increases if they push other player into lava

        // Updating player scores
        m_TMPPlayer1Score.text = "Player 1: " + m_player1PointsValue;
        m_TMPPlayer2Score.text = "Player 2: " + m_player2PointsValue;

     
    }

    void UpdateLavaTimer()
    {
        if (!inTallieScreen)
        {

            if (SceneManager.GetActiveScene().name == "Attic")
            {
                TallieScore.gameOver = true;
                gameOver = true;
                inGameUI.SetActive(false);
                inTallieScreen = true;
            }

            // Updating lava timer
            if (Time.time > currentTime + 1)
            {
                m_lavaTimerValue--;
                m_LavaTimerText.text = "Time: " + m_lavaTimerValue;
                currentTime = Time.time;
            }

            if (m_lavaTimerValue == 0)
            {
                
                 inGameUI.SetActive(false);
                 inTallieScreen = true;
                 GameController.instance.EndRoom();
                
            }
        }
    }

    void ResetLavaTimer()
    {
        m_lavaTimerValue = 30;
    }

    IEnumerator DelayLevelLoad()
    {
        yield return new WaitForSeconds(2);
        GameController.instance.EndRoom();

    }
}