using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandlePassenger : MonoBehaviour
{
    private GameObject Player;
    public CarMovement CarMovement;
    public GameObject ExitPosition;

    void Awake()
    {
        Player = GameObject.FindWithTag("Player");
    }

    public void Enter()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player");
        Player.SetActive(false);
        CarMovement.enabled = true;
    }

    public void Exit()
    {
        Player.transform.position = ExitPosition.transform.position;
        Player.SetActive(true);
        CarMovement.enabled = false;
    }
}
