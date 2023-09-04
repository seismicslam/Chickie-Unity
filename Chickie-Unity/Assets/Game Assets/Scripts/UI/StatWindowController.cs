using UnityEngine;
using UnityEngine.UI;

public class StatWindowController : MonoBehaviour
{
    public Image happyBar;
    public Image foodBar;
    public Image waterBar;
    public Image healthBar;
    public Image energyBar;
    public NeedsController needsController;

    void Update()
    {
        UpdateStats();
    }

    public void UpdateStats()
    {
        if(needsController != null)
        {
            happyBar.fillAmount = needsController.currentNeeds.happiness/100;
            foodBar.fillAmount = needsController.currentNeeds.food/100;
            waterBar.fillAmount = needsController.currentNeeds.water/100;
            healthBar.fillAmount = needsController.currentNeeds.health/100;
            energyBar.fillAmount = needsController.currentNeeds.energy/100;
        }
    }
}
