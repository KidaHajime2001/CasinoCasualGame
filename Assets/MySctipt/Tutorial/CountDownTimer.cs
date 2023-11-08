using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField] Image gage;
    [SerializeField] TextMeshProUGUI tmpText;
    [SerializeField] Image progressRing;

    float startTime = 0.0f;
    float remaining = 0.0f;
    bool isCountingDown = false;

    void Start()
    {
        this.tmpText.text = "0";
        this.gage.fillAmount = 0.0f;
        this.progressRing.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isCountingDown)
        {
            this.CountingDown();
            // �S�̂̎c�莞�Ԃ��Q�[�W�ŕ\��
            float percentage = this.remaining / this.startTime;
            this.gage.fillAmount = percentage;
            // �c�莞�Ԃ̕\���ɓK������
            int second = (int)Mathf.Ceil(this.remaining);
            this.tmpText.text = second.ToString();
            // �v���O���X�����O�ɒl��K������
            float milliSec = this.remaining - Mathf.Floor(this.remaining);
            this.progressRing.fillAmount = milliSec;
        }
    }
    void CountingDown()
    {
        this.remaining -= Time.deltaTime;
        // ����0.0�ȉ��ɂȂ�����A
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
    }

    public bool IsCountingDown()
    {
        return this.isCountingDown;
    }

    public float GetRemaining()
    {
        return this.remaining;
    }
}