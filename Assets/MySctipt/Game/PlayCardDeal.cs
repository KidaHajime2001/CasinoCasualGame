using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayCardDeal : MonoBehaviour
{
    [SerializeField] private string firstStageFile;
    [SerializeField] private GameObject adventPositionNeutral;
    [SerializeField] private GameObject adventPositionPlayer;
    [SerializeField] private GameObject adventPositionEnemy;
    [SerializeField] private GameObject gameNameText;
    TextMeshProUGUI nameText;
    private List<GameObject> reverseList;
    GameDataManager gameDataManager;
    GameData gameData;
    [SerializeField]
    private Playingcards plCards;

    private const float ADJUSTMENT_CARDPOS_Z = 0.1f;
    private const float ADJUSTMENT_CARDPOS_X = 1.5f;
    private const int ADJUSTMENT_CARD_SCALE = 20;
    private Quaternion ADJUSTMENT_CARD_QUATERNION_OBVERSE = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    private Quaternion ADJUSTMENT_CARD_QUATERNION_REVERSE = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    private float[] ADJUSTMENT_CARD_ANGLE= {45.0f,27.5f,0.0f,-27.5f,-45.0f };

    Dictionary<CardCamp, Vector3> campPosDic;

    float rTime=1.0f;
    float f = 0;
    bool rF=false;
    // Start is called before the first frame update
    void Start()
    {
        reverseList = new List<GameObject>();
        campPosDic = new Dictionary<CardCamp, Vector3>();
        campPosDic[CardCamp.Player]=adventPositionPlayer.transform.position;
        campPosDic[CardCamp.Enemy] =adventPositionEnemy.transform.position;
        campPosDic[CardCamp.Neutral]=adventPositionNeutral.transform.position;


        gameDataManager = this.GetComponent<GameDataManager>();
        gameData = gameDataManager.LoadGameData( gameDataManager.GetDataPath(firstStageFile));
        
        foreach(CardCamp _camp in Enum.GetValues(typeof(CardCamp)))
        {
            AdventPokerCard(_camp);
            Debug.Log(_camp);
        }

        nameText=gameNameText.GetComponent<TextMeshProUGUI>();
        nameText.text  = gameDataManager.GetGameName( gameData.gameType) +"\n" +"Å~" + gameDataManager.GetScoreTextData(gameData.magnitude);
        Debug.Log(gameObject.name+":"+ gameData.buttleScore);
    }

    // Update is called once per frame
    void Update()
    {
        if(rF)
        {
            f += Time.deltaTime;

            foreach (var obj in reverseList)
            {
                obj.transform.rotation = Quaternion.Lerp(ADJUSTMENT_CARD_QUATERNION_REVERSE, ADJUSTMENT_CARD_QUATERNION_OBVERSE, f / rTime);
            }
        }
        
    }
    public void AdventPokerCard(CardCamp _camp)
    {
        int i = 0;
        foreach(var cardData in gameData.cardData)
        {
            if(cardData.camp==_camp)
            {
                Quaternion q= Quaternion.Euler(0.0f, 0.0f, 0.0f); ;
                if (cardData.state==CardState.Obverse)
                {
                    q = ADJUSTMENT_CARD_QUATERNION_OBVERSE;
                }
                else if(cardData.state == CardState.Reverse)
                {
                    q = ADJUSTMENT_CARD_QUATERNION_REVERSE;
                }
                //q.x = ADJUSTMENT_CARD_ANGLE[i];

                var pos = campPosDic[_camp];
                if(_camp==CardCamp.Neutral)
                {
                    pos.x += (ADJUSTMENT_CARDPOS_X * (i - 2));
                }
                else
                {
                    pos.x += (ADJUSTMENT_CARDPOS_X * (i));
                }
                
                pos.z += (ADJUSTMENT_CARDPOS_Z * i);
                var obj=plCards.AdventCard(pos,q,cardData.suit,cardData.Number-1);
                if (cardData.state == CardState.Reverse)
                {
                    reverseList.Add( obj);
                }

                i++;
            }
        }
    }
    public bool GetResult()
    {
        return gameData.buttleScore;
    }
    public float GetMagnitude()
    {
        return gameData.magnitude;
    }
    public int GetPulus()
    {
        return gameData.pulusChip;
    }
    public void ReverseCard()
    {

        rF = true;

    }
    public bool GetReverseComplete()
    {
        if(Quaternion.Angle(reverseList[0].transform.rotation,ADJUSTMENT_CARD_QUATERNION_OBVERSE)<=3.0f)
        {
            return true;
        }
        return false;
    }

}
