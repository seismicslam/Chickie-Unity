using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] FoodData[] foodDatas;
    [SerializeField][ReadOnly] FoodData currentSpawnedFoodData;
    [SerializeField][ReadOnly] Transform currentFood; 

    [Space(20)]
    [SerializeField] Transform spawnOrigin;
    [SerializeField] Transform foodDestination;
    [SerializeField] float travelSpeed;
    [ReadOnly] public bool hasFoodSpawn;
    [ReadOnly] public bool readyToEat;

    public static FoodManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        hasFoodSpawn = currentSpawnedFoodData != null;

        if(hasFoodSpawn)
        {
            currentFood.position = Vector3.MoveTowards(currentFood.position, foodDestination.position, travelSpeed * Time.deltaTime);
            if(currentFood.position == foodDestination.position)
            {
                readyToEat = true;
            }
        }
    }

    public void SpawnFood(string name)
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.NONE || 
        PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING)
        {
            foreach (var datas in foodDatas)
            {
                if(datas.foodName == name && CoinsManager.Instance.currentCoins >= datas.price)
                {
                    currentSpawnedFoodData = datas;
                    CoinsManager.Instance.ChangeCoins(-datas.price);
                    currentFood = Instantiate(currentSpawnedFoodData.foodPrefab, spawnOrigin.position, Quaternion.identity).transform;
                }
            }
        }
    }

    public FoodData GetCurrentFoodData() => currentSpawnedFoodData;

    public void ResetFood()
    {
        Destroy(currentFood.gameObject);
        currentSpawnedFoodData = null;
    }
}
