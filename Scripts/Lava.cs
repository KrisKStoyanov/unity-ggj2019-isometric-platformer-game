using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetCharacterScore(int player)
    {
        if (player == 1)
        {
            ScoreManager.m_player1PointsValue = 0;
        }
        else
        {
            ScoreManager.m_player2PointsValue = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerProperties playerProperties = other.gameObject.GetComponent<PlayerProperties>();
        if (playerProperties != null &&
            !playerProperties.finishedLevel)
        {
            playerProperties.isAlive = false;
            playerProperties.GetComponent<CharController>().enabled = false;
            SetCharacterScore((int)playerProperties.playerType);
        }
    }
}
