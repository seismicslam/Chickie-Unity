using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetMoveManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform moveNode;
    [SerializeField] Transform startNode;

    [Space(20)]
    [SerializeField] float walkRadius;

    NavMeshHit hit;

    public static PetMoveManager Instance;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeAndMoveToNode(startNode.position);
    }


    public bool HasReachedDestination() =>
    (agent.remainingDistance <= agent.stoppingDistance) && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);

    public void GoToMoveNode() 
    { 
        StopMoving();
        agent.SetDestination(moveNode.position); 
    } 

    public void ChangeMoveNode(Vector3 newPosition)
    {
        moveNode.position = new Vector3(newPosition.x,0.03153408f,newPosition.z);
    }
    public void ChangeMoveNodeRandom()
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        if(hit.position != Vector3.positiveInfinity)
            moveNode.position = hit.position;
        else
            ChangeMoveNodeRandom();
    }

    public void ChangeAndMoveToRandomNode()
    {
        ChangeMoveNodeRandom();
        GoToMoveNode();
    }


    public void ChangeAndMoveToNode(Vector3 newPosition)
    {
        ChangeMoveNode(newPosition);
        GoToMoveNode();
    }

    public void StopMoving() 
    { 
        agent.ResetPath();
    }

    public Vector3 GetMoveNodePosition() => moveNode.position;
}
