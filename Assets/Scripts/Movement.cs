using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float runSpeed = 10f;
    public float xAxis;

    public GameObject face;

    public AudioClip steps;
    private AudioSource audioSource;
    private bool tiempoCooldown;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        canMove = true;
        tiempoCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        xAxis = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 direction = new Vector2(xAxis, y);
        if(canMove)Run(direction);
        Debug.Log(rb.velocity.magnitude);
        if(rb.velocity.magnitude > 0 && tiempoCooldown)
        {
            StartCoroutine(TimerSound());
        }

        if(xAxis < 0 && transform.position.x < -11f)
        {
            canMove = false;
            rb.velocity = Vector2.zero;
            direction = Vector2.zero;
        }
        else {
            canMove = true;
        }

        ///Cara
        var pos = face.transform.localPosition;
        pos.x = Mathf.Clamp(pos.x+(xAxis/10), 0.1f, 0.9f);
        face.transform.localPosition = pos;
        ///
    }
    public void Run(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * runSpeed, 0);//rb.velocity.y
       
    }

    IEnumerator TimerSound()
    {
        Debug.Log("Suenaaaa");
        tiempoCooldown = false;
        audioSource.PlayOneShot(steps);
        float ran = Random.Range(0.1f,0.3f);
        ///random range al pitch y volumen
        audioSource.pitch = Random.Range(1,1.5f);
        audioSource.volume = Random.Range(0.8f,1.2f);
        yield return new WaitForSeconds(steps.length+ran);
        tiempoCooldown = true;
    }

}