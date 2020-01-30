using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFlyerShipState : MonoBehaviour
{

    //점수 출력용 텍스트
    [SerializeField] private Text _scoreText;



    void Update()
    {
        _scoreText.text = "Score:" + CSessionData.Score;
    }
}
