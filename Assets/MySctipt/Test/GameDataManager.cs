using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

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


    //GameType  :ゲームの種類     （ポーカーなど）
    //CardData  :カードデータ     5枚セット（ゲームによっては５枚使わないため”データ無し”が必要）
    //PulusChip :増減する枚数     勝利による倍率以外での増加枚数
    //Magnitude :賭けの倍率
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
    [SerializeField]
    private GameData Data;
    private GameData gameData;

    [SerializeField]GameObject inputFieldObj;
    TMP_InputField inputField;
    [SerializeField]
    private GameObject outputFieldObj;

    // ファイルパス
    private string _dataPath;
    private string _InputPath;
    private void Awake()
    {
        inputField = inputFieldObj.GetComponent<TMP_InputField>();
        Debug.Log(inputField);
        _dataPath = Application.persistentDataPath + "/GameWaveData1.json";
        Debug.Log(Application.persistentDataPath);

        gameData = LoadGameData(_dataPath);
        
        SaveGameData(Data);
        Debug.Log(Data);
    }
    public void InputText()
    {
      
    }
    public void CreateGameData()
    {
        Debug.Log(inputField.text);
        _InputPath = Application.dataPath + "/JsonFile/"+inputField.text+".json";
        Debug.Log("CreateData:" + _InputPath);
        gameData=LoadGameData(_InputPath);
        SaveGameData(gameData);

    }

    private void SaveData(List<GameWave> data)
    {
        string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
        StreamWriter writer = new StreamWriter(_dataPath, false);//初めに指定したデータの保存先を開く
        writer.WriteLine(jsonstr);//JSONデータを書き込み
        writer.Flush();//バッファをクリアする
        writer.Close();//ファイルをクローズする
    }
    private void SaveGameData(GameData data)
    {
        string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
        StreamWriter writer = new StreamWriter(_dataPath, false);//初めに指定したデータの保存先を開く
        writer.WriteLine(jsonstr);//JSONデータを書き込み
        writer.Flush();//バッファをクリアする
        writer.Close();//ファイルをクローズする
    }
    // JSON形式をロードしてデシリアライズ
    private List<GameWave> LoadData(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("ファイルが見当たりませんでした");
            List<GameWave> data = new List<GameWave>();

            data = waves;
            string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
            StreamWriter writer = new StreamWriter(dataPath, false);//初めに指定したデータの保存先を開く
            writer.WriteLine(jsonstr);//JSONデータを書き込み
            writer.Flush();//バッファをクリアする
            writer.Close();//ファイルをクローズする
            Debug.Log(data);
        }
        // JSONデータとしてデータを読み込む
        var json = File.ReadAllText(dataPath);

        return JsonUtility.FromJson<List<GameWave>>(json);
    }
    private GameData LoadGameData(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("ファイルが見当たりませんでした");
            GameData data = new GameData();
            data = Data;
            string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
            StreamWriter writer = new StreamWriter(dataPath, false);//初めに指定したデータの保存先を開く
            writer.WriteLine(jsonstr);//JSONデータを書き込み
            writer.Flush();//バッファをクリアする
            writer.Close();//ファイルをクローズする
            Debug.Log(data);
        }
        // JSONデータとしてデータを読み込む
        var json = File.ReadAllText(dataPath);

        //return JsonUtility.FromJson<List<GameWave>>(json);
        return JsonUtility.FromJson<GameData>(json);
    }
}

