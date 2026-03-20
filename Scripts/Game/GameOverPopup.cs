using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject losePopup;
    public GameObject newBestScorePopup;

    private const string ScoreBoxName = "ScoreBox";
    private const string PopupScoreTextName = "PopupScoreText";

    [SerializeField] private TMP_Text popupScoreText;

    private Score score;

    void Start()
    {
        score = FindObjectOfType<Score>();
        popupScoreText = ResolvePopupScoreText();
        gameOverPopup.SetActive(false);
    }

    void OnEnable()
    {
        GameEvent.GameOver += OnGameOver;
    }

    void OnDisable()
    {
        GameEvent.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        gameOverPopup.SetActive(true);
        losePopup.gameObject.SetActive(false);
        newBestScorePopup.SetActive(true);
        UpdatePopupScore();
    }

    private void UpdatePopupScore()
    {
        if (popupScoreText == null)
        {
            popupScoreText = ResolvePopupScoreText();
        }

        if (popupScoreText == null)
        {
            return;
        }

        if (score == null)
        {
            score = FindObjectOfType<Score>();
        }

        popupScoreText.text = score != null ? score.CurrentScore.ToString() : "0";
    }

    private TMP_Text ResolvePopupScoreText()
    {
        if (popupScoreText != null)
        {
            return popupScoreText;
        }

        var scoreBox = gameOverPopup != null ? gameOverPopup.transform.Find(ScoreBoxName) : null;
        if (scoreBox == null)
        {
            return null;
        }

        var existingText = scoreBox.GetComponentInChildren<TMP_Text>(true);
        if (existingText != null)
        {
            return existingText;
        }

        var scoreTextObject = new GameObject(PopupScoreTextName, typeof(RectTransform));
        scoreTextObject.transform.SetParent(scoreBox, false);

        var scoreTextRect = scoreTextObject.GetComponent<RectTransform>();
        scoreTextRect.anchorMin = Vector2.zero;
        scoreTextRect.anchorMax = Vector2.one;
        scoreTextRect.offsetMin = Vector2.zero;
        scoreTextRect.offsetMax = Vector2.zero;

        var createdText = scoreTextObject.AddComponent<TextMeshProUGUI>();
        createdText.alignment = TextAlignmentOptions.Center;
        createdText.fontSize = 90;
        createdText.color = Color.black;
        createdText.text = "0";

        if (score == null)
        {
            score = FindObjectOfType<Score>();
        }

        if (score != null && score.scoreText != null)
        {
            createdText.font = score.scoreText.font;
            createdText.fontSharedMaterial = score.scoreText.fontSharedMaterial;
            createdText.color = score.scoreText.color;
        }

        return createdText;
    }
}
