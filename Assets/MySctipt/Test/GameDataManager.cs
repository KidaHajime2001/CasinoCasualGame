using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    enum CardCamp
    {
        Player,
        Enemy,
        Neutral,
    }

    enum CardState
    {
        Obverse,
        Reverse,
        Empty,
    }

    enum Suit
    {
        Spade,
        Diamond,
        Club,
        Heart,
    }

    enum GameType
    {
        Poker,
        BlackJack,
        Baccarat,
    }
    [System.Serializable]
    struct CardData
    {
        [SerializeField] private CardCamp camp;
        [SerializeField] private CardState state;
        [SerializeField] private Suit suit;
        [SerializeField] private int Number;
    }


    //GameType  :�Q�[���̎��     �i�|�[�J�[�Ȃǁj
    //CardData  :�J�[�h�f�[�^     5���Z�b�g�i�Q�[���ɂ���Ă͂T���g��Ȃ����߁h�f�[�^�����h���K�v�j
    //PulusChip :�������閇��     �����ɂ��{���ȊO�ł̑�������
    //Magnitude :�q���̔{��
    [System.Serializable]
    struct GameData
    {
        [SerializeField] private GameType gameType;
        [SerializeField] private List<CardData> cardData;
        [SerializeField] private int pulusChip;
        [SerializeField] private float magnitude;
        [SerializeField] private bool buttleScore;
        public void Init()
        {
            pulusChip = 0;
            magnitude = 0;
        }

    }
    [System.Serializable]
    struct GameWave
    {
        [SerializeField] private GameData gameDataL;

        [SerializeField] private GameData gameDataR;


    }


    [SerializeField]
    private  List<GameWave> waves;
    private List<GameWave> _data;
    // �t�@�C���p�X
    private string _dataPath;

    private void Awake()
    {
        _dataPath = Application.persistentDataPath + "/GameWaveData1.json";
        Debug.Log(Application.persistentDataPath);
        _data = LoadData(_dataPath);
        SaveData(waves);
        //Debug.Log(waves[0]);
    }


    private void SaveData(List<GameWave> data)
    {
        string jsonstr = JsonUtility.ToJson(data);//�󂯎����PlayerData��JSON�ɕϊ�
        StreamWriter writer = new StreamWriter(_dataPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
        writer.WriteLine(jsonstr);//JSON�f�[�^����������
        writer.Flush();//�o�b�t�@���N���A����
        writer.Close();//�t�@�C�����N���[�Y����
    }
    // JSON�`�������[�h���ăf�V���A���C�Y
    private List<GameWave> LoadData(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("�t�@�C������������܂���ł���");
            List<GameWave> data = new List<GameWave>();
            
            data = waves;
            string jsonstr = JsonUtility.ToJson(data);//�󂯎����PlayerData��JSON�ɕϊ�
            StreamWriter writer = new StreamWriter(dataPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
            writer.WriteLine(jsonstr);//JSON�f�[�^����������
            writer.Flush();//�o�b�t�@���N���A����
            writer.Close();//�t�@�C�����N���[�Y����
            Debug.Log(data);
        }

        

        // JSON�f�[�^�Ƃ��ăf�[�^��ǂݍ���
        var json = File.ReadAllText(dataPath);

        return JsonUtility.FromJson<List<GameWave>>(json);
    }

}
