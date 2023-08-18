using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FunnyAlgorithm;
public class MyVariable : MonoBehaviour
{

    public Image image;
    public Text text;
    private new string name;
   
    public void SetName(string name)
    {
        this.name = name;
    }
    public void setValue(int value)
    {
        text.text = name + " = " + value;
    }
    public void setValue(string name,int value)
    {
        text.text = name + " = " + value ;
        this.name = name;
    }

    public void Fade()
    {
        image.DOFade(0, MoveTool.duration);
        text.DOFade(0, MoveTool.duration);
        Destroy(gameObject, 1f);
    }
}
