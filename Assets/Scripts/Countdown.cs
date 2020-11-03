﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Photon.Pun;
using System;
using Photon.Realtime;


public class Countdown : MonoBehaviour
{

    float currentTime = 0f;
    float startingTime = 80.49999f;
    float totalTime;
    double minutes;
    string min;
    string sec;
    float seconds;
    bool started;
    bool ended;

    GameObject GameController;

    public Image bg;


    [SerializeField] TextMeshProUGUI countdown;


    private void Start()
    {

        currentTime = startingTime;
        countdown.text = timeToString(currentTime);
        started = false;
        ended = false;
        

        GameController = GameObject.FindWithTag("GameController");
    }

    private void Update()
    {

        StartCoroutine(Delay());
        if (started)
        {
            if (currentTime >= 10)
            {
                currentTime -= 1 * Time.deltaTime;

                countdown.color = Color.white;
                countdown.text = timeToString(currentTime);
            }
            else if (currentTime < 10 && currentTime > 0)
            {
                currentTime -= 1 * Time.deltaTime;

                countdown.text = timeToString(currentTime);
                countdown.color = Color.red;

                countdown.fontSize = getFontSize(currentTime);
            }
            else if (currentTime <= 0)
            {
                countdown.fontSize = 60;
                countdown.text = "Game Over";
                countdown.color = Color.red;
                countdown.fontStyle = FontStyles.Bold;
                
                // End Game Sequence
                if (!ended)
                {
                    StartCoroutine("EndGame");
                    ended = true;
                }
            }

        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        started = true;
    }

    IEnumerator EndGame()
    {
        GameController.GetComponent<GameComplete>().stopMoving();

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / 4)
        {
            Color newColor = new Color(Mathf.Lerp(0.0f, 0.0f, t), Mathf.Lerp(0.0f, 0.0f, t), Mathf.Lerp(0.0f, 0.0f, t), Mathf.Lerp(0.0f, 1, t));

            bg.color = newColor;

            yield return null;
        }

        GameController.GetComponent<GameComplete>().enabled = true;

        Destroy(countdown);
    }



    string timeToString(float time)
    {
        if (currentTime > 60)
        {
            minutes = Math.Floor(currentTime / 60);
            min = minutes.ToString("0");
            seconds = currentTime % 60;
            sec = seconds.ToString("00");

            return (min + ":" + sec);
        }
        if (currentTime >= 10)
        {
            return currentTime.ToString("0");
        }
        else if (currentTime < 10 && currentTime > 0)
        {
            return currentTime.ToString("0.0");
        }
        else if (currentTime == 0)
        {
            return "GAME OVER";
        }
        return "GAME OVER";
    }

    int getFontSize(float time)
    {
        float perc = time % 1;

        return (int)Mathf.Lerp(40, 50, perc);
    }
}

  

