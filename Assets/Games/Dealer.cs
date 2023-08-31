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
        // ゲームタイプを決定(ポーカーやブラックジャック等)
        this.SelectGames();
        // 倍率を決定＆ゲームに設定する。
        this.SelectMagnification();
    }

    // Update is called once per frame
    void Update()
    {
        // ベットされている量を表示
        this.betText.text = "Bet : "+this.betButtons.GetComponent<BetButtons>().GetBet().ToString();

        // 時間制限内なら
        if (this.timer > 0)
        {
            this.timer -= Time.deltaTime;

            this.Bet();
        
            // カウントダウンを表示する時間になったら
            if(this.timer < this.countDownTime)
            {
                this.countDownText.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            // ベットできないようにする。
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);
            // カウントダウン表示を終了
            this.countDownText.text = "";

            // 裏返っているカードを表に向ける処理が必要

            // 結果から精算
            this.Settle();

            // デバッグ用(時間を戻す事で再度開始)
            this.timer = this.betTime;
            // 通常：非アクティブにすることで処理を終了
            //this.gameObject.SetActive(false);
        }
    }


    [SerializeField] TextMeshProUGUI lGameNameText;
    [SerializeField] TextMeshProUGUI rGameNameText;
    [SerializeField] TextMeshProUGUI lMagText;
    [SerializeField] TextMeshProUGUI rMagText;
    // 左右のゲームを決定する。
    void SelectGames()
    {
        // 複数あるゲームからランダムorレベル設計で設定

        // 仮置きでゲームの種類のみ表示
        this.lGameNameText.text = lGame.name;
        this.rGameNameText.text = rGame.name;
    }

    // 左右のゲームの倍率を決定する。
    void SelectMagnification()
    {
        // 本来：倍率と役成立フラグをそれぞれのゲームに渡す(この情報から手札が作成される)

        // 仮置：倍率のみを表示
        this.lMagText.text = '×' + lMag.ToString();
        this.rMagText.text = '×' + rMag.ToString();
    }

    // 精算処理
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

        // ベット用のボタンが保有するベット値をリセット
        this.betButtons.GetComponent<BetButtons>().ResetBet();
    }

    // ベット受付処理
    void Bet()
    {
        // プレイヤーのベットを受け付ける
        // 右か左を選択
        this.selectButtons.SetActive(true);
        // ベットボタンを表示
        this.betButtons.SetActive(true);

        this.betOnLeft = this.selectButtons.GetComponent<Select>().IsSelectedLeft();
    }
}