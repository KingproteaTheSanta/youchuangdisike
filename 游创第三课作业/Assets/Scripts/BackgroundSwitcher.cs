using UnityEngine;
using System.Collections;

public class BackgroundSwitcher2D : MonoBehaviour
{
    public SpriteRenderer backgroundRenderer; 
    public Sprite[] backgrounds;              
    public float switchInterval = 3f;         
    public float flashDuration = 0.2f;       

    private int currentIndex = 0;

    void Start()
    {
        if (backgrounds.Length == 0 || backgroundRenderer == null)
        {
            Debug.LogError("ÎÞÍ¼Æ¬");
            return;
        }

        backgroundRenderer.sprite = backgrounds[0];
        StartCoroutine(SwitchRoutine());
    }

    IEnumerator SwitchRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchInterval);
            StartCoroutine(FlashAndSwitch());
        }
    }

    IEnumerator FlashAndSwitch()
    {
        // µ­³ö
        float timer = 0f;
        Color c = backgroundRenderer.color;

        while (timer < flashDuration)
        {
            c.a = Mathf.Lerp(1f, 0f, timer / flashDuration);
            backgroundRenderer.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        // ÇÐ»»
        currentIndex = (currentIndex + 1) % backgrounds.Length;
        backgroundRenderer.sprite = backgrounds[currentIndex];

        // µ­Èë
        timer = 0f;
        while (timer < flashDuration)
        {
            c.a = Mathf.Lerp(0f, 1f, timer / flashDuration);
            backgroundRenderer.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        c.a = 1f;
        backgroundRenderer.color = c;
    }
}
