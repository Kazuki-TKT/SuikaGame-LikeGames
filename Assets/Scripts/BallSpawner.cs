using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BallSpawner : MonoBehaviour
{
    //�{�[���𐶐�����|�W�V�����̃I�u�W�F�N�g
    [SerializeField]
    GameObject spawnPositionObject;

    //��������{�[���̃��X�g
    [SerializeField]
    List<GameObject> ballPrefabs = new List<GameObject>();

    //�������Ă���{�[��
    GameObject possesionBall;
    public GameObject PossesionBall { get => possesionBall; }

    //���ɐ�������{�[���̐���
    int nextBallNumber;

    //���ɐ�������{�[����\������C���[�W
    [SerializeField]
    Image nextBallImage;

    //�{�[���𗎂Ƃ��Ă��邩�ǂ����̔���
    bool isDrop = false;

    //�{�[��ID
    int newBallID = 0;

    [SerializeField]
    AudioSource dropAudioSource;

    void Update()
    {
        if (
            GameManager.Instance.CurrentGameState != GameState.Playing||
            isDrop
            ) return;

        //�{�[���𓮂���
        possesionBall.transform.position = spawnPositionObject.transform.position;

        //�}�E�X���N���b�N�����珊�����Ă���{�[���𗎂Ƃ�
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
    /// �{�[���𐶐�����֐�
    /// </summary>
    /// <returns>���������{�[�������^�[��</returns>
    GameObject CreateBall(int number)
    {
        isDrop = false;

        GameObject spawnball = Instantiate(ballPrefabs[number], spawnPositionObject.transform.position, Quaternion.identity);

        if (spawnball.GetComponent<MargeBall>() != null)
        {
            spawnball.GetComponent<MargeBall>().myBallID = newBallID++;
        }
        //Debug.Log($"<color=red>{spawnball.name}</color>�𐶐����܂���");
        return spawnball;
    }

    /// <summary>
    /// �ҋ@���Ԍo�ߌ�Ƀ{�[���𐶐�����R���[�`��
    /// </summary>
    IEnumerator CreateBallCoroutin()
    {
        yield return new WaitForSeconds(1);
        possesionBall = CreateBall(nextBallNumber);
        ShowNextBall();
    }

    /// <summary>
    /// �{�[���𗎂Ƃ��A�{�[�����Đ�������֐�
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