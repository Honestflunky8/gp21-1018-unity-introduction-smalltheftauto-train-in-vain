using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCar : MonoBehaviour
{
    [SerializeField] private int MaxCars;
    public GameObject NewCarPrefab;
    private PlayerInteract playerInteract;
    private bool spawnOnCoolDown;
    List<Vector3> spawnPositions = new List<Vector3>();
    private List<GameObject> npcCars = new List<GameObject>();

    void Start()
    {
        playerInteract = FindObjectOfType<PlayerInteract>();
        SpawnUpgraded(new Vector3(-10, 1.8f, 0));
        SpawnUpgraded(new Vector3(-30, 1.8f, 0));
        SpawnUpgraded(new Vector3(-20, 1.8f, 0));
        
        var spawnObjects = FindObjectsOfType<TAG_TrafficPoint>().ToList();
        foreach (TAG_TrafficPoint t in spawnObjects)
        { 
            spawnPositions.Add(t.gameObject.transform.position);
        }
        SpawnCarsInTraffic();
    }

    private void SpawnCarsInTraffic()
    {
        for (var i = 0; i < MaxCars; i++)
        {
            var randomizedSpawn = spawnPositions[Random.Range(0, spawnPositions.Count)];
            var car = SpawnUpgradedAndReturn(randomizedSpawn, Quaternion.identity, true);
            npcCars.Add(car);
        }
    }


    public void SpawnUpgraded(Vector3 spawnPosition, Quaternion rotation = new Quaternion(), bool isNPCDriver = false)
    {
        var car = Instantiate(NewCarPrefab, spawnPosition, rotation);
        car.GetComponent<AiDriving>().NPCInCar = isNPCDriver;
        playerInteract.Interactables.Add(car);
    }
    
    public GameObject SpawnUpgradedAndReturn(Vector3 spawnPosition, Quaternion rotation = new Quaternion(), bool NPCDriver = false)
    {
        var car = Instantiate(NewCarPrefab, spawnPosition, rotation);
        car.GetComponent<AiDriving>().NPCInCar = NPCDriver;
        playerInteract.Interactables.Add(car);
        return car;
    }
}
