using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Title,
    Prepare,
    Playing,
    Setting,
    GameOver
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 現在の状態
    private GameState currentGameState;
    public GameState CurrentGameState { get => currentGameState; }

    [SerializeField]
    BallSpawner ballSpawner;

    [SerializeField]
    GameObject titleGameObject;
    [SerializeField]
    GameObject gameoverGameObject;

    [SerializeField]
    CustomButton[] playButton;

    [SerializeField]
    AudioSource clickAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SetCurrentState(GameState.Title);
    }

    private void Start()
    {
        if (playButton != null)
        {
            foreach (CustomButton customButton in playButton)
            {
                customButton.onClickCallback += () => { 
                    SetCurrentState(GameState.Prepare);
                    clickAudioSource.Play();
                };
            }
        }
        
    }

    // 外からこのメソッドを使って状態を変更
    public void SetCurrentState(GameState state)
    {
        currentGameState = state;
        OnGameStateChanged(currentGameState);
    }

    // 状態が変わったら何をするか
    void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Title:
                TitleAction();
                break;
            case GameState.Prepare:
                PrepareAction();
                //StartCoroutine(PrepareCoroutine());
                break;
            case GameState.Playing:
                PlayingAction();
                break;
            case GameState.GameOver:
                GameOverAction();
                break;
            default:
                break;
        }
    }

    // Startになったときの処理
    void TitleAction()
    {
        if (gameoverGameObject.activeSelf) gameoverGameObject.SetActive(false);
        if (!titleGameObject.activeSelf) titleGameObject.SetActive(true);
    }
    // Playingになったときの処理
    void PlayingAction()
    {
        ballSpawner.StartBallSpawne();
    }
    // Prepareになったときの処理
    void PrepareAction()
    {
        //オブジェクトを非表示にする
        if (titleGameObject.activeSelf) titleGameObject.SetActive(false);
        if (gameoverGameObject.activeSelf) gameoverGameObject.SetActive(false);

        DestroyBallObject();

        // スコアを0にする
        ScoreManager.Instance.NowScore = 0;
        //ステートをPlayingに変更
        SetCurrentState(GameState.Playing);
    }

    // GameOverになったときの処理
    void GameOverAction()
    {
        gameoverGameObject.SetActive(true);
        gameoverGameObject.GetComponent<AudioSource>().Play();
        DestroyBallObject();
        ScoreManager.Instance.ResetScore();
    }

    //ボールオブジェクトを消去する
    void DestroyBallObject()
    {
        // シーン上のすべてのゲームオブジェクトを取得
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        // すべてのゲームオブジェクトをチェックし、Ballタグを持つオブジェクトを破棄する
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Ball"))
            {
                Destroy(obj);
            }
        }
    }

    //ゲーム終了
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
