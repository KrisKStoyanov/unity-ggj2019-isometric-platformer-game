using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TallieScore : MonoBehaviour
{
    public TextMeshProUGUI m_player1CurrentScoreText;
    public TextMeshProUGUI m_player2CurrentScoreText;

    public TextMeshProUGUI m_player1BankedScoreText;
    public TextMeshProUGUI m_player2BankedScoreText;

    public List<GameObject> m_coins;

    public List<GameObject> m_coinSpawnLocation;

    int m_player1CurrentScore;
    int m_player2CurrentScore;

    int coin1SpawnDelay;
    int coin2SpawnDelay;

    int offsetCoinSpawnLocation1;
    int offsetCoinSpawnLocation2;

    public GameObject TallieCanvas;
    public GameObject CongratulationsCanvus;
    public TextMeshProUGUI CongratsPlayerText;
    bool CongratsDone;

    public bool beginTally = false;

    public static bool gameOver = false;

    bool delayDone = false;


    // Start is called before the first frame update
    void Start()
    {

        //m_player1CurrentScore = 60;
        //m_player2CurrentScore = 60;
        //m_player1BankedScore = 0;
        //m_player2BankedScore = 0;

        m_player1CurrentScoreText.text = "Player 1 current score: " + ScoreManager.m_player1PointsValue;
        m_player2CurrentScoreText.text = "Player 2 current score: " + ScoreManager.m_player2PointsValue;
        m_player1BankedScoreText.text = "Player 1 banked score: " + ScoreManager.m_player1BankedScore;
        m_player2BankedScoreText.text = "Player 2 banked score: " + ScoreManager.m_player2BankedScore;

        coin1SpawnDelay = 0;
        coin2SpawnDelay = 0;

        CongratsDone = false;
        
    }

    private void Awake()
    {
        delayDone = false;
    }

    // Update is called once per frame
    void Update()
    {
        
         CalculatePlayerScores();
        

        if (ScoreManager.m_player1PointsValue == 0 && ScoreManager.m_player2PointsValue == 0 && !CongratsDone && !delayDone)
        {
            print(SceneManager.GetActiveScene().buildIndex);
            print(GameController.instance.nextSceneIndex);
            delayDone = true;
            StartCoroutine("Delay");
        }
    }

    void CalculatePlayerScores()
    {
        //Checking if score has been counted
        if(ScoreManager.m_player1PointsValue > 0)
        {
            //check if to spawn a coin
            if (coin1SpawnDelay == 0)
            {
                
                int coinToSpawn = Random.Range(0, 2);
                GameObject coin = Instantiate(m_coins[coinToSpawn]); //Choosing which coin to spawn
                offsetCoinSpawnLocation1 = Random.Range(-10, 10); 
                coin.transform.position = m_coinSpawnLocation[0].transform.position;
                coin.transform.position += new Vector3(offsetCoinSpawnLocation1, 0, 0); //Setting position and adding offset to coin
                coin1SpawnDelay = 2; //Resetting coin spawn delay
            }

            // Manageing score
            ScoreManager.m_player1PointsValue--;
            coin1SpawnDelay--;
            ScoreManager.m_player1BankedScore++;

            // Update text
            m_player1CurrentScoreText.text = "Player 1 current score: " + ScoreManager.m_player1PointsValue;
            m_player1BankedScoreText.text = "Player 1 banked score: " + ScoreManager.m_player1BankedScore;
          
        }


        if (ScoreManager.m_player2PointsValue > 0)
        {
            if (coin2SpawnDelay == 0)
            {
                int coinToSpawn = Random.Range(0, 2);
                GameObject coin = Instantiate(m_coins[coinToSpawn]);
                offsetCoinSpawnLocation2 = Random.Range(-10, 10);
                coin.transform.position = m_coinSpawnLocation[1].transform.position;
                coin.transform.position += new Vector3(offsetCoinSpawnLocation2, 0, 0);
                coin2SpawnDelay = 2;
            }

            ScoreManager.m_player2PointsValue--;
            coin2SpawnDelay--;
            ScoreManager.m_player2BankedScore++;

            m_player2CurrentScoreText.text = "Player 2 current score: " + ScoreManager.m_player2PointsValue;
            m_player2BankedScoreText.text = "Player 2 banked score: " + ScoreManager.m_player2BankedScore;

        }

    }

    void AnnounceWinner()
    {
        string m_winner = "";

        // Checks if any player high a higher score
        if (ScoreManager.m_player1BankedScore > ScoreManager.m_player2BankedScore)
        {
            m_winner = "Player 1";
        }
        else if (ScoreManager.m_player1BankedScore < ScoreManager.m_player2BankedScore)
        {
            m_winner = "Player 2";
        }
        
        // Active right canvas
        TallieCanvas.SetActive(false);
        CongratulationsCanvus.SetActive(true);

        // Check if was draw and change text to suit result
        if (m_winner != "")
        {
            CongratsPlayerText.text = "Congratulations " + m_winner + " You have won!";
        }
        else
        {
            CongratsPlayerText.text = "We have a draw! Well done both!";

        }

    }

   
    IEnumerator Delay()
    {
        Debug.Log("Game over value in delay is: " + ScoreManager.gameOver);
        if (!ScoreManager.gameOver && !GameController.FinishedGame)
        {
            yield return new WaitForSeconds(2);

            ScoreManager.inTallieScreen = false;
            ScoreManager.inGameUI.SetActive(true);
            print(GameController.instance.nextSceneIndex);
            SceneManager.LoadScene(GameController.instance.nextSceneIndex);
        }
        else
        {
            ScoreManager.inTallieScreen = false;
            ScoreManager.inGameUI.SetActive(false);
            CongratsDone = true;
            yield return new WaitForSeconds(2);
            AnnounceWinner();
        }
       
    }
}
