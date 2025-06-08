using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    [SerializeField] private Text scoreTxt;

    public void ScorePoints(int scorePointsQnt)
    {
        score += scorePointsQnt;
        scoreTxt.text = score.ToString();
    } 
}
