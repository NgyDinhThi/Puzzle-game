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

    private Config.squareColor currentSquareColor_ = Config.squareColor.NotSet;

    public Config.squareColor GetCurrentColor()
    {
        return currentSquareColor_;
    }    

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
    public void PlaceShapeOnBoard(Config.squareColor color)
    {
        currentSquareColor_ = color;
        ActivateSquare();
    }    

    public void ActivateSquare()
    {
        hooverImage.gameObject.SetActive(false);
        activeImage.gameObject.SetActive(true);
        selected = true;
        SquareOccupied = true;
    }    
    public void Deactivate()
    {
        currentSquareColor_ = Config.squareColor.NotSet;
        activeImage.gameObject.SetActive(false);
    }    
    public void ClearOccupied()
    {
        currentSquareColor_ = Config.squareColor.NotSet;
        selected = false;
        SquareOccupied = false ;
    }    

    private void OnTriggerStay2D(Collider2D collision)
    {
        selected = true;
        if (SquareOccupied == false)
        {
         
            hooverImage.gameObject.SetActive(true);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            selected = false;
            hooverImage.gameObject.SetActive(false);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (SquareOccupied == false)
        {
            selected = true;
            hooverImage.gameObject.SetActive(true);
            
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
       
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];


    }    

}
