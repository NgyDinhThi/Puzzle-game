using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShapeSquare : MonoBehaviour
{
    public Image occupiedImage;

    void Start()
    {
        occupiedImage.gameObject.SetActive(false);
    }

    public void DeaactivateShape()
    {
       gameObject.GetComponent<BoxCollider2D>().enabled = false;
       gameObject.SetActive(false);
    }    
    public void ActivateShape()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);

    }    
}
