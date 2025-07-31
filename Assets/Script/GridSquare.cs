using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GridSquare : MonoBehaviour
{

    public Image normalImage;
    public List<Sprite> normalImages;

    private void Start()
    {
        
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalImages[1] : normalImages[0];


    }    

}
