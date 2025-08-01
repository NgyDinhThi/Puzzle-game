using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridSquare : MonoBehaviour
{

    public Image normalImage;
    public List<Sprite> normalImages;
    public Image hooverImage;
    public Image activeImage;

    public bool selected {  get; set; }
    public int SquareIndex {  get; set; }
    public bool SquareOccupied { get; set; }

    private void Start()
    {
        selected = false;
        SquareOccupied = false;

    }

    public bool CanUseSquare()
    {
        return hooverImage.gameObject.activeSelf;
    }    

    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        selected = true;
        SquareOccupied = true;
    }    

    private void OnTriggerStay2D(Collider2D collision)
    {
        hooverImage.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hooverImage.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hooverImage.gameObject.SetActive(true);
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];


    }    

}
