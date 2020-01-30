using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFlyerShipMovement : MonoBehaviour
{

    //카메라와 비행선간의 거리
    [SerializeField] private float _distanceFromCamera = 75f;

    //비행선의 움직임을 감쇠 시키는데 사용되는 계수
    private const float expDampingCoef = -20f;

    //타겟 마커가 수평을 유지하게 하는 회전 계수
    private const float bankingCeof = 3f;

    //비행기의 움직임이 적용되는 Damping 크기
    [SerializeField] private float _damping = 0.5f;

    //비행선 시작 위치
    private Vector3 _flyerShipStartPos;

    //비행선 시작 회전
    private Quaternion _flyerShipStartRot;

    //진행 방향을 나타내는 타겟 마커
    public Transform _targetMarker;

    //카메라 컨테이너
    public Transform _cameraContainer;

    //타겟 마커 시작 위치
    private Vector3 _targetMakerStartPos;

    //타겟 마커 시작 회전
    private Quaternion _targetMakerStartRot;

    //카메라 컨테이너
    private Vector3 _cameraContainerStartPos;


    //카메라 참조
    [SerializeField] private Transform _camera;

    //이동속도
    [SerializeField] private float _moveSpeed;

    public void StartPosition()
    {
        //현재 비행기의 위치를 시작 위치로 지정
        _flyerShipStartPos = transform.position;

        //현재 비행의 회전을 시작 회전으로 지정
        _flyerShipStartRot = transform.rotation;

        //게임 종료 시 마커를 시작위치로 돌리기위해 위치와 회전을 지정함

        _targetMakerStartPos = _targetMarker.position;
        _targetMakerStartRot = _targetMarker.rotation;

        //게임 종료 시 카메라의 위치를 시작위치로 돌리기 위해 위치를 저장함
        _cameraContainerStartPos = _cameraContainer.position;


    }

    //처음 저장된 위치로 비행선,타겟마커,카메라 컨테이너의 위치를 설정함
    public void ResetPosition()
    {
        transform.position = _flyerShipStartPos;
        transform.rotation = _flyerShipStartRot;
        _targetMarker.position = _targetMakerStartPos;
        _targetMarker.rotation = _targetMakerStartRot;
        _cameraContainer.position = _cameraContainerStartPos;
    }

    

    public void StartMoveFlyer()
    {
        StartPosition();

        StartCoroutine(MoveFlyer());
        //Debug.Log("비행선 이동 시작");
    }

    public void StopMoveFlyer()
    {

        StopCoroutine(MoveFlyer());
        ResetPosition();
        
       // Debug.Log("비행선 이동 중지");
    }

    IEnumerator MoveFlyer()
    {
        //게임 진행중일때만 비행선이동
        while(CGameController.IsGameRunning)
        {

            //카메라의 회전 각도 (시선각도)를 저장함
            Quaternion headRotation = Camera.main.transform.rotation;

            //카메라의 위치에 헤드 트래킹의 방향으로 지정된 가녁 거리만큼을 떨어진
            //위치로 이동 타겟의 방향 위치를 설정함

            _targetMarker.position = _camera.position + (headRotation * Vector3.forward) * _distanceFromCamera;

            //카메라를 지정된 속도로 이동시킴
            _cameraContainer.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);


            //타겟마커의 위치로 비행선을 부드럽게 이동시킴
            transform.position = Vector3.Lerp(transform.position,_targetMarker.position,_damping*(1f-Mathf.Exp(expDampingCoef*Time.deltaTime)));

            //타겟 위치와 비행선간의 거리를 계산함
            Vector3 dist = transform.position - _targetMarker.position;


            //타겟 마커의 x회전을 비행기의 y축 거리에 맞추고 비행선의 x축의 거리를 z회전에 설정한다
            _targetMarker.eulerAngles = new Vector3(dist.y, 0f, dist.x) * bankingCeof;

            //비행선을 타겟 마커의 방향에 맞게 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetMarker.rotation, _damping * (1f - Mathf.Exp(expDampingCoef * Time.deltaTime)));

            yield return null;


        }
    }

}
