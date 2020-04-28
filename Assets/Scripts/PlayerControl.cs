using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float pullForce = 100f;
    public float rotateSpeed = 360f;
    public float slowmoRate = 0.5f;

    public Color closestTowerColor = new Color(0, 172, 255, 128);

    private Rigidbody2D rb2D;
    private AudioSource myAudio;
    private GameObject closestTower;
    private GameObject hookedTower;
    private UIControllerScript uiControl;
    
    private bool isPulled = false;

    [HideInInspector] public bool isCrashed = false;

    private Color baseColor;
    private Vector3 startPosition;

    public void RestartPosition()
    {
        //Set to start position
        this.transform.position = startPosition;

        //Restart rotation
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        //Set isCrashed to false
        isCrashed = false;
        isPulled = false;

        Time.timeScale = 1f;

        if (closestTower)
        {
            closestTower.GetComponent<SpriteRenderer>().color = baseColor;
            closestTower = null;
        }

        if (hookedTower)
        {
            hookedTower.GetComponent<SpriteRenderer>().color = baseColor;
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

        if (collision.gameObject.CompareTag("Tower"))
        {
            baseColor = collision.gameObject.GetComponent<SpriteRenderer>().color;

        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tower") && !isPulled)
        {
            if (closestTower != collision.gameObject)
            {
                closestTower = collision.gameObject;
            }
            
            //Change tower color as indicator
            collision.gameObject.GetComponent<SpriteRenderer>().color = closestTowerColor;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //if (isPulled || isCrashed) return;

        if (collision.gameObject.CompareTag("Goal") && !isCrashed)
        {

            uiControl.EndGame();

        }

        if (collision.gameObject.CompareTag("Tower"))
        {
            closestTower = null;

            //Change tower color back to normal
            collision.gameObject.GetComponent<SpriteRenderer>().color = baseColor;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = this.gameObject.GetComponent<Rigidbody2D>();
        myAudio = this.gameObject.GetComponent<AudioSource>();
        startPosition = this.transform.position;
        closestTower = null;
        uiControl = GameObject.Find("Canvas").GetComponent<UIControllerScript>();

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
            
        }

        //Mengaktifkan tower terdekat dan Menarik Player dengan menekan Z atau klik kiri di mana saja
        if ((Input.GetKey(KeyCode.Z) || Input.GetMouseButtonDown(0)) && !isPulled)
        //if (Input.GetKey(KeyCode.Z)  && !isPulled)
        {
            if (closestTower != null)
            {
                hookedTower = closestTower;
            }

            if (hookedTower)
            {
                // Mengukur jarak antara tower dengan player
                float distance = Vector2.Distance(transform.position, hookedTower.transform.position);

                // Menentukan arah gravitasi dari player ke tower
                Vector3 pullDirection = (hookedTower.transform.position - transform.position).normalized;

                // Menghitung besar gaya tarik
                float newPullForce = Mathf.Clamp(pullForce / distance, 30, 100);
                rb2D.AddForce(pullDirection * newPullForce);

                // Menghitung besar gaya angular
                rb2D.angularVelocity = -rotateSpeed / distance;

                isPulled = true;
                //print(hookedTower);
            }


        }

        if (Input.GetKeyUp(KeyCode.Z) || Input.GetMouseButtonUp(0))
        //if (Input.GetKeyUp(KeyCode.Z))
        {
            isPulled = false;
            hookedTower = null; 
            closestTower = null;
            rb2D.angularVelocity = 0f;

        }

    }
}
