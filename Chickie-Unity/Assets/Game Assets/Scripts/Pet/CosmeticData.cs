using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "AI Chat Bot/Create New Cosmetic Data")]
public class CosmeticData : ScriptableObject
{
    public string cosmeticName;
    public int price;
    public bool unlocked;
}