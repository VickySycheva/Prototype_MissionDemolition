using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
    {
        idle,
        playing,
        levelEnd
    }

public class MissionDemotion : MonoBehaviour
{
    static private MissionDemotion S;

    public TMP_Text uitLevel;
    public TMP_Text uitShots;
    public TMP_Text uitButton;

    public Vector3 castlePos;
    public GameObject[] castles;

    public int level;
    public int levelMax;
    public int shotsTaken;

    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";

    void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy (pTemp);
        }

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "") eView = uitButton.text;
        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
            FollowCam.POI = null;
            uitButton.text = "Show Castle";
            break;

            case "Show Castle":
            FollowCam.POI = S.castle;
            uitButton.text = "Show Both";
            break;

            case "Show Both":
            FollowCam.POI = GameObject.Find ("ViewBoth");
            uitButton.text = "Show Slingshot";
            break;
        }
    }

    void UpdateGUI ()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update ()
    {
        UpdateGUI();
        if ((mode == GameMode.playing ) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke(nameof(NextLevel), 2f);
        }
    }

    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
