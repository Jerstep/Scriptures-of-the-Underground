using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpUI : MonoBehaviour
{
    public TMP_Text textbox;
    public Image icon;

    float targetAlphaFadeIn = 255f;
    float targetAlphaFadeOut = 0f;
    public float FadeRate;

    public float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeTextToFullAlpha(textbox, icon));
    }

    public void StartFade(string title, Sprite iconSprite)
    {
        textbox.text = title;
        icon.sprite = iconSprite;
        StartCoroutine(FadeText(textbox, icon));
    }

    public IEnumerator FadeText(TMP_Text _titleText, Image _icon)
    {
        yield return new WaitForSeconds(waitTime);
        Color curColor = icon.color;
        while (icon.color.a < targetAlphaFadeIn)
        {

            curColor.a = Mathf.Lerp(curColor.a, targetAlphaFadeIn, FadeRate * Time.deltaTime);
            icon.color = curColor;
            _titleText.color = curColor;
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);
        curColor = icon.color;

        while (icon.color.a > targetAlphaFadeOut)
        {

             curColor.a = Mathf.Lerp(curColor.a, targetAlphaFadeOut, FadeRate / Time.deltaTime);
             icon.color = curColor;
             _titleText.color = curColor;
             yield return null;
        }
    }

}
