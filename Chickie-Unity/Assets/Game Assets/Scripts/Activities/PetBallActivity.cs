using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PetBallActivity : MonoBehaviour
{
    [SerializeField] Rigidbody ballPrefab;
    [SerializeField] float throwStrength;
    [SerializeField] float fetchCheckRadius;
    [SerializeField] LayerMask ballLayer;
    [SerializeField] Needs onFetchNeeds;
    [SerializeField] int coinsReward;

    [Space(20)]
    [SerializeField] Transform pet;
    [SerializeField] Transform cam;
    [SerializeField] GameObject throwButton;
    [SerializeField] Transform startNode;
    [SerializeField] Transform chickenHead;

    [SerializeField][ReadOnly] Transform currentBall;
    [SerializeField][ReadOnly] bool ballThrown;

    void Start()
    {
        throwButton.SetActive(false);
    }

    public void EquipBall()
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING &&
        currentBall == null)
        {
            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.BALL);
            PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.TURNING);
            throwButton.SetActive(true);
        }
    }

    public void ThrowBall()
    {
        if(!ballThrown)
        {
            ballThrown = true;

            throwButton.SetActive(false);
            currentBall = Instantiate(ballPrefab, new Vector3(cam.position.x, cam.position.y, cam.position.z + 4), Quaternion.identity).transform;
            currentBall.GetComponent<Rigidbody>().AddForce(Vector3.forward * throwStrength);
        }
    }

    void Update()
    {
        if(ballThrown)
        {
            PetMoveManager.Instance.ChangeAndMoveToNode(currentBall.position);
            PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

            if(Physics.CheckSphere(pet.position, fetchCheckRadius, ballLayer))
            {
                ballThrown = false;

                currentBall.GetComponent<Collider>().enabled = false;
                Destroy(currentBall.GetComponent<Rigidbody>());

                StartCoroutine(FetchBall());
            }
        }
    }

    IEnumerator FetchBall()
    {         
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.EATING);

        yield return new WaitForSeconds(0.5f);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

        yield return new WaitForSeconds(0.3f);
         // Anchor ball to chicken mouth
        currentBall.transform.parent = chickenHead;
        currentBall.localPosition = new Vector3(0.05f, 0.1f, 0);

        PetMoveManager.Instance.ChangeAndMoveToNode(startNode.position);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }

        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.EATING);
        yield return new WaitForSeconds(0.5f);

        Destroy(currentBall.gameObject);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);

        yield return new WaitForSeconds(0.2f);

        CoinsManager.Instance.ChangeCoins(coinsReward);
        NeedsController.Instance.ChangeNeeds(onFetchNeeds);
        PetPatrol.Instance.StartPatrolling();
        CameraMovement.Instance.ChangeAllow(true);
    }
}
