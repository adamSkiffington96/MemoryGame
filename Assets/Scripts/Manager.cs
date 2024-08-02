using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DTT;
using DTT.UI.ProceduralUI;
using UnityEngine.UI;
//using System;

public class Manager : MonoBehaviour
{
    public RoundedImage[] selectorList;

    public Animator[] Letters;
    public GameObject[] ObjectLetters;

    private int level = 4;

    private int guesses = 0;

    private int wins = 0;

    private List<int> generatedNumbers = new();
    private List<int> playerNumbers = new();

    public Text generatedNumsUI;
    public Text playerNumsUI;

    public Text winCountUI;

    private float currentTime = 0f;

    private int flashingCycle = 0;

    private float delayTime = 0f;

    public GameObject ButtonParent;

    private bool addLevel = false;


    void Start()
    {
        SetupGame();
    }

    void Update()
    {
        ManageTime();
    }

    private void ManageTime()
    {
        if (delayTime > 0f)
        {
            delayTime -= Time.deltaTime;

            if (delayTime <= 0f)
            {
                foreach (GameObject item in ObjectLetters)
                {
                    item.SetActive(false);
                }

                ObjectLetters[generatedNumbers[flashingCycle]].SetActive(true);
            }
            else
            {
                return;
            }
        }

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                ObjectLetters[generatedNumbers[flashingCycle]].SetActive(false);

                if (flashingCycle < generatedNumbers.Count - 1)
                {
                    flashingCycle++;

                    ObjectLetters[generatedNumbers[flashingCycle]].SetActive(true);

                    currentTime = 1f;
                }
                else
                {
                    foreach (GameObject item in ObjectLetters)
                    {
                        item.SetActive(true);
                    }

                    ButtonParent.SetActive(true);
                }
            }
        }
    }

    private void SetupGame()
    {
        ButtonParent.SetActive(false);

        foreach(RoundedImage item in selectorList)
            item.enabled = false;

        delayTime = 2f;

        NextGame();

        currentTime = 1f;

        flashingCycle = 0;
    }

    private void NextGame()
    {
        guesses = 0;

        if(generatedNumbers.Count > 0)
            generatedNumbers.Clear();
        //generatedNumsUI.text = string.Empty;

        if(playerNumbers.Count > 0)
            playerNumbers.Clear();
        //playerNumsUI.text = string.Empty;

        for(int i = 0; i < level; i++)
        {
            int chosenLetterIndex = Random.Range(0, 4);

            generatedNumbers.Add(chosenLetterIndex);
            //generatedNumsUI.text += chosenLetterIndex.ToString();
        }
    }

    public void MakeGuess(int index)
    {
        playerNumbers.Add(index);
        //playerNumsUI.text += index.ToString();

        if (index == generatedNumbers[guesses])
        {
            print("Correct");

            guesses++;
        }
        else
        {
            print("Incorrect");

            guesses = 0;

            wins = 0;

            winCountUI.text = string.Empty;

            level = 4;

            addLevel = false;

            SetupGame();
        }

        if(guesses == generatedNumbers.Count)
        {
            print("Game complete");

            wins++;

            winCountUI.text = wins.ToString() + "00";

            if (addLevel)
            {
                level++;
                addLevel = false;
            }
            else
            {
                addLevel = true;
            }

            SetupGame();
        }

        Letters[index].SetTrigger("flip");

        print("My nums: " + playerNumbers.Count.ToString() + ", Gen nums: " + generatedNumbers.Count.ToString());

    }

    public void HoverOn(int index)
    {
        if (selectorList[index].enabled)
        {
            selectorList[index].enabled = false;
        }
        else
        {
            if(flashingCycle == generatedNumbers.Count - 1)
            {
                selectorList[index].enabled = true;
            }
        }
    }
}
