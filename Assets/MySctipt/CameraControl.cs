using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //[SerializeField]
    //private GameObject parent;

    public struct CameraParameter
    {
        public Vector3 position;
        public Quaternion angle;
        public void Init(Vector3 _pos,Quaternion _angle)
        {
            position = _pos;
            angle = _angle;
        }
    
    }

    public enum CameraPosition
    {
        FrontR,
        FrontL,
        Middle,
        Back,
    }

    enum CameraState
    {
        Moveing,
        Stay,
    
    }
    GameStageProgress progress;

    CameraPosition cameraPosition = CameraPosition.Back;

    [SerializeField]
    private GameObject mainCamera; //カメラ
    CameraParameter mainCameraPParameter;
    [SerializeField]
    private GameObject mainCameraDecoy; //カメラ
    [SerializeField]
    private GameObject middleCameraPosition; //第一のカメラ
    CameraParameter middleCameraPParameter;

    [SerializeField]
    private GameObject frontCameraPositionR; //第一のカメラ
    CameraParameter frontCameraPParameterR;

    [SerializeField]
    private GameObject frontCameraPositionL; //第二のカメラ
    CameraParameter frontCameraPParameterL;

    CameraParameter nowCameraParameter;
    CameraParameter aimCameraParameter;
    [SerializeField]float smoothSpeed = 3.0f;

    bool nowFlag = false;//nertral=false;

    private CameraState cameraState;


    private Vector3 touchStartPos;//画面タップ開始地点の座標
    private Vector3 touchNowPos;//現在の座標
    private bool isTouch;//タッチされているかどうか
    private string direction;//現在のタッチの状態を代入するstring

    private const int FLICKSIZE=3;
    // Start is called before the first frame update
    void Start()
    {
        cameraState = CameraState.Stay;
        //カメラの設定
        frontCameraPParameterR.Init(frontCameraPositionR.transform.position, frontCameraPositionR.transform.rotation);
        frontCameraPParameterL.Init(frontCameraPositionL.transform.position, frontCameraPositionL.transform.rotation);
        middleCameraPParameter.Init(middleCameraPosition.transform.position,middleCameraPosition.transform.rotation);

        mainCameraPParameter.Init(mainCamera.transform.position,mainCamera.transform.rotation);
        nowCameraParameter= mainCameraPParameter;
        aimCameraParameter = mainCameraPParameter;
    }

    // Update is called once per frame
    void Update()
    {
        switch (progress)
        {
            case GameStageProgress.Walking:
                aimCameraParameter = mainCameraPParameter;
                middleCameraPParameter.Init(middleCameraPosition.transform.position, middleCameraPosition.transform.rotation);
                break;
            case GameStageProgress.Thinking:
                nowCameraParameter = LerpCamera(nowCameraParameter, aimCameraParameter, smoothSpeed);
                mainCamera.transform.position = nowCameraParameter.position;
                mainCamera.transform.rotation = nowCameraParameter.angle;

                frontCameraPParameterR.Init(frontCameraPositionR.transform.position, frontCameraPositionR.transform.rotation);
                frontCameraPParameterL.Init(frontCameraPositionL.transform.position, frontCameraPositionL.transform.rotation);
                middleCameraPParameter.Init(middleCameraPosition.transform.position, middleCameraPosition.transform.rotation);
                mainCameraPParameter.Init(mainCameraDecoy.transform.position, mainCameraDecoy.transform.rotation);

                Flick();
                break;
            case GameStageProgress.Result:
                nowCameraParameter = LerpCamera(nowCameraParameter, aimCameraParameter, smoothSpeed);
                mainCamera.transform.position = nowCameraParameter.position;
                mainCamera.transform.rotation = nowCameraParameter.angle;

                frontCameraPParameterR.Init(frontCameraPositionR.transform.position, frontCameraPositionR.transform.rotation);
                frontCameraPParameterL.Init(frontCameraPositionL.transform.position, frontCameraPositionL.transform.rotation);
                middleCameraPParameter.Init(middleCameraPosition.transform.position, middleCameraPosition.transform.rotation);
                mainCameraPParameter.Init(mainCameraDecoy.transform.position, mainCameraDecoy.transform.rotation);
                break;
        
        }

       
        
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            aimCameraParameter = middleCameraPParameter;

            Debug.Log("↑");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            aimCameraParameter = frontCameraPParameterR;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            aimCameraParameter = frontCameraPParameterL;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            aimCameraParameter = mainCameraPParameter;
        }
        
    }
    
    //public void Chenge()
    //{
    //}

    public CameraParameter LerpCamera(CameraParameter _aimCamera, CameraParameter _mainCamera,float _t)
    {
        
        CameraParameter _ret;
        _ret.position = Vector3.Lerp(_aimCamera.position, _mainCamera.position, Time.deltaTime * _t);
        _ret.angle = Quaternion.Lerp(_aimCamera.angle, _mainCamera.angle, Time.deltaTime * _t);
        cameraState = CameraState.Moveing;

        return _ret;
    }
    public void SetProgress(GameStageProgress _progress)
    {
        progress = _progress;
    }
    public void SetFar()
    {
        aimCameraParameter = mainCameraPParameter;
    }
    public void SetMiddle()
    {
        aimCameraParameter = middleCameraPParameter;
    }
    public void SetFR()
    {
        if (progress == GameStageProgress.Thinking)
        {
            aimCameraParameter = frontCameraPParameterR;
        }

        
    }
    public void SetFL()
    {
        if (progress == GameStageProgress.Thinking)
        {
            aimCameraParameter = frontCameraPParameterL;
        }
        
    }
    void Flick()
    {
        //画面がタップされた時
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            isTouch = true;
        }
        //画面から指が離れた時
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //タッチを検出
            if (direction == "touch")
            {
            }
            isTouch = false;
        }

        //現在タッチされている場合
        if (isTouch)
        {
            touchNowPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            GetDirection();//座標からタッチ、フリックの状態を管理
        }
    }

    void GetDirection()
    {
        //現在の座標と開始地点の座標の差を代入
        float directionX = touchNowPos.x - touchStartPos.x;
        float directionY = touchNowPos.y - touchStartPos.y;

        //差の大きさによって条件分岐
        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (FLICKSIZE < directionX)
            {
                direction = "R";
                //右向きにフリック
                SetFL();
            }
            else if (-FLICKSIZE > directionX)
            {
                direction = "L";
                //左向きにフリック
                
                SetFR();
            }
        }
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (FLICKSIZE < directionY)
            {
                //上向きにフリック
                direction = "up";
                SetMiddle();
            }
            else if (-FLICKSIZE > directionY)
            {
                direction = "down";
                //下向きのフリック
                
            }
        }

        //フリック操作がなかった場合
        else
        {
            //タッチを検出
            direction = "touch";
        }
        if (direction == null || direction == "touch") { return; }

        isTouch = false;
    }
}
