using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room : MonoBehaviour
{
    public Transform[] interactiveObjects;

    bool hasProgressed = false;


    public Transform ReturnClosestInteractible(Vector3 characterPosition, float range)
    {
        List<Transform> closeItems = new List<Transform>();
        for(int i = 0; i < interactiveObjects.Length; i++)
        {
            if(interactiveObjects[i].GetComponent<PhysicalItem>().isPickedUp)
            {
                continue;
            }
            if(Vector3.Distance(interactiveObjects[i].position, characterPosition)< range)
            {
                closeItems.Add(interactiveObjects[i]);
            }
        }

        if(closeItems.Count == 0)
        {
            return null;
        }

        else if(closeItems.Count == 1)
        {
            return closeItems[0];
        }

        Transform closest = interactiveObjects[0];
        float closestDistance = Vector3.Distance(closest.position, characterPosition);
        for(int i = 1; i < interactiveObjects.Length; i++)
        {
            float distance = Vector3.Distance(interactiveObjects[i].position, characterPosition);

            if(distance < closestDistance)
            {
                closest = interactiveObjects[i];
                closestDistance = distance;
            }
        }
        return closest;
        
    }
    

    private void SetCharacterScore(int player)
    {
        if (player == 1)
        {
            ScoreManager.m_player1PointsValue = ScoreManager.m_lavaTimerValue;
            ScoreManager.m_player2PointsValue = 0;
        }
        else
        {
            ScoreManager.m_player2PointsValue = ScoreManager.m_lavaTimerValue;
            ScoreManager.m_player1PointsValue = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerProperties playerProperties = other.GetComponent<PlayerProperties>();
        if(playerProperties != null && !hasProgressed)
        {
            hasProgressed = true;
            ScoreManager.inTallieScreen = true;
            ScoreManager.inGameUI.SetActive(false);
            GameController.instance.nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SetCharacterScore((int)playerProperties.playerType);
            
            GameController.instance.EndRoom();
            playerProperties.finishedLevel = true;
            //ScoreManager.earlyAdvanceLevel = true;
        }
    }
}
