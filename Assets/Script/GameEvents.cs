using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool> GameOver;

    public static Action<int> AddScores;

    public static Action CheckIfShapeCanbePlaced;

    public static Action MoveShapeToStartPosition;
       
    public static Action RequestNewShape;

    public static Action SetShapeInactive;

    public static Action<int, int> UpdateBestScoreBar;

    public static Action<Config.squareColor> UpdateSquareColor;

    public static Action ShowWritingWord;

}
