using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class TowerControl : MonoBehaviour
{
    public PlayerControlNew playerControl;
    public Color baseColor;
    public Color chosenTowerColor = new Color(0, 172, 255, 128);
    public float rotadir = 1;

    void OnMouseDown()
    {

        playerControl.hookedTower = this.gameObject;
        this.gameObject.GetComponent<SpriteRenderer>().color = chosenTowerColor;

    }

    void OnMouseUp()
    {
        playerControl.hookedTower = null;
        this.gameObject.GetComponent<SpriteRenderer>().color = baseColor;

    }


    // Start is called before the first frame update
    void Start()
    {
        baseColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }
}
