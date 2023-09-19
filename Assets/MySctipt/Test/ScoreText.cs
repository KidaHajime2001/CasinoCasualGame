using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private PlayerData playerData;
    private int chipData; 
    // ������
    private void Start()
    {
        playerData = JsonDataManager.LoadData(JsonDataManager.GetPath());
    }

    // �X�V
    void Update()
    {

        TextMeshProUGUI _text = text.GetComponent<TextMeshProUGUI>();
        if (_text)
        {
        //    playerData = JsonDataManager.LoadData(JsonDataManager.GetPath());
            // �e�L�X�g�̕\�������ւ���
            _text.text = chipData.ToString();
        }
            
    }
    public void GetChipData(int _chipData)
    {
        chipData = _chipData;
    }
}