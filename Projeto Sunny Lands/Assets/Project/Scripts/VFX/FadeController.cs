using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;

    public Image fadeBackground;
    public Color startColor;
    public Color endColor;
    public float fadeTotalDuration;
    private float fadeCurrentDuration;
    public bool isFading;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator StartFadeEffect()
    {
        isFading = true;
        fadeCurrentDuration = 0f;

        while(fadeCurrentDuration <= fadeTotalDuration)
        {
            fadeBackground.color = Color.Lerp(startColor, endColor, fadeCurrentDuration / fadeTotalDuration);
            fadeCurrentDuration += Time.deltaTime;
            yield return null;
        }

        isFading = false;
        fadeBackground.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartFadeEffect());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
