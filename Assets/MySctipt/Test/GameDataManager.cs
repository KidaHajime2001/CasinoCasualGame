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
    [SerializeField]
    private GameData Data;
    private GameData gameData;

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
        
        Debug.Log(inputField);
        _dataPath = Application.persistentDataPath + "/GameWaveData1.json";

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


    private void SaveGameData(GameData data)
    {
        string jsonstr = JsonConvert.SerializeObject(data);//受け取ったPlayerDataをJSONに変換
        StreamWriter writer = new StreamWriter(_dataPath, false);//初めに指定したデータの保存先を開く
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
        return Application.dataPath + "/JsonFile/" + _fileName + ".json";
    }
}

