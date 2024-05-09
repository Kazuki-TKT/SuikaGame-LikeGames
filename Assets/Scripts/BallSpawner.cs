using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BallSpawner : MonoBehaviour
{
    //ボールを生成するポジションのオブジェクト
    [SerializeField]
    GameObject spawnPositionObject;

    //生成するボールのリスト
    [SerializeField]
    List<GameObject> ballPrefabs = new List<GameObject>();

    //所持しているボール
    GameObject possesionBall;
    public GameObject PossesionBall { get => possesionBall; }

    //次に生成するボールの数字
    int nextBallNumber;

    //次に生成するボールを表示するイメージ
    [SerializeField]
    Image nextBallImage;

    //ボールを落としているかどうかの判定
    bool isDrop = false;

    //ボールID
    int newBallID = 0;

    [SerializeField]
    AudioSource dropAudioSource;

    void Update()
    {
        if (
            GameManager.Instance.CurrentGameState != GameState.Playing||
            isDrop
            ) return;

        //ボールを動かす
        possesionBall.transform.position = spawnPositionObject.transform.position;

        //マウスをクリックしたら所持しているボールを落とす
        if (Input.GetMouseButtonDown(0))
        {
            DropBall();
        }
    }

    public void StartBallSpawne()
    {
        ShowNextBall();
        possesionBall = CreateBall(Random.Range(0, ballPrefabs.Count));
    }

    /// <summary>
    /// ボールを生成する関数
    /// </summary>
    /// <returns>生成したボールをリターン</returns>
    GameObject CreateBall(int number)
    {
        isDrop = false;

        GameObject spawnball = Instantiate(ballPrefabs[number], spawnPositionObject.transform.position, Quaternion.identity);

        if (spawnball.GetComponent<MargeBall>() != null)
        {
            spawnball.GetComponent<MargeBall>().myBallID = newBallID++;
        }
        //Debug.Log($"<color=red>{spawnball.name}</color>を生成しました");
        return spawnball;
    }

    /// <summary>
    /// 待機時間経過後にボールを生成するコルーチン
    /// </summary>
    IEnumerator CreateBallCoroutin()
    {
        yield return new WaitForSeconds(1);
        possesionBall = CreateBall(nextBallNumber);
        ShowNextBall();
    }

    /// <summary>
    /// ボールを落とし、ボールを再生成する関数
    /// </summary>
    void DropBall()
    {
        dropAudioSource.Play();

        possesionBall.GetComponent<Rigidbody2D>().simulated = true;
        possesionBall = null;
        isDrop = true;

        StartCoroutine(CreateBallCoroutin());
    }

    void ShowNextBall()
    {
        nextBallNumber = Random.Range(0, ballPrefabs.Count);
        nextBallImage.sprite = ballPrefabs[nextBallNumber].GetComponent<SpriteRenderer>().sprite;
    }
}