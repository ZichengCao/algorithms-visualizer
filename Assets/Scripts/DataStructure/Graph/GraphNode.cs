using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FunnyAlgorithm;

public class GraphNode : Node
{
    public char val;
    private void OnEnable()
    {
        //1000*500  --> 60*60, 25
        //500*250  --> 30*30,18
        float standrad = 600 * 500;
        Vector2 size = transform.parent.GetComponent<RectTransform>().sizeDelta;
        float S = size.x * size.y;
        float times = Mathf.Sqrt(standrad / S);
        GetComponent<RectTransform>().sizeDelta = new Vector2(50 / times, 50 / times);
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    
    public void SetValue(char val)
    {
        this.val = val;
        GetComponentInChildren<Text>().text = val.ToString();
        //num.text = val.ToString();
    }

}
