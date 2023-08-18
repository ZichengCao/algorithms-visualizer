using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;




namespace FunnyAlgorithm
{
    public class MainControl : MonoBehaviour
    {
        public static string[] sortName = { "插入排序", "选择排序", "冒泡排序", "希尔排序", "归并排序", "快速排序" };
        public static Dictionary<string, string> ColorSetting = new Dictionary<string, string>
        { { "normal","FFFFFF" },{"selected","FF8566"},{"successed","66DD66" },{ "failed","FF0000"},{"signed","0099FF" },{"orangeButton",  "F59084"},{"blueButton","71B8FF" },{ "disable","C8C8C8"} };

        public Slider Slider_Delay;

        public GameObject[] TopButtonGroups;
        [HideInInspector] public List<Image> images_TopButton = new List<Image>();
        [HideInInspector] public List<RectTransform> rects_TopButton = new List<RectTransform>();
        [HideInInspector] public List<Text> texts_TopButton = new List<Text>();

        public static SecondMenuType secondMenuType = SecondMenuType.UNINITIALIZE;

        private void Awake()
        {

        }
        private void Update()
        {

        }
        private void Start()
        {
            for (int i = 0; i < TopButtonGroups.Length; i++)
            {
                images_TopButton.Add(TopButtonGroups[i].GetComponent<Image>());
                rects_TopButton.Add(TopButtonGroups[i].GetComponent<RectTransform>());
                texts_TopButton.Add(TopButtonGroups[i].GetComponentInChildren<Text>());
            }
        }

        #region LoadScene

        public void LoadStartInterface()
        {
            SceneManager.LoadScene("StartInterface");
        }


        public void LoadDemoInterface()
        {
            SceneManager.LoadScene("DemoInterface");
        }

        public void LoadDemoInterface(int index)
        {
            //一开始忘记把start界面设置为场景了，懒得改了，直接加个1吧
            //SceneManager.LoadScene(index + 1);
            switch (index)
            {
                case 0:
                    SceneManager.LoadScene("Sort");
                    break;
                case 1:
                    SceneManager.LoadScene("Search");
                    break;
                case 2:
                    SceneManager.LoadScene("Linklist");
                    break;
                case 3:
                    SceneManager.LoadScene("Stack");
                    break;
                case 4:
                    SceneManager.LoadScene("Queue");
                    break;
                case 5:
                    SceneManager.LoadScene("Tree");
                    break;
            }

        }

        public void LoadPraticeInterface()
        {
            SceneManager.LoadScene("PracticeInterface");
        }


        public void LoadProgrammingInterface()
        {
            SceneManager.LoadScene("ProgrammingInterface");
        }


        public void Quit()
        {
            Application.Quit();
        }

        #endregion

        #region TopButton
        public void TopButtonMouseIn(int index)
        {
            rects_TopButton[index].DOSizeDelta(new Vector2(120 * 1.1f, 50 * 1.1f), 0.2f);
            texts_TopButton[index].fontSize += 3;
        }

        public void TopButtonMouseOut(int index)
        {
            rects_TopButton[index].DOSizeDelta(new Vector2(120, 50), 0.2f);
            texts_TopButton[index].fontSize -= 3;
        }
        #endregion

        #region BottomButton
        public void SetSpeed_Btn()
        {
            MoveTool.duration = (10f - Slider_Delay.value) / 10f;
        }

        #endregion
    }


}
