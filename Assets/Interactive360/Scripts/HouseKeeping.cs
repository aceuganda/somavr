using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseKeeping : MonoBehaviour
{
    [Space]
    [Header("House Keeping")]
    public GameObject WelcomeScreen;
    // public GameObject TeachingScreen;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowNecessaryItems()
    {
        WelcomeScreen.SetActive(false);
    }
}
