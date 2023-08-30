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
        // ゲームタイプを決定(ポーカーやブラックジャック等)

        // 倍率を決定

        // 左右どちらが勝利になるか決定
        // (どちらも勝利どちらも敗北の場合が追加されるかも)
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームを呼出し倍率と勝利状態を渡す。

        timer -= Time.deltaTime;
        if (timer >= 0)
        {
            // プレイヤーのベットを受け付ける
            // 右か左を選択

            // 下に1,5,10のボタンを表示

            // キャンセル受付

            // 裏返っているカードを表に向ける

            // 負けなら全額没収
            // 価値なら倍にして返却
        }
        else
        {
            this.timer = BetTime;
            this.betButtons.ResetBet();
        }
    }
}
