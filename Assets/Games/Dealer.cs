using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour
{
    [SerializeField] GameObject leftGame;
    [SerializeField] int leftMag;

    [SerializeField] GameObject rightGame;
    [SerializeField] int rightMag;

    [SerializeField] bool leftGameWinner;
    [SerializeField] bool betOnLeft;

    [SerializeField] float betTime;
    [SerializeField] float countDownTime;
    [SerializeField] float timer;

    [SerializeField] GameObject betButtons;
    [SerializeField] GameObject selectButtons;

    [SerializeField] Chip playerChip;

    // Start is called before the first frame update
    void Start()
    {
        this.timer = betTime;

        // ゲームタイプを決定(ポーカーやブラックジャック等)

        // 倍率を決定

        // 左右どちらが勝利になるか決定
        // (どちらも勝利どちらも敗北の場合が追加されるかも)
    }

    // Update is called once per frame
    void Update()
    {
        // このゲームオブジェクトが有効化または生成されたら下の処理が実行される。

        // ゲームを呼出し、倍率と勝利状態を渡す。

        this.timer -= Time.deltaTime;
        if (timer > 0)
        {
            // プレイヤーのベットを受け付ける
            // 右か左を選択
            this.selectButtons.SetActive(true);
            // ベットボタンを表示
            betButtons.SetActive(true);

            this.betOnLeft = this.selectButtons.GetComponent<Select>().IsSelectedLeft();
        }
        else
        {
            this.selectButtons.SetActive(false);
            this.betButtons.SetActive(false);

            // 裏返っているカードを表に向ける

            // 精算
            // (負けなら全額没収、勝ちなら倍にして返却)
            playerChip.PassTheBet(this.betButtons.GetComponent<BetButtons>().GetBet());
            if(betOnLeft)
            {
                if(this.leftGameWinner)
                {
                    playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * leftMag);
                }
            }
            else
            {
                if(!this.leftGameWinner)
                {
                    playerChip.ReceivingBet(this.betButtons.GetComponent<BetButtons>().GetBet() * rightMag);
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
}
