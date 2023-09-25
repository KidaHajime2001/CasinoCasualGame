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
//GameType  :�Q�[���̎��     �i�|�[�J�[�Ȃǁj
//CardData  :�J�[�h�f�[�^     5���Z�b�g�i�Q�[���ɂ���Ă͂T���g��Ȃ����߁h�f�[�^�����h���K�v�j
//PulusChip :�������閇��     �����ɂ��{���ȊO�ł̑�������
//Magnitude :�q���̔{��
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