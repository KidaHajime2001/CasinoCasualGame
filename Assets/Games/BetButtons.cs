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

    [SerializeField] uint bet;

    // Start is called before the first frame update
    void Start()
    {
        bet = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public uint GetBet()
    {
        return this.bet;
    }

    public void ResetBet()
    {
        this.bet = 0;
    }

    public void Bet1()
    {
        ++bet;
    }

    public void Bet10()
    {
        bet += 10;
    }

    public void Bet100()
    {
        bet += 100;
    }
}
