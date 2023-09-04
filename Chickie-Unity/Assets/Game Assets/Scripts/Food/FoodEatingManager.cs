using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEatingManager : MonoBehaviour
{
    [SerializeField] Transform pet;
    [SerializeField] Transform eatingPosNode;
    [SerializeField] float eatTime;
    FoodManager foodManager;
    bool goingToEat;

    void Start()
    {
        foodManager = FoodManager.Instance;
    }

    void Update()
    {
        if(!goingToEat && foodManager.hasFoodSpawn)
        {
            goingToEat = true;
            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.EATING);

            StartCoroutine(GoToFood());
        }
    }

    IEnumerator GoToFood()
    {
        PetMoveManager.Instance.ChangeAndMoveToNode(eatingPosNode.position);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }
        while(!foodManager.readyToEat)
        {
            yield return 0;
            continue;
        }

        pet.rotation = Quaternion.Euler(0,270,0);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.EATING);

        yield return new WaitForSeconds(eatTime);

        NeedsController.Instance.ChangeNeeds(foodManager.GetCurrentFoodData().changeNeeds);
        foodManager.ResetFood();
        yield return new WaitForSeconds(0.2f);
        goingToEat = false;
        PetPatrol.Instance.StartPatrolling();
    }
}
