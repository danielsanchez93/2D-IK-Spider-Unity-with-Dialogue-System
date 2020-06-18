using UnityEngine.UI;
using UnityEngine;

public class AnswerObject: MonoBehaviour
{
    public int score;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(()=>SendScore());
    }

    public void Constructor(string text)
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
    }

    public void SendScore()
    {
        GameObject.FindObjectOfType<GameManager>().ChangeScore(score);
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.dialogueObject.IncreaseIndexMessage();
        dialogueManager.ClosePanels();
        dialogueManager.CloseDialogue();
        dialogueManager.Closeinteracting();
        //incrementar el index
    }
}
