using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FadeController : MonoBehaviour
{
    public Room[] rooms;
    public int currentRoomIndex;
    public bool isTransitioning;
    private float fadeTime;
    private float currentLerpValue;

    public bool test;
    private bool activatedRoomObjects = false;

    private void Awake()
    {
        SetInitialReferernces();
    }

    private void Update()
    {
        if(isTransitioning)
        {
            Transition(currentRoomIndex);
        }
    }

    private void SetInitialReferernces()
    {
        isTransitioning = false;
    }

    public void BeginTransition(int roomToTransitionTo, float transitionSpeed)
    {
        fadeTime = transitionSpeed;

        currentRoomIndex = roomToTransitionTo;
        isTransitioning = true;
    }
    

    public void Transition(int toRoomIndex)
    {
        currentLerpValue += Time.deltaTime / fadeTime;

        if(currentLerpValue >= 1)
        {
            isTransitioning = false;
            activatedRoomObjects = false;
        }
    }

    
}
