using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �n���ꂽ�l����A����I�����A�\�����邾��
// �ŏ���4�p�^�[�����x��

public class Poker : MonoBehaviour
{
    [SerializeField] uint magnification;
    [SerializeField] bool isWin;
    struct Role
    {
        uint magnification;
        bool isWin;

        GameObject first;
        GameObject second;
        GameObject third;
        GameObject fourth;
        GameObject fifth;
    }
    // �c�[�y�A2
    [SerializeField] Role two;
    [SerializeField] Role missingTwo;
    // �X���[�J�[�h3
    [SerializeField] Role three;
    [SerializeField] Role missingThree;



    // �X�g���[�g4

    // �t���b�V��5

    // �t���n�E�X7

    // �t�H�[�J�[�h20

    // �X�g���[�g�t���b�V��50

    // ���C�����X�g���[�g�t���b�V��100

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
