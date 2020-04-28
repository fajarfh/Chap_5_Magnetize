using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControlNew : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    public float initRot = 90f;
    public float slowmoRate = 0.5f;


    private Rigidbody2D rb2D;
    private AudioSource myAudio;
    [HideInInspector] public GameObject hookedTower;
    private UIControllerScriptNew uiControl;
    
    [HideInInspector] public bool isCrashed = false;

    private Vector3 startPosition;




    public void RestartPosition()
    {
        //Set to start position
        this.transform.position = startPosition;

        //Restart rotation
        this.transform.rotation = Quaternion.Euler(0f, 0f, initRot);

        //Set isCrashed to false
        isCrashed = false;

        Time.timeScale = 1f;

        if (hookedTower)
        {
            hookedTower.GetComponent<SpriteRenderer>().color = hookedTower.GetComponent<TowerControl>().baseColor;
            hookedTower = null;
        }

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Hide game object
            //this.gameObject.SetActive(false);
            //uiControl.RestartGame();
            if (!isCrashed)
            {
                isCrashed = true;
                //Play SFX
                myAudio.Play();
                rb2D.velocity = new Vector3(0f, 0f, 0f);
                rb2D.angularVelocity = 0f;
                
            }
        }
 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Slowmo"))
        {
            Time.timeScale = slowmoRate;
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            if (!isCrashed)
            {
                uiControl.EndGame();
            }
        }
        else if (collision.gameObject.CompareTag("Gateway"))
        {
            if (!isCrashed)
            {
                uiControl.RestartGame(collision.GetComponent<MainGateways>().sceneName);
            }
        }

    }



    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        myAudio = this.gameObject.GetComponent<AudioSource>();
        startPosition = this.transform.position;
        uiControl = GameObject.Find("Canvas").GetComponent<UIControllerScriptNew>();

        hookedTower = null;

    }

    // Update is called once per frame
    void Update()
    {
        if (isCrashed)
        {
            if (!myAudio.isPlaying)
            {
                //Restart scene
                //uiControl.RestartGame();
                RestartPosition();
            }
        }
        else
        {
            //Move the object
            rb2D.velocity = -transform.up * moveSpeed;

            //Mengaktifkan tower terdekat dan Menarik Player saat HookedTower tidak null akibat tower di klik
            if (hookedTower)
            {
                // Mengukur jarak antara tower dengan player
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);
                float rotadir = hookedTower.GetComponent<TowerControl>().rotadir;

                // Menentukan arah gravitasi dari player ke tower
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;

                // Menghitung besar gaya tarik
                float newPullForce = Mathf.Clamp(pullForce / distance, 30, 50);
                rb2D.AddForce(pullDirection * newPullForce);

                // Menghitung besar gaya angular
                float cekSudut =  rotadir * rotateSpeed / distance ;

                rb2D.angularVelocity = -cekSudut;

                //print(distance);
                //print(rb2D.angularVelocity);
                //print(pullDirection);
                //print(rb2D.velocity);
                //print(newPullForce);

            }
            else
            {
                rb2D.angularVelocity = 0f;
            }

        }

    }
}
