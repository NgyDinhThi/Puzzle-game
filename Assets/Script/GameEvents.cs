using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action CheckIfShapeCanbePlaced;

    public static Action MoveShapeToStartPosition;
       
    public static Action RequestNewShape;

    public static Action SetShapeInactive;
}
