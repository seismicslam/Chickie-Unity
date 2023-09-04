using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetStateManager : MonoBehaviour
{
    [SerializeField] PetStates currentPetState;
    [SerializeField] Button eatButton;
    [SerializeField] Button activityButton;
    [SerializeField] Button cosmeticsButton;
    public enum PetStates { NONE, PATROLLING, EATING, BALL, PETTING, SHOWERING, EXPLORING, SLEEPING };

    public static PetStateManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        eatButton.interactable = GetCurrentPetState() == PetStates.PATROLLING;
        activityButton.interactable = GetCurrentPetState() == PetStates.PATROLLING;
        cosmeticsButton.interactable = GetCurrentPetState() == PetStates.PATROLLING;
    }

    public PetStates GetCurrentPetState() => currentPetState;
    public void ChangePetState(PetStates newState) { currentPetState = newState; }
}
