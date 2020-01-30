using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//오브젝트 풀
public class CObjectPool : MonoBehaviour
{

    //오브젝트 풀에 사용할 프리팹
    [SerializeField] private GameObject[] _prefabs;

    //오브젝트 풀 갯수
    [SerializeField] private int _numberInPool;


    //오브젝트 풀 리스트
    private List<GameObject> _pool = new List<GameObject>();


    void Awake()
    {
        for(int i=0; i<_numberInPool; i++)
        {
            AddToPool();
        }
    }

    void AddToPool()
    {
        //풀링할 오브젝트 프리팹 인덱스를 선택함
        int randomIndex = Random.Range(0, _prefabs.Length);

        //지정한 프리팹을 오브젝트로 생성함

        GameObject instance = Instantiate(_prefabs[randomIndex]);

        //생성한 오브젝트를 풀링 오브젝트의 자식으로 연결함
        instance.transform.parent = transform;

        //오브젝트를 비활성화 함
        instance.SetActive(false);

        //오브젝트 풀에 생성한 오브젝트를 추가함
        _pool.Add(instance);

        

    }


    //오브젝트 풀에서 재사용 가능한 오브젝트를 반환 받음
    public GameObject GetGameObjectFromPool()
    {
        //남아있는 오브젝트가 없다면 추가
        if (_pool.Count <= 0) AddToPool();

        //리스트에 첫번째 오브젝트를 반환받음
        GameObject ret = _pool[0];

        //0번째 항목을 제거함
        _pool.RemoveAt(0);

        //오브젝트 활성화
        ret.SetActive(true);

        //부로 참조 해제,루트 오브젝트 로 변환
        ret.transform.parent = null;

        return ret;

    }

    public void ReturnGameObjectPool(GameObject go)
    {
        //오브젝트 풀에 오브젝트를 추가함
        _pool.Add(go);

        //반환된 오브젝트는 비활성화함
        go.SetActive(false);

        //오브젝트풀의 자식오브젝트로 변환

        go.transform.parent = transform;
    }




}
