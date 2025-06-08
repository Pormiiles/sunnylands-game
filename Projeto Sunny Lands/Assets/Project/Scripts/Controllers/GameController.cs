using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    [SerializeField] private Text scoreTxt;
    public AudioSource audioSource;
    public AudioClip collectedCarrotSound;

    public void ScorePoints(int scorePointsQnt)
    {
        score += scorePointsQnt;
        scoreTxt.text = score.ToString();

        audioSource.PlayOneShot(collectedCarrotSound, 0.3F);
    } 
}
