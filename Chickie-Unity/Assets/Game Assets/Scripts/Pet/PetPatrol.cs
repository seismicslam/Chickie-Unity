using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetPatrol : MonoBehaviour
{
    [SerializeField] float idleTime;
    PetStateManager petStateManager;
    Coroutine patrolCoroutine;

    bool doPatrol;

    public static PetPatrol Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        petStateManager = PetStateManager.Instance;

        if(!PlayerPrefs.HasKey("Last Sleep Timer Chosen") && !PlayerPrefs.HasKey("Last Explore Timer Chosen"))
        {
            StartPatrolling();
        }
    }

    public void StartPatrolling()
    {
        doPatrol = true;

        PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.PATROLLING);
        patrolCoroutine = StartCoroutine(PerformPatrolAction());
    }

    public void StopPatrolling()
    {
        if(doPatrol)
        {
            doPatrol = false;

            StopCoroutine(patrolCoroutine);
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.NONE);
            PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);
            PetMoveManager.Instance.StopMoving();

        }
    }

    IEnumerator PerformPatrolAction()
    {
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);
        yield return new WaitForSeconds(idleTime - 1);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.TURNING);
        yield return new WaitForSeconds(1);

        PetMoveManager.Instance.ChangeAndMoveToRandomNode();
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.WALKING);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }

        if(doPatrol) StartCoroutine(PerformPatrolAction());
    }
}
