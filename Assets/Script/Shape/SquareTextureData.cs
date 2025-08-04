using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu]
[System.Serializable]


public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.squareColor color;

    }

    public int tresholdVal = 10;
    private const int startTresholdVal = 100;
    public List<TextureData> activeSquareTexture;

    public Config.squareColor currentColors;
    public Config.squareColor _nextColor;

    private int GetCurrentColorIndex()
    {
        var currentIndex = 0;
        for (int index = 0; index < activeSquareTexture.Count; index++)
        {
            if (activeSquareTexture[index].color == currentColors)
            {
                currentIndex = index;
            }
        }
        return currentIndex;

    }    

    public void UpdateColors(int current_score)
    {
        currentColors = _nextColor;
        var currentColorsIndex = GetCurrentColorIndex();
        if (currentColorsIndex == activeSquareTexture.Count - 1)
            _nextColor = activeSquareTexture[0].color;
        else
        {
            _nextColor = activeSquareTexture[currentColorsIndex + 1].color;
        }

        tresholdVal = startTresholdVal + current_score;
    }    

    public void SetStartColors()
    {
        tresholdVal = startTresholdVal;
        currentColors = activeSquareTexture[0].color;
        _nextColor = activeSquareTexture[1].color;


    }

    private void Awake()
    {
        SetStartColors();

    }

    private void OnEnable()
    {
        SetStartColors();
    }
}
