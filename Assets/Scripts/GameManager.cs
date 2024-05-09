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

    // ���݂̏��
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

    // �O���炱�̃��\�b�h���g���ď�Ԃ�ύX
    public void SetCurrentState(GameState state)
    {
        currentGameState = state;
        OnGameStateChanged(currentGameState);
    }

    // ��Ԃ��ς�����牽�����邩
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

    // Start�ɂȂ����Ƃ��̏���
    void TitleAction()
    {
        if (gameoverGameObject.activeSelf) gameoverGameObject.SetActive(false);
        if (!titleGameObject.activeSelf) titleGameObject.SetActive(true);
    }
    // Playing�ɂȂ����Ƃ��̏���
    void PlayingAction()
    {
        ballSpawner.StartBallSpawne();
    }
    // Prepare�ɂȂ����Ƃ��̏���
    void PrepareAction()
    {
        //�I�u�W�F�N�g���\���ɂ���
        if (titleGameObject.activeSelf) titleGameObject.SetActive(false);
        if (gameoverGameObject.activeSelf) gameoverGameObject.SetActive(false);

        DestroyBallObject();

        // �X�R�A��0�ɂ���
        ScoreManager.Instance.NowScore = 0;
        //�X�e�[�g��Playing�ɕύX
        SetCurrentState(GameState.Playing);
    }

    // GameOver�ɂȂ����Ƃ��̏���
    void GameOverAction()
    {
        gameoverGameObject.SetActive(true);
        gameoverGameObject.GetComponent<AudioSource>().Play();
        DestroyBallObject();
        ScoreManager.Instance.ResetScore();
    }

    //�{�[���I�u�W�F�N�g����������
    void DestroyBallObject()
    {
        // �V�[����̂��ׂẴQ�[���I�u�W�F�N�g���擾
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        // ���ׂẴQ�[���I�u�W�F�N�g���`�F�b�N���ABall�^�O�����I�u�W�F�N�g��j������
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("Ball"))
            {
                Destroy(obj);
            }
        }
    }

    //�Q�[���I��
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
    }
}
