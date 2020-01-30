using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 게임시작 버튼의 방향을 유도하는 화살표를표시함
 * 
 * 
 */


public class CGUIArrows : MonoBehaviour
{

    //페이드 효과 지연 시간
    [SerializeField] private float _fadeDuration = 0.5f;

    //화살표가 표기될 시선의 각도
    [SerializeField] private float _showAngle = 60f;

    //화살표 표시 기준이 되는 시선 벡터
    [SerializeField] private Transform _desireDirection;

    //카메라 참조
    [SerializeField] private Transform _camera;

    //페이드 아웃 처리할 화살표들의 렌더러 참조
    [SerializeField] private Renderer[] _arrowRenderers;

    //현재 알파값
    private float _currentAlpha;
    //목표 알파값
    private float _targetAlpha;
    //페이드 처리 속도
    private float _fadeSpeed;

    private const string _materialPropertyName = "_Alpha";


    // Use this for initialization
    void Start()
    {
        _fadeSpeed = 1f / _fadeDuration;
    }

    // Update is called once per frame
    void Update()
    {
        //화살표 표시 기준이 되는 시선 벡터가 없으면 월드의 정방향 벡터로 설정함

        Vector3 desiredForward = _desireDirection == null ? Vector3.forward : _desireDirection.forward;

        //평면에 투영된 카메라의 시선 벡터를 정규화 함
        //(수직 각도를 무시한 카메라의 방향 벡터를 구함)

        Vector3 flatCamForward = Vector3.ProjectOnPlane(_camera.forward, Vector3.up).normalized;

        //카메라의 시선과 기준 시선간의 각도를 구함
        float angleDelta = Vector3.Angle(desiredForward, flatCamForward);

        //각도의 차이가 지정한 각도보다 크면 화살표 오브젝트의
        //메터리얼의 알파값을 1 아니면 0 으로 설정함
        _targetAlpha = angleDelta > _showAngle ? 1f : 0f;


        //화살표들의 알파값을 지정된 페이드 속도로 변환함
        _currentAlpha = Mathf.MoveTowards(_currentAlpha, _targetAlpha, _fadeSpeed * Time.deltaTime);


        // 렌더러들의 메터리얼의 알파값들을 현재 계산된 알파값으로 적용함
        for(int i=0; i<_arrowRenderers.Length; i++)
        {
            _arrowRenderers[i].material.SetFloat(_materialPropertyName, _currentAlpha);
        }



    }
}
