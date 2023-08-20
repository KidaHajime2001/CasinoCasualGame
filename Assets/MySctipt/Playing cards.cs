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
    enum Suit
    { 
        spade,
        herat,
        diamond,
        club,
    }

    //トランプのデータ
    struct PlayCardData
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

    // Start is called before the first frame update
    void Start()
    { 



        for(int i=0; i<13; i++)
        {
            spadeList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            playCardDataS[i].Init(Suit.spade,i,spadeList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            heartList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            playCardDataH[i].Init(Suit.herat, i, heartList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            diamondList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            playCardDataD[i].Init(Suit.diamond, i, diamondList[i]);
        };

        for (int i = 0; i < 13; i++)
        {
            clubList[i].transform.localScale = new Vector3(ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE, ADJUSTMENT_CARD_SCALE);
            playCardDataC[i].Init(Suit.club, i, clubList[i]);
        };

      
        SummonPoker(playCardDataS[1], playCardDataS[1], playCardDataS[1], playCardDataS[1], playCardDataS[1],new Vector3(0,10,0));
    }

    // Update is called once per frame
    void Update()
    {
    }

     void SummonPoker(PlayCardData card1, PlayCardData card2, PlayCardData card3, PlayCardData card4, PlayCardData card5,Vector3 position)
    {
        Instantiate(card3.GetGameObject(), position, ADJUSTMENT_CARD_QUATERNION);
        Instantiate(card1.GetGameObject(), new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * 2), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * 2)), ADJUSTMENT_CARD_QUATERNION);
        Instantiate(card2.GetGameObject(), new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * 1), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * 1)), ADJUSTMENT_CARD_QUATERNION);
        Instantiate(card4.GetGameObject(), new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * -1), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * -1)), ADJUSTMENT_CARD_QUATERNION);
        Instantiate(card5.GetGameObject(), new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * -2), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * -2)), ADJUSTMENT_CARD_QUATERNION);

        Debug.Log("生成");

        //card1.GetGameObject().transform.position = new Vector3(position.x + (ADJUSTMENT_CARDPOS_X*2) ,position.y,position.z+(ADJUSTMENT_CARDPOS_Z*2));
        //card2.GetGameObject().transform.position = new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * 1), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * 1));
        //card4.GetGameObject().transform.position = new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * -1), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * -1));
        //card5.GetGameObject().transform.position = new Vector3(position.x + (ADJUSTMENT_CARDPOS_X * -2), position.y, position.z + (ADJUSTMENT_CARDPOS_Z * -2));
    }
}
