using System.Collections;
using System;
using NaughtyAttributes;
using UnityEngine;

public class NeedsController : MonoBehaviour
{
    #region Variables
    [BoxGroup("Stats")][SerializeField] Needs defaultNeeds;
    [BoxGroup("Stats")][ReadOnly] public Needs currentNeeds;

    [BoxGroup("Decrease Rate")][SerializeField] float happinessDropRate;
    [BoxGroup("Decrease Rate")][SerializeField] float foodDropRate;
    [BoxGroup("Decrease Rate")][SerializeField] float waterDropRate;
    [BoxGroup("Decrease Rate")][SerializeField] float healthDropRate;
    [BoxGroup("Decrease Rate")][SerializeField] float energyDropRate;
    [BoxGroup("Decrease Rate")][SerializeField] float affectionDropRate;

    [BoxGroup("Time Rate")][SerializeField] float happinessTimeRate;
    [BoxGroup("Time Rate")][SerializeField] float foodTimeRate;
    [BoxGroup("Time Rate")][SerializeField] float waterTimeRate;
    [BoxGroup("Time Rate")][SerializeField] float healthTimeRate;
    [BoxGroup("Time Rate")][SerializeField] float energyTimeRate;
    [BoxGroup("Time Rate")][SerializeField] float affectionTimeRate;

    [BoxGroup("AFK Rate")] [SerializeField] float happinessAFKRate;
    [BoxGroup("AFK Rate")] [SerializeField] float foodAFKRate;
    [BoxGroup("AFK Rate")] [SerializeField] float waterAFKRate;
    [BoxGroup("AFK Rate")] [SerializeField] float healthAFKRate;
    [BoxGroup("AFK Rate")] [SerializeField] float energyAFKRate;
    [BoxGroup("AFK Rate")] [SerializeField] float affectionAFKRate;
    #endregion

    public static NeedsController Instance;

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        StartDropRates();
        if (PlayerPrefs.HasKey("First")) LoadNeeds();
        UpdateNeeds();
    }

    void UpdateNeeds()
    {
        if (PlayerPrefs.HasKey("Last Open"))
        {
            double timeLapsed = (System.DateTime.UtcNow - 
                DateTime.Parse(PlayerPrefs.GetString("Last Open"))).TotalSeconds;
            ChangeHappiness((float)-timeLapsed / happinessAFKRate * happinessDropRate);
            ChangeFood((float)-timeLapsed / foodAFKRate * foodDropRate);
            ChangeWater((float)-timeLapsed / waterAFKRate * waterDropRate);
            ChangeHealth((float)-timeLapsed / healthAFKRate * healthDropRate);
            ChangeEnergy((float)-timeLapsed / energyAFKRate * energyDropRate);
            ChangeAffection((float)-timeLapsed / affectionAFKRate * affectionDropRate);
        }
    }

    void StartDropRates()
    {
        StartCoroutine(PerformHappinessRateDrop());
        StartCoroutine(PerformFoodRateDrop());
        StartCoroutine(PerformWaterRateDrop());
        StartCoroutine(PerformHealthRateDrop());
        StartCoroutine(PerformEnergyRateDrop());
        StartCoroutine(PerformAffectionRateDrop());
    }
    IEnumerator PerformHappinessRateDrop()
    {
        yield return new WaitForSeconds(happinessTimeRate);
        currentNeeds.happiness -= happinessDropRate;
        StartCoroutine(PerformHappinessRateDrop());
    }
    IEnumerator PerformFoodRateDrop()
    {
        yield return new WaitForSeconds(foodTimeRate);
        currentNeeds.food -= foodDropRate;
        StartCoroutine(PerformFoodRateDrop());
    }
    IEnumerator PerformWaterRateDrop()
    {
        yield return new WaitForSeconds(waterTimeRate);
        currentNeeds.water -= waterDropRate;
        StartCoroutine(PerformWaterRateDrop());
    }
    IEnumerator PerformHealthRateDrop()
    {
        yield return new WaitForSeconds(healthTimeRate);
        currentNeeds.health -= healthDropRate;
        StartCoroutine(PerformHealthRateDrop());
    }
    IEnumerator PerformEnergyRateDrop()
    {
        yield return new WaitForSeconds(energyTimeRate);
        currentNeeds.energy -= energyDropRate;
        StartCoroutine(PerformEnergyRateDrop());
    }
    IEnumerator PerformAffectionRateDrop()
    {
        yield return new WaitForSeconds(affectionTimeRate);
        currentNeeds.affection -= affectionDropRate;
        StartCoroutine(PerformAffectionRateDrop());
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Last Open", System.DateTime.UtcNow.ToString());
        SaveNeeds();
    }

    public void SetDefaultNeeds()
    {
        SetNeeds(defaultNeeds);
    }
    public void ChangeNeeds(Needs newNeeds) 
    {
        ChangeHappiness(newNeeds.happiness);
        ChangeFood(newNeeds.food);
        ChangeWater(newNeeds.water);
        ChangeHealth(newNeeds.health);
        ChangeEnergy(newNeeds.energy);
        ChangeAffection(newNeeds.affection);
        
        if (!PlayerPrefs.HasKey("Happiness"))
        {
            PlayerPrefs.SetFloat("Happiness", newNeeds.happiness);
        }
        if (!PlayerPrefs.HasKey("Food"))
        {
            PlayerPrefs.SetFloat("Food", newNeeds.food);
        }
        if (!PlayerPrefs.HasKey("Water"))
        {
            PlayerPrefs.SetFloat("Water", newNeeds.water);
        }
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetFloat("Health", newNeeds.health);
        }
        if (!PlayerPrefs.HasKey("Energy"))
        {
            PlayerPrefs.SetFloat("Energy", newNeeds.energy);
        }
        if (!PlayerPrefs.HasKey("Affection"))
        {
            PlayerPrefs.SetFloat("Affection", newNeeds.affection);
        }
    }
    public void SetNeeds(Needs newNeeds)
    {
        currentNeeds = newNeeds;
        if (!PlayerPrefs.HasKey("Happiness"))
        {
            PlayerPrefs.SetFloat("Happiness", newNeeds.happiness);
        }
        if (!PlayerPrefs.HasKey("Food"))
        {
            PlayerPrefs.SetFloat("Food", newNeeds.food);
        }
        if (!PlayerPrefs.HasKey("Water"))
        {
            PlayerPrefs.SetFloat("Water", newNeeds.water);
        }
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetFloat("Health", newNeeds.health);
        }
        if (!PlayerPrefs.HasKey("Energy"))
        {
            PlayerPrefs.SetFloat("Energy", newNeeds.energy);
        }
        if (!PlayerPrefs.HasKey("Affection"))
        {
            PlayerPrefs.SetFloat("Affection", newNeeds.affection);
        }
    }
    public void LoadNeeds()
    {
        if(PlayerPrefs.HasKey("Happiness"))
        {
            currentNeeds.happiness = PlayerPrefs.GetFloat("Happiness");
            currentNeeds.food = PlayerPrefs.GetFloat("Food");
            currentNeeds.water = PlayerPrefs.GetFloat("Water");
            currentNeeds.health = PlayerPrefs.GetFloat("Health");
            currentNeeds.energy = PlayerPrefs.GetFloat("Energy");
            currentNeeds.affection = PlayerPrefs.GetFloat("Affection");
        }
    }
    public void SaveNeeds()
    {
        PlayerPrefs.SetFloat("Happiness", currentNeeds.happiness);
        PlayerPrefs.SetFloat("Food", currentNeeds.food);
        PlayerPrefs.SetFloat("Water", currentNeeds.water);
        PlayerPrefs.SetFloat("Health", currentNeeds.health);
        PlayerPrefs.SetFloat("Energy", currentNeeds.energy);
        PlayerPrefs.SetFloat("Affection", currentNeeds.affection);
    }

    public void ChangeHappiness(float change)
    {
        currentNeeds.happiness = Mathf.Clamp(currentNeeds.happiness+change, 0, 100);
    }
    public void ChangeFood(float change)
    {
        currentNeeds.food = Mathf.Clamp(currentNeeds.food + change, 0, 100);
    }
    public void ChangeWater(float change)
    {
        currentNeeds.water = Mathf.Clamp(currentNeeds.water + change, 0, 100);
    }
    public void ChangeHealth(float change)
    {
        currentNeeds.health = Mathf.Clamp(currentNeeds.health + change, 0, 100);
    }
    public void ChangeEnergy(float change)
    {
        currentNeeds.energy = Mathf.Clamp(currentNeeds.energy + change, 0, 100);
    }
    public void ChangeAffection(float change)
    {
        currentNeeds.affection = Mathf.Clamp(currentNeeds.affection + change, 0, 100);
    }

    public void ResetNeeds()
    {
        PlayerPrefs.DeleteKey("Happiness"); 
        PlayerPrefs.DeleteKey("Food");
        PlayerPrefs.DeleteKey("Water");
        PlayerPrefs.DeleteKey("Health");
        PlayerPrefs.DeleteKey("Energy");
        PlayerPrefs.DeleteKey("Affection");

        SetDefaultNeeds();
    }
    
}

[System.Serializable]
public struct Needs
{   
    public float happiness;
    public float food;
    public float water;
    public float health;
    public float energy;
    public float affection;
}
