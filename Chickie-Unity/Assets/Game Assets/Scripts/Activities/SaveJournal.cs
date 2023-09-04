using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveJournal : MonoBehaviour
{
    string rantText;
    string gratuityText;
    public TMP_InputField rantInput;
    public TMP_InputField gratuityInput;

    public static SaveJournal Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadJournals();
    }

    void OnApplicationQuit()
    {
        SaveJournals();
    }

    void SaveJournals()
    {
        if (!PlayerPrefs.HasKey("Rant"))
        {
            PlayerPrefs.SetString("Rant", "");
        }
        if (!PlayerPrefs.HasKey("Gratuity"))
        {
            PlayerPrefs.SetString("Grauitiy", ""); 
        }

        PlayerPrefs.SetString("Rant", rantInput.text);
        PlayerPrefs.SetString("Gratuity", gratuityInput.text);
    }

    void LoadJournals()
    {
        rantInput.text = PlayerPrefs.GetString("Rant");
        gratuityInput.text = PlayerPrefs.GetString("Gratuity");
    }

    public void ResetRant()
    {
        rantInput.text = "";
        PlayerPrefs.SetString("Rant", rantInput.text);
    }

    public void ResetGratiuity()
    {
        gratuityInput.text = "";
        PlayerPrefs.SetString("Gratuity", gratuityInput.text);
    }
}
