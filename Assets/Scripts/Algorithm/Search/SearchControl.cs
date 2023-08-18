using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyAlgorithm
{
    public class SearchControl : MonoBehaviour
    {
        public SearchView view;
        public GameObject Area;
        private SearchBasicModel demo;
        public static List<int> nums = null;
        public static int x;

        public static bool IsStart = false;
        public searchType search_type = searchType.LINEARSEARCH;
        public static int data_length = 10, data_type = 1;
        private bool play_or_pause = true;//开始、暂停


        // Start is called before the first frame update
        void Start()
        {
            x = Random.Range(0, data_length);
            ArrayNode.verticalStandard = 100;
        }
        private void OnEnable()
        {
            //Debug.Log(1);
            Restart_Btn();
            view.Btn_NextStep.onClick.AddListener(NextStep_Btn);
            view.Btn_LastStep.onClick.AddListener(LastStep_Btn);
            view.Btn_StartButton.onClick.AddListener(Auto_Btn);
            view.Btn_Restart.onClick.AddListener(Restart_Btn);
            //if (search_model == model.DEMOMODEL)
            //    gameObject.GetComponentInChildren<Text>().text = MainControl.searchName[(int)search_type];
        }
        private void OnDisable()
        {
            view.Btn_NextStep.onClick.RemoveListener(NextStep_Btn);
            view.Btn_LastStep.onClick.RemoveListener(LastStep_Btn);
            view.Btn_StartButton.onClick.RemoveListener(Auto_Btn);
            view.Btn_Restart.onClick.RemoveListener(Restart_Btn);
            ArrayNode.DestoryArrayNodes(Area, demo.nodes);
        }

        private SearchBasicModel initialize()
        {
            SearchBasicModel SBM;
            view.TargetNode.GetComponentInChildren<Text>().text = "目标：" + x.ToString();
            List<ArrayNode> nodes = ArrayNode.CreatArryNodes(data_length, search_type == searchType.LINEARSEARCH ? 1 : 2, view.NODE, Area, Area.GetComponent<RectTransform>().sizeDelta, nums, 0f);

            switch (search_type)
            { 
                case searchType.LINEARSEARCH:
                    SBM = new LinearSearchStudyModel(x, nodes);
                    break;
                default:
                case searchType.BINARYSEARCH:
                    SBM = new BinarySearchStudyModel(x, nodes);
                    break;
            }
            SetInitialAnnotationText();

            
            SBM.RecordProce();
            return SBM;

        }

        public void Reinitialize()
        {
            view.TargetNode.GetComponentInChildren<Text>().text = "目标：" + x.ToString();
            demo.demoQueue.Clear();
            demo.executedStack.Clear();
            demo.x = x;
            demo.RecordProce();
        }

        public void SetInitialAnnotationText()
        {
            switch ((int)search_type)
            {
                case 0:
                    view.Text_Annotation.text = "<color='red'>线性查找</color>：是在一个已知无序(或有序）数组中找出与给定关键字相同的值的位置。原理是让关键字与数组中的元素逐个比较，直到找出与给定关键字相同的值为止，它的缺点是效率低下";
                    break;
                case 1:
                    view.Text_Annotation.text = "<color='red'>二分查找</color>：也称折半查找，它是一种效率较高的查找方法。但是，折半查找要求线性表必须采用顺序存储结构，而且表中元素按关键字有序排列";
                    break;
                default:
                    break;
            }
        }
        #region BottomButton


        public void Restart_Btn()
        {
            StopAllCoroutines();
            if (Area.transform.childCount > 1)
                ArrayNode.DestoryArrayNodes(Area, demo.nodes);
            play_or_pause = true;
            IsStart = false;
            view.Btn_LastStep.interactable = false;
            view.Btn_NextStep.interactable = true;
            view.Btn_StartButton.interactable = true;
            view.Text_StartButton.text = "自动";
            view.Text_Annotation.alignment = TextAnchor.MiddleLeft;
            demo = initialize();
        }

        public void LastStep_Btn()
        {
            StartCoroutine(LastStep());
        }

        private IEnumerator LastStep()
        {
            view.Btn_NextStep.interactable = true;
            view.Btn_StartButton.interactable = true;
            if (demo.demoQueue.Count == 0)
            {
                play_or_pause = true;
                view.Text_StartButton.text = "自动";
            }

            if (demo.executedStack.Count > 0)
            {
                view.Btn_LastStep.interactable = false;
                bool flag;
                do
                {
                    flag = demo.PlayBackward();
                    if (!flag)
                    {
                        yield return new WaitForSeconds(MoveTool.duration);
                    }
                } while (flag);
                view.Btn_LastStep.interactable = true;
                if (demo.executedStack.Count == 0)
                    view.Btn_LastStep.interactable = false;
            }
        }
        public void NextStep_Btn()
        {
            view.Text_Annotation.alignment = TextAnchor.MiddleCenter;
            StartCoroutine(NextStep());
        }

        private IEnumerator NextStep()
        {
            if (demo.demoQueue.Count > 0)
            {
                view.Btn_NextStep.interactable = false;
                IsStart = true;
                bool flag;
                do
                {
                    flag = demo.PlayForward();
                    if (!flag)
                        yield return new WaitForSeconds(MoveTool.duration);
                } while (flag);
                view.Btn_NextStep.interactable = true;
                if (demo.demoQueue.Count == 0)
                {
                    view.Btn_NextStep.interactable = false;
                    view.Btn_StartButton.interactable = false;
                    view.Text_StartButton.text = "结束";
                }
            }
            view.Btn_LastStep.interactable = true;
        }
        public void Auto_Btn()
        {
            view.Text_Annotation.alignment = TextAnchor.MiddleCenter;
            if (play_or_pause)
            {
                view.Btn_LastStep.interactable = false;
                view.Btn_NextStep.interactable = false;
                view.Text_StartButton.text = "暂停";
                StartCoroutine("AutoPlay");
            }
            else
            {
                view.Btn_LastStep.interactable = true;
                view.Btn_NextStep.interactable = true;
                view.Text_StartButton.text = "继续";
                StopCoroutine("AutoPlay");
            }
            play_or_pause = !play_or_pause;

        }
        private IEnumerator AutoPlay()
        {
            while (demo.demoQueue.Count > 0)
            {
                bool flag;
                do
                {
                    flag = demo.PlayForward();
                    if (!flag)
                        yield return new WaitForSeconds(MoveTool.duration);
                } while (flag);
            }
            view.Text_StartButton.text = "结束";
            view.Btn_StartButton.interactable = false;
            view.Btn_NextStep.interactable = false;
            view.Btn_LastStep.interactable = true;
        }

        #endregion
    }
}