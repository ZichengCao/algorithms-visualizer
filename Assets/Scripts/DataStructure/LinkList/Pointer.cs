using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FunnyAlgorithm;

public class Pointer : MonoBehaviour
{
    public Image image;
    public RectTransform rect;
    public Text text;
    private void OnEnable()
    {

    }

    public void RightShift(float value, float duration = 1f)
    {
        rect.DOAnchorPosX(rect.anchoredPosition.x + value, duration);
    }

    public void LeftShit(float value, float duration = 1f)
    {
        rect.DOAnchorPosX(rect.anchoredPosition.x - value, duration);
    }
    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void initialize(Vector2 pos, string name, bool flag)
    {
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        text.text = name ;
        if (flag)
        {
            Reverse();
        }

    }
    private bool flag = true;
    public void Reverse()
    {
        if (flag)
        {
            text.GetComponent<RectTransform>().rotation= Quaternion.Euler(0, 0, 180);
            rect.rotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            text.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            rect.rotation = Quaternion.Euler(0, 0, 0);
        }
        flag = !flag;
    }

    public void SetDirection(direction dir)
    {
        if (dir == direction.RIGHT)
        {
            text.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90);
            //text.gameObject.SetActive(false);
            rect.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    // 耻辱柱，设计时未考虑清楚，某些地方并不希望Fade同时进行destroy
    public void Fade(bool flag = true)
    {
        image.DOFade(0, MoveTool.duration);
        text.DOFade(0, MoveTool.duration);
        if(flag)
            Destroy(gameObject, 1f);
    }

    public void MoveTo(Vector2 destination)
    {
        MoveTool.MoveTo(rect, destination, MoveTool.duration) ;
    }

    public void Show()
    {
        image.DOFade(1, MoveTool.duration);
        text.DOFade(1, MoveTool.duration);
    }
    public void SetText(string str)
    {
        text.text = str;
    }

}
