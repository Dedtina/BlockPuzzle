using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public ShapeStorage shapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPos = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffSet = 0.0f;
    public SquareTextureData squareTextureData;
    private Vector2 offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private LineIndicator _lineIndicator;
    private Config.SquareColor currentActiveSquareColor = Config.SquareColor.NotSet;
    private List<Config.SquareColor> colorsInGrid = new List<Config.SquareColor>();

    void OnEnable()
    {
        GameEvent.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor += OnUpdateSquareColor;
        GameEvent.CheckIfPlayerLost += CheckIfPlayerLost;
    }
    void OnDisable()
    {
        GameEvent.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvent.UpdateSquareColor -= OnUpdateSquareColor;
        GameEvent.CheckIfPlayerLost -= CheckIfPlayerLost;
    }
    
    void Start()
    {
        _lineIndicator = GetComponent<LineIndicator>();
        CreateGrid();
        currentActiveSquareColor = squareTextureData.activeSquareTextures[0].squareColor;

    }

    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        currentActiveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllSquareColorsInGrid()
    {
        var colors = new List<Config.SquareColor>();
        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquares>();
            if(gridSquare.SquareOccupied)
            {
                var color = gridSquare.CurrentSquareColor();
                if(colors.Contains(color) == false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }
    
    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        int squaresIndex = 0;
        for(var row = 0; row < rows; ++row)
        {
            for(var column = 0; column < columns; ++column)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquares>().SquareIndex = squaresIndex;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquares>().SetImage(_lineIndicator.GetGridSquareIndex(squaresIndex) % 2 == 0);
                squaresIndex++;
            }
        }
    }

    private void SetGridSquaresPosition()
    {
        int column_number = 0;
        int row_number = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;
        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffSet;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffSet;

        foreach(GameObject square in _gridSquares)
        {
            if(column_number + 1 > columns)
            {
                square_gap_number.x = 0.0f;
                //go to next col
                column_number = 0;
                row_number++;
                row_moved = false;
            }

            var pos_x_offset = offset.x * column_number + (square_gap_number.x * squaresGap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * squaresGap);

            if(column_number > 0 && column_number % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;
            }

            if(row_number > 0 && row_number % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;
            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + pos_x_offset, startPos.y - pos_y_offset);
            square.GetComponent<RectTransform>().localPosition = new Vector3(startPos.x + pos_x_offset, startPos.y - pos_y_offset, 0.0f);
            column_number++;
        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexs = new List<int>(); 
        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquares>();

            if(gridSquare.Selected && !gridSquare.SquareOccupied)
            {
                squareIndexs.Add(gridSquare.SquareIndex);
                gridSquare.Selected = false;
                //gridSquare.ActivateSquare();    
            }
        }

        var currentSelectedShape = shapeStorage.GetCurrentSelectedShape();
        if(currentSelectedShape == null) //there is no selected shape
        {
            return;
        }

        if(currentSelectedShape.totalSquareNumber == squareIndexs.Count)
        {
            foreach(var squareIndex in squareIndexs)
            {
                _gridSquares[squareIndex].GetComponent<GridSquares>().PlaceShapeOnBoard(currentActiveSquareColor);
            }

            var shapeLeft = 0;
            foreach(var shape in shapeStorage.shapeList)
            {
               if(shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
               {
                   shapeLeft++;
               }
            }

            if(shapeLeft == 0)
            {
                GameEvent.RequestNewShape();
            }
            else
            {
                GameEvent.SetShapeInactive();
            }
            CheckIfAnyCompleteLine();
        }
        else
        {
            GameEvent.MoveShapeToStartPosition();
        }
    }

    void CheckIfAnyCompleteLine()
    {
        List<int[]> lines = new List<int[]>();
        //col
        foreach(var column in _lineIndicator.columnIndexes)
        {
            lines.Add(_lineIndicator.GetVerticalLine(column));
        }

        //row
        for(var row = 0; row < 9; row++)
        {
            List<int> data = new List<int>(9);
            for(var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.lineData[row, index]);
            }

            lines.Add(data.ToArray());
        }

     /* //square
        for(var square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for(var index = 0; index < 9; index++)
            {
                data.Add(_lineIndicator.squareData[square, index]);
            }

            lines.Add(data.ToArray());
        } */

        //This needs to be called before CheckIfSquaresAreCompleted
        colorsInGrid = GetAllSquareColorsInGrid();

        var completedLines = CheckIfSquaresAreCompleted(lines);

        if(completedLines >= 2)
        {
            GameEvent.ShowCongratulationWritting();
        }

        //Add score 
        var totalscore = 10 * completedLines;
        var bonusScore = ShouldPlayColorBonusAnimation();
        GameEvent.AddScores(totalscore + bonusScore);
        GameEvent.CheckIfPlayerLost();
    }

    private int ShouldPlayColorBonusAnimation()
    {
        var colorsInGridAfterLineRemoved = GetAllSquareColorsInGrid();
        Config.SquareColor colorToPlayBonus = Config.SquareColor.NotSet;
        foreach(var squareColor in colorsInGrid)
        {
            if(colorsInGridAfterLineRemoved.Contains(squareColor) == false)
            {
                colorToPlayBonus = squareColor;
            }
        }

        if(colorToPlayBonus == Config.SquareColor.NotSet)
        {
            Debug.Log("Cant not find color bonus to play");
            return 0;
        }

        //Should never play bonus for current color
        if(colorToPlayBonus == currentActiveSquareColor)
        {
            return 0;
        }

        GameEvent.ShowBonusScreen(colorToPlayBonus);

        return 50; //bonus score
    }
    private int CheckIfSquaresAreCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();
        
        var linesCompleted = 0;
        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                if(comp.SquareOccupied == false)
                {
                    lineCompleted = false;
                }
            }
            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }

        foreach(var line in completedLines)
        {
            var completed = false;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                comp.DeactivateSquare();
                completed = true;
            }

            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquares>();
                comp.ClearOccupied();
            }

            if(completed)
            {
                linesCompleted++;
            }
        }
        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;
        for(var index = 0; index < shapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = shapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if(CheckIfShapeCanBePlacedOnGrid(shapeStorage.shapeList[index]) && isShapeActive)
            {
                shapeStorage.shapeList[index]?.ActiveShape();
                validShapes++;
            }
        }
        
        if (validShapes == 0)
        {
            GameEvent.GameOver(false);
            Debug.Log("Game Over"); 
        }
    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape currentShape)
    {
        var currentShapeData = currentShape.currentShapeData;
        var shapeColmns = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        //All indexes of filled up squares
        List<int> originalShapeFilledUpSquares = new List<int>();
        var squareIndex = 0;

        for(var rowIndex = 0; rowIndex < shapeRows; rowIndex++)
        {
            for(var columnIndex = 0; columnIndex < shapeColmns; columnIndex++)
            {
                if(currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }
        }
        if(currentShape.totalSquareNumber != originalShapeFilledUpSquares.Count)
        {
            Debug.LogError("Number of filled up squares are not the same as the original shape");
        }
        
        var squareList = GetAllSquaresCombination(shapeColmns, shapeRows);

        bool canBePlaced = false;
        foreach(var number in squareList)
        {
            bool shapeCanBePlacedOnBoard = true;
            foreach(var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[number[squareIndexToCheck]].GetComponent<GridSquares>();
                if(comp.SquareOccupied == true)
                {
                    shapeCanBePlacedOnBoard = false;
                }
             
            }

            if(shapeCanBePlacedOnBoard)
            {
                canBePlaced = true;
            }
        }

        return canBePlaced;
    }
    
    private List<int[]> GetAllSquaresCombination(int columns, int rows)
    {
        var squareList = new List<int[]>();
        var lastColumnIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while(lastRowIndex + (rows-1) < 9)
        {
            var rowData = new List<int>();
            for(var row = lastRowIndex; row < lastRowIndex+rows; row++)
            {
                for(var column = lastColumnIndex; column < lastColumnIndex + columns; column++)
                {
                    rowData.Add(_lineIndicator.lineData[row,column]);
                }
            }

            squareList.Add(rowData.ToArray());
            lastColumnIndex++;

            if(lastColumnIndex + (columns-1) >= 9 )
            {
                lastRowIndex++;
                lastColumnIndex = 0;
            }
            safeIndex++;
            if(safeIndex > 100)
            {
                break;
            }
        }
        return squareList;  
    }
}
