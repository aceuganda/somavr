using System;
using System.IO;
using UnityEngine;
using TMPro;
 
public class SavingSystem : MonoBehaviour
{

    [Space]
    [Header("Saving Feature")]

    // public InputField nameInputField;
    [SerializeField] public TextMeshProUGUI YourPoints;
 
    public void SaveToJson()
    {
        PlayerPoints data = new PlayerPoints();
        // data.Name = nameInputField.text;
        data.Name = "Tusiime Allan";
        data.Points = YourPoints.text;
        data.Date = DateTime.Now.ToString();
 
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/ACE_Training_Data.json", json);
        Debug.Log("Data is Being saved! ");
    }
 

    //Incase we shall ever need to Load this data
    //
    //
    //
    // public void LoadFromJson()
    // {
    //     string json = File.ReadAllText(Application.dataPath + "/ACE_Training_Data.json");
    //     PlayerPoints data = JsonUtility.FromJson<PlayerPoints>(json);
 
    //     nameInputField.text = data.Name;
    //     YourPoints.text = data.Points;
    // }
}