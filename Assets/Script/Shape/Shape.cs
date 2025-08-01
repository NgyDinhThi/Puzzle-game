using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler ,IEndDragHandler, IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0f, 900f);


    [HideInInspector]
    public Shapedata currentShapeData;


    public int totalSquareNumber { get; set; }
    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private bool _isDragging = true;
    private Canvas canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;

    private void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        _isDragging = true;
        _startPosition = _transform.localPosition;
        _shapeActive = true;
    }
    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
    }
    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        
    }

    private void Start()
    {
       
    }

    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPosition;
    }    

    public bool IsAnyOfShapeSquaresActive()
    {
        foreach (var square in _currentShape)
        {
            if (square.gameObject.activeSelf)
                return true;
        }

        return false;

    }  
    
    public void DeactivateShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeaactivateShape();
            }
        }
        _shapeActive=false;

    }    
    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape(); 
            }
        }
        _shapeActive = true;
    }    

    public void RequestNewshape(Shapedata shapeData)
    {
        _transform.localPosition = _startPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(Shapedata shapeData)
    {
        currentShapeData = shapeData;
        totalSquareNumber = GetNumberOfSquares(shapeData);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
       _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanbePlaced();
    }
    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPosition;
    }    
}
