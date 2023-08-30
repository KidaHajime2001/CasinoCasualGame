using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    [SerializeField] GameObject gameType1;
    [SerializeField] uint magnification1;

    [SerializeField] GameObject gameType2;
    [SerializeField] uint magnification2;

    [SerializeField] bool leftGameWinner;
    [SerializeField] bool betOnLeft;

    [SerializeField] const float BetTime = 0;
    [SerializeField] const float CountDownTime = 0;
    [SerializeField] float timer;

    [SerializeField] BetButtons betButtons;

    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���^�C�v������(�|�[�J�[��u���b�N�W���b�N��)

        // �{��������

        // ���E�ǂ��炪�����ɂȂ邩����
        // (�ǂ���������ǂ�����s�k�̏ꍇ���ǉ�����邩��)
    }

    // Update is called once per frame
    void Update()
    {
        // �Q�[�����ďo���{���Ə�����Ԃ�n���B

        timer -= Time.deltaTime;
        if (timer >= 0)
        {
            // �v���C���[�̃x�b�g���󂯕t����
            // �E������I��

            // ����1,5,10�̃{�^����\��

            // �L�����Z����t

            // ���Ԃ��Ă���J�[�h��\�Ɍ�����

            // �����Ȃ�S�z�v��
            // ���l�Ȃ�{�ɂ��ĕԋp
        }
        else
        {
            this.timer = BetTime;
            this.betButtons.ResetBet();
        }
    }
}
