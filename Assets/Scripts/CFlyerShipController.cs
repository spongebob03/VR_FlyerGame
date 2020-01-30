using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CFlyerShipController : MonoBehaviour
{

    //비행선 이동처리
    [SerializeField] private CFlyerShipMovement _movement;

    //게임 총 시간
    [SerializeField] private int _gameDuration;

    //진행된 게임시간
    private float _endTime;

    //게임이 끝날때 까지 남은시간
    private float _timeRemaining;

    //게임 타이머 게이지
    [SerializeField] Image _timeBar;

    public Action OnGameEnd;

    [SerializeField] private CAlignmentChecker _alignChecker;

    [SerializeField] private CFlyerShipHealth _flayerHealth;



    //게임을 시작함
    public void GameStart()
    {
        //비행선 이동 시작
        _movement.StartMoveFlyer();

        //비행선의 체력 초기화
        _flayerHealth.Init();

        //게임타이머 시작
        _alignChecker.StartAlignCheck();
        StartCoroutine(GameTimeCheck());
    }

    //게임이 종료됨
    public void GameEnd()
    {
        //게임타이머 중지
        StopCoroutine(GameTimeCheck());

        _alignChecker.StopAlignCheck();

        //비행선 이동 중지
        _movement.StopMoveFlyer();
    }


    IEnumerator GameTimeCheck()
    {
        //현재 시간에 
        _endTime = Time.time + _gameDuration;

        do
        {
            //남은 시간 계산
            _timeRemaining = _endTime - Time.time;

            //남은 시간 게이지를 표시함 (남은 시간 비율 계산)
            _timeBar.fillAmount = _timeRemaining / _gameDuration;

            //Time.time 을 정확히 계산하기위해 null로 처리해준다.
            yield return null;

            //시간도남고 사망상태가 아니면 반복
        } while (_timeRemaining > 0&&!_flayerHealth.isDie);

        //게임 종료 처리
        if(OnGameEnd!=null)
        {
            OnGameEnd();
        }
    }

}
