using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FontReplacement : MonoBehaviour
{
    public Font msyhlFont; // 在Inspector中将MSYHL字体赋值给这个变量

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnEnable()
    {
        // 注册在对象激活时进行检测的函数
        CheckTextComponents();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Text[] textComponents = FindObjectsOfType<Text>();

        foreach (Text textComponent in textComponents)
        {
            // 检查字体是否为Arial
            if (textComponent.font == null || textComponent.font.name == "Arial")
            {
                // 替换字体为MSYHL
                textComponent.font = msyhlFont;
            }
        }
    }

    private void CheckTextComponents()
    {
        Text[] textComponents = GetComponentsInChildren<Text>(true);

        foreach (Text textComponent in textComponents)
        {
            // 检查字体是否为Arial
            if (textComponent.font == null || textComponent.font.name == "Arial")
            {
                // 替换字体为MSYHL
                textComponent.font = msyhlFont;
            }
        }
    }
}
