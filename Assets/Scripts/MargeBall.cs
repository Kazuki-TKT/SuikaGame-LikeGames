using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MargeBall : MonoBehaviour
{
    //このボールが持つ固有のID
    public int myBallID;

    //次に生成するボール
    [SerializeField]
    GameObject nextSpawnBall;

    //スコアに加点するポイント
    [SerializeField]
    int addPoint;

    public bool isDroped = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ball") ||
            collision.gameObject.CompareTag("Wall"))
        {
            isDroped = true;
        }

        if (nextSpawnBall == null) return;

        if (collision.gameObject.CompareTag("Ball")
            && collision.gameObject.name.Contains(this.gameObject.name)
            && collision.gameObject.GetComponent<MargeBall>().myBallID > myBallID)
        {
            //Debug.Log(collision.gameObject.name + "にぶつかった");

            //スコアを加点する
            ScoreManager.Instance.AddScore(addPoint);

            //ぶつかった相手との中間のポジションを得る
            Vector2 spawnPosition;
            spawnPosition.x = (this.gameObject.transform.position.x + collision.gameObject.transform.position.x) / 2;
            spawnPosition.y = (this.gameObject.transform.position.y + collision.gameObject.transform.position.y) / 2;

            //次の生成先のボールを、先ほど得た中間のポジションに生成
            GameObject spawnball = Instantiate(nextSpawnBall, spawnPosition, Quaternion.identity);

            //生成したボールのシミュレーションをON(True)にする
            spawnball.GetComponent<Rigidbody2D>().simulated = true;
            // SEを鳴らす
            spawnball.GetComponent<AudioSource>().Play();

            //生成したボールがMargeBall.csのballIDの変数を変更する
            if (spawnball.GetComponent<MargeBall>() != null)
            {
                spawnball.GetComponent<MargeBall>().myBallID = myBallID;
            }

            //オブジェクトを消す
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}


