using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class Bet : MonoBehaviour
{
    [SerializeField] GameObject chipImg;
    [SerializeField] GameObject chipImgPosL;
    [SerializeField] GameObject chipImgPosR;

    [SerializeField] GameObject chipImgPosLSecond;
    [SerializeField] GameObject chipImgPosRSecond;
    [SerializeField] TextMeshProUGUI text;

    private Vector3 nowChipPosL;
    private Vector3 nowChipPosR;
    BetState betState;
    private int betNum = 0;
    private int betSum = 0;
    private int betMax = 0;
    [SerializeField] ChipGenerator chipGenerator;



    // Start is called before the first frame update
    void Start()
    {
        
        nowChipPosL =   chipImgPosL.transform.position;
        nowChipPosR = chipImgPosR.transform.position;
        betState = BetState.N;

        betMax=chipGenerator.GetChipNum();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = betNum.ToString();
        if (betState == BetState.R)
        {
            chipImg.transform.position =nowChipPosR;
        }
        else if(betState == BetState.L)
        {
            chipImg.transform.position = nowChipPosL;
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

    public void ResetBetState()
    {
        betState=BetState.N;
        nowChipPosL = chipImgPosLSecond.transform.position;
        nowChipPosR = chipImgPosRSecond.transform.position;
    }
    public void Bet1()
    {
        
        betNum += 1;
        if (betMax <= betNum)
        {
            betNum = betMax;
        }
    }
    public void Bet10()
    {
        betNum += 10;

        if (betMax <= betNum)
        {
            betNum = betMax;
        }
    }
    public void Bet100()
    {
        betNum += 100;

        if (betMax <= betNum)
        {
            betNum = betMax;
        }
    }

}
