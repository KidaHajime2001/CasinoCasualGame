using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Newtonsoft.Json;
public class GameDataManager : MonoBehaviour
{
   // [SerializeField]
   //private GameData Data;
    public GameData gameData;

    public GameObject inputFieldObj;
    TMP_InputField inputField;

    // ファイルパス
    private string _dataPath;
    private string _InputPath;
    private void Awake()
    {
        if(inputFieldObj!=null)
        {
            inputField = inputFieldObj.GetComponent<TMP_InputField>();
        }
        
        _dataPath = Application.persistentDataPath + "/GameWaveData1.json";

        gameData = LoadGameData(_dataPath);
        //Debug.Log(Data);
    }
    public void InputText()
    {
      
    }
    public void CreateGameData()
    {
        Debug.Log(inputField.text);
        _InputPath = Application.dataPath + "/"+inputField.text+".json";
        Debug.Log("CreateData:" + _InputPath);
        LoadGameData(_InputPath);
        SaveGameData(gameData,_InputPath);
        Debug.Log("保存しました。");
    }


    private void SaveGameData(GameData data,string _inputPath)
    {
        string jsonstr = JsonConvert.SerializeObject(data);//受け取ったPlayerDataをJSONに変換
        StreamWriter writer = new StreamWriter(_inputPath, false);//初めに指定したデータの保存先を開く
        writer.WriteLine(jsonstr);//JSONデータを書き込み
        writer.Flush();//バッファをクリアする
        writer.Close();//ファイルをクローズする
    }
    // JSON形式をロードしてデシリアライズ

    public GameData LoadGameData(string dataPath)
    {
        Debug.Log("Loading..."+dataPath);

        if (!File.Exists(dataPath))
        {
            Debug.Log("ファイルが見当たりませんでした");
            GameData data = new GameData();
            string jsonstr = JsonConvert.SerializeObject(data);//受け取ったPlayerDataをJSONに変換
            StreamWriter writer = new StreamWriter(dataPath, false);//初めに指定したデータの保存先を開く
            writer.WriteLine(jsonstr);//JSONデータを書き込み
            writer.Flush();//バッファをクリアする
            writer.Close();//ファイルをクローズする
            Debug.Log(data);
        }
        // JSONデータとしてデータを読み込む
        var json = File.ReadAllText(dataPath);

        //return JsonUtility.FromJson<List<GameWave>>(json);
        return JsonConvert.DeserializeObject<GameData>(json);
       
    }
    public string GetDataPath(string _fileName)
    {
        return Application.streamingAssetsPath+"/"+ _fileName + ".json";
    }


    public string GetGameName(GameType _gameType)
    {
        switch (_gameType)
        {
            case GameType.Poker:
                return "Poker";
            case GameType.BlackJack:
                return "BlackJack";
            case GameType.Baccarat:
                return "Baccarat";
            default:
                return "null";
        }
    }
    public string GetScoreTextData(float _rate)
    {

        return _rate.ToString();
    }

}

