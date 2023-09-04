using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PetPettingActivity : MonoBehaviour
{
    [SerializeField] Needs pettingNeedsChanges;
    [SerializeField] LayerMask pettingTriggerMask;
    [SerializeField] float pettingNeedsChangeDelay;
    [SerializeField] GameObject pettingStatus;
    [SerializeField] GameObject pettingHand;
    [SerializeField] ParticleSystem hearts;
    
    [Space(20)]
    [SerializeField] Camera cam;
    [SerializeField] Transform startNode;
    [SerializeField] Transform pet;
    [SerializeField][ReadOnly] bool withinTrigger;
    [SerializeField][ReadOnly] bool goingToPetting;
    [SerializeField][ReadOnly] bool readyForPetting;
    [SerializeField][ReadOnly] bool beingPetted;

    Coroutine needsChangeRoutine;
    
    void Start()
    {
        pettingStatus.SetActive(false);
    }

    void Update()
    {
        withinTrigger = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, pettingTriggerMask);
        if(readyForPetting)
        {
            pettingHand.gameObject.SetActive(Input.GetMouseButton(0));

            beingPetted = Input.GetMouseButton(0);
            // && (Mathf.Abs(Input.GetAxis("Mouse X")) > 0|| Mathf.Abs(Input.GetAxis("Mouse Y")) > 0)
            if(beingPetted)
            {
                if(needsChangeRoutine == null) needsChangeRoutine = StartCoroutine(ChangePettingNeeds());
            }
            else
            {
                if(needsChangeRoutine != null)
                {
                    hearts.Stop();
                    StopCoroutine(needsChangeRoutine);
                    needsChangeRoutine = null;
                }
            }
        }
    }

    IEnumerator ChangePettingNeeds()
    {
        yield return new WaitForSeconds(pettingNeedsChangeDelay);
        NeedsController.Instance.ChangeNeeds(pettingNeedsChanges);
        hearts.Play();

        needsChangeRoutine = null;
    }

    public void EquipPetting()
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING && !goingToPetting)
        {
            goingToPetting = true;
            pettingStatus.SetActive(true);

            CameraMovement.Instance.allowMovement = false;
            CameraMovement.Instance.resetPos = true;

            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.PETTING);

            StartCoroutine(RunTowardsCenter());
        }
    }

    public void StopPetting()
    {
        readyForPetting = false;
        goingToPetting = false;

        pettingHand.SetActive(false);

        CameraMovement.Instance.allowMovement = true;
        CameraMovement.Instance.resetPos = false;

        pettingStatus.SetActive(false);
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

        readyForPetting = true;
    }
}
