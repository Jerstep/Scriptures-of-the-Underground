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
    public bool fadedIn;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(FadeTextToFullAlpha(textbox, icon));
        anim = GetComponent<Animator>();
    }

    public void StartFade(string title, Sprite iconSprite)
    {
        textbox.text = title;
        icon.sprite = iconSprite;
        fadedIn = !fadedIn;
        anim.SetBool("fadedIn", fadedIn);
        StartCoroutine(FadeFalse());
    }

    public IEnumerator FadeFalse()
    {
        yield return new WaitForSeconds(waitTime);
        fadedIn = false;
        anim.SetBool("fadedIn", fadedIn);
    }


    public IEnumerator FadeText(TMP_Text _titleText, Image _icon)
    {
        yield return new WaitForSeconds(waitTime);
        Color curColor = _icon.color;
        while (_icon.color.a < targetAlphaFadeIn && !fadedIn)
        {

            curColor.a = Mathf.Lerp(curColor.a, targetAlphaFadeIn, FadeRate * Time.deltaTime);
            _icon.color = curColor;
            _titleText.color = curColor;

            /*if (curColor.a >= targetAlphaFadeIn - 1)
            {
                fadedIn = true;
                //StartCoroutine(FadeTextOut(_titleText, _icon));
                //StopCoroutine(FadeText(_titleText, _icon));
            }*/
            yield return null;
        }

       
    }

    public IEnumerator FadeTextOut(TMP_Text _titleText, Image _icon)
    {
        Debug.Log("faded IN");
        fadedIn = true;
        yield return new WaitForSeconds(waitTime);
        Color curColor = _icon.color;

        while (_icon.color.a > targetAlphaFadeOut)
        {

            curColor.a = Mathf.Lerp(curColor.a, targetAlphaFadeOut, FadeRate / Time.deltaTime);
            _icon.color = curColor;
            _titleText.color = curColor;
            if (curColor.a <= targetAlphaFadeOut)
            {
                fadedIn = false;
                Debug.Log("faded OUT");
                // StopAllCoroutines();
            }
            yield return null;
        }
    }
}

