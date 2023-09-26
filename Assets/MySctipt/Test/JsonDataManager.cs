using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonDataManager: MonoBehaviour
{
    // ファイルパス
    private static string _dataPath;
    public PlayerData _data;
    private void Awake()
    {
        _dataPath = Application.persistentDataPath + "/PlayerChipData.json";

        _data = LoadData(_dataPath);

        if(!_data.GetFirstPlayFlag())
        {
            _data.InitData();
            Debug.Log(_data._chipNum);
        }
        Debug.Log("FirstPlay:"+_data.GetFirstPlayFlag());

    }

    private void Update()
    {
        // 1キー押下で現在位置をセーブする
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //OnSave();
        }

        // 2キー押下で現在位置をロードする
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //OnLoad();
        }
    }
    static public string GetPath()
    {
        return _dataPath;
    }

    static public void SaveData(PlayerData data)
    {
        string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
        StreamWriter writer = new StreamWriter(_dataPath, false);//初めに指定したデータの保存先を開く
        writer.WriteLine(jsonstr);//JSONデータを書き込み
        writer.Flush();//バッファをクリアする
        writer.Close();//ファイルをクローズする
    }
    // JSON形式をロードしてデシリアライズ
    static public PlayerData LoadData(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("ファイルが見当たりませんでした");
            PlayerData data = new PlayerData();
            string jsonstr = JsonUtility.ToJson(data);//受け取ったPlayerDataをJSONに変換
            StreamWriter writer = new StreamWriter(dataPath, false);//初めに指定したデータの保存先を開く
            writer.WriteLine(jsonstr);//JSONデータを書き込み
            writer.Flush();//バッファをクリアする
            writer.Close();//ファイルをクローズする
        }



        // JSONデータとしてデータを読み込む
        var json = File.ReadAllText(dataPath);
        
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
