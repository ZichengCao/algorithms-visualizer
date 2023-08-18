using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FunnyAlgorithm;

public class DS_processControl : MonoBehaviour
{
    public Button next_btn, Switch;
    public static bool passport = false;
    public static bool isAutoPlay = true;
    private static bool isRun = false;
    private void OnEnable()
    {
        next_btn.onClick.AddListener(NextStep);
        Switch.onClick.AddListener(Play_Pause);
    }

    public void NextStep()
    {
        passport = true;
    }

    public static IEnumerator Wait(float duration)
    {
        if (isAutoPlay)
        {
            // 等待 duration 秒时长，返回函数
            yield return new WaitForSeconds(duration);
        }
        else
        {
            // 等待 passport 为true，返回函数
            yield return new WaitUntil(() => passport);
            passport = false;
        }
    }

    public void AutoSwitch()
    {

    }
    public void Play_Pause()
    {
        if (isAutoPlay)
        {
            passport = false;
            Switch.GetComponentInChildren<Text>().text = "逐步";
            next_btn.gameObject.SetActive(true);
        }
        else
        {
            //自动播放
            passport = true;
            Switch.GetComponentInChildren<Text>().text = "自动";
            next_btn.gameObject.SetActive(false);
        }
        isAutoPlay = !isAutoPlay;
    }
}
