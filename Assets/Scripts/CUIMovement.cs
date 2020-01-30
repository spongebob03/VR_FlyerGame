using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI 빌보드 처리
public class CUIMovement : MonoBehaviour
{
    //카메라 시선과 일치 여부
    [SerializeField] private bool _lookAtCamera = true;

    //빌보드 처리할 UI 요소
    [SerializeField] private Transform _uiElement;

    //카메라
    [SerializeField] private Transform _camera;


    // Update is called once per frame
    void Update()
    {
       if(_lookAtCamera)
        {
            _uiElement.rotation = Quaternion.LookRotation(_uiElement.position - _camera.position);


        }
    }
}
