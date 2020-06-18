using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogeObject : MonoBehaviour
{
    public Charla[] dialogues;

    public Sprite portrait;

    [SerializeField]
    private int indexMessage;

    public string charName;

    public string actualDialogue;

    private GameObject panelDialogue;
   

    public string GetMessage()
    {
        return dialogues[indexMessage].message;
    }

    public bool CheckIsQuestion()
    {
        return dialogues[indexMessage].isQuestion;
    }

    public void IncreaseIndexMessage()
    {
        if(CheckTheresOptions()) indexMessage++;
    }

    /// <summary>
    /// Comprueba que haya más líneas de diálogo
    /// </summary>
    public bool CheckTheresOptions()
    {
        if(indexMessage < dialogues.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string[] ReturnQuestions()
    {
        return dialogues[indexMessage].questions;
    }

}

[System.Serializable]
public class Charla
{
    public bool isQuestion;
    public string[] questions;
    public int[] respectValues;
    [TextArea]
    public string message;
}

