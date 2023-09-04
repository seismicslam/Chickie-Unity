using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UnityEvent FirstLaunch;
    public AudioSource audioSource;
    public Slider audioSlider;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CheckFirstLaunch();

        audioSlider.value = audioSource.volume;
    }

    void Update()
    {
        audioSource.volume = audioSlider.value;
    }

    void CheckFirstLaunch()
    {
        if(!PlayerPrefs.HasKey("First"))
        {
            PlayerPrefs.SetInt("First", 1);
            PlayerPrefs.SetString("Last Login Date", DateTime.Now.Date.ToString("yyyy-MM-dd"));
            FirstLaunch?.Invoke();
        }
    }
}
