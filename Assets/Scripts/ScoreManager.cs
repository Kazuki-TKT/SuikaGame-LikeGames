using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    Text scoreText;
    [SerializeField]
    Text gameOverScoreText;

    int nowScore;
    public int NowScore { get => nowScore; set => nowScore = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
    }

    /// <summary>
    /// �|�C���g�����_����֐�
    /// </summary>
    /// <param name="point"></param>
    public void AddScore(int point)
    {
        nowScore += point;
        scoreText.text = $"{nowScore}";
        gameOverScoreText.text = $"�X�R�A : {nowScore}";
    }
    /// <summary>
    /// �X�R�A�����Z�b�g����֐�
    /// </summary>
    public void ResetScore()
    {
        nowScore = 0;
        scoreText.text = $"{nowScore}";
    }
}