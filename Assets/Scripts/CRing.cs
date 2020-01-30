using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CRing : MonoBehaviour
{
    //카메라 참조 (카메라 를 넘으면 파괴시키기 위해)

    private Transform _camera;

    //링 삭제를 위한 카메라와의 거리
    private const float _removeDistance = 50f;

    //링 파괴 이벤트 액션
    public event Action<CRing> OnRingRemove;

    //링 통과 점수 
    [SerializeField] private int _score = 100;

    //링 생성 시 기본 색상
    [SerializeField] private Color _baseColor;

    //링과 함선이 정렬이 일치할때 변경되는 색상
    [SerializeField] private Color _shipAlignedColor;

    //비행선이 통과된 후 생장(점수 부여되는 순간)
    [SerializeField] private Color _activateColor;

    //상태별 색상 메테리얼
    private List<Material> _materials;

    //비행선과 링이 정렬되어 있는지 여부
    private bool _shipAligned;

    //링이 비행선을 이미 통과했는지 여부
    private bool _hasTriggered;

    public bool ShipAligned
    {
        set
        {
            _shipAligned = value;

            if (_hasTriggered) return;
            //조건 : 비행기가 링과 정렬된 상태라면 정렬색상을 아니면 기본색을 설정함
            SetRingColor(_shipAligned ? _shipAlignedColor : _baseColor);
        }

        get { return _shipAligned; }
    }

    //링의 색상을 변경함
    void SetRingColor(Color color)
    {
        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i].color = color;
        }
    }


    private void Awake()
    {
        //메테리얼 리스트를 생성함
        _materials = new List<Material>();

        //현재 링의 자식 렌더러들을 참조함
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        //자식 링 오브젝트들의 렌더러들의 메터리얼을 메터리얼 리스트에 추가함
        for (int i = 0; i < renderers.Length; i++)
        {
            _materials.Add(renderers[i].material);
        }

        //메인 카메라 참조
        _camera = Camera.main.transform;
    }



    // Update is called once per frame
    void Update()
    {

        //현재 링이 카메라 뒤로 넘어가면 링 삭제 요청
        if (transform.position.z < _camera.position.z - _removeDistance)
        {
            //링을 관통안하고 지나쳤을경우
            if(!_hasTriggered)
            {
                //HealthDown 메소드를 호출시키고 체력을 -10 깎음
                GameObject.FindGameObjectWithTag("Player").SendMessage("HealthDown", 10);
            }

            //링 삭제를 요청함
            if (OnRingRemove != null)
            {
                //링 삭제 이벤트 발생 (자기자신)
                OnRingRemove(this);
            }
        }
    }

    //링과 비행선 충돌 처리
    private void OnTriggerEnter(Collider collision)
    {
        //이미 비행선을 통과한 링
        if (_hasTriggered||collision.tag!="Player") return;

        //링이 비행선을 통과함

        _hasTriggered = true;

        //점수를 저장함
        CSessionData.AddScore(_score);

        SetRingColor(_activateColor);

  

    }

    public void Restart()
    {
        //링의 색상을 기본 색상을 변경함
        SetRingColor(_baseColor);


        //링이 통과 되기 전 상태로 초기화 함
        _hasTriggered = false;
    }
}
