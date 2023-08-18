using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class SortControl : MonoBehaviour
    {
        public static List<int> nums = null;
        public SortView view;
        public GameObject Area;
        private SortBasicModel demo;
        public sortType sort_type = sortType.INSERTSORT;
        private bool play_or_pause = true;//开始、暂停
        public static model sort_model = model.STUDYMODEL;
        public static int data_length = 10, data_type = 0;

        void Start()
        {
            ArrayNode.verticalStandard = 100;
            //demo = Initialize(sort_type, Area);
        }
        private void OnEnable()
        {
            //Debug.Log(1);

            Restart_Btn();
            view.Btn_NextStep.onClick.AddListener(NextStep_Btn);
            view.Btn_LastStep.onClick.AddListener(LastStep_Btn);
            view.Btn_StartButton.onClick.AddListener(Auto_Btn);
            view.Btn_Restart.onClick.AddListener(Restart_Btn) ;
            if(sort_model==model.DEMOMODEL)
                gameObject.GetComponentInChildren<Text>().text = MainControl.sortName[(int)sort_type];
        }
        private void OnDisable()
        {
            view.Btn_NextStep.onClick.RemoveListener(NextStep_Btn);
            view.Btn_LastStep.onClick.RemoveListener(LastStep_Btn);
            view.Btn_StartButton.onClick.RemoveListener(Auto_Btn);
            view.Btn_Restart.onClick.RemoveListener(Restart_Btn);
            ArrayNode.DestoryArrayNodes(Area, demo.nodes);
        }

        private  SortBasicModel initialize()
        {
            SortBasicModel SBM;
            List<ArrayNode> nodes = ArrayNode.CreatArryNodes(data_length, data_type, view.NODE, Area, Area.transform.parent.GetComponent<GridLayoutGroup>().cellSize, nums, 0f);
            if (sort_model == model.STUDYMODEL)
            {
                switch (sort_type)
                {
                    case sortType.INSERTSORT:
                        SBM = new InsertSortStudyModel(nodes);
                        break;
                    case sortType.SELECTSORT:
                        SBM = new SelectSortStudyModel(nodes);
                        break;
                    case sortType.BUBBLESORT:
                        SBM = new BubbleSortStudyModel(nodes);
                        break;
                    case sortType.SHELLSORT:
                        SBM = new ShellSortStudyModel(nodes);
                        break;
                    case sortType.MERGESORT:
                        SBM = new MergeSortStudyModel(nodes);
                        break;
                    default:
                    case sortType.QUICKSORT:
                        SBM = new QuickSortStudyModel(nodes);
                        break;
                }
                SetInitialAnnotationText();
            }
            else
            {
                switch (sort_type)
                {
                    case sortType.INSERTSORT:
                        SBM = new InsertSortDemoModel(nodes);
                        break;
                    case sortType.SELECTSORT:
                        SBM = new SelectSortDemoModel(nodes);
                        break;
                    case sortType.BUBBLESORT:
                        SBM = new BubbleSortDemoModel(nodes);
                        break;
                    case sortType.SHELLSORT:
                        SBM = new ShellSortDemoModel(nodes);
                        break;
                    case sortType.MERGESORT:
                        SBM = new MergeSortDemoModel(nodes);
                        break;
                    default:
                    case sortType.QUICKSORT:
                        SBM = new QuickSortDemoModel(nodes);
                        break;
                }
            }
            
            SBM.RecordProce();
            return SBM;
        }
  
        public void SetInitialAnnotationText()
        {
            switch ((int)sort_type)
            {
                case 0:
                    view.Text_Annotation.text = "<color='red'>插入排序</color>：从第二个元素开始，依次向前插入在相应位置，插入位置的前一个元素应当不大于插入元素";
                    break;
                case 1:
                    view.Text_Annotation.text = "<color='red'>选择排序</color>：找出最小元素，与第一个元素交换，再从剩余元素中找最小元素，与第二个元素交换，以此类推，直至数组有序";
                    break;
                case 2:
                    view.Text_Annotation.text = "<color='red'>冒泡排序</color>：从前往后，依次比较相邻的两个元素，将较大的元素向后移，重复该操作，直至数组有序";
                    break;
                case 3:
                    view.Text_Annotation.text = "<color='red'>希尔排序</color>：特殊的插入排序。先取一个小于数组长度的数d，可将数组分为d个组，所有距离为d的倍数的元素为同一组，在各组内进行插入排序。再取一个更小的d，重复上述操作，直到d为0，排序结束。（本例中选初始增量d为N/2，缩量d=d/2）";
                    break;
                case 4:
                    view.Text_Annotation.text = "<color='red'>归并排序</color>：将数组划分为若干个数量不多于两个的子数组，可直接进行归并，再不断两两合并成一个较大的有序数组，最终得到一个有序数组";
                    break;
                case 5:
                    view.Text_Annotation.text = "<color='red'>快速排序</color>：将要排序的数据分割成独立的两部分，使得其中一部分的所有元素比另一部分的所有元素都要小，然后再按此方法对这两部分数据分别进行处理，以此类推，直至数组有序";
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
            view.Btn_LastStep.interactable = false;
            view.Btn_NextStep.interactable = true;
            view.Btn_StartButton.interactable = true;
            view.Text_StartButton.text = "自动";
            view.Text_Annotation.alignment = TextAnchor.MiddleLeft;
            demo = initialize() ;
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
