using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class DialogueManager : MonoBehaviour
{
    ///TO DO:
    //tener un objeto UI que muestre el nombre del personaje
    //Clase que tenga la información del personaje// tener un script con el nombre del personaje (ID/name del personaje)
    //Tener un indicador de que es interactuable (Presiona E para hablar/interactuar)
    //tener un objeto con el dialogo del personaje
    ///tener un contructor que se encargue de comunicar el panel de dialogo con los datos del personaje
    ///Posiblemente tener paneles con preguntas

    public DialogeObject dialogueObject;
        ///TENER EN CUENTA EN QUÉ DIALOGUE OBJECT ESTOY ACTUALMENTE

    [SerializeField]
    private GameObject PanelDialogue = null;
    private TMPro.TextMeshProUGUI textDialogue = null;
    [SerializeField] private Image portrait=null;

    [SerializeField]
    private GameObject PanelAnswers = null;
    public GameObject answerObject;

    [SerializeField]
    private GameObject PanelName = null;

    [SerializeField]
    private GameObject PanelPressE=null;
    

    private bool interacting;
    private bool keycodeCooldown;

    private void Start()
    {
        textDialogue = PanelDialogue.GetComponentInChildren<TextMeshProUGUI>();
        ClosePanels();
        PanelDialogue.SetActive(false);
        keycodeCooldown = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && keycodeCooldown )
        {
            StartCoroutine(timerCooldown());
            if(interacting)
            {
                PanelDialogue.SetActive(true);
                ClosePanels();
            }
            ///abro el panel de interración
            ///cerrar el panel con el nombre
        }      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactuable"))
        {
            GetCharacterInfo(collision.gameObject);
            interacting = true;
            PanelPressE.SetActive(true);
            PanelName.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactuable"))
        {
            interacting = false;
            ClosePanels();
        }

        PanelDialogue.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(interacting )
        {
            if(Input.GetKeyDown(KeyCode.E) && keycodeCooldown && PanelDialogue.activeSelf)
            {
                dialogueObject = collision.GetComponentInChildren<DialogeObject>();
                collision.GetComponentInChildren<DialogeObject>().IncreaseIndexMessage();
                StartCoroutine(timerCooldown());
                GetCharacterInfo(collision.gameObject);
            }
        }
    }

    public void Closeinteracting()
    {
        interacting = false;
    }

    public void ClosePanels()
    {
        PanelName.SetActive(false);
        PanelPressE.SetActive(false);
        PanelAnswers.SetActive(false);
    }

    public void CloseDialogue()
    {

        PanelDialogue.SetActive(false);
    }

    private void GetCharacterInfo(GameObject character)
    {
        DialogeObject characterDialogue = character.GetComponentInChildren<DialogeObject>();
        PanelName.GetComponentInChildren<TextMeshProUGUI>().text = characterDialogue.charName;
        portrait.sprite = characterDialogue.portrait;
        if(characterDialogue.CheckTheresOptions() )
        {
            textDialogue.text = character.GetComponentInChildren<DialogeObject>().GetMessage();
            if(characterDialogue.CheckIsQuestion())
            {
                foreach(string answer in characterDialogue.ReturnQuestions())
                {
                    //split de valor de respeto
                    GameObject instancedAnswerObject =  Instantiate(answerObject,PanelAnswers.transform) as GameObject;
                    answerObject.GetComponent<AnswerObject>().Constructor(answer);
                }
                PanelAnswers.SetActive(true);
            }
            else
            {
                textDialogue.text = character.GetComponentInChildren<DialogeObject>().GetMessage();
            }
        }
        ///Tomar panel del dialogo e ingresar a sus opciones que son rellenadas según el DialogueObject
    }

    IEnumerator timerCooldown()
    {
        keycodeCooldown = false;
        yield return new WaitForSeconds(0.5f);
        keycodeCooldown = true;
    }
}
