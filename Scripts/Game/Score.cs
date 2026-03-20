using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class BestScoreData
{
    public int score = 0;
    
}
public class Score : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public TMP_Text scoreText;
    public int CurrentScore => currenScore;

    private bool newBestScore = false;
    private BestScoreData bestScores = new BestScoreData();
    private int currenScore;
    private string bestScoreKey_ = "bsdata";
    void Awake()
    {
        if(BinaryDataStream.Exists(bestScoreKey_))
        {
           StartCoroutine(ReadFileData());
        }
    }

    private IEnumerator ReadFileData()
    {
        bestScores = BinaryDataStream.Read<BestScoreData>(bestScoreKey_);
        yield return new WaitForEndOfFrame();
        GameEvent.UpdateBestScoreBar(currenScore, bestScores.score);
    }
    void Start()
    {
        currenScore = 0;
        newBestScore = false;
        squareTextureData.SetStartColor();
        UpdateScoreText();
    }

    void OnEnable()
    {
        GameEvent.AddScores += AddScore;
        GameEvent.GameOver += SaveBestScore;
    }
    void OnDisable()
    {
        GameEvent.AddScores -= AddScore;
        GameEvent.GameOver -= SaveBestScore;
    }

    public void SaveBestScore(bool newBestScore)
    {
        BinaryDataStream.Save<BestScoreData>(bestScores, bestScoreKey_);
    }
    private void AddScore(int scores)
    {
        currenScore += scores;
        if(currenScore > bestScores.score)
        {
            newBestScore = true;
            bestScores.score = currenScore;    
            SaveBestScore(true);
        }
        UpdateScoreText();    
        GameEvent.UpdateBestScoreBar(currenScore, bestScores.score);
        UpdateScoreText();
    }

    private void UpdateSquareColor()
    {
        if(GameEvent.UpdateSquareColor != null && currenScore >= squareTextureData.treshholdVal)
        {
            squareTextureData.UpdateColors(currenScore);
            GameEvent.UpdateSquareColor(squareTextureData.currentColor);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = currenScore.ToString();
    }

}
