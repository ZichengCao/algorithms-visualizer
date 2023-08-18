using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class SortView : MonoBehaviour
    {
        public SortControl[] Control;
        private int DemoArea_count = 2;
        public Button Btn_Restart;
        public Button Btn_NextStep;
        public Button Btn_StartButton;
        public Button Btn_LastStep;
        public Text Text_StartButton;
        public Text Text_Annotation;

        public GameObject NODE;//预制体 
        public GameObject GO_BORDER;//预制体 

        #region TopButton
        public GameObject[] SortButtonGroups;
        private bool[] SortButtonGroupsFlag = { true, true, false, false, false, false };
        [HideInInspector] public List<Image> images_SortButton = new List<Image>();
        #endregion

        #region LeftButton
        public Image image_CompareDemoSwitch;
        public Dropdown Drop_DataNums;
        public Dropdown Drop_DataType;
        #endregion

        #region CompareArea
        public GameObject CompareDemoArea;
        private GridLayoutGroup Grid;
        #endregion
        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < SortButtonGroups.Length; i++)
            {
                images_SortButton.Add(SortButtonGroups[i].GetComponent<Image>());
            }
            Grid = CompareDemoArea.GetComponent<GridLayoutGroup>();
            //Grid.cellSize = new Vector2(100, 250);l
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region TopButton
        public void ModelSelect()
        {
            if (SortControl.sort_model == model.DEMOMODEL)
            {
                SortControl.sort_model = model.STUDYMODEL;
                Clean();
                ArrayNode.verticalStandard = 100f;
                Control[0].gameObject.GetComponentInChildren<Text>().text = "" ;
                image_CompareDemoSwitch.color = new Color(1f, 1f, 1f);
                image_CompareDemoSwitch.gameObject.GetComponentInChildren<Text>().text = "对比演示：关";
                Btn_LastStep.gameObject.SetActive(true);
                Btn_NextStep.gameObject.SetActive(true);
                Text_Annotation.gameObject.SetActive(true);
                for (int i = 1; i < 4; i++)
                {
                    Control[i].gameObject.SetActive(false);
                }
                //Control[0].gameObject.GetComponent<Image>().enabled = false;
                Control[0].sort_type = sortType.INSERTSORT;
                Control[0].gameObject.SetActive(true);
                ResetCompareDemoNums();
                Control[0].Restart_Btn();
            }
            else
            {
                SortControl.sort_model = model.DEMOMODEL;
                GridLayoutGroupResize(2);

                switch (SortControl.data_type)
                {
                    case 0:
                        SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                        break;
                    case 1:
                        SortControl.nums = MyTools.GetRandomList(SortControl.data_length);
                        break;
                    case 2:
                        SortControl.nums = MyTools.GetAscendList(SortControl.data_length);
                        break;
                    case 3:
                        SortControl.nums = MyTools.GetDescendList(SortControl.data_length);
                        break;
                    case 4:
                        SortControl.nums = MyTools.GetAlmostOrderedList(SortControl.data_length);
                        break;
                    case 5:
                        SortControl.nums = MyTools.GetEqualList(SortControl.data_length);
                        break;
                    default:
                        SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                        break;
                }
                ArrayNode.verticalStandard = 40f;
                Btn_LastStep.gameObject.SetActive(false);
                Btn_NextStep.gameObject.SetActive(false);
                image_CompareDemoSwitch.color = MyTools.Color_HexToRgb(MainControl.ColorSetting["blueButton"]);
                image_CompareDemoSwitch.gameObject.GetComponentInChildren<Text>().text = "对比演示：开";
                Text_Annotation.gameObject.SetActive(false);
                Control[0].sort_type = sortType.INSERTSORT;
                Control[0].gameObject.SetActive(false);
                Control[0].gameObject.SetActive(true);
                //Control[0].gameObject.GetComponent<Image>().enabled = true;
                //Control[0].GetComponentInChildren<Text>().text = "插入排序";
                Control[1].sort_type = sortType.SELECTSORT;
                SortButtonGroups[0].GetComponent<Image>().color = new Color(0.96078f, 0.56470f, 0.51764f);
                SortButtonGroups[1].GetComponent<Image>().color = new Color(0.96078f, 0.56470f, 0.51764f);
                Control[1].gameObject.SetActive(true);
                for (int i = 2; i < 6; i++)
                {
                    SortButtonGroups[i].GetComponent<Button>().interactable = true;
                    SortButtonGroups[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                }
                fontResize(2);
            }
        }

        public void Clean()
        {
            SortButtonGroups[0].GetComponent<Image>().color = new Color(0.96078f, 0.56470f, 0.51764f);
            for (int i = 1; i < 6; i++)
            {
                SortButtonGroups[i].GetComponent<Button>().interactable = true;
                SortButtonGroups[i].GetComponent<Image>().color = new Color(1f, 1f, 1f);
            }
            GridLayoutGroupResize(1);
            fontResize(1);
            DemoArea_count = 2;
            SortButtonGroupsFlag[0] = true;
            SortButtonGroupsFlag[1] = true;
            SortButtonGroupsFlag[2] = false;
            SortButtonGroupsFlag[3] = false;
            SortButtonGroupsFlag[4] = false;
            SortButtonGroupsFlag[5] = false;
        }
        #endregion

        #region LeftButton

        public void SetSortType_Button(int index)
        {
            if (SortControl.sort_model == model.STUDYMODEL)
            {
                if (index != (int)Control[0].sort_type)
                {
                    images_SortButton[(int)Control[0].sort_type].color = new Color(1f, 1f, 1f);
                    Control[0].sort_type = (sortType)index;
                    images_SortButton[(int)Control[0].sort_type].color = new Color(0.96078f, 0.56470f, 0.51764f);
                    Control[0].Restart_Btn();
                }
            }
            else
            {
                //情况1：未满，增加一个
                //情况2：未满，删除一个
                //情况3：已满，删除一个，同上
                //情况4：已满，余下不交互
                //删除一个
                if (SortButtonGroupsFlag[index])
                {
                    //只有一个，不删
                    if (DemoArea_count == 1) return;
                    SortButtonGroupsFlag[index] = false;
                    string name = SortButtonGroups[index].GetComponentInChildren<Text>().text;
                    SortButtonGroups[index].GetComponent<Image>().color = new Color(1f, 1f, 1f);
                    for (int i = 0; i < 4; i++)
                    {
                        if (Control[i].gameObject.GetComponentInChildren<Text>().text.Equals(name))
                        {
                            Control[i].gameObject.SetActive(false);
                        }
                    }
                    //原来满的，减少一个
                    if (DemoArea_count == 4)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            SortButtonGroups[i].GetComponent<Button>().interactable = true;
                        }
                    }
                    DemoArea_count--;
                }
                else
                {
                    //未满，增加一个
                    if (DemoArea_count < 4)
                    {
                        SortButtonGroupsFlag[index] = true;
                        SortButtonGroups[index].GetComponent<Image>().color = new Color(0.96078f, 0.56470f, 0.51764f);
                        for (int i = 0; i < 4; i++)
                        {
                            if (!Control[i].gameObject.activeSelf)
                            {
                                Control[i].sort_type = (sortType)index;
                                Control[i].gameObject.SetActive(true);
                                break;
                            }
                        }

                        DemoArea_count++;
                    }
                    //满了，余下按钮拒绝响应
                    if (DemoArea_count == 4)
                    {
                        for (int i = 0; i < 6; i++)
                        {
                            if (!SortButtonGroupsFlag[i])
                            {
                                SortButtonGroups[i].GetComponent<Button>().interactable = false;
                            }
                        }
                    }
                }
                GridLayoutGroupResize(DemoArea_count);
                fontResize(DemoArea_count);
                for (int i = 0; i < 4; i++)
                {
                    if (Control[i].gameObject.activeSelf)
                    {
                        Control[i].Restart_Btn();
                    }
                }
            }

        }
        private void GridLayoutGroupResize(int count)
        {

            if (count == 1)
            {
                Grid.cellSize = new Vector2(1000, 500);

            }
            else if (count == 2)
            {
                Grid.cellSize = new Vector2(1000, 250);
            }
            else if (count == 3 || count == 4)
            {
                Grid.cellSize = new Vector2(500, 250);
            }
            else return;
        }

        private void fontResize(int count)
        {
            int[] sizePreset = { 30, 25, 20, 20 };
            for (int i = 0; i < 4; i++)
            {
                if (Control[i].gameObject.activeSelf)
                {
                    Control[i].GetComponentInChildren<Text>().fontSize = sizePreset[count - 1];
                }
            }
        }

        #endregion

        #region RightButton
        public void SetDataNums_Btn()
        {
            int[] value_to_nums = { 5, 10, 15, 20, 30, 50 };
            SortControl.data_length = value_to_nums[Drop_DataNums.value];

            switch (SortControl.data_type)
            {
                case 0:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
                case 1:
                    SortControl.nums = MyTools.GetRandomList(SortControl.data_length);
                    break;
                case 2:
                    SortControl.nums = MyTools.GetAscendList(SortControl.data_length);
                    break;
                case 3:
                    SortControl.nums = MyTools.GetDescendList(SortControl.data_length);
                    break;
                case 4:
                    SortControl.nums = MyTools.GetAlmostOrderedList(SortControl.data_length);
                    break;
                case 5:
                    SortControl.nums = MyTools.GetEqualList(SortControl.data_length);
                    break;
                default:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
            }

            for (int i = 0; i < 4; i++)
            {
                if (Control[i].gameObject.activeSelf)
                {
                    Control[i].Restart_Btn();
                }
            }
        }
        public void SetDataType_Btn()
        {
            SortControl.data_type = Drop_DataType.value;
            switch (SortControl.data_type)
            {
                case 0:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
                case 1:
                    SortControl.nums = MyTools.GetRandomList(SortControl.data_length);
                    break;
                case 2:
                    SortControl.nums = MyTools.GetAscendList(SortControl.data_length);
                    break;
                case 3:
                    SortControl.nums = MyTools.GetDescendList(SortControl.data_length);
                    break;
                case 4:
                    SortControl.nums = MyTools.GetAlmostOrderedList(SortControl.data_length);
                    break;
                case 5:
                    SortControl.nums = MyTools.GetEqualList(SortControl.data_length);
                    break;
                default:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
            }
            for (int i = 0; i < 4; i++)
            {
                if (Control[i].gameObject.activeSelf)
                {
                    Control[i].Restart_Btn();
                }
            }
        }

        public void ResetCompareDemoNums()
        {
            switch (SortControl.data_type)
            {
                case 0:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
                case 1:
                    SortControl.nums = MyTools.GetRandomList(SortControl.data_length);
                    break;
                case 2:
                    SortControl.nums = MyTools.GetAscendList(SortControl.data_length);
                    break;
                case 3:
                    SortControl.nums = MyTools.GetDescendList(SortControl.data_length);
                    break;
                case 4:
                    SortControl.nums = MyTools.GetAlmostOrderedList(SortControl.data_length);
                    break;
                case 5:
                    SortControl.nums = MyTools.GetEqualList(SortControl.data_length);
                    break;
                default:
                    SortControl.nums = MyTools.GetNoRepeatList(SortControl.data_length);
                    break;
            }
        }
        #endregion

    }

}
