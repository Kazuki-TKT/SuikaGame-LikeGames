using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;

    private float gameOverTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball")
            && collision.gameObject.GetComponent<MargeBall>().isDroped == true)
        {
            gameOverTime += Time.deltaTime;

            if (gameOverTime > 1)
            {
                GameManager.Instance.SetCurrentState(GameState.GameOver);
                gameOverTime = 0;
            }
        }
    }
}


