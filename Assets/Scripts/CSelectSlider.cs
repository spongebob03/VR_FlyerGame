using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//액션사용
using System;

public class CSelectSlider : MonoBehaviour
{


    //슬라이더
    [SerializeField] private Slider _slider;

    //현재 지연 시간
    float _timer = 0;

    //게이지를 가득 채우기 위한 지연 시간
    [SerializeField] private float _maxDurationTime;

    //슬라이더에 시선이 위치해 있는지 여부
    private bool _isSliderOver = false;

    //액션변수선언
    public Action OnSelectionComplete;


    //슬라이더 채우기용 코루틴
    IEnumerator FillCoroutine()
    {
        _timer = 0;

        //가득 채우기 시간 설정
        float fillTime = _maxDurationTime;


        //채우기 시간이 완료되지 않았다면
        while(_timer<fillTime)
        {
           _timer+=Time.deltaTime;

            //현재 진행 시간의 비율을 슬라이더에 적용함
            SetSliderValue(_timer / fillTime);

            yield return null;  //Update 단위와 동일하게 지연 

            if (_isSliderOver) continue;

            yield break; //코루틴을 빠져나감

        }
        //게임시작

        Debug.Log("게임시작!");
        if(OnSelectionComplete!=null)
        {
            //슬라이더 진행이 끝나면 통지받을 옵저버들의 메소드를 모두 실행해라
            OnSelectionComplete();
            
        }


        //타이머 초기화
        _timer = 0;

        //슬라이더 초기화
        SetSliderValue(0f);

    }


    public void SetSliderValue(float sliderValue)
    {
        if (_slider) _slider.value = sliderValue;
    }


    //시선이 슬라이더를 볼때
    public void OnSliderPointerEnter()
    {
        //코루틴이 2개 생성될수도있으니
        StopCoroutine(FillCoroutine());

        StartCoroutine(FillCoroutine());
        _isSliderOver = true;
    }

    public void OnSliderPointerExit()
    {
        _isSliderOver = false;

        _timer = 0;

        SetSliderValue(0f);

    }
}
