using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogue : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogBox;
    [SerializeField]
    private TextMeshProUGUI dialogTextHeader;

    [SerializeField]
    private GameObject[] choices;

    [HideInInspector]
    public string lastChoiceTaken = "";

    private TextMeshProUGUI[] choicesTexts;

    private Story currentStory;

    private void Awake()
    {
        dialogBox.SetActive(false);

        choicesTexts = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesTexts[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private bool CanContinueStory()
    {
        if (!currentStory)
            return false;

        if (currentStory.canContinue)
            return true;
        else
        {
            dialogBox.SetActive(false);
            dialogTextHeader.text = "";
            currentStory = null;
            return false;
        }
    }

    public bool ContinueStory()
    {
        if (!CanContinueStory())
            return false;

        if (currentStory.currentChoices.Count == 0)
            dialogTextHeader.text = currentStory.Continue();

        DisplayChoices();

        return true;
    }

    public void StartNewDialogue(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        dialogBox.SetActive(true);

        HideAllChoices();

        ContinueStory();
    }

    private void DisplayChoices()
    {
        List<Choice> currectChoices = currentStory.currentChoices;

        if (currectChoices.Count > choices.Length)
        {
            Debug.Log("Too much choices");
        }

        int index = 0;
        foreach (Choice choice in currectChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesTexts[index].text = choice.text;
            index++;
            Cursor.lockState = CursorLockMode.None;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }

    private void HideAllChoices()
    {
        foreach (var item in choices)
        {
            item.SetActive(false);
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        lastChoiceTaken = currentStory.currentChoices[choiceIndex].text;

        currentStory.ChooseChoiceIndex(choiceIndex);

        ContinueStory();

        Cursor.lockState = CursorLockMode.Locked;
    }
}
