﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.IO;

/// <summary>
/// This script is used for the main setup of the game.
/// It instantiates the avatars for each player based on the previously indicated character choices, and calls functions to activate the correct map.
/// It also attaches all required components to each character for the accurate functionality.
/// </summary>
public class GameSetUp : MonoBehaviour
{
    // Singleton
    public static GameSetUp GS;

    // Arena Controller for Map setting
    public GameObject ArenaCon;
    public int mapIndex;
    public int Category;
    public int catLevel;
    public int Difficulty;
    public bool Tutorial;

    // Spawn points for each player
    public Transform spawnPoint;
    public Transform[] spawnPoints1;
    public Transform[] spawnPoints2;
    public Transform[] spawnPoints3;
    public Transform[] spawnPoints4;

    // Parameters for instantiation of each avatar
    public GameObject canvas;
    public GameObject countdownPanel;
    public GameObject question;
    public TextMeshProUGUI countdown;
    public GameObject player;
    public GameObject slot;
    public GameObject playerCam;

    // Points list in UI to track player points
    public TextMeshProUGUI pointsUI;
    string curUserName;

    /// <summary>
    /// This function is called at the very start of the game, to allow the script to access the choices made by players in the previous scene.
    /// </summary>

    private void OnEnable()
    {
        if (GameSetUp.GS == null)
        {
            GameSetUp.GS = this;
        }
    }

    /// <summary>
    /// This function is called at the start of the game, to initialize all required settings and instantiate the players and components
    /// </summary>

    void Start()
    {
        // Activate correct map based on mapIndex

        try{
            mapIndex = MapController.mapIndex;
            ArenaCon.GetComponent<ArenaController>().setUpMap(mapIndex-1);
        }
        catch (Exception e){
            print("In Tutorial");
        }


        // // select correct spawnpoints based on map chosen
        // Transform[] spawnPoints = null;
        // switch (mapIndex)
        // {
        //     case 0:
        //         spawnPoints = spawnPoints1;
        //         break;

        //     case 1:
        //         spawnPoints = spawnPoints2;
        //         break;

        //     case 2:
        //         spawnPoints = spawnPoints3;
        //         break;

        //     case 3:
        //         spawnPoints = spawnPoints4;
        //         break;
        // }

        // Initialize player avatar settings

        try
        {
            curUserName = Login.username;
        }
        catch (Exception e)
        {
            curUserName = "Tester";
        }

        if (curUserName == null) { curUserName = "Tester"; };

        int avatarSelection = 21;

        try
        {
            avatarSelection = LobbySetUp.LS.playerData;
            Category = LobbySetUp.LS.category;
            catLevel = LobbySetUp.LS.catLevel;
        }
        catch (Exception e)
        {
            avatarSelection = 21;
            Category = 0;
            catLevel = 1;
        }

        string avatarPath = "Astronaut";
        switch (avatarSelection / 10)
        {
            case 1:
                avatarPath = "Mummy 1";
                break;

            case 2:
                avatarPath = "Astronaut";
                break;

            case 3:
                avatarPath = "robotSphere 1";
                break;


            default:
                break;
        }

        // Instantiate correct avatar
        print("create Player");
        var player_prefab = Resources.Load(avatarPath);
        player = (GameObject)Instantiate(player_prefab, spawnPoint.transform.position, Quaternion.identity);
        player.tag = "Player";


        // Instantiate player camera and attach to player
        print("create Cam");
        var cam_prefab = Resources.Load("Main Camera");
        playerCam = (GameObject)Instantiate(cam_prefab, Vector3.zero, Quaternion.Euler(26.618f, 0f, 0f));
        playerCam.GetComponent<Camera>().enabled = true;
        playerCam.GetComponent<CameraFollow>().setTarget(player);

        // Instantion Question UI element and assign to player
        print("create Q");
        var q_prefab = Resources.Load("Question");
        question = (GameObject)Instantiate(q_prefab, canvas.transform.position, Quaternion.identity);
        question.GetComponent<DoQuestion>().tag = "Q1";
        question.GetComponent<DoQuestion>().player = player;
        question.GetComponent<DoQuestion>().QM = gameObject.GetComponent<QuestionManager>();
        if (Tutorial)
        {
            question.GetComponent<DoQuestion>().changeMode(true);
        }
        question.transform.SetParent(canvas.transform);
        question.SetActive(false);

        // Assign player parameters
        player.GetComponent<PlayerController>().playerName = curUserName;
        player.GetComponent<PlayerController>().question = question.gameObject;
        player.GetComponent<PlayerController>().colorIndex = avatarSelection % 10;

        player.gameObject.tag = "Player";
    }
}
