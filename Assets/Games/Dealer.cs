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


    [SerializeField] Chip playerChip;
    [SerializeField] GameObject betSystem;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI betTxt;
    [SerializeField] TextMeshProUGUI countDownTxt;
    [SerializeField] TextMeshProUGUI lGameNameTxt;
    [SerializeField] TextMeshProUGUI rGameNameTxt;
    [SerializeField] TextMeshProUGUI lMagTxt;
    [SerializeField] TextMeshProUGUI rMagTxt;

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
        this.betTxt.text = this.betSystem.GetComponent<BetSystem>().GetBet().ToString();

        // ���Ԑ������Ȃ�
        if (this.timer > 0)
        {
            this.timer -= Time.deltaTime;

            this.Bet();
        
            // �J�E���g�_�E����\�����鎞�ԂɂȂ�����
            if(this.timer < this.countDownTime)
            {
                this.countDownTxt.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            // �x�b�g�ł��Ȃ��悤�ɂ���B
            this.betSystem.SetActive(false);
            // �J�E���g�_�E���\�����I��
            this.countDownTxt.text = "";

            // ���Ԃ��Ă���J�[�h��\�Ɍ����鏈�����K�v

            // ���ʂ��琸�Z
            this.Settle();

            // �f�o�b�O�p(���Ԃ�߂����ōēx�J�n)
            this.timer = this.betTime;
            // �ʏ�F��A�N�e�B�u�ɂ��邱�Ƃŏ������I��
            //this.gameObject.SetActive(false);
        }
    }

    // ���E�̃Q�[�������肷��B
    void SelectGames()
    {
        // ��������Q�[�����烉���_��or���x���݌v�Őݒ�

        // ���u���ŃQ�[���̎�ނ̂ݕ\��
        this.lGameNameTxt.text = lGame.name;
        this.rGameNameTxt.text = rGame.name;
    }

    // ���E�̃Q�[���̔{�������肷��B
    void SelectMagnification()
    {
        // �{���F�{���Ɩ𐬗��t���O�����ꂼ��̃Q�[���ɓn��(���̏�񂩂��D���쐬�����)

        // ���u�F�{���݂̂�\��
        this.lMagTxt.text = '�~' + lMag.ToString();
        this.rMagTxt.text = '�~' + rMag.ToString();
    }

    // ���Z����
    void Settle()
    {
        this.playerChip.PassTheBet(this.betSystem.GetComponent<BetSystem>().GetBet());
        if (betOnLeft)
        {
            if (this.leftGameWinner)
            {
                playerChip.ReceivingBet(this.betSystem.GetComponent<BetSystem>().GetBet() * lMag);
            }
        }
        else
        {
            if (!this.leftGameWinner)
            {
                playerChip.ReceivingBet(this.betSystem.GetComponent<BetSystem>().GetBet() * rMag);
            }
        }

        // �x�b�g�p�̃{�^�����ۗL����x�b�g�l�����Z�b�g
        this.betSystem.GetComponent<BetSystem>().BetReset();
    }

    // �x�b�g��t����
    void Bet()
    {
        // �v���C���[�̃x�b�g���󂯕t����
        // �x�b�g�{�^����\��
        this.betSystem.SetActive(true);

        this.betOnLeft = this.betSystem.GetComponent<BetSystem>().IsSelectedLeft();
    }
}