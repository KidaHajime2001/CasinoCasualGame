using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BetSystem : MonoBehaviour
{
    [SerializeField] List<Button> lrSelectButtons;
    [SerializeField] List<Button> betButtons;
    [SerializeField] Chip playerChip;
    [SerializeField] int bet = 0;
    
    [Tooltip("左右どちらのゲームにBetするのか?")]
    [SerializeField] bool isSelectedLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // ベットの許可が降りたら実行開始

        // 1.左右どちらに賭けるのかを選ばせる

        // 2.ベットを許可(ボタンを押せるようにする。)

        // 3.ボタンが押されたら、プレイヤーのチップ数が足りるかを見ながら、ベットの量を変更

        // 4.時間が経過したらディーラーから終了指示が来る
    }

    public void SetSelectLeft(bool _isSelectLeft)
    {
        isSelectedLeft = _isSelectLeft;
    }

    public bool IsSelectedLeft()
    {
        return this.isSelectedLeft;
    }


    public int GetBet()
    {
        return this.bet;
    }

    public void BetReset()
    {
        this.bet = 0;
    }

    public void Bet(int _value)
    {
        // ベットレートに、チップが足りていれば
        if(this.playerChip.GetComponent<Chip>().Get() >= this.bet + _value)
        {
            this.bet += _value;
        }
    }
}