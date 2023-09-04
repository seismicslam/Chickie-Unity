using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfCareActivities : MonoBehaviour
{
    public List<SelfCareTask> allTasks = new List<SelfCareTask>();
    public List<SelfCareTask> uncompleted = new List<SelfCareTask>();
    public List<SelfCareTask> completed = new List<SelfCareTask>();
    public GameObject taskUI;
    public Transform taskScrollView;
    public Color disabledColor;

    public static SelfCareActivities Instance;

    public GameObject titleEntry;
    public GameObject descEntry;

    void Awake()
    { 
        Instance = this;
    }

    void Start()
    {
        DateTime lastLoginTime = PlayerPrefs.HasKey("Last Login Date") ? 
            DateTime.Parse(PlayerPrefs.GetString("Last Login Date")) : DateTime.MaxValue;
        if (DateTime.Now.Date > lastLoginTime)
        {
            RefreshTask();
            PlayerPrefs.SetString("Last Login Date", DateTime.Now.Date.ToString("yyyy-MM-dd"));
        }
        TaskLoad();
        SpawnTask();
    }

    private void OnApplicationQuit()
    {
        TaskSave();
    }

    public void CustomTask()
    {
        
    }

    void TaskSave()
    {
        foreach (SelfCareTask task in uncompleted) PlayerPrefs.SetInt(task.name, 0);
        foreach (SelfCareTask task in completed) PlayerPrefs.SetInt(task.name, 1);
    }

    void TaskLoad()
    {
        foreach (SelfCareTask task in allTasks)
        {
            if (!PlayerPrefs.HasKey(task.name)) PlayerPrefs.SetInt(task.name, 0);
            if (PlayerPrefs.GetInt(task.name) == 0) uncompleted.Add(task);
            else completed.Add(task);
        }
    }

    public void RefreshTask()
    {
        foreach (SelfCareTask task in completed)
        {
            uncompleted.Add(task);
            PlayerPrefs.SetInt(task.name, 0);
        }
        completed.Clear();
        SortTasks();
        UpdateUI();
    }

    void SpawnTask()
    {
        foreach (SelfCareTask task in uncompleted)
        {
            Transform newTask = Instantiate(taskUI, taskScrollView).transform;
            newTask.GetChild(0).GetComponent<TMP_Text>().text = task.name;
            newTask.GetChild(1).GetComponent<TMP_Text>().text = task.description;
        }
        foreach (SelfCareTask task in completed)
        {
            Transform newTask = Instantiate(taskUI, taskScrollView).transform;
            newTask.GetChild(0).GetComponent<TMP_Text>().text = task.name;
            newTask.GetChild(1).GetComponent<TMP_Text>().text = task.description;
        }
        UpdateUI();
    }

    public void CompleteTask(string name)
    {
        foreach (SelfCareTask task in completed)
        {
            if (task.name.Equals(name)) return;
        }

        if (uncompleted.Contains(uncompleted.Find(x => x.name == name)))
        {
            completed.Add(uncompleted.Find(x => x.name == name));
            uncompleted.Remove(uncompleted.Find(x => x.name == name));
            CoinsManager.Instance.ChangeCoins(20);
        }
        SortTasks();
    }

    void UpdateUI()
    {
        foreach (Transform ui in taskScrollView)
        {
            if (ui.name == "New Custom Task") continue;
            if (completed.Contains(completed.Find(x => x.name == ui.GetChild(0).GetComponent<TMP_Text>().text)))
            {
                ui.transform.GetChild(2).GetComponent<Button>().interactable = false;
                ui.transform.GetComponent<Image>().color = disabledColor;
            }
        }
    }

    void SortTasks()
    {
        for (int i = taskScrollView.childCount - 1; i >= 1; i--)
        {
            Transform child = taskScrollView.GetChild(i);
            Destroy(child.gameObject);
        }

        SpawnTask();
    }
}

[System.Serializable] 

public struct SelfCareTask
{
    public string name;
    public string description;
}