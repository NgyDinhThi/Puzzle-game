using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public GameObject squareShapeImage;

    //[HideInInspector]
    public Shapedata currentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();


    private void Start()
    {
        RequestNewshape(currentShapeData);
    }

    public void RequestNewshape(Shapedata shapeData)
    {
        CreateShape(shapeData);
    }

    public void CreateShape(Shapedata shapeData)
    {
        currentShapeData = shapeData;
        var totalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count <= totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);

        }
        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInlist = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInlist].SetActive(true);
                    _currentShape[currentIndexInlist].GetComponent<RectTransform>().localPosition = new Vector2(GetXposition4ShapeSquares(shapeData, column, moveDistance),GetYposition4ShapeSquares(shapeData, row, moveDistance)) ;
                    currentIndexInlist++;
                }
            }
        }
    }

    private float GetYposition4ShapeSquares(Shapedata shapeData, int row, Vector2 moveDistance)
    {
        float shiftOny = 0f;
        if (shapeData.rows > 1)
        {
            if (shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;

                if (row < middleSquareIndex)
                {
                    shiftOny = moveDistance.y * 1;
                    shiftOny *= multiplier;
                }
                else if (row > middleSquareIndex)
                {
                    shiftOny = moveDistance.y * -1;
                    shiftOny *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                var multiplier = shapeData.rows / 2;

                if (row == middleSquareIndex1 || row == middleSquareIndex2)
                {
                    if (row == middleSquareIndex2)
                        shiftOny = (moveDistance.y / 2) * -1;
                    if (row == middleSquareIndex1)
                        shiftOny = (moveDistance.y / 2);
                }
                if (row < middleSquareIndex1 && row < middleSquareIndex2)
                {
                    shiftOny = moveDistance.y * 1;
                    shiftOny *= multiplier;
                }
                else if(row > middleSquareIndex1 && row >middleSquareIndex2 )
                {
                    shiftOny = moveDistance.y * -1;
                    shiftOny *= multiplier;
                }

            }
        }
        return shiftOny;

    }

    private float GetXposition4ShapeSquares(Shapedata shapeData, int column, Vector2 moveDistance)
    {
        float shiftOnx = 0f;
        if (shapeData.columns > 1)
        {
            if (shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if (column < middleSquareIndex)
                {
                    shiftOnx = moveDistance.x * -1;
                    shiftOnx *= multiplier;
                }
                else if (column > middleSquareIndex)
                {
                    shiftOnx = moveDistance.x * 1;
                    shiftOnx *= multiplier;
                }
                   
                
            }
            else
            {
                var middleSquaresIndex2 = (shapeData.columns == 2) ?1 : (shapeData.columns / 2);
                var middleSquaresIndex1 = (shapeData.columns == 2) ?0 : (shapeData.columns - 1);
                var multiplier = shapeData.columns / 2;

                if (column == middleSquaresIndex1 || column == middleSquaresIndex2)
                {
                    if (column == middleSquaresIndex2)
                    {
                        shiftOnx = moveDistance.x / 2;
                    }
                    if (column == middleSquaresIndex1)
                    {
                        shiftOnx = (moveDistance.x / 2) * -1;
                    }
                }

                if (column < middleSquaresIndex1 && column < middleSquaresIndex2)
                {
                    shiftOnx = moveDistance.x * -1;
                    shiftOnx *= multiplier;
                }
                else if(column > middleSquaresIndex1 && column > middleSquaresIndex2)
                {
                    shiftOnx = moveDistance.x * 1;
                    shiftOnx *= multiplier;
                }

            }
        }
        return shiftOnx;

    }

    private int GetNumberOfSquares(Shapedata shapedata)
    {
        int number = 0;
        foreach (var rowData in shapedata.board)
        {
            foreach (var active in rowData.column)
            {
                if (active) number++;
        
            }
        }
        return number;
    }
}
