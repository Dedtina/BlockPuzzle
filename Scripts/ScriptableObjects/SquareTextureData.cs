using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
[System.Serializable]
public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }
    public int treshholdVal = 10;

    private const int StartTresholdVal =  10;
    public List<TextureData> activeSquareTextures;

    public Config.SquareColor currentColor;
    private Config.SquareColor nextColor;

    public int GetCurrentColorIndex()
    {
        var currentIndex = 0;
        for(int index = 0; index < activeSquareTextures.Count; index++)
        {
            if(activeSquareTextures[index].squareColor == currentColor)
            {
                currentIndex = index;
            }
        }
        return currentIndex;
    }

    public void UpdateColors(int current_Score)
    {
        currentColor = nextColor;
        var currentColorIndex = GetCurrentColorIndex();
        if(currentColorIndex == activeSquareTextures.Count - 1)
        {
            nextColor = activeSquareTextures[0].squareColor;
        }
        else
        {
            nextColor = activeSquareTextures[currentColorIndex + 1].squareColor;
        }

        treshholdVal = StartTresholdVal + current_Score;
    }    

    public void SetStartColor()
    {
        treshholdVal = StartTresholdVal;
        currentColor = activeSquareTextures[0].squareColor;
        nextColor = activeSquareTextures[1].squareColor;
    }

    private void Awake()
    {
        SetStartColor();
    }

    private void OnEnable()
    {
        SetStartColor();
    }

}
