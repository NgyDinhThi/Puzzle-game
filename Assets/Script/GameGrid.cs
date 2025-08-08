using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject _gridsquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squaresScale = 0.5f;
    public float everySquareOffset = 0.0f;
    public SquareTextureData squareTextureData;

    private Vector2 offset = new Vector2(0.0f, 0.0f);
    public System.Collections.Generic.List<GameObject> gridSquares = new System.Collections.Generic.List<GameObject>();

    private LineIndicator lineIndicator;
    private Config.squareColor currentActiveSquareColor_ = Config.squareColor.NotSet;
    private List<Config.squareColor> colorIntheGrid = new List<Config.squareColor>();

    private void Start()
    {
        lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();

        if (GamePersistence.Instance != null)
        {
            GamePersistence.Instance.grid = this;
            GamePersistence.Instance.shapeStorage = this.shapeStorage;
            GamePersistence.Instance.requestBtn = UnityEngine.Object.FindFirstObjectByType<RequestNewShapeButton>(FindObjectsInactive.Include);
            GamePersistence.Instance.scoreManager = UnityEngine.Object.FindFirstObjectByType<Score>(FindObjectsInactive.Include);
            StartCoroutine(LoadGridNextFrame());
        }

        currentActiveSquareColor_ = squareTextureData.activeSquareTexture[0].color;
    }

    private System.Collections.IEnumerator LoadGridNextFrame()
    {
        yield return null; // đợi 1 frame
        GamePersistence.Instance?.LoadNow();
    }

    private void OnUpdateSquareColor(Config.squareColor color)
    {
        currentActiveSquareColor_ = color;
    }    

    private List<Config.squareColor> GetAllSquareColor()
    {
        var color = new List<Config.squareColor>();

        foreach (var square in gridSquares)
        {
            var griDSquare = square.GetComponent<GridSquare>();
            if (griDSquare.SquareOccupied)
            {
                var Squarecolor = griDSquare.GetCurrentColor();
                if (color.Contains(Squarecolor) == false)
                {
                     color.Add(Squarecolor);
                }


            }
        }

        return color;
    }    

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }



    private void SpawnGridSquares()
    {
        int squareIndex = 0;
        for (var row = 0; row < rows; ++row)
        {
            for (var column = 0; column < columns; ++column)
            {
                gridSquares.Add(Instantiate(_gridsquare) as GameObject);

                gridSquares[gridSquares.Count -1].GetComponent<GridSquare>().SquareIndex = squareIndex;
                gridSquares[gridSquares.Count -1].transform.SetParent(this.transform);
                gridSquares[gridSquares.Count - 1].transform.localScale = new Vector3(squaresScale, squaresScale, squaresScale);

                gridSquares[gridSquares.Count -1].GetComponent<GridSquare>().SetImage(lineIndicator.GetGridSquareIndex(squareIndex) % 2 == 0);
                squareIndex++;
            }
        }


    }

    private void SetGridSquaresPosition()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 squaresGapNumber = new Vector2(0.0f, 0.0f);
        bool rowMoved = false;

        var square_rect = gridSquares[0].GetComponent<RectTransform>();

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in gridSquares)
        {
            if (columnNumber + 1 > columns)
            {
                squaresGapNumber.x = 0;
                columnNumber = 0;
                rowNumber++;
                rowMoved = false;
            }


            var posXoffset = offset.x * columnNumber + (squaresGapNumber.x * squaresGap);
            var posYoffset = offset.y * rowNumber + (squaresGapNumber.y * squaresGap);

            if (columnNumber > 0 && columnNumber % 3 == 0)
            {
                squaresGapNumber.x++;
                posXoffset += squaresGap;

            }
            if (rowNumber > 0 && rowNumber % 3 == 0 && rowMoved == false)
            {
                rowMoved = true;
                squaresGapNumber.y++;
                posYoffset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + posXoffset, startPosition.y - posYoffset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + posXoffset, startPosition.y - posYoffset, 0.0f);

            columnNumber++;

        }
    }

    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanbePlaced += CheckIfShapeCanbePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;
        GameEvents.CheckifPlayerLost += CheckLostCondition;
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanbePlaced -= CheckIfShapeCanbePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;
        GameEvents.CheckifPlayerLost -= CheckLostCondition;
    }

    private void CheckIfShapeCanbePlaced()
    {
        var squareIndex = new List<int>();
        foreach (var square in gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if (gridSquare.selected && !gridSquare.SquareOccupied)
            {
                squareIndex.Add(gridSquare.SquareIndex);
                gridSquare.selected = false;
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null) return;

        if (currentSelectedShape.totalSquareNumber == squareIndex.Count)
        {
            foreach (var index in squareIndex)
            {
                gridSquares[index].GetComponent<GridSquare>().PlaceShapeOnBoard(currentActiveSquareColor_);
            }

            currentSelectedShape.DeactivateAfterPlacement();

         
            int unplacedShapes = 0;
            foreach (var shape in shapeStorage.shapeList)
            {
                if (!shape.IsPlaced()) unplacedShapes++;
            }
         
            if (unplacedShapes == 0)
            {
                GameEvents.RequestNewShape(); 
            }
            else
            {
                foreach (var shape in shapeStorage.shapeList)
                {
                    if (!shape.IsPlaced() && !shape.IsOnStartPosition() && shape.IsAnyOfShapeSquaresActive())
                    {
                        shape.DeactivateShape(); 
                    }
                }
            }

            CheckAnylineIsComplete();
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }


    private void CheckAnylineIsComplete()
    {
        List<int[]> lines = new List<int[]>();
        //columns
        foreach (var column in lineIndicator.columnIndex)
        {
            lines.Add(lineIndicator.GetVerticalLine(column));
        }
        //row
        for (var row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for (var i = 0; i < 9; i++)
            {
                data.Add(lineIndicator.line_data[row, i]);
            }
            lines.Add(data.ToArray());
        }
        //square
        for (var square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for (var i = 0; i < 9; i++)
            {
                data.Add(lineIndicator.square_data[square, i]);   
            }
            lines.Add(data.ToArray());  
        }
        //cái này hoạt động song rồi mới xuống dưới
        colorIntheGrid = GetAllSquareColor();

        var completedLines = CheckifSquareAreCompleted(lines);
        if (completedLines >= 2)
        {
            //todo thêm hoạt ảnh
            GameEvents.ShowWritingWord();
        }
        //todo thêm điểm
        var totalScore = 10 * completedLines;
        var bonusScore = ShouldPlayColorBonus();
        GameEvents.AddScores(totalScore + bonusScore);
        GameEvents.CheckifPlayerLost();
        
    }    

    private int ShouldPlayColorBonus()
    {
        var colorInTheGridRemove = GetAllSquareColor();
        Config.squareColor colorToplayBonus = Config.squareColor.NotSet;
        foreach (var squareColors in colorIntheGrid)
        {
            if (colorInTheGridRemove.Contains(squareColors) == false)
            {
                colorToplayBonus = squareColors;
            }
        }
        if (colorToplayBonus == Config.squareColor.NotSet)
        {
            Debug.Log("Ko có màu nào ");
            return 0;
        }
        //không thêm điểm bonus cho màu hiện tại
        if (colorToplayBonus == currentActiveSquareColor_)
        {
            return 0;   
        }
        GameEvents.ShowBonus(colorToplayBonus);
        return 50;
    }    

    private int CheckifSquareAreCompleted(List<int[]> data)
    {
        List<int[]> completeLine = new List<int[]>();
        var linesCompleted = 0;

        foreach (var line in data)
        {
            var lineCompleted = true;
            foreach (var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                if (comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }

            if (lineCompleted)
            {
                completeLine.Add(line);
            }
        }
        foreach (var line in completeLine)
        {
            var completed = false;
            foreach (var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.Deactivate();
                completed = true;
            }

            foreach (var squareIndex in line)
            {
                var comp = gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();

            }
            if (completed)
            {
                linesCompleted++;
            }
        }
        return linesCompleted;
    }

    private void CheckLostCondition()
    {
        var validShapes = 0;

        foreach (var shape in shapeStorage.shapeList)
        {
            // 💥 THÊM: Bỏ qua shape đã đặt rồi
            if (shape.IsPlaced())
                continue;

            if (CheckCanbePlaceOnGrid(shape))
            {
                shape.ActivateShape();
                validShapes++;
            }
        }

        if (validShapes == 0)
        {
            Debug.Log("Thua Cuộc");
            GameEvents.GameOver(false);
        }
    }


    private bool CheckCanbePlaceOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.currentShapeData;
        var ShapeColumns = currentShapeData.columns;
        var ShapeRows = currentShapeData.rows;

        //Lưu trữ các số ô đc điền
        List<int> originalShapeFillSquare = new List<int>();
        var squareIndex = 0;
        for (var rowIndex = 0; rowIndex < ShapeRows; rowIndex++)
        {
            for (var columnIndex = 0; columnIndex < ShapeColumns; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFillSquare.Add(squareIndex);
                }
                squareIndex++;  
            }
        }

        if (currentShape.totalSquareNumber != originalShapeFillSquare.Count)
            Debug.LogError("Số ô được dùng không như số ô ban đầu");
        
        var sqaureList = GetAllCombination(ShapeColumns, ShapeRows);
        bool canbePlaced = false;
        foreach (var number in sqaureList)
        {
            bool ShapeCanbePlacedOnTheBoard = true;
            foreach (var sqaureIndexToCheck in originalShapeFillSquare)
            {
                var comp = gridSquares[number[sqaureIndexToCheck]].GetComponent<GridSquare>();
                if (comp.SquareOccupied)
                {
                    ShapeCanbePlacedOnTheBoard = false;
                }
            }

            if (ShapeCanbePlacedOnTheBoard)
            {
                canbePlaced = true;
            }
        }
        return canbePlaced;
    }

    private List<int[]> GetAllCombination(int columns, int rows)
    {

        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;
        int safeIndex = 0;

        while (lastRowIndex +(rows -1 )< 9 )
        {
            var rowData = new List<int>();
            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(lineIndicator.line_data[row, column]);   
                }
            }

            squareList.Add(rowData.ToArray());
            lastColumnIndex++;

            if (lastColumnIndex + (columns - 1) >= 9)
            {
                lastRowIndex++;
                lastColumnIndex = 0;    
            }
            safeIndex++;
            if (safeIndex > 100)
            {
                break;
            }
        }
        return squareList;
    }    
}
 