using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSessionData
{
    //점수 저장 칸
    private const string dataKeyName = "FlyerData";

    //최고 점수
    private static int highScore;

    //현재 점수
    private static int score;

    public static int HighScore { get { return highScore; } }

    public static int Score { get { return score; } }

    //최고 점수 갱신
    private static void SetHighScore()
    {

        //최고 점수 설정
        highScore = score;

        //최고 점수 저장

        PlayerPrefs.SetInt(dataKeyName, score);
        PlayerPrefs.Save();
        
    }

    //최고 점수 로드
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(dataKeyName, 0);
    }

    //최고 점수 체크
    private static void CheckHighScore()
    {
        //현재 점수가 최고 점수보다 크다면
        if (score > highScore)
            SetHighScore();
    }


    public static void ReStart()
    {
        score = 0;
        highScore = GetHighScore();
    }

    public static void AddScore(int score)
    {
        CSessionData.score += score;

        CheckHighScore();
    }


}
