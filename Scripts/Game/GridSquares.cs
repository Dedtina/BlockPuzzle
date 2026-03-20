using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GridSquares : MonoBehaviour
{
    public Image hoverImage;
    public Image actviteImage;
    public Image normalImage;
    public List<Sprite> normalSprites;

    private Config.SquareColor currentSquareColor = Config.SquareColor.NotSet;

    public Config.SquareColor CurrentSquareColor()
    {
        return currentSquareColor;
    }

    public bool Selected { get; set; }
    public int SquareIndex { get; set; }
    public bool SquareOccupied { get; set; }

    void Start()
    {
        Selected = false;
        SquareOccupied = false;
    }

    public bool CanBeUseThisSquare()
    {
        return hoverImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard(Config.SquareColor squareColor)
    {
        currentSquareColor = squareColor;
        ActivateSquare();
    }
    public void ActivateSquare()
    {
       hoverImage.gameObject.SetActive(false);
       actviteImage.gameObject.SetActive(true);
       Selected = true;
       SquareOccupied = true;
    }
    public void DeactivateSquare()
    {
        currentSquareColor = Config.SquareColor.NotSet;
        actviteImage.gameObject.SetActive(false);
    }
    public void ClearOccupied()
    {
        currentSquareColor = Config.SquareColor.NotSet;
        Selected = false;
        SquareOccupied = false;
    }
    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage ? normalSprites[1] : normalSprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(SquareOccupied == false)
        {
            Selected = true;
            hoverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Selected = true;
        if(SquareOccupied == false)
        {
            hoverImage.gameObject.SetActive(true);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(SquareOccupied == false)
        {
            Selected = false;
            hoverImage.gameObject.SetActive(false);
        }
        else if(collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().UnSetOccupied();
        }
    }
    
}
