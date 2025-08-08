using System;
using System.Collections.Generic;

[Serializable]
public class CellData
{
    public bool o; // occupied
    public int c; // color (cast từ Config.squareColor)
}

[Serializable]
public class GameStateData
{
    public int rows;
    public int columns;

    public List<CellData> cells = new List<CellData>();

    public int requestLeft;            // số lần Request còn lại
    public List<int> trayShapeIndices; // index của ShapeData trên từng slot; -1 = slot trống
    public int currentScore;           // điểm hiện tại
}
