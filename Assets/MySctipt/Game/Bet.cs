using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Bet : MonoBehaviour
{
    [SerializeField] GameObject chipImg;
    [SerializeField] GameObject chipImgPosL;
    [SerializeField] GameObject chipImgPosR;
    [SerializeField] TextMeshProUGUI text; 
   
    BetState betState;
    private int betNum = 0;
    private int betMax = 0;
    private int betNow = 0;
    [SerializeField] ChipGenerator chipGenerator;



    // Start is called before the first frame update
    void Start()
    {
        
        betState = BetState.N;

        betNow=betMax=chipGenerator.GetChipNum();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = betNum.ToString();
        if (betState == BetState.R)
        {
            chipImg.transform.position = chipImgPosR.transform.position;
        }
        else if(betState == BetState.L)
        {
            chipImg.transform.position = chipImgPosL.transform.position;
        }
        else
        {
            chipImg.SetActive(false);
        }
        
    }
    public void BetR()
    {
        betState = BetState.R;
        chipImg.SetActive(true);
    }
    public void BetL()
    {
        betState = BetState.L;
        chipImg.SetActive(true);
    }
    public BetState GetBetState()
    {
        return betState;
    }
    public int  GetBetNum()
    {
        return betNum;
    }
    public void Bet1()
    {
        if(betNum+1<=betMax||betNow-1>=0)
        {
            betNum += 1;
            betNow -= 1;
        }
    }
    public void Bet10()
    {
        if (betNum + 10 <= betMax || betNow - 1 >= 0)
        {
            betNum += 10;
            betNow -= 10;
        }
    }
    public void Bet100()
    {
        if (betNum + 100 <= betMax || betNow - 1 >= 0)
        {
            betNum += 100;
            betNow -= 100;
        }
        
    }

}
