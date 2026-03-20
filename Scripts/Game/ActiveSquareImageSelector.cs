using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSquareImageSelector : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public bool updateImageOnRechedTreshhold = false;

    private void OnEnable()
    {
        UpdateSquareColorBasedOnCurrentScore();
        if (updateImageOnRechedTreshhold)
        {
            GameEvent.UpdateSquareColor += UpdateSquaresColor;
        }
    }
    private void OnDisable()
    {
        if (updateImageOnRechedTreshhold)
        {
            GameEvent.UpdateSquareColor -= UpdateSquaresColor;
        }
    }

    private void UpdateSquareColorBasedOnCurrentScore()
    {
        foreach(var squareTexture in squareTextureData.activeSquareTextures)
        {
            if(squareTextureData.currentColor == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }   
    }

    private void UpdateSquaresColor(Config.SquareColor color)
    {
        foreach(var squareTexture in squareTextureData.activeSquareTextures)
        {
            if(color == squareTexture.squareColor)
            {
                GetComponent<Image>().sprite = squareTexture.texture;
            }
        }
    }
}
