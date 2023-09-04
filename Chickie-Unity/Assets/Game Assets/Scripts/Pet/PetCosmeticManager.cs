using System.Linq;
using UnityEngine;

public class PetCosmeticManager : MonoBehaviour
{
    [SerializeField] CosmeticData[] cosmeticData;

    public GameObject cosmetics;

    public static PetCosmeticManager Instance;

    public GameObject pet;
    public Transform cosmeticsScrollView;

    public enum CosmeticType { HAT, FACE };

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        SyncCosmetics();
        UpdateUI();
    }

    private void OnApplicationQuit()
    {
        SaveCosmetics();
    }

    public void SyncCosmetics()
    {
        if (!PlayerPrefs.HasKey("Captain Hat"))
        {
            PlayerPrefs.SetInt("Captain Hat", 0);
        }
        if (!PlayerPrefs.HasKey("Cowboy Hat"))
        {
            PlayerPrefs.SetInt("Cowboy Hat", 0);
        }
        if (!PlayerPrefs.HasKey("Crown"))
        {
            PlayerPrefs.SetInt("Crown", 0);
        }
        if (!PlayerPrefs.HasKey("Miner Hat"))
        {
            PlayerPrefs.SetInt("Miner Hat", 0);
        }
        if (!PlayerPrefs.HasKey("Mustache"))
        {
            PlayerPrefs.SetInt("Mustache", 0);
        }
        if (!PlayerPrefs.HasKey("Pajama Hat"))
        {
            PlayerPrefs.SetInt("Pajama Hat", 0);
        }
        if (!PlayerPrefs.HasKey("Police Cap"))
        {
            PlayerPrefs.SetInt("Police Cap", 0);
        }
        if (!PlayerPrefs.HasKey("Shower Cap"))
        {
            PlayerPrefs.SetInt("Shower Cap", 0);
        }
        if (!PlayerPrefs.HasKey("Top Hat"))
        {
            PlayerPrefs.SetInt("Top Hat", 0);
        }
        if (!PlayerPrefs.HasKey("Viking Helmet"))
        {
            PlayerPrefs.SetInt("Viking Helmet", 0);
        }
        if (!PlayerPrefs.HasKey("Wizard Hat"))
        {
            PlayerPrefs.SetInt("Wizard Hat", 0);
        }

        if (PlayerPrefs.HasKey("Active Hat"))
        {
            ActivateHat(PlayerPrefs.GetString("Active Hat"));
        }
        if (PlayerPrefs.HasKey("Active Face"))
        {
            ActivateFace(PlayerPrefs.GetString("Active Face"));
        }

        foreach (CosmeticData data in cosmeticData)
        {
            data.unlocked = PlayerPrefs.GetInt(data.cosmeticName) == 1 ? true : false;
        }
    }

    public void ResetCosmetics()
    {
        for (int i = 0; i < cosmeticData.Length; i++)
        {
            cosmeticData[i].unlocked = false;
        }
        SaveCosmetics();
        foreach (Transform child in pet.transform.GetComponentsInChildren<Transform>(true))
        {
            if (child.tag == "Hat" || child.tag == "Face") child.gameObject.SetActive(false);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform ui in cosmeticsScrollView)
        {
            if (cosmeticData[GetIndexByName(ui.name)].unlocked)
            {
                ui.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                ui.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    public void SaveCosmetics()
    {
        foreach (CosmeticData data in cosmeticData)
        {
            PlayerPrefs.SetInt(data.cosmeticName, data.unlocked ? 1 : 0);
        }

        //PlayerPrefs.SetInt("Captain Hat", 0);
        //PlayerPrefs.SetInt("Cowboy Hat", 0);
        //PlayerPrefs.SetInt("Crown", 0);
        //PlayerPrefs.SetInt("Miner Hat", 0);
        //PlayerPrefs.SetInt("Mustache", 0);
        //PlayerPrefs.SetInt("Pajama Hat", 0);
        //PlayerPrefs.SetInt("Police Cap", 0);
        //PlayerPrefs.SetInt("Shower Cap", 0);
        //PlayerPrefs.SetInt("Top Hat", 0);
        //PlayerPrefs.SetInt("Viking Helmet", 0);
        //PlayerPrefs.SetInt("Wizard Hat", 0);
    }

    public void ActivateHat(string cosmeticName)
    {
        ActivateCosmetic(cosmeticName, CosmeticType.HAT);
        PlayerPrefs.SetString("Active Hat", cosmeticName);
    }

    public void ActivateFace(string cosmeticName)
    {
        ActivateCosmetic(cosmeticName, CosmeticType.FACE);
        PlayerPrefs.SetString("Active Face", cosmeticName);
    }

    public void ActivateCosmetic(string cosmeticName, CosmeticType cosmeticType)
    {
        if (cosmeticData[GetIndexByName(cosmeticName)].unlocked)
        {
            if (cosmeticType == CosmeticType.HAT)
            {
                foreach (Transform child in pet.transform.GetComponentsInChildren<Transform>(true))
                {
                    if (child.tag == "Hat")
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                pet.transform.GetComponentsInChildren<Transform>(true).ToList<Transform>().Find(x => x.name == cosmeticName).gameObject.SetActive(true);
            }

            else if (cosmeticType == CosmeticType.FACE)
            {
                foreach (Transform child in pet.transform.GetComponentsInChildren<Transform>(true))
                {
                    if (child.tag == "Face")
                    {
                        child.gameObject.SetActive(false);
                    }
                }
                pet.transform.GetComponentsInChildren<Transform>(true).ToList<Transform>().Find(x => x.name == cosmeticName).gameObject.SetActive(true);
            }
        }

        else
        {
            if (cosmeticData[GetIndexByName(cosmeticName)].price <= CoinsManager.Instance.currentCoins)
            {
                CoinsManager.Instance.ChangeCoins(-20);
                cosmeticData[GetIndexByName(cosmeticName)].unlocked = true;
                ActivateCosmetic(cosmeticName, cosmeticType);
            }
        }
        UpdateUI();
    }

    public int GetIndexByName(string cosmeticName)
    {
        for (int i = 0; i < cosmeticData.Length; i++)
        {
            if (cosmeticData[i].cosmeticName.Equals(cosmeticName))
                return i;
        }

        Debug.LogWarning("No cosmetic found named " + cosmeticName);
        return -1;
    }
}