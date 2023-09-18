using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameSceneControl : MonoBehaviour
{
    struct CheckPoint
    {
        public Vector3 Position;
        public bool NextFlag;
        public void Init(Vector3 vec)
        {
            Position = vec;
            NextFlag = false;
        }
    }

    [SerializeField] List<GameObject> objPoint;
    private CheckPoint[] checkPoints;
    private int nowWave;
    [SerializeField] private GameObject player;
    private float waveLerpValue=0.0f;
    private Vector3 nowAimPosition;
    [SerializeField]
    private float walkSpeed;
    
    private void Start()
    {
        nowWave = 0;
        checkPoints = new CheckPoint[objPoint.Count];
        for(int i=0; i<objPoint.Count;i++)
        {
            checkPoints[i].Position = objPoint[i].transform.position;
        }
        player.transform.position = checkPoints[0].Position;
    }
    public void StartGame()
    {
        
        
    }

    private void Update()
    {
        nowAimPosition = checkPoints[nowWave].Position;
        player.transform.position = Vector3.Lerp(player.transform.position, nowAimPosition, Time.deltaTime * walkSpeed);



    }

    public void NextPosition()
    {
        checkPoints[nowWave].NextFlag = true;
        if (nowWave+1<checkPoints.Count())
        {
            nowWave++;
        }
        
    }
}
