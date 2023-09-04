using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void CompleteTask()
    {
        SelfCareActivities.Instance.CompleteTask(transform.GetChild(0).GetComponent<TMP_Text>().text);
    }
}
