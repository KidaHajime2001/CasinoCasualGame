using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonDataManager: MonoBehaviour
{
    // �t�@�C���p�X
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
        // 1�L�[�����Ō��݈ʒu���Z�[�u����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //OnSave();
        }

        // 2�L�[�����Ō��݈ʒu�����[�h����
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
        string jsonstr = JsonUtility.ToJson(data);//�󂯎����PlayerData��JSON�ɕϊ�
        StreamWriter writer = new StreamWriter(_dataPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
        writer.WriteLine(jsonstr);//JSON�f�[�^����������
        writer.Flush();//�o�b�t�@���N���A����
        writer.Close();//�t�@�C�����N���[�Y����
    }
    // JSON�`�������[�h���ăf�V���A���C�Y
    static public PlayerData LoadData(string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            Debug.Log("�t�@�C������������܂���ł���");
            PlayerData data = new PlayerData();
            string jsonstr = JsonUtility.ToJson(data);//�󂯎����PlayerData��JSON�ɕϊ�
            StreamWriter writer = new StreamWriter(dataPath, false);//���߂Ɏw�肵���f�[�^�̕ۑ�����J��
            writer.WriteLine(jsonstr);//JSON�f�[�^����������
            writer.Flush();//�o�b�t�@���N���A����
            writer.Close();//�t�@�C�����N���[�Y����
        }



        // JSON�f�[�^�Ƃ��ăf�[�^��ǂݍ���
        var json = File.ReadAllText(dataPath);
        
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
