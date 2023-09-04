using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    public CinemachineVirtualCamera cam;
    CinemachineTrackedDolly dolly;

    public float swipeSens;
    float currentPath;

    public bool resetPos = false;
    public bool allowMovement = true;
    float resetSpeed = 3;

    public static CameraMovement Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        dolly = cam.GetCinemachineComponent<CinemachineTrackedDolly>();
        currentPath = 0.5f;
    }

    void Update()
    {
        currentPath = Mathf.Clamp(currentPath, 0f, 1f);
        dolly.m_PathPosition = currentPath;

        if (!resetPos && Input.GetMouseButton(0) && allowMovement)
        {
            if (Input.mousePosition.x <= Screen.width/6.0f) {
                currentPath -= swipeSens;
            }
            
            else if (Input.mousePosition.x >= (Screen.width - Screen.width/6.0f))
            {
                currentPath += swipeSens;
            }
        }

        if (resetPos)
        {
            currentPath = Mathf.MoveTowards(currentPath, 0.5f, Time.deltaTime * resetSpeed);
        }
    }

    public void ChangeAllow(bool bl)
    {
        allowMovement = bl;
    }
}

