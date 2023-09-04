using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PetShowerActivity : MonoBehaviour
{
    [SerializeField] Needs showerNeedsChanges;
    [SerializeField] float showerNeedChangeDelay;
    [SerializeField] Transform showerHead;
    [SerializeField] GameObject showerStatus;
    [SerializeField] Transform startNode;
    [SerializeField] Transform pet;

    [SerializeField][ReadOnly] bool goingToShower;
    [SerializeField][ReadOnly] bool readyForShower;

    Coroutine needsChangeRoutine;

    void Start()
    {
        showerStatus.SetActive(false);
    }

    void Update()
    {
        if(readyForShower)
        {
            showerHead.gameObject.SetActive(Input.GetMouseButton(0));

            if(Input.GetMouseButton(0))
            {
                showerHead.GetComponentInChildren<ParticleSystem>().Play();
                if(needsChangeRoutine == null) needsChangeRoutine = StartCoroutine(ChangeShowerNeeds());
            }
            else
            {
                showerHead.GetComponentInChildren<ParticleSystem>().Stop();
                if(needsChangeRoutine != null)
                {
                    StopCoroutine(needsChangeRoutine);
                    needsChangeRoutine = null;
                }
            }
        }
    }

    IEnumerator ChangeShowerNeeds()
    {
        yield return new WaitForSeconds(showerNeedChangeDelay);
        NeedsController.Instance.ChangeNeeds(showerNeedsChanges);

        needsChangeRoutine = null;
    }

    public void EquipShower()
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING && !goingToShower)
        {
            goingToShower = true;
            showerStatus.SetActive(true);

            CameraMovement.Instance.allowMovement = false;
            CameraMovement.Instance.resetPos = true;

            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.SHOWERING);

            StartCoroutine(RunTowardsCenter());
        }
    }

    public void StopShower()
    {
        readyForShower = false;
        goingToShower = false;

        CameraMovement.Instance.allowMovement = true;
        CameraMovement.Instance.resetPos = false;

        showerStatus.SetActive(false);
        showerHead.gameObject.SetActive(false);
        PetPatrol.Instance.StartPatrolling();
        CameraMovement.Instance.ChangeAllow(true);
    }

    IEnumerator RunTowardsCenter()
    {
        PetMoveManager.Instance.StopMoving();
        PetMoveManager.Instance.ChangeAndMoveToNode(startNode.position);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }

        pet.rotation = Quaternion.Euler(0,180,0);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);

        readyForShower = true;
    }
}
