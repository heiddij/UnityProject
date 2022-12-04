using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // JATKA VIDEOTA 23MIN KOHDALTA :)

    // Fields
    // Window
    public GameObject window;

    // Indicator
    public GameObject indicator;

    // Text component
    public TMP_Text dialogueText;

    // Dialogues List
    public List<string> dialogues;

    // Writing speeed
    public float writingSpeed;

    // Index on dialogue
    private int index;

    //Character index
    private int charIndex;
    // Started boolean
    private bool started;
    // Wait for next boolean
    private bool waitForNext;

    private void Awake()
    {
        ToggleIndicator(false);
        ToggleWindow(false);
    }

    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    // Start dialogue
    public void StartDialogue()
    {
        if(started)
            return;

        // Boolean to indicate we have stared
        started = true;
        // Show the window
        ToggleWindow(true);
        // Hide the indicator
        ToggleIndicator(false);
        // Start with first dialogue
        GetDialogue(0);
    }

    private void GetDialogue(int i)
    {
        // Start index at zero
        index = i;
        // Reset the char index
        charIndex = 0;
        // Clear the dialogue component text
        dialogueText.text = string.Empty;
        // Start writing
        StartCoroutine(Writing());
    }

    // End dialogue
    public void EndDialogue()
    {
        //Stared is disabled
        started = false;
        //Disable wait for next as well
        waitForNext = false;
        //Stop all Ienumerators
        StopAllCoroutines();
        //Hide the window
        ToggleWindow(false);
    }

    // Writing logic
    IEnumerator Writing()
    {
        yield return new WaitForSeconds(writingSpeed);

        string currentDialogue = dialogues[index];
        // Write the character
        dialogueText.text += currentDialogue[charIndex];
        // Increase the character index
        charIndex++;
        // Make sure you have end of the sentence
        if (charIndex < currentDialogue.Length)
        {
            // Wait x seconds
            yield return new WaitForSeconds(writingSpeed);
            // Restart the same process
            StartCoroutine(Writing());
        }
        else
        {
            // End this sentence and wait for the next one
            waitForNext = true;
        }
    }

    private void Update()
    {
        if (!started)
            return;

        if (waitForNext && Input.GetKeyDown(KeyCode.E))
        {
            waitForNext = false;
            index++;

            // Check if we are in the scope to dialogues list
            if (index < dialogues.Count)
            {
                // If so fetch the dialogue
                GetDialogue(index);
            }
            else
            {
                // If not end the dialogue process
                ToggleIndicator(true);
                EndDialogue();
            }
        }
    }
}
