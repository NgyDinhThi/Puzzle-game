using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject _gridsquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squaresScale = 0.5f;
    public float everySquareOffset = 0.0f;

    private Vector2 offset = new Vector2(0.0f, 0.0f);
    public System.Collections.Generic.List<GameObject> gridSquares = new System.Collections.Generic.List<GameObject>();

    private LineIndicator lineIndicator;


    private void Start()
    {
        lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
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
    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanbePlaced -= CheckIfShapeCanbePlaced;
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
                //gridSquare.ActivateSquare();
            }
        }
        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if (currentSelectedShape == null)
         return;// không có hình được chọn

        if (currentSelectedShape.totalSquareNumber == squareIndex.Count)
        {
            foreach (var squareIndexs in squareIndex)
            {
                gridSquares[squareIndexs].GetComponent<GridSquare>().PlaceShapeOnBoard();
            }

            currentSelectedShape.DeactivateAfterPlacement(); 

            var shapeLeft = 0;
            foreach (var shape in shapeStorage.shapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquaresActive())
                {
                    shapeLeft++;
                }
            }

            if (shapeLeft == 0)
            {
                GameEvents.RequestNewShape();
            }
            else
            {
                GameEvents.SetShapeInactive();
            }
        }
        else
        {
            GameEvents.MoveShapeToStartPosition();
        }
    }    

}
 