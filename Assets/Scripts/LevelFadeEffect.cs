using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class LevelFadeEffect : MonoBehaviour
{
    [SerializeField] CanvasGroup level;
    [SerializeField] float timeToFadeOut;
    [SerializeField] float timeToFadeIn;
    //HOW TO USE: Dej Objektu (Canvas) CanvasGroup a pøidej ke skriptu, zadej timeTo... a užívej :)
    public void FadeIn() 
    {
        StartCoroutine(FadeInIE());
    }

    private IEnumerator FadeInIE()
    {
        while (level.alpha < 1f)
        {
            level.alpha += Time.deltaTime / timeToFadeIn;
            yield return null;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutIE());
    }

    private IEnumerator FadeOutIE() //IEnumerator zlepšuje výkon oproti void Public
    {
        while (level.alpha > 0f)
        {
            level.alpha -= Time.deltaTime / timeToFadeOut;
            yield return null;
        }
    }

}
