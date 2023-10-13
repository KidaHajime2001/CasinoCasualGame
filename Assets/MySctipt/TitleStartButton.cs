using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class TitleStartButton : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.color=new Color(text.color.r, text.color.g, text.color.b, Mathf.PingPong(Time.time,2)) ;
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("FirstScene");
    }
    public void RetrunTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
