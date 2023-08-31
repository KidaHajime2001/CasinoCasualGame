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
        this.SelectGames();
        // �{�������聕�Q�[���ɐݒ肷��B
        this.SelectMagnification();
    }

    // Update is called once per frame
    void Update()
    {
        // �x�b�g����Ă���ʂ�\��
        this.betText.text = "Bet : "+this.betButtons.GetComponent<BetButtons>().GetBet().ToString();

        // ���Ԑ������Ȃ�
        if (this.timer > 0)
        {
            this.timer -= Time.deltaTime;

            this.Bet();
        
            // �J�E���g�_�E����\�����鎞�ԂɂȂ�����
            if(this.timer < this.countDownTime)
            {
                this.countDownText.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            // �x�b�g�ł��Ȃ��悤�ɂ���B
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);
            // �J�E���g�_�E���\�����I��
            this.countDownText.text = "";

            // ���Ԃ��Ă���J�[�h��\�Ɍ����鏈�����K�v

            // ���ʂ��琸�Z
            this.Settle();

            // �f�o�b�O�p(���Ԃ�߂����ōēx�J�n)
            this.timer = this.betTime;
            // �ʏ�F��A�N�e�B�u�ɂ��邱�Ƃŏ������I��
            //this.gameObject.SetActive(false);
        }
    }


    [SerializeField] TextMeshProUGUI lGameNameText;
    [SerializeField] TextMeshProUGUI rGameNameText;
    [SerializeField] TextMeshProUGUI lMagText;
    [SerializeField] TextMeshProUGUI rMagText;
    // ���E�̃Q�[�������肷��B
    void SelectGames()
    {
        // ��������Q�[�����烉���_��or���x���݌v�Őݒ�

        // ���u���ŃQ�[���̎�ނ̂ݕ\��
        this.lGameNameText.text = lGame.name;
        this.rGameNameText.text = rGame.name;
    }

    // ���E�̃Q�[���̔{�������肷��B
    void SelectMagnification()
    {
        // �{���F�{���Ɩ𐬗��t���O�����ꂼ��̃Q�[���ɓn��(���̏�񂩂��D���쐬�����)

        // ���u�F�{���݂̂�\��
        this.lMagText.text = '�~' + lMag.ToString();
        this.rMagText.text = '�~' + rMag.ToString();
    }

    // ���Z����
    void Settle()
    {
        this.playerChip.PassTheBet(this.betButtons.GetComponent<BetButtons>().GetBet());
        if (betOnLeft)
        {
            if (this.leftGameWinner)
            {
                playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * lMag);
            }
        }
        else
        {
            if (!this.leftGameWinner)
            {
                playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * rMag);
            }
        }

        // �x�b�g�p�̃{�^�����ۗL����x�b�g�l�����Z�b�g
        this.betButtons.GetComponent<BetButtons>().ResetBet();
    }

    // �x�b�g��t����
    void Bet()
    {
        // �v���C���[�̃x�b�g���󂯕t����
        // �E������I��
        this.selectButtons.SetActive(true);
        // �x�b�g�{�^����\��
        this.betButtons.SetActive(true);

        this.betOnLeft = this.selectButtons.GetComponent<Select>().IsSelectedLeft();
    }
}