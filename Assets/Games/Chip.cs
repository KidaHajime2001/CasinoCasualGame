using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] int chip;
    [SerializeField] TextMeshProUGUI chipText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.chipText != null)
        {
            chipText.text = "Have : " + this.chip.ToString();
        }
    }

    public int GetChip()
    {
        return this.chip;
    }

    public int PassTheBet(int _value)
    {
        this.chip -= _value;
        return _value;
    }

    public void ReceivingBet(int _value)
    {
        this.chip += _value;
    }
}