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

    [SerializeField] TextMeshProUGUI debugText;

    // �t�@�C���p�X
    private string _dataPath;
    private string _InputPath;
    private void Awake()
    {
        if(inputFieldObj!=null)
        {
            inputField = inputFieldObj.GetComponent<TMP_InputField>();
        }
        
        _dataPath = Application.persistentDataPath + "/GameWaveData1.json";

        //gameData = LoadGameData(_dataPath);
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
        Debug.Log("�ۑ����܂����B");
    }


    private void SaveGameData(GameData data,string _inputPath)
    {
        string jsonstr = JsonConvert.SerializeObject(data);//�󂯎����PlayerData��JSON�ɕϊ�
        StreamWriter writer = new StreamWriter(_inputPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
        writer.WriteLine(jsonstr);//JSON�f�[�^����������
        writer.Flush();//�o�b�t�@���N���A����
        writer.Close();//�t�@�C�����N���[�Y����
    }
    // JSON�`�������[�h���ăf�V���A���C�Y

    public GameData LoadGameData(string dataPath)
    {
        UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(dataPath);
        www.SendWebRequest();
        while (!www.isDone)
        {
        }
        //return www.downloadHandler.text;
        //Debug.Log("Loading..."+dataPath);

        ////// �f�o�b�O�p ���̊֐����s��ꂽ���Ƃ�����
        ////this.debugText.text += "LoadGameData() : ";

        //if (!File.Exists(dataPath))
        //{
        //    Debug.Log("�t�@�C������������܂���ł���");
        //    GameData data = new GameData();
        //    string jsonstr = JsonConvert.SerializeObject(data);//�󂯎����PlayerData��JSON�ɕϊ�
        //    StreamWriter writer = new StreamWriter(dataPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
        //    writer.WriteLine(jsonstr);//JSON�f�[�^����������
        //    writer.Flush();//�o�b�t�@���N���A����
        //    writer.Close();//�t�@�C�����N���[�Y����
        //    Debug.Log(data);

        //    // �f�o�b�O�p ���̊֐������s�������Ƃ�����
        //    this.debugText.text += "���s\n";
        //}
        // JSON�f�[�^�Ƃ��ăf�[�^��ǂݍ���
        // var json = File.ReadAllText(dataPath);
        var json = www.downloadHandler.text;
        // �f�o�b�O�p ���̊֐��������������Ƃ������B�f�[�^���m�F���邽�߂ɁApulusChip�̒l��\��������
        //this.debugText.text += JsonConvert.DeserializeObject<GameData>(json).pulusChip.ToString() + "\n";

        //return JsonUtility.FromJson<List<GameWave>>(json);
        return JsonConvert.DeserializeObject<GameData>(json);
       
    }
    public string GetDataPath(string _fileName)
    {
        _fileName = _fileName  +".json";
        //Android�̏ꍇ
        if (Application.platform == RuntimePlatform.Android)
        {
            _fileName = "jar:file://" + Application.dataPath + "!/assets" + "/" + _fileName;
        }
        //Unity Editor�AWindows�ALinux�v���C���[�APS4�AXbox One�ASwitch
        else
        {
            _fileName = Application.streamingAssetsPath + "/" + _fileName;
        }
        return _fileName;
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

