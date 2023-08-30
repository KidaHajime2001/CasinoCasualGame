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

    [SerializeField] float betTime;
    [SerializeField] float countDownTime;
    [SerializeField] float timer;

    [SerializeField] GameObject betButtons;
    [SerializeField] GameObject selectButtons;

    // Start is called before the first frame update
    void Start()
    {
        this.timer = betTime;

        // �Q�[���^�C�v������(�|�[�J�[��u���b�N�W���b�N��)

        // �{��������

        // ���E�ǂ��炪�����ɂȂ邩����
        // (�ǂ���������ǂ�����s�k�̏ꍇ���ǉ�����邩��)
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃Q�[���I�u�W�F�N�g���L�����܂��͐������ꂽ�牺�̏��������s�����B

        // �Q�[�����ďo���A�{���Ə�����Ԃ�n���B

        this.timer -= Time.deltaTime;
        if (timer > 0)
        {
            // �v���C���[�̃x�b�g���󂯕t����
            // �E������I��
            this.selectButtons.SetActive(true);

            // �x�b�g�{�^����\��
            betButtons.SetActive(true);
        }
        else
        {
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);

            // ���Ԃ��Ă���J�[�h��\�Ɍ�����

            // ���Z
            // (�����Ȃ�S�z�v���A�����Ȃ�{�ɂ��ĕԋp)

            // �ǂ���Ύ��ɐi�ޏ���
            if(this.selectButtons.GetComponent<Select>().IsSelectedLeft())
            {
                Debug.Log("����"+ this.betButtons.GetComponent<BetButtons>().GetBet()+"��");
            }
            else
            {
                Debug.Log("�E��" + this.betButtons.GetComponent<BetButtons>().GetBet()+"��");
            }
            //this.timer = betTime;
            this.betButtons.GetComponent<BetButtons>().ResetBet();
            this.gameObject.SetActive(false);
        }
    }
}
