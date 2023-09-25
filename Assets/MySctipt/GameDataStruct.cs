using System.Collections.Generic;
using UnityEngine;

public enum CardCamp
{
    Player,
    Enemy,
    Neutral,
}

public enum CardState
{
    Obverse,
    Reverse,
    Empty,
}

public enum Suit
{
    Spade,
    Diamond,
    Club,
    Heart,
}

public enum GameType
{
    Poker,
    BlackJack,
    Baccarat,
}
[System.Serializable]
public struct CardData
{
    public CardCamp camp;
    public CardState state;
    public Suit suit;
    public int Number;
}

[System.Serializable]
//GameType  :ゲームの種類     （ポーカーなど）
//CardData  :カードデータ     5枚セット（ゲームによっては５枚使わないため”データ無し”が必要）
//PulusChip :増減する枚数     勝利による倍率以外での増加枚数
//Magnitude :賭けの倍率
public struct GameData
{
     public GameType gameType;
     public List<CardData> cardData;
     public int pulusChip;
     public float magnitude;
     public bool buttleScore;
    public void Init()
    {
        pulusChip = 0;
        magnitude = 0;
    }

}

public enum GameStageProgress
{
    Walking,
    Thinking,
    Result,
    Ending,
}
public enum BetState
{
    R,
    L,
    N,
}