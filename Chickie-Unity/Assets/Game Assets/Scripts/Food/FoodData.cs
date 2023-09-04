using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "AI Chat Bot/Create New Food Data")]
public class FoodData : ScriptableObject
{
    public string foodName;
    public GameObject foodPrefab;
    public int price;

    [Space(15)]
    public Needs changeNeeds;
}
