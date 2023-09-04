using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    public int currentCoins;
    [SerializeField] int startingCoins;
    [SerializeField] TMP_Text currentCoinText;

    public static CoinsManager Instance;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        LoadCoins();
    }

    void Update()
    {
        currentCoinText.text = currentCoins + "";
    }

    public void ChangeCoins(int change)
    {
        currentCoins = Mathf.Clamp(currentCoins+change, 0, 1000000);
    }

    void OnApplicationQuit()
    {
        SaveCoins();
    }

    public void LoadCoins()
    {
        if(!PlayerPrefs.HasKey("Coins"))
            ResetCoins(false);
        
        currentCoins = PlayerPrefs.GetInt("Coins");
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", currentCoins);
    }

    public void ResetCoins(bool zero)
    {
        if(zero)
            PlayerPrefs.SetInt("Coins", 0);
        else
            PlayerPrefs.SetInt("Coins", startingCoins);
        LoadCoins();
    }
}
