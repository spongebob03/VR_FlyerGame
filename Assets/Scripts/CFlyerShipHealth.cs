using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFlyerShipHealth : MonoBehaviour
{

    //시작 체력
    [SerializeField] private float _startingHealth = 100f;

    //현재 체력
    private float _currentHealth;

    //체력바
    [SerializeField] private Image _healBar;


    //사망상태
    private bool _isDie;

    public bool isDie
    {
        get { return _isDie; }
    }


    //정보 초기화
    public void Init()
    {
        _currentHealth = _startingHealth;

        _healBar.fillAmount = 1f;

        _isDie = false;
    }

    //체력 감소
    public void HealthDown(int downValue)
    {

        //현재 체력을 감소시킴
        _currentHealth -= downValue;

        //최소,최대 범위를 넘지못하게함 (0f~100F) 까지
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _startingHealth);

        _healBar.fillAmount = _currentHealth / _startingHealth;

        //Abs 혹시 현재체력이 -일때  ,float.Epsilon : 0에 가까워지면
        if(Mathf.Abs(_currentHealth) <=float.Epsilon)
        {
            //사망 상태로 설정
            _isDie = true;
        }
    }


}
