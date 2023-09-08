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
        // ゲームタイプを決定(ポーカーやブラックジャック等)
        this.SelectGames();
        // 倍率を決定＆ゲームに設定する。
        this.SelectMagnification();
    }

    // Update is called once per frame
    void Update()
    {
        // ベットされている量を表示
        this.betTxt.text = this.betSystem.GetComponent<BetSystem>().GetBet().ToString();

        // 時間制限内なら
        if (this.timer > 0)
        {
            this.timer -= Time.deltaTime;

            this.Bet();
        
            // カウントダウンを表示する時間になったら
            if(this.timer < this.countDownTime)
            {
                this.countDownTxt.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            // ベットできないようにする。
            this.betSystem.SetActive(false);
            // カウントダウン表示を終了
            this.countDownTxt.text = "";

            // 裏返っているカードを表に向ける処理が必要

            // 結果から精算
            this.Settle();

            // デバッグ用(時間を戻す事で再度開始)
            this.timer = this.betTime;
            // 通常：非アクティブにすることで処理を終了
            //this.gameObject.SetActive(false);
        }
    }

    // 左右のゲームを決定する。
    void SelectGames()
    {
        // 複数あるゲームからランダムorレベル設計で設定

        // 仮置きでゲームの種類のみ表示
        this.lGameNameTxt.text = lGame.name;
        this.rGameNameTxt.text = rGame.name;
    }

    // 左右のゲームの倍率を決定する。
    void SelectMagnification()
    {
        // 本来：倍率と役成立フラグをそれぞれのゲームに渡す(この情報から手札が作成される)

        // 仮置：倍率のみを表示
        this.lMagTxt.text = '×' + lMag.ToString();
        this.rMagTxt.text = '×' + rMag.ToString();
    }

    // 精算処理
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

        // ベット用のボタンが保有するベット値をリセット
        this.betSystem.GetComponent<BetSystem>().BetReset();
    }

    // ベット受付処理
    void Bet()
    {
        // プレイヤーのベットを受け付ける
        // ベットボタンを表示
        this.betSystem.SetActive(true);

        this.betOnLeft = this.betSystem.GetComponent<BetSystem>().IsSelectedLeft();
    }
}