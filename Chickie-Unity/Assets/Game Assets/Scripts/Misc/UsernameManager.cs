using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsernameManager : MonoBehaviour
{
    [SerializeField] 
    public void setUsername(string username)
    {
        if (!PlayerPrefs.HasKey("Username"))
        {
            PlayerPrefs.SetString("Username", username);
        }
    }
}
