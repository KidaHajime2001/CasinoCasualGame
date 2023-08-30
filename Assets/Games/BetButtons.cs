using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BetButtons : MonoBehaviour
{
    [SerializeField] Button bet1;
    [SerializeField] Button bet10;
    [SerializeField] Button bet100;
    [SerializeField] Button cancel;
    [SerializeField] Chip chip;

    [SerializeField] int bet = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int GetBet()
    {
        return this.bet;
    }

    public void ResetBet()
    {
        this.bet = 0;
    }

    public void Bet(int _value)
    {
        if(this.chip.GetComponent<Chip>().GetChip() >= this.bet + _value)
        {
            this.bet += _value;
        }
    }
}
