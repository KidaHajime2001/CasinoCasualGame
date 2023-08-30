using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �n���ꂽ�l����A����I�����A�\�����邾��
// �ŏ���4�p�^�[�����x��

public class Poker : MonoBehaviour
{
    [SerializeField] uint magnification;
    [SerializeField] bool isWin;

    enum Type
    {
        Spade,
        Heart,
        Diamond,
        Club,
        Joker
    };
    struct Card
    {
        Type type;
        uint number;
    };

    struct Role
    {
        Card first;
        Card second;
        Card third;
        Card fourth;
        Card fifth;
    }
    // �c�[�y�A2
    [SerializeField] Role twoPair;
    [SerializeField] Role missTwoPair;
    // �X���[�J�[�h3
    [SerializeField] Role threeCard;
    [SerializeField] Role missThreeCard;

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
