using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FunnyAlgorithm;
public class Warning : MonoBehaviour
{
    public GameObject warning;
    private Text text_warning;
    private Image image_warning;
    void Start()
    {
        text_warning = warning.GetComponentInChildren<Text>();
        image_warning = warning.GetComponent<Image>();
    }

    public void showWarning(string text)
    {
        StartCoroutine(warning_coroutine(text));
    }
    private IEnumerator warning_coroutine(string text)
    {
        warning.SetActive(true);
        text_warning.text = text;
        image_warning.color = new Color(1, 0.4353f, 0.4353f, 1);
        text_warning.color = Color.black;
        yield return new WaitForSeconds(1);
        image_warning.DOFade(0, MoveTool.duration);
        text_warning.DOFade(0, MoveTool.duration);
        yield return new WaitForSeconds(MoveTool.duration) ;
        warning.SetActive(false);
    }
}
