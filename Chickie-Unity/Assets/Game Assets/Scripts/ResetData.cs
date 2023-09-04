using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetData : MonoBehaviour
{
    public void Resetting()
    {
        NeedsController.Instance.SetDefaultNeeds();
        CoinsManager.Instance.ResetCoins(false);
        SelfCareActivities.Instance.RefreshTask();
        PetCosmeticManager.Instance.ResetCosmetics();
        SaveJournal.Instance.ResetGratiuity();
        SaveJournal.Instance.ResetRant();
        PlayerPrefs.DeleteKey("First");
    }
}
