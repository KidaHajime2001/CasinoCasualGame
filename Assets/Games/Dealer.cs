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
        // 今は仮置きで決まったゲームが設定。
        this.SelectGames();

        // 倍率を決定
    }

    // Update is called once per frame
    void Update()
    {
        // このゲームオブジェクトが有効化または生成されたら下の処理が実行される。

        // ゲームを呼出し、倍率と勝利状態を渡す。

        this.timer -= Time.deltaTime;
        this.betText.text = "Bet : "+this.betButtons.GetComponent<BetButtons>().GetBet().ToString();

        if (this.timer > 0)
        {
            // プレイヤーのベットを受け付ける
            // 右か左を選択
            this.selectButtons.SetActive(true);
            // ベットボタンを表示
            betButtons.SetActive(true);

            this.betOnLeft = this.selectButtons.GetComponent<Select>().IsSelectedLeft();
        
            // カウントダウンを表示する時間になったら
            if(this.timer < this.countDownTime)
            {
                this.countDownText.text = Mathf.Ceil(this.timer).ToString();
            }
        }
        else
        {
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);
            this.countDownText.text = "";   // 今0が表示されない状態

            // 裏返っているカードを表に向ける

            // 精算
            // (負けなら全額没収、勝ちなら倍にして返却)
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

            // 良ければ次に進む処理
            if(this.selectButtons.GetComponent<Select>().IsSelectedLeft())
            {
                Debug.Log("左に"+ this.betButtons.GetComponent<BetButtons>().GetBet()+"枚");
            }
            else
            {
                Debug.Log("右に" + this.betButtons.GetComponent<BetButtons>().GetBet()+"枚");
            }
            this.betButtons.GetComponent<BetButtons>().ResetBet();
            //this.gameObject.SetActive(false);

            // デバッグ用(時間を戻すと再度開始)
            this.timer = this.betTime;
        }
    }


    [SerializeField] TextMeshProUGUI lGameNameText;
    [SerializeField] TextMeshProUGUI rGameNameText;
    [SerializeField] TextMeshProUGUI lMagText;
    [SerializeField] TextMeshProUGUI rMagText;
    // 左右のゲームを決定する。
    void SelectGames()
    {
        // 左右のゲームの種類と倍率を表示
        this.lGameNameText.text = lGame.name;
        this.lMagText.text = '×' + lMag.ToString();
        this.rGameNameText.text = rGame.name;
        this.rMagText.text = '×' + rMag.ToString();
    }

    void SelectMagnification()
    {

    }
}