using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PetExploreActivity : MonoBehaviour
{
    [SerializeField] int coinsReward;
    [SerializeField] Needs exploreFinishNeeds1;
    [SerializeField] Needs exploreFinishNeeds2;
    [SerializeField] Needs exploreFinishNeeds3;
    [SerializeField] Needs exploreFinishNeeds4;
    [SerializeField] float startingTime;
    [SerializeField] GameObject petModel;
    [SerializeField] Transform exploreNode;
    [SerializeField] GameObject exploringStatus;
    [SerializeField] GameObject normalCam;
    [SerializeField] GameObject exploreCam;

    [Space(20)]
    [SerializeField] int timerButtonChosen;
    [SerializeField] Outline timer1;
    [SerializeField] Outline timer2;
    [SerializeField] Outline timer3;
    [SerializeField] Outline timer4;
    [SerializeField] TMP_Text currentExploreText;
    [SerializeField] ParticleSystem starParticles;
    [SerializeField][ReadOnly] double currentTimeLeft;
    [SerializeField][ReadOnly] float chosenTime;

    void Awake()
    {
        if(PlayerPrefs.HasKey("Last Explore Timer Chosen"))
        {
            petModel.SetActive(false);

            PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);
            timerButtonChosen = PlayerPrefs.GetInt("Last Explore Timer Chosen");
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
    public void EquipExploring()
    {
        if(PetStateManager.Instance.GetCurrentPetState() == PetStateManager.PetStates.PATROLLING)
        {
            PlayerPrefs.SetInt("Last Explore Timer Chosen", timerButtonChosen);
            ExploringUIActive(true);

            PetPatrol.Instance.StopPatrolling();
            PetStateManager.Instance.ChangePetState(PetStateManager.PetStates.EXPLORING);

            StartCoroutine(RunTowardsExplore());
        }
    }
    void ExploringUIActive(bool active)
    {
        exploringStatus.SetActive(active);

        exploreCam.SetActive(active);
        normalCam.SetActive(!active);
    }

    IEnumerator RunTowardsExplore()
    {
        PetMoveManager.Instance.StopMoving();
        PetMoveManager.Instance.ChangeAndMoveToNode(exploreNode.position);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.RUNNING);

        yield return new WaitForSeconds(0.2f);

        while(!PetMoveManager.Instance.HasReachedDestination())
        {
            yield return 0;
            continue;
        }
        petModel.SetActive(false);
        PetCosmeticManager.Instance.cosmetics.SetActive(false);
        PetAnimationManager.Instance.SetAnimationState(PetAnimationManager.AnimationState.IDLE);
        PlayerPrefs.SetString("Explore Time Stop", System.DateTime.UtcNow.AddSeconds(chosenTime).ToString());
    }

    void UpdateTimer()
    {   
        if(PlayerPrefs.HasKey("Explore Time Stop"))
        {
            currentTimeLeft = (DateTime.Parse(PlayerPrefs.GetString("Explore Time Stop")) - System.DateTime.UtcNow).TotalSeconds;
            if(currentTimeLeft <= 0)
            {
                // IF EXPLORE HAS FINISHED
                starParticles.Play();
                EndExplore(true);
            } else
            {
                // IF EXPLORE IS STILL GOING
                currentExploreText.text = "Explore Timer: " + currentTimeLeft.ToString("F2");
                ExploringUIActive(true);
            }
        }
        else
        {
            currentExploreText.text = "";
        }
    }

    // CALLED ON EXPLORE CANCEL OR FINISH
    public void EndExplore(bool recieveRewards)
    {
        if(recieveRewards)
        {
            if(timerButtonChosen == 2) NeedsController.Instance.ChangeNeeds(exploreFinishNeeds2);
            else if(timerButtonChosen == 3) NeedsController.Instance.ChangeNeeds(exploreFinishNeeds3);
            else if(timerButtonChosen == 4) NeedsController.Instance.ChangeNeeds(exploreFinishNeeds4);
            else NeedsController.Instance.ChangeNeeds(exploreFinishNeeds1);

            CoinsManager.Instance.ChangeCoins(coinsReward);
        }

        PlayerPrefs.DeleteKey("Explore Time Stop");
        PlayerPrefs.DeleteKey("Last Explore Timer Chosen");

        ExploringUIActive(false);

        petModel.transform.parent.position = exploreNode.position;
        petModel.SetActive(true);
        PetCosmeticManager.Instance.cosmetics.SetActive(true);
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
