using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MargeBall : MonoBehaviour
{
    //���̃{�[�������ŗL��ID
    public int myBallID;

    //���ɐ�������{�[��
    [SerializeField]
    GameObject nextSpawnBall;

    //�X�R�A�ɉ��_����|�C���g
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
            //Debug.Log(collision.gameObject.name + "�ɂԂ�����");

            //�X�R�A�����_����
            ScoreManager.Instance.AddScore(addPoint);

            //�Ԃ���������Ƃ̒��Ԃ̃|�W�V�����𓾂�
            Vector2 spawnPosition;
            spawnPosition.x = (this.gameObject.transform.position.x + collision.gameObject.transform.position.x) / 2;
            spawnPosition.y = (this.gameObject.transform.position.y + collision.gameObject.transform.position.y) / 2;

            //���̐�����̃{�[�����A��قǓ������Ԃ̃|�W�V�����ɐ���
            GameObject spawnball = Instantiate(nextSpawnBall, spawnPosition, Quaternion.identity);

            //���������{�[���̃V�~�����[�V������ON(True)�ɂ���
            spawnball.GetComponent<Rigidbody2D>().simulated = true;
            // SE��炷
            spawnball.GetComponent<AudioSource>().Play();

            //���������{�[����MargeBall.cs��ballID�̕ϐ���ύX����
            if (spawnball.GetComponent<MargeBall>() != null)
            {
                spawnball.GetComponent<MargeBall>().myBallID = myBallID;
            }

            //�I�u�W�F�N�g������
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}


