using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private PlayerData playerData;
    // 初期化
    private void Start()
    {
        playerData = JsonDataManager.LoadData(JsonDataManager.GetPath());
    }

    // 更新
    void Update()
    {

        TextMeshProUGUI _text = text.GetComponent<TextMeshProUGUI>();
        if (_text)
        {
            playerData = JsonDataManager.LoadData(JsonDataManager.GetPath());
            // テキストの表示を入れ替える
            _text.text = playerData._chipNum.ToString();
        }
            
    }
}
