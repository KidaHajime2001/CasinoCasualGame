using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] Image gage;
    [SerializeField] TextMeshProUGUI tmpText;
    [SerializeField] Image progressRing;
    [SerializeField] TextMeshProUGUI speedMagText;
    [SerializeField] List<int> speedMags;

    float startTime = 0.0f;
    float remaining = 0.0f;
    bool isCountingDown = false;
    [SerializeField]float speedMag = 1.0f;

    void Start()
    {
        this.tmpText.text = "0";
        this.gage.fillAmount = 0.0f;
        this.progressRing.fillAmount = 0.0f;
        UpdateSpeedMag(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isCountingDown)
        {
            this.CountingDown();
            // 全体の残り時間をゲージで表示
            float percentage = this.remaining / this.startTime;
            this.gage.fillAmount = percentage;
            // 残り時間の表示に適応する
            int second = (int)Mathf.Ceil(this.remaining);
            this.tmpText.text = second.ToString();
            // プログレスリングに値を適応する
            float milliSec = this.remaining - Mathf.Floor(this.remaining);
            this.progressRing.fillAmount = milliSec;
        }
    }
    void CountingDown()
    {
        this.remaining -= Time.deltaTime * this.speedMag;
        // もし0.0以下になったら、
        if(this.remaining <= 0.0f)
        {
            this.remaining = 0.0f;
            this.isCountingDown = false;
        }
    }

    public void SetTimer(float _second)
    {
        this.remaining = _second;
        this.startTime = _second;
        this.isCountingDown = true;
        UpdateSpeedMag(1);
    }

    public bool IsCountingDown()
    {
        return this.isCountingDown;
    }

    public float GetRemaining()
    {
        return this.remaining;
    }

    // 速度倍率ボタンから呼び出し、倍率を変更していく
    public void ChangeSpeedMag()
    {
        for(int i = 0;i < this.speedMags.Count; ++i)
        {
            // 現在の倍率を見つけ
            if(this.speedMag == this.speedMags[i])
            {
                // 現在の倍率が末尾でなかったら
                if (i != this.speedMags.Count - 1)
                {
                    UpdateSpeedMag(this.speedMags[i + 1]);
                }
                else
                {
                    UpdateSpeedMag(this.speedMags[0]);
                }
                break;
            }
        }
    }

    void UpdateSpeedMag(int _mag)
    {
        if(this.speedMagText == null)
        {
            return;
        }
        this.speedMag = _mag;
        this.speedMagText.text = "×" + this.speedMag.ToString();
    }
}