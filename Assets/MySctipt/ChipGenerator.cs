using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChipGenerator : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI text;
    enum ChipRate //チップのレート
    {
        low,
        medium,
        high,
        
    }
    [SerializeField] private bool onSwitch;
    [SerializeField] private List<GameObject> spawnPoint = new List<GameObject>();

    private PlayerData playerData;


    [SerializeField] private List<GameObject> chipObj = new List<GameObject>();   //チップのプレハブ
    private PoolManager[] poolManager;//3個のチップ用オブジェクトプール

    private const int POOL_SIZE = 100; //チップの限度数

    //debug
    WaitForSeconds waitForSeconds = new WaitForSeconds(0.3f);
    private int chipNum = 300;
    private int beforeChipNum;
    private int lowNum = 0;
    private int midiumNum = 0;
    private int highNum = 0;

    private const int CHIP_HIGH_RATE= 100;
    private const int CHIP_MIDIUM_RATE= 10;
    private const int CHIP_LOW_RATE= 1;

    [SerializeField]
    private GameObject scoreTextObj;
    private ScoreText scoreText;


    Dictionary<ChipRate, int> RateDic;
    Dictionary<ChipRate, int> RateDicNow;

    public int ChipNum()
    {
        return chipNum;
    }


    // Start is called before the first frame update
    void Start()
    {
        scoreText = scoreTextObj.GetComponent<ScoreText>();

        RateDic = new Dictionary<ChipRate, int>();
        RateDicNow = new Dictionary<ChipRate, int>();
        playerData = JsonDataManager.LoadData(JsonDataManager.GetPath());
        //ChipNumberCalculation((int)playerData._chipNum) ;
        ChipNumberCalculation(100) ;
        SetRateDisc(); 




        foreach (var rate in Enum.GetValues(typeof(ChipRate)))
        {
            this.gameObject.AddComponent<PoolManager>();
        }
        poolManager = GetComponents<PoolManager>();

        //チップのレートの数オブジェクトプールを作成
        foreach (var rate in Enum.GetValues(typeof(ChipRate)))
        {
            poolManager[(int)rate].EntryPrefab(chipObj[(int)rate]);
            poolManager[(int)rate].CreateObjectPool(POOL_SIZE);
        }
        if(onSwitch)
        {
            foreach (ChipRate rate in Enum.GetValues(typeof(ChipRate)))
            {
                for (int i = 0; i < RateDic[rate]; i++)
                {
                    poolManager[(int)rate].SpawnGameObject(spawnPoint[(int)rate].transform.position, spawnPoint[(int)rate].transform.rotation);
                }
                
            }
            ////debug
            //StartCoroutine(DebugCoroutine());
        }
        
    }

    IEnumerator DebugCoroutine()
    {
        while (true)
        {
            
            foreach (var rate in Enum.GetValues(typeof(ChipRate)))
            {

                poolManager[(int)rate].GetObj();
            }
                yield return waitForSeconds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = chipNum.ToString();
        // 1キー押下で現在位置をセーブする
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int i = UnityEngine.Random.Range(0, 10);
            Debug.Log(i);
        }

        // 2キー押下で現在位置をロードする
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int i = UnityEngine.Random.Range(0, 10);
            Debug.Log(i);
            chipNum-= i;
            
        }

        


        ChipNumberCalculation(chipNum);

        foreach (ChipRate rate in Enum.GetValues(typeof(ChipRate)))
        {
            int sub = RateDicNow[rate]-RateDic[rate];
            if (sub>=0)
            {
                for (int i = 0; i < sub; i++)
                {

                    //Debug.Log(RateDicNow[rate]);
                    //Debug.Log(RateDic[rate]);
                    //Debug.Log("sub:"+sub);
                    poolManager[(int)rate].SpawnGameObject(spawnPoint[(int)rate].transform.position, spawnPoint[(int)rate].transform.rotation);
                }
            }
            else
            {
                sub *= -1;
                for (int i = 0; i < sub; i++)
                {
                    poolManager[(int)rate].ReleaseObj();
                }
            }
            

        }
        scoreText.GetChipData((int)chipNum);
        
        //playerData._chipNum = (uint)chipNum;
        //JsonDataManager.SaveData(playerData);
        beforeChipNum = chipNum;
        SetRateDisc();
    }

    private void ChipNumberCalculation(int _chipMaxNumber)
    {
        //Debug.Log(_chipMaxNumber);
        int surplus = 0;
        highNum = _chipMaxNumber / CHIP_HIGH_RATE;
        surplus = _chipMaxNumber % CHIP_HIGH_RATE;
        midiumNum = surplus / CHIP_MIDIUM_RATE;
        surplus = surplus % CHIP_MIDIUM_RATE;
        lowNum = surplus / CHIP_LOW_RATE;

        SetRateDiscNow(highNum*10, midiumNum*10, lowNum*3);
    }
    private void SetRateDisc()
    {
        RateDic[ChipRate.high] = RateDicNow[ChipRate.high];
        RateDic[ChipRate.medium] = RateDicNow[ChipRate.medium];
        RateDic[ChipRate.low] = RateDicNow[ChipRate.low];
    }
    private void SetRateDiscNow(int h,int m,int l)
    {
        RateDicNow[ChipRate.high] = h;
        RateDicNow[ChipRate.medium] = m;
        RateDicNow[ChipRate.low] = l;
    }

    public int GetChipNum()
    {
        return chipNum;
    }
    public void SetChip(int _num)
    {
        chipNum = _num;
    }
    //public void AddChipLow()
    //{
    //    chipNum += CHIP_LOW_RATE;
    //    Debug.Log(chipNum);
    //}
    //public void AddChipMid()
    //{
    //    chipNum += CHIP_MIDIUM_RATE;
    //}
    //public void AddChipHigh()
    //{
    //    chipNum += CHIP_HIGH_RATE;
    //}
}
