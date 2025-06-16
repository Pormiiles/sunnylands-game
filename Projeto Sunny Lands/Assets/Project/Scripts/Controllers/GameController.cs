using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private int score;
    [SerializeField] private Text scoreTxt;
    public AudioSource audioSource;
    public AudioClip collectedCarrotSound;
    [SerializeField] private Sprite[] lifeBarSprites;
    [SerializeField] private Image lifeBar;
    [SerializeField] private GameObject gameOverPopup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ScorePoints(int scorePointsQnt)
    {
        score += scorePointsQnt;
        scoreTxt.text = score.ToString();

        PlaySFX(collectedCarrotSound, 0.5F);
    } 

    public void PlaySFX(AudioClip clip, float clipVolume)
    {
        audioSource.PlayOneShot(clip, clipVolume);
    }

    public void UpdateLifeBarSprite(int life)
    {
        lifeBar.sprite = lifeBarSprites[life];
    }

    public void ActivedGameOverPopup()
    {
        StartCoroutine(LoadGameOverPopup());
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    IEnumerator LoadGameOverPopup()
    {
        yield return new WaitForSeconds(2.5f);
        gameOverPopup.SetActive(true);
    }
}
