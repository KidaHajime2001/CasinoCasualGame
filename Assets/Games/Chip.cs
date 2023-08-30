using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] int chip;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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