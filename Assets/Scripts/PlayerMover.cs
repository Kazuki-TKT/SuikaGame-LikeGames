using System.Collections.Generic;
using UnityEngine;


public class PlayerMover : MonoBehaviour
{
    //プレイヤーの行動範囲
    float playerPositionX_L;
    float playerPositionX_R;
    public float playerPositionY;

    //ボールごとの動く範囲
    public List<RangeOfBall> rangeOfBalls = new List<RangeOfBall>();

    [SerializeField]
    BallSpawner ballSpawner;

    private void Start()
    {
        ballSpawner = this.gameObject.GetComponent<BallSpawner>();
    }
    void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;
        SpecifyRange();
        //プレイヤーを動かす
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.x = Mathf.Clamp(mousePosition.x, playerPositionX_L, playerPositionX_R);
        mousePosition.y = playerPositionY;
        this.gameObject.transform.position = mousePosition;
    }

    //生成したオブジェクトに合わして動ける範囲を決定する
    public void SpecifyRange()
    {
        if (ballSpawner.PossesionBall != null)
        {
            foreach (RangeOfBall range in rangeOfBalls)
            {
                if (ballSpawner.PossesionBall.name.Contains(range.BallPrefab.name))
                {
                    // 同じオブジェクトが見つかった場合の処理
                    playerPositionX_L = range.positionX_L;
                    playerPositionX_R = range.positionX_R;
                }
            }
        }
    }
}

[System.Serializable]
public class RangeOfBall
{
    [SerializeField]
    GameObject ballPrefab;
    public GameObject BallPrefab { get => ballPrefab; }

    public float positionX_L;
    public float positionX_R;
}


