using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRingGenerator : MonoBehaviour
{
    //오브젝트 풀
    [SerializeField] private CObjectPool _ringObjectPool;

    //생성관련 정보


    //생성 주기
    [SerializeField] private float _ringSpawnFrequency;

    //카메라와 생성 위치간의 거리
    [SerializeField] private float _spawnZoneDistance;

    //링 생성 영역 반지름
    [SerializeField] private float _ringSpawnZoneRadius;

    //생성 여부
    private bool _spawning;

    //카메라 참조
    [SerializeField] private Transform _camera;

    //생성된 링을 보관할 리스트
    private List<CRing> _rings;

    public void StartGenerate()
    {
        //생성된 링을 보관할 리스트를 생성함
        _rings = new List<CRing>();

        _spawning = true;

        //주기적으로 링을 생성해주는 코루틴 시작
        StartCoroutine(GenerateRing());
    }

    public void EndGenerate()
    {
        _spawning = false;

        while(_rings.Count>0)
        {
            //활성화된 링 리스트의 0번째 링을 제거함
            HandleRingRemove(_rings[0]);
        }
    }

    
    //링 생성 타이머
    IEnumerator GenerateRing()
    {
        yield return new WaitForSeconds(_ringSpawnFrequency);

        do
        {
            //링 생성
            SpawnRing();

            //링 생성 주기만큼 대기함
            yield return new WaitForSeconds(_ringSpawnFrequency);


        } while (_spawning);

        
    }

    //링을 생성함

    private void SpawnRing()
    {
        //링 오브젝트 풀에서 게이트를 꺼냄
        GameObject ringGameObject = _ringObjectPool.GetGameObjectFromPool();

        //링을 카메라로부터 떨어진 위치에 랜덤하게 회전된 상태로 생성함
        Vector3 ringPosition = _camera.position + Vector3.forward * _spawnZoneDistance + Random.insideUnitSphere * _ringSpawnZoneRadius;

        //오브젝트 풀에서 꺼낸 링의 위치를 설정함
        ringGameObject.transform.position = ringPosition;


        //링 컴포넌트를 참조함
        CRing ring = ringGameObject.GetComponent<CRing>();

        //링을 초기화해준다
        ring.Restart(); 
        //링 리스트에 생성된 링을 추가함

        _rings.Add(ring);

        //링의 링 제거 이벤트 통지 메소드 연결
        ring.OnRingRemove+=HandleRingRemove;
    }

    //링 반환 처리
    private void HandleRingRemove(CRing ring)
    {
        //링의 링 제거 이벤트 통지 메소드를 해제
        ring.OnRingRemove -= HandleRingRemove;

        _rings.Remove(ring);

        _ringObjectPool.ReturnGameObjectPool(ring.gameObject);
    }

}
