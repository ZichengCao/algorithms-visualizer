using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FunnyAlgorithm;

public class LinkNode : Node
{
    
    public Text num_text;
    public GameObject next, prior; // next piror箭头

    private RectTransform next_rect, prior_rect;
    public GameObject inner; // 内部的数字

    /// <summary>
    /// 调整指针GameObject的指向
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="flag">0：next,1:prior</param>
    public void AdjustPointTo(direction dir, int flag = 0)
    {
        GameObject gb = flag == 0 ? next : prior;
        float horizontalValue = flag == 0 ? 0f : -180f;
        switch (dir)
        {
            case direction.UP:
                gb.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 90f), MoveTool.duration);
                break;
            case direction.DOWN:
                gb.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, -90f), MoveTool.duration);
                break;
            case direction.LEFT:
            case direction.RIGHT:
                gb.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, horizontalValue), MoveTool.duration);
                break;
            default:
                break;

        }
    }
    public void initializePosition(Vector2 pos)
    {
        rect.anchorMin = new Vector2(0, 0.5f);
        rect.anchorMax = new Vector2(0, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = pos;
        next_rect = next.GetComponent<RectTransform>();
        prior_rect = prior.GetComponent<RectTransform>();
    }

    public void nextPointShift(float value = -11)
    {
        next_rect.anchoredPosition = new Vector2(next_rect.anchoredPosition.x, next_rect.anchoredPosition.y + value) ;
    }

    public void priorPointShift(float value = 11)
    {
        prior_rect.anchoredPosition = new Vector2(prior_rect.anchoredPosition.x, prior_rect.anchoredPosition.y + value);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="flag">0：next，1：prior</param>
    public void HidePoint(int flag = 0)
    {
        if (flag == 0)
            next.SetActive(false);
        else
            prior.SetActive(false);

        
        //next.GetComponent<Image>().DOFade(0, 0.75f);
        //image.enabled = false;
    }

    public void ShowPoint()
    {
        next.SetActive(true);
        //next.GetComponent<Image>().DOFade(1, 0.75f);
        //image.enabled = true;
    }

    public void SetArrowColor(Color color)
    {
        next.GetComponent<Image>().color = color;
    }

    public void SetInnerColor(Color color)
    {
        inner.GetComponent<Image>().color = color;
    }
    public void SetValue(int val)
    {
        this.num = val;
        num_text.text = val.ToString();
    }

    public void Fade()
    {
        image.DOFade(0, MoveTool.duration);
        inner.GetComponent<Image>().DOFade(0, MoveTool.duration);
        num_text.DOFade(0, MoveTool.duration);
        next.GetComponent<Image>().DOFade(0, MoveTool.duration);
        prior.GetComponent<Image>().DOFade(0, MoveTool.duration);
        inner.GetComponent<Image>().DOFade(0, MoveTool.duration);
        Destroy(gameObject, 1f);
    }
}
