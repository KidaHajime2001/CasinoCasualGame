using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayCardDeal : MonoBehaviour
{
    [SerializeField] private string firstStageFile;
    [SerializeField] private GameObject adventPosition;
    GameDataManager gameDataManager;
    GameData gameData;
    [SerializeField]
    private Playingcards plCards;

    private const float ADJUSTMENT_CARDPOS_Z = 0.1f;
    private const float ADJUSTMENT_CARDPOS_X = 1.5f;
    private const int ADJUSTMENT_CARD_SCALE = 20;
    private Quaternion ADJUSTMENT_CARD_QUATERNION_OBVERSE = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    private Quaternion ADJUSTMENT_CARD_QUATERNION_REVERSE = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    private float[] ADJUSTMENT_CARD_ANGLE= {45.0f,27.5f,0.0f,-27.5f,-45.0f };
    
    // Start is called before the first frame update
    void Start()
    {
        gameDataManager = this.AddComponent<GameDataManager>();
        gameData = gameDataManager.LoadGameData( gameDataManager.GetDataPath(firstStageFile));

        AdventPokerNeutral();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AdventPokerNeutral()
    {
        int i = 0;
        foreach(var cardData in gameData.cardData)
        {
            if(cardData.camp==CardCamp.Neutral)
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

                var pos = adventPosition.transform.position;
                pos.x += (ADJUSTMENT_CARDPOS_X * (i-2));
                pos.z += (ADJUSTMENT_CARDPOS_Z * i);
                Debug.Log(cardData.Number - 1);
                plCards.AdventCard(pos,q,cardData.suit,cardData.Number-1);
                i++;
            }
        }
    }
}
