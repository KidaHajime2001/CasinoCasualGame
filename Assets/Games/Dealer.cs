using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    [SerializeField] GameObject lGame;
    [SerializeField] int lMag;

    [SerializeField] GameObject rGame;
    [SerializeField] int rMag;

    [SerializeField] bool leftGameWinner;
    [SerializeField] bool betOnLeft;

    [SerializeField] float betTime;
    [SerializeField] float countDownTime;
    [SerializeField] float timer;

    [SerializeField] GameObject betButtons;
    [SerializeField] GameObject selectButtons;

    [SerializeField] Chip playerChip;

    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI betText;

    // Start is called before the first frame update
    void Start()
    {
        this.timer = betTime;

        // �Q�[���^�C�v������(�|�[�J�[��u���b�N�W���b�N��)
        // ���͉��u���Ō��܂����Q�[�����ݒ�B
        this.SelectGames();

        // �{��������
    }

    // Update is called once per frame
    void Update()
    {
        // ���̃Q�[���I�u�W�F�N�g���L�����܂��͐������ꂽ�牺�̏��������s�����B

        // �Q�[�����ďo���A�{���Ə�����Ԃ�n���B

        this.timer -= Time.deltaTime;
        this.betText.text = "Bet : "+this.betButtons.GetComponent<BetButtons>().GetBet().ToString();

        if (this.timer > 0)
        {
            // �v���C���[�̃x�b�g���󂯕t����
            // �E������I��
            this.selectButtons.SetActive(true);
            // �x�b�g�{�^����\��
            betButtons.SetActive(true);

            this.betOnLeft = this.selectButtons.GetComponent<Select>().IsSelectedLeft();
        
            // �J�E���g�_�E����\�����鎞�ԂɂȂ�����
            if(this.timer < this.countDownTime)
            {
                this.countDownText.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);
            this.countDownText.text = "";   // ��0���\������Ȃ����

            // ���Ԃ��Ă���J�[�h��\�Ɍ�����

            // ���Z
            // (�����Ȃ�S�z�v���A�����Ȃ�{�ɂ��ĕԋp)
            playerChip.PassTheBet(this.betButtons.GetComponent<BetButtons>().GetBet());
            if(betOnLeft)
            {
                if(this.leftGameWinner)
                {
                    playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * lMag);
                }
            }
            else
            {
                if(!this.leftGameWinner)
                {
                    playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * rMag);
                }
            }

            // �ǂ���Ύ��ɐi�ޏ���
            if(this.selectButtons.GetComponent<Select>().IsSelectedLeft())
            {
                Debug.Log("����"+ this.betButtons.GetComponent<BetButtons>().GetBet()+"��");
            }
            else
            {
                Debug.Log("�E��" + this.betButtons.GetComponent<BetButtons>().GetBet()+"��");
            }
            this.betButtons.GetComponent<BetButtons>().ResetBet();
            //this.gameObject.SetActive(false);

            // �f�o�b�O�p(���Ԃ�߂��ƍēx�J�n)
            this.timer = this.betTime;
        }
    }


    [SerializeField] TextMeshProUGUI lGameNameText;
    [SerializeField] TextMeshProUGUI rGameNameText;
    [SerializeField] TextMeshProUGUI lMagText;
    [SerializeField] TextMeshProUGUI rMagText;
    // ���E�̃Q�[�������肷��B
    void SelectGames()
    {
        // ���E�̃Q�[���̎�ނƔ{����\��
        this.lGameNameText.text = lGame.name;
        this.lMagText.text = '�~' + lMag.ToString();
        this.rGameNameText.text = rGame.name;
        this.rMagText.text = '�~' + rMag.ToString();
    }

    void SelectMagnification()
    {

    }
}