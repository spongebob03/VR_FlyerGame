using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//액션
using System;
using UnityEngine.UI;



public class CGameController : MonoBehaviour
{
    //게임 진행여부
    public static bool IsGameRunning = false;

    //인트로 UI
    [SerializeField] private GameObject _introUI;

    //아웃트로 UI
    [SerializeField] private GameObject _outroUI;

    //게임 시작 타임 슬라이더
    [SerializeField] private CSelectSlider _selectStartSlider;

    //게임 종료 타임 슬라이더

    [SerializeField] private CSelectSlider _selectReStartSlider;

    
    [SerializeField] private GameObject _reticle;

    //비행선
    [SerializeField] private CFlyerShipController _flyerShipCtrl;

    //타겟 마커
    [SerializeField] private GameObject _targetMarker;

    //링 생성기
    [SerializeField] private CRingGenerator _ringGenerator;

    //현재점수
    [SerializeField] private Text _currentScoreText;

    //최고점수
    [SerializeField] private Text _highScoreText;
    
    // Use this for initialization
    void Start()
    {

        //액션등록 (OnSelectionComplete 통지자에게 OnGameStart 메소드를 옵저버로 등록함)
        _selectStartSlider.OnSelectionComplete += OnGameStart;
        _selectReStartSlider.OnSelectionComplete += OnGameStart;

        _flyerShipCtrl.OnGameEnd += OnGameEnd;
    }

    public void OnGameStart()
    {
        IsGameRunning = true;

        //리티클,인트로화면 비활성화
        _reticle.SetActive(false);

        _introUI.SetActive(false);

        //재시작화면 비활성화
        _outroUI.SetActive(false);

        CSessionData.ReStart();

        //타겟 마커 활성화
        _targetMarker.SetActive(true);
        _flyerShipCtrl.gameObject.SetActive(true);
        //비행선 게임시작
        _flyerShipCtrl.GameStart();

        //링 생성 시작
        _ringGenerator.StartGenerate();
    }

    public void OnGameEnd()
    {
        IsGameRunning = false;


        _currentScoreText.text = CSessionData.Score.ToString();

        _highScoreText.text = CSessionData.HighScore.ToString();
        
        //시선초기화
        GvrCardboardHelpers.Recenter(); 



        //링 생성중지
        _ringGenerator.EndGenerate();

        _outroUI.SetActive(true);

      
        _flyerShipCtrl.GameEnd();
        //비행선 컨트롤러 제어 종료
        _targetMarker.SetActive(false);


        _reticle.SetActive(true);


        _flyerShipCtrl.gameObject.SetActive(false); 
    }


}
