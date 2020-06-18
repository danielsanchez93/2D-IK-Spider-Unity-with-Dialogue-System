using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCrow : MonoBehaviour
{
    [SerializeField]
    private GameObject TextCloud = null;

    private void Start()
    {
        TextCloud.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TextCloud.SetActive(true);
        }
    }
}
