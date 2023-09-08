using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [Tooltip("���L���Ă���`�b�v�̗�")]
    [SerializeField] int have;

    [Header("UI")]
    [Tooltip("���L���Ă���`�b�v�̗ʂ�\���������ꍇ�ATMP�ɃA�^�b�`")]
    [SerializeField] TextMeshProUGUI haveTxt;

    void Start()
    {
        UpdateHaveTMP();
    }

    void Update()
    {

    }

    void UpdateHaveTMP()
    {
        if(this.haveTxt != null)
        {
            haveTxt.text = this.have.ToString();
        }
    }

    public int Get()
    {
        return this.have;
    }

    public int PassTheBet(int _value)
    {
        this.have -= _value;
        UpdateHaveTMP();

        return _value;
    }

    public void ReceivingBet(int _value)
    {
        this.have += _value;
        UpdateHaveTMP();
    }
}