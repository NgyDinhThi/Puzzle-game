using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
   public SquareTextureData squareTextureData;
   public bool updateImageReachTreshol = false;

    private void OnEnable()
    {
        UpdateSquareColorBaseOnPoint();
        if (updateImageReachTreshol)
            GameEvents.UpdateSquareColor += UpdateSquareColor;
    }
    private void OnDisable()
    {
        if (updateImageReachTreshol)
            GameEvents.UpdateSquareColor -= UpdateSquareColor;
    }
    private void UpdateSquareColorBaseOnPoint()
    {
        foreach (var squareTexture in squareTextureData.activeSquareTexture)
        {
            if (squareTextureData.currentColors == squareTexture.color)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }

    private void UpdateSquareColor(Config.squareColor color)
    {
        foreach (var sqaureTexture in squareTextureData.activeSquareTexture)
        {
            if (color == sqaureTexture.color)
            {
                GetComponent<Image>().sprite = sqaureTexture.texture;
            }
        }
    }
}
