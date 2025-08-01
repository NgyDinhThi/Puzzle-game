using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public GameObject squareShapeImage;

    [HideInInspector]
    public Shapedata currentShapeData;

    private List<GameObject> _currentShape = new List<GameObject>();


    private void Start()
    {
       
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

    private float GetXposition4ShapeSquares(Shapedata shapeData, int column, Vector2 moveDistance)
    {
        float width = shapeData.columns;
        float offset = (width - 1) / 2f;
        return (column - offset) * moveDistance.x;
    }

    private float GetYposition4ShapeSquares(Shapedata shapeData, int row, Vector2 moveDistance)
    {
        float height = shapeData.rows;
        float offset = (height - 1) / 2f;
        return -(row - offset) * moveDistance.y;
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
