using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Variables
    public Text scoreText;
    public Text highScoreText;

    public float scoreCount;

    [SerializeField]
    private float highScoreCount;

    [SerializeField]
    private float pointsPerSecond = 5f;

    public bool scoreIncreasing;

    #endregion

    private void Start()
    {
        if(PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCount = PlayerPrefs.GetFloat("HighScore");
        }
    }
    void Update()
    {
        if (scoreIncreasing)
        {
            scoreCount += pointsPerSecond * Time.deltaTime;
        }

        if(scoreCount > highScoreCount)
        {
            highScoreCount = scoreCount;
            PlayerPrefs.SetFloat("HighScore", highScoreCount);
        }

        scoreText.text = " " + Mathf.Round(scoreCount);
        highScoreText.text = " " + Mathf.Round(highScoreCount);
    }

    public void AddScore(int pointsToAdd)
    {
        scoreCount += pointsToAdd;
    }
}
