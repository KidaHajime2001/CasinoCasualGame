using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

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

    CameraPosition cameraPosition = CameraPosition.Back;

    [SerializeField]
    private GameObject mainCamera; //カメラ

    [SerializeField]
    private GameObject middleCameraPosition; //第一のカメラ
    CameraParameter middleCameraPParameter;

    [SerializeField]
    private GameObject frontCameraPositionR; //第一のカメラ
    CameraParameter frontCameraPParameterR;

    [SerializeField]
    private GameObject frontCameraPositionL; //第二のカメラ
    CameraParameter frontCameraPParameterL;


    float smoothSpeed = 3.0f;

    bool nowFlag = false;//nertral=false;

    // Start is called before the first frame update
    void Start()
    {
        //カメラの設定
        frontCameraPParameterR.Init(frontCameraPositionR.transform.position, frontCameraPositionR.transform.rotation);
        frontCameraPParameterL.Init(frontCameraPositionL.transform.position, frontCameraPositionL.transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {



    }
    //public void Chenge()
    //{
    //}

    public CameraParameter LerpCamera(CameraParameter _aimCamera, CameraParameter _mainCamera,float _t,CameraParameter _ret)
    {
        _ret.position = Vector3.Lerp(_aimCamera.position, _mainCamera.position,Time.deltaTime * _t);
        _ret.angle = Quaternion.Lerp(_aimCamera.angle, _mainCamera.angle, Time.deltaTime * _t);
        return _ret;
    }
}
