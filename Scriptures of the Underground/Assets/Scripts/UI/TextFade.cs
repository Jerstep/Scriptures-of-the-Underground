using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{
    public Text textbox;
    public Image icon;

    float targetAlpha = 255f;
    public float FadeRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>(), icon));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   /* void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<Text>()));
        }
    }*/



    public IEnumerator FadeTextToFullAlpha(float t, Text i, Image _icon)
    {
        Color curColor = icon.color;

        while(curColor.a < targetAlpha)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, FadeRate * Time.deltaTime);
        }

        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
