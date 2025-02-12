using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Points;

    private static int scoreValue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddPoints(int toAdd)
    {
        scoreValue = scoreValue + toAdd;

        Points.text = scoreValue.ToString();
    }
}
