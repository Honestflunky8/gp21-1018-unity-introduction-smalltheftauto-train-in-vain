using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRaceController : MonoBehaviour
{
    private List<GameObject> carRaceComponents = new List<GameObject>();
    private List<GameObject> placerPrefabs = new List<GameObject>();
    private List<GameObject> checkPointPostions = new List<GameObject>();

    public SpawnCar SpawnCar;
    public GameObject CarSpawnPosition;
    
    public GameObject GoalPrefab;
    public GameObject CheckPointPrefab;
    
    public GameObject QuestCar { get; private set; }

    
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            carRaceComponents.Add(child.gameObject);
        }
        
        DisplayQuest(false);
        ScanCheckPointPostion();
    }

    
    
    public void ActivateCarRaceQuest()
    {
        QuestCar = SpawnCar.SpawnAndReturn(CarSpawnPosition.transform.position, CarSpawnPosition.transform.rotation);

        var goalObject = Instantiate(GoalPrefab, transform);
        goalObject.transform.position = transform.Find("FinishPosition").gameObject.transform.position;
        goalObject.transform.rotation = transform.Find("FinishPosition").gameObject.transform.rotation;
        placerPrefabs.Add(goalObject);
        DisplayQuest(true);
        PlaceCheckPoints();
    }

    private void PlaceCheckPoints()
    {
        foreach (var checkPointPostion in checkPointPostions)
        {
            var checkpoint = Instantiate(CheckPointPrefab, transform);
            checkpoint.transform.position = checkPointPostion.transform.position;
            checkpoint.transform.rotation = checkPointPostion.transform.rotation;
            placerPrefabs.Add(checkpoint);
        }
        
    }

    void ScanCheckPointPostion()
    {
        var currentPostion = gameObject;
        for (int index = 1; currentPostion != null; index++)
        {
            try
            {
                currentPostion = transform.Find("CheckPointPostion" + index).gameObject;
                checkPointPostions.Add(currentPostion);
            }
            catch 
            {
                currentPostion = null;
            }
        }
    }

    /// <summary>
    /// RaceCompleted
    /// </summary>
    /// <param name="playerWin">True or false for if the player wom.</param>
    /// <param name="totalTime">Total time of race.</param>
    public void RaceCompleted(bool playerWin, int totalTime)
    {
        if (playerWin)
        {
            Debug.Log($"You won the race!");
            GameObject.FindGameObjectWithTag("PhoneBox").GetComponent<QuestMenuController>().QuestIsActive = false;
            DisplayQuest(false);
        }
    }


    private void DisplayQuest(bool value)
    {
        foreach (GameObject childComponent in carRaceComponents)
        {
            childComponent.SetActive(value);
        }

        if (!value)
        {
            Debug.Log("Destroyed prefabs");
            DestroyPrefabs();
        }
    }

    private void DestroyPrefabs()
    {
        foreach (var prefab in placerPrefabs)
        {
            Destroy(prefab);
        }
    }
}
