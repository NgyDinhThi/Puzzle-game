using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
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
    private Canvas canvas;
    private Vector3 _startPosition;
    private bool _shapeActive = true;
    private bool isPlaced = false;

    private void Awake()
    {
        _shapeStartScale = GetComponent<RectTransform>().localScale;
        _transform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        _startPosition = _transform.localPosition;
        _shapeActive = true;
    }

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvents.SetShapeInactive += SetShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvents.SetShapeInactive -= SetShapeInactive;
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
        _shapeActive = false;
    }

    public void DeactivateAfterPlacement()
    {
        foreach (var square in _currentShape)
        {
            square.SetActive(false);
        }
        _shapeActive = false;
        isPlaced = true;
    }

    public bool IsPlaced()
    {
        return isPlaced;
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
        isPlaced = false;
        CreateShape(shapeData);
    }

    public void CreateShape(Shapedata shapeData)
    {
        currentShapeData = shapeData;
        totalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count <= totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform));
        }

        foreach (var square in _currentShape)
        {
            square.transform.localPosition = Vector3.zero;
            square.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(
            squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y
        );

        int currentIndex = 0;
        for (int row = 0; row < shapeData.rows; row++)
        {
            for (int col = 0; col < shapeData.columns; col++)
            {
                if (shapeData.board[row].column[col])
                {
                    var square = _currentShape[currentIndex];
                    square.SetActive(true);
                    square.GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPosition(shapeData, col, moveDistance), GetYPosition(shapeData, row, moveDistance));
                    currentIndex++;
                }
            }
        }
    }

    private float GetXPosition(Shapedata shapeData, int column, Vector2 moveDistance)
    {
        float width = shapeData.columns;
        float offset = (width - 1) / 2f;
        return (column - offset) * moveDistance.x;
    }

    private float GetYPosition(Shapedata shapeData, int row, Vector2 moveDistance)
    {
        float height = shapeData.rows;
        float offset = (height - 1) / 2f;
        return -(row - offset) * moveDistance.y;
    }

    private int GetNumberOfSquares(Shapedata shapeData)
    {
        int count = 0;
        foreach (var row in shapeData.board)
        {
            foreach (var cell in row.column)
            {
                if (cell) count++;
            }
        }
        return count;
    }

    public void OnPointerClick(PointerEventData eventData) { }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = Vector2.zero;
        _transform.anchorMax = Vector2.zero;
        _transform.pivot = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, Camera.main, out Vector2 pos))
        {
            _transform.localPosition = pos + offset;
        }
    }

    public void OnPointerUp(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanbePlaced();
    }

    private void MoveShapeToStartPosition()
    {
        _transform.localPosition = _startPosition;
    }
    private void SetShapeInactive()
    {
        if (isPlaced) return; // ✅ nếu đã được đặt rồi thì đừng đụng vào

        if (!IsOnStartPosition() && IsAnyOfShapeSquaresActive())
        {
            bool shapePlaced = false;
            foreach (var square in _currentShape)
            {
                if (square.GetComponent<ShapeSquare>().occupiedImage.gameObject.activeSelf)
                {
                    shapePlaced = true;
                    break;
                }
            }

            if (shapePlaced)
            {
                foreach (var square in _currentShape)
                {
                    square.SetActive(false);
                }
            }
        }
    }
}
