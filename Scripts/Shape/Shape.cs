using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler,
IPointerDownHandler
{
    public GameObject squareShapeImage;
    public Vector3 shapeSelectedScale;
    public Vector2 offset = new Vector2(0, 700f);

    [HideInInspector]
    public ShapeData currentShapeData;

    public int totalSquareNumber{get; set;}

    private List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private bool _shapeDraggable = true;
    private Canvas _canvas;
    private Vector3 _startPos;
    private bool _shapeActive = true;
    // Start is called before the first frame update

    public void Awake()
    {
        _transform = this.GetComponent<RectTransform>();
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _canvas = GetComponentInParent<Canvas>();
        _shapeDraggable = true;
        _startPos = transform.localPosition;
        _shapeActive = true;
    }
    private void OnEnable()
    {
        GameEvent.MoveShapeToStartPosition += MoveShapeToStartPosition;
        GameEvent.SetShapeInactive += SetShapeInactive;  
    }
    private void OnDisable()
    {
        GameEvent.MoveShapeToStartPosition -= MoveShapeToStartPosition;
        GameEvent.SetShapeInactive -= SetShapeInactive;
    }
    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _startPos;
    }

    public bool IsAnyOfShapeSquareActive()
    {
        foreach(var square in _currentShape)
        {
            if(square.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void DeactiveShape()
    {
        if(_shapeActive)
        {
            foreach(var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeactiveateShape();
            }
            
        }
        _shapeActive = false;
    }

    private void SetShapeInactive()
    {
        if(IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
           foreach(var square in _currentShape)
            {
                square.gameObject.SetActive(false);
            }
        }
    }
    public void ActiveShape()
    {
        if(!_shapeActive)
        {
            foreach(var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActiveateShape();
            }
        }
        _shapeActive = true;
    }

    public void RequestNewShape(ShapeData shapeData)
    {
        _transform.localPosition = _startPos;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        totalSquareNumber = GetNumberOfSquares(shapeData);
        
        while(_currentShape.Count <= totalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage,transform) as GameObject);
        }

        foreach(var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;
        //set pos to form final shape
        for(var row = 0; row < shapeData.rows; row++)
        {
            for(var column = 0; column < shapeData.columns; column++)
            {
                if(shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = 
                        new Vector2(GetXPosForShapeSquare(shapeData, column, moveDistance), 
                            GetYPosForShapeSquare(shapeData, row, moveDistance));
                    
                    currentIndexInList++;
                }
            }
        }
    }
    private float GetYPosForShapeSquare(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if(shapeData.rows > 1 )
        {
            if(shapeData.rows % 2 != 0)
            {
                var middleSquareIndex = (shapeData.rows - 1) / 2;
                var multiplier = (shapeData.rows - 1) / 2;
                if(row < middleSquareIndex) //move it on minus
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if(row > middleSquareIndex) //move it on plus
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                var multiplier = shapeData.rows / 2;

                if(row == middleSquareIndex1 || row == middleSquareIndex2) 
                {
                    if(row == middleSquareIndex2)
                    {
                        shiftOnY = (moveDistance.y / 2) * -1;
                    }
                    if(row == middleSquareIndex1)
                    {
                        shiftOnY = moveDistance.y / 2;
                    }
                }

                if(row < middleSquareIndex1 && row < middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y * 1;
                    shiftOnY *= multiplier;
                }
                else if(row > middleSquareIndex1 && row > middleSquareIndex2)
                {
                    shiftOnY = moveDistance.y * -1;
                    shiftOnY *= multiplier;
                }
            }
        }
        return shiftOnY;
    }
    private float GetXPosForShapeSquare(ShapeData shapeData, int colunm, Vector2 moveDistance)
    {
        float shiftOnX = 0f;
        if(shapeData.columns > 1 )
        {
            if(shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if(colunm < middleSquareIndex) //move it on the negative
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(colunm > middleSquareIndex) //move it on plus
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                var multiplier = shapeData.columns / 2;

                if(colunm == middleSquareIndex1 || colunm == middleSquareIndex2) 
                {
                    if(colunm ==  middleSquareIndex2)
                    {
                        shiftOnX = moveDistance.x / 2;
                    }
                    if(colunm == middleSquareIndex1)
                    {
                        shiftOnX = (moveDistance.x / 2) * -1;
                    }
                }

                if(colunm < middleSquareIndex1 && colunm < middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if(colunm > middleSquareIndex1 && colunm > middleSquareIndex2)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }
        return shiftOnX;
    }

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int number = 0;
        
        foreach(var rowData in shapeData.board)
        {
            foreach(var active in rowData.column)
            {
                if(active)
                {
                    number++;
                }
            }
        }
        return number;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Handle pointer click event
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Handle pointer up event
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Handle begin drag event
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Handle end drag event
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvent.CheckIfShapeCanBePlaced();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, 
        eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Handle pointer down event
    }

    private void MoveShapeToStartPosition()
    {
        _transform.transform.localPosition = _startPos;
    }
}
