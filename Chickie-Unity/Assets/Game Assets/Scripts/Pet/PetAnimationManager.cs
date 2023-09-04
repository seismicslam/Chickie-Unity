using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAnimationManager : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationState animationState;

    public enum AnimationState { IDLE, WALKING, RUNNING, EATING, TURNING, SLEEPING };

    public static PetAnimationManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        animator.SetBool("Walk", animationState == AnimationState.WALKING);
        animator.SetBool("Run", animationState == AnimationState.RUNNING);
        animator.SetBool("Eat", animationState == AnimationState.EATING);
        animator.SetBool("Turn Head", animationState == AnimationState.TURNING);
        animator.SetBool("Sleep", animationState == AnimationState.SLEEPING);
    }

    public void SetAnimationState(AnimationState newState)
    {
        animationState = newState;
    }
    public AnimationState GetCurrentState() => animationState;
}
