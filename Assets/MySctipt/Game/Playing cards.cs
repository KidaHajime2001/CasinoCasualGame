using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playingcards : MonoBehaviour
{
    //各トランプオブジェクト

    [SerializeField] List<GameObject> spadeList      = new List<GameObject>();
    [SerializeField] List<GameObject> heartList      = new List<GameObject>();
    [SerializeField] List<GameObject> diamondList    = new List<GameObject>();
    [SerializeField] List<GameObject> clubList       = new List<GameObject>();

    //トランプのマーク


    //トランプのデータ
    public struct PlayCardData
    {
        Suit _suit;//マーク
        int _number;//数字
        GameObject _object; //モデル
        public void Init(Suit suit, int number,GameObject @object)
        {
            _suit = suit;
            _number = number;
            _object = @object;
        }
        public Suit GetSuit()
        {
            return _suit;
        }
        public int GetNumber()
        {
            return _number;
        }
        public GameObject GetGameObject()
        {
            return _object;
        }
    }

    private PlayCardData[] playCardDataS = new PlayCardData[13];
    private PlayCardData[] playCardDataH = new PlayCardData[13];
    private PlayCardData[] playCardDataD = new PlayCardData[13];
    private PlayCardData[] playCardDataC = new PlayCardData[13];

    private const float ADJUSTMENT_CARDPOS_Z = 0.1f;
    private const float ADJUSTMENT_CARDPOS_X = 1.5f;
    private const int ADJUSTMENT_CARD_SCALE = 20;
    private  Quaternion ADJUSTMENT_CARD_QUATERNION =  Quaternion.Euler(0.0f, 180.0f, 0.0f);

    Dictionary<Suit, PlayCardData[]> dataDic;
    // Start is called before the first frame update
    void Start()
    {
        dataDic=new Dictionary<Suit, PlayCardData[]>();
        dataDic[Suit.Spade]=playCardDataS;
        dataDic[Suit.Heart] = playCardDataH;
        dataDic[Suit.Diamond] = playCardDataD;
        dataDic[Suit.Club] = playCardDataC;


        for (int i=0; i<13; i++)
        {
            spadeList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            
            var rig =spadeList[i].GetComponent<Rigidbody>();
            if(rig)
            {
                rig.useGravity = false;
            }

            playCardDataS[i].Init(Suit.Spade,i,spadeList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            heartList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            var rig = heartList[i].GetComponent<Rigidbody>();
            if (rig)
            {
                rig.useGravity = false;
            }
            playCardDataH[i].Init(Suit.Heart, i, heartList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            diamondList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            var rig = diamondList[i].GetComponent<Rigidbody>();
            if (rig)
            {
                rig.useGravity = false;
            }
            playCardDataD[i].Init(Suit.Diamond, i, diamondList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            clubList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            var rig = clubList[i].GetComponent<Rigidbody>();
            if (rig)
            {
                rig.useGravity = false;
            }
            playCardDataC[i].Init(Suit.Club, i, clubList[i]);
        };

      //ポーカー（5枚）分を生成
    
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AdventCard(Vector3 _position,Quaternion _rotate,Suit _suit,int _number)
    {
       
        Instantiate(dataDic[_suit][_number].GetGameObject(), _position,_rotate);

            dataDic[_suit][_number].GetGameObject().GetComponent<BoxCollider>().enabled = false;
    }
}
