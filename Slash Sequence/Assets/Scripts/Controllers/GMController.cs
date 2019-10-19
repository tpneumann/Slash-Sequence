using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMController : MonoBehaviour
{
    public static GMController Instance { get; private set; }

    public bool cameraZoom = false;
    public float timeSpeed = 0.1f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }
    
    public void slowTime()
    {
        Time.timeScale = timeSpeed;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
    }

    public void normalTime()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.2f;
    }
}
