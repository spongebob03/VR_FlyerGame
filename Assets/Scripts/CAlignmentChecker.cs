using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CAlignmentChecker : MonoBehaviour
{
    //정렬 체크 반지름
    [SerializeField] private float _radius = 10f;


    //현재 정렬을 체크하려는 링

    private CRing _currentRing;



    public void StartAlignCheck()
    {
        StartCoroutine(CheckAlignment());
    }

    public void StopAlignCheck()
    {
        StopCoroutine(CheckAlignment());
    }

    IEnumerator CheckAlignment()
    {
       while(CGameController.IsGameRunning)
        {
            //현재 링이 있다면
            if(_currentRing) 
            {
                _currentRing.ShipAligned = false;
            }

            //현재 비행선의 앞 방향으로 레이를 생성함
            Ray ray = new Ray(transform.position, Vector3.forward);

            //구 모양의 정렬 상태를 체크함
            RaycastHit hit;

            if (Physics.SphereCast(ray, _radius, out hit))
            {
                if(hit.transform.gameObject !=null)
                {
                    //그 오브젝트에 링 컴포넌트가 존재하면
                    CRing ring = hit.transform.GetComponent<CRing>();

                    if(ring)
                    {
                        //현재 체크중인 링에 충돌된 링을 설정함
                        _currentRing = ring;

                        //현재 링의 색상을 정렬 중인 색상으로 변경함
                        _currentRing.ShipAligned = true;

                    }
                }
            }

            yield return null;
        }
    }




}
