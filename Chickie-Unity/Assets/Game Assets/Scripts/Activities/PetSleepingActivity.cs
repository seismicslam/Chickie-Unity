using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetSleepingActivity : MonoBehaviour
{
    [SerializeField] int coinsReward;
    [SerializeField] Needs sleepFinishNeeds1;
    [SerializeField] Needs sleepFinishNeeds2;
    [SerializeField] Needs sleepFinishNeeds3;
    [SerializeField] Needs sleepFinishNeeds4;
    [SerializeField] float startingTime;
    [SerializeField] Transform pet;
    [SerializeField] Transform startNode;
    [SerializeField] GameObject sleepingStatus;

    [Space(20)]
    [SerializeField] int timerButtonChosen;
    [SerializeField] Outline timer1;
    [SerializeField] Outline timer2;
    [SerializeField] Outline timer3;
    [SerializeField] Outline timer4;
    [SerializeField] TMP_Text currentSleepText;
    [SerializeField] ParticleSystem starParticles;
    [SerializeField][ReadOnly] double currentTimeLeft;
    [SerializeField][ReadOnly] float chosenTime;

    void Awake()
    {
        if(PlayerPrefs.HasKey("Last Sleep Timer Chosen"))
        {
            PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.SLEEPING);
            timerButtonChosen = PlayerPrefs.GetInt("Last Sleep Timer Chosen");
        }
    }

    void Start()
    {
        timerButtonChosen = 1;
        chosenTime = startingTime;
    }

    void Update()
    {
        UpdateTimerUI();
        UpdateTimer();
    }

    // CALLED ON CONFIRM TIMER
    public void EquipSleeping()
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING)
        {
            SleepingUIActive(true);

            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.SLEEPING);

            StartCoroutine(RunTowardsCenter());
        }
    }
    void SleepingUIActive(bool active)
    {
        sleepingStatus.SetActive(active);

        CameraMovement.Instance.allowMovement = !active;
        CameraMovement.Instance.resetPos = active;
    }

    IEnumerator RunTowardsCenter()
    {
        PetMoveManager.Instance.StopMoving();
        PetMoveManager.Instance.ChangeAndMoveToNode(startNode.position);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }

        pet.rotation = Quaternion.Euler(0,180,0);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.SLEEPING);
        PlayerPrefs.SetString("Sleep Time Stop", System.DateTime.UtcNow.AddSeconds(chosenTime).ToString());
    }

    void UpdateTimer()
    {   
        if(PlayerPrefs.HasKey("Sleep Time Stop"))
        {
            currentTimeLeft = (DateTime.Parse(PlayerPrefs.GetString("Sleep Time Stop")) - System.DateTime.UtcNow).TotalSeconds;
            if(currentTimeLeft <= 0)
            {
                // IF SLEEP HAS FINISHED
                starParticles.Play();
                EndSleep(true);
            } else
            {
                // IF SLEEP IS STILL GOING
                currentSleepText.text = "Sleep Timer: " + currentTimeLeft.ToString("F2");
                SleepingUIActive(true);
            }
        }
        else
        {
            currentSleepText.text = "";
        }
    }

    // CALLED ON SLEEP CANCEL OR FINISH
    public void EndSleep(bool recieveRewards)
    {
        if(recieveRewards)
        {
            CoinsManager.Instance.ChangeCoins(coinsReward);
            if(timerButtonChosen == 2) NeedsController.Instance.ChangeNeeds(sleepFinishNeeds2);
            else if(timerButtonChosen == 3) NeedsController.Instance.ChangeNeeds(sleepFinishNeeds3);
            else if(timerButtonChosen == 4) NeedsController.Instance.ChangeNeeds(sleepFinishNeeds4);
            else NeedsController.Instance.ChangeNeeds(sleepFinishNeeds1);
        }

        PlayerPrefs.DeleteKey("Sleep Time Stop");
        PlayerPrefs.DeleteKey("Last Sleep Timer Chosen");

        SleepingUIActive(false);
        PetPatrol.Instance.StartPatrolling();
    }

    // CALLED ON EACH TIMER BUTTONS
    public void SetCurrentTimer(float time)
    {
        chosenTime = time;
    }

    // CALLED ON EACH TIMER BUTTONS
    public void SetTimerButtonChosen(int button)
    {
        if(button > 4 || button < 1) return;
        timerButtonChosen = button;
        PlayerPrefs.SetInt("Last Sleep Timer Chosen", button);
    }

    void UpdateTimerUI()
    {
        if(timerButtonChosen == 2)
        {
            timer1.enabled = false;
            timer2.enabled = true;
            timer3.enabled = false;
            timer4.enabled = false;
        } else if(timerButtonChosen == 3)
        {
            timer1.enabled = false;
            timer2.enabled = false;
            timer3.enabled = true;
            timer4.enabled = false;
        } else if(timerButtonChosen == 4)
        {
            timer1.enabled = false;
            timer2.enabled = false;
            timer3.enabled = false;
            timer4.enabled = true;
        } else
        {
            timer1.enabled = true;
            timer2.enabled = false;
            timer3.enabled = false;
            timer4.enabled = false;
        }
    }
}
