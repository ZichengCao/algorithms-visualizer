using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyAlgorithm {
    class PracticeModel : MonoBehaviour
    {
        public SortBasicModel demo;
        public string QuestionText;
        public string AnswerText;
        public questionType QType;
        public sortType SortType;
        public int length;
        public int pass;
        private List<int> arr = new List<int>();
       
        //数组长度限制
        private int[] Floor = { 2, 2, 2, 1, 3, 1 };
        private int[] Ceil = { 4, 4, 4, 3, 5, 2 };
        public PracticeModel()
        {
           
        }

        public void GererateQuestion()
        {
            //问题类型
            QType = (questionType)Random.Range(0, 2);
            SortType = (sortType)Random.Range(0, 6);
            
            // SortType = sortType.INSERTSORT;
            //归并排序 交换次数没法判断
            if (QType == questionType.EXCHANGECOUNT && SortType == sortType.MERGESORT)
            {
                SortType = sortType.QUICKSORT;
            }

            //设置数组长度
            if (QType == questionType.PASS)
            {
                length = Random.Range(5, 10);
            }
            //如果是计算交换、比较次数的问题，把数组数量整小一点
            else if (QType == questionType.EXCHANGECOUNT)
            {
                length = Random.Range(4, 6);
            }

            if (QType == questionType.PASS)
            {
                pass = Random.Range(Floor[(int)SortType], Ceil[(int)SortType]);
                string str_1 = "在此版本的{" + Basis.SortName[(int)SortType] + "}下，给定下列数据，请给出第{" + pass + "}次执行完";
                string str_2, str_3;
                if (SortType == sortType.QUICKSORT)
                {
                    str_2 = "{Partition}函数时";
                }
                else if (SortType == sortType.MERGESORT)
                {
                    str_2 = "{Merge}函数时";
                }
                else
                {
                    str_2 = "{最外层}循环时";
                }
                str_3 = "的数据情况";
                QuestionText = MyTools.BoldImportantWord(str_1 + str_2 + str_3);
            }
            else if (QType == questionType.EXCHANGECOUNT)
            {
                string str = "在此版本的{" + Basis.SortName[(int)SortType] + "}下，给定下列数据，排序完成会进行多少次{交换}？";
                QuestionText = MyTools.BoldImportantWord(str);
            }
        }

        public void RecordProce(List<ArrayNode> nodes)
        {
            arr.Clear();
            for (int i = 0; i < nodes.Count; i++)
            {
                arr.Add(nodes[i].num);
            }
            switch (SortType)
            {
                case sortType.BUBBLESORT:
                    demo = new BubbleSortDemoModel(nodes);
                    break;
                case sortType.INSERTSORT:
                    demo = new InsertSortDemoModel(nodes);
                    break;
                case sortType.SELECTSORT:
                    demo = new SelectSortDemoModel(nodes);
                    break;
                case sortType.SHELLSORT:
                    demo = new ShellSortDemoModel(nodes);
                    break;
                case sortType.QUICKSORT:
                    demo = new QuickSortDemoModel(nodes);
                    break;
                case sortType.MERGESORT:
                    demo = new MergeSortDemoModel(nodes);
                    break;
                default:
                    break;
            }
            demo.RecordProce(QType == questionType.PASS ? pass : -1);
        }
        public bool CheckAnswer(string response)
        {
            AnswerText = "";
            if (QType == questionType.PASS)
            {
                for (int i = 0; i < demo.arr.Count; i++)
                {
                    AnswerText += demo.arr[i].num.ToString() + "  ";
                }
            }

            else if (QType == questionType.EXCHANGECOUNT)
            {
                AnswerText = MyTools.GetSwapCount(arr, SortType).ToString();
            }

            if (AnswerText.CompareTo(response) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

