using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FunnyAlgorithm
{

    public class SearchModel : MonoBehaviour
    {

    }

    public abstract class SearchBasicModel : SortBasicModel
    {
        
        public Text annotation; //上方文字讲解
        public int x;

        protected SearchBasicModel(int x, List<ArrayNode>Nodes) : base(Nodes)
        {
            this.x = x;
        }

        public override bool PlayForward()
        {
            bool flag = true;
            if (!IsRun)
            {
                IsRun = true;
                Activity LOG = demoQueue.Peek();
                if (LOG.type == activityType.NOTE)
                {
                    NoteEvent log = (NoteEvent)demoQueue.Dequeue();
                    annotation.text = log.note;
                }
                else if (LOG.type == activityType.MOVE)
                {
                    Movement log = (Movement)demoQueue.Dequeue();
                    nodes[log.index].Move(log.x_steps, log.y_steps); ;
                }
                else if (LOG.type == activityType.TURN_COLOR)
                {
                    TurnColor log = (TurnColor)demoQueue.Dequeue();
                    nodes[log.index].SetColor(log.targetColor);
                }
                else if (LOG.type == activityType.DISABLE)
                {
                    Disable log = (Disable)demoQueue.Dequeue();
                    nodes[log.index].SetColor(MainControl.ColorSetting["disable"]);
                }

                executedStack.Push(LOG);
                IsRun = false;
                return LOG.hasNext && flag;
            }
            return false;
        }
        public override bool PlayBackward()
        {
            if (!IsRun)
            {
                bool flag = true;
                IsRun = true;
                Activity LOG = executedStack.Peek();
                if (LOG.type == activityType.NOTE)
                {
                    NoteEvent log = (NoteEvent)executedStack.Pop();
                    annotation.text = log.note;
                }
                else if (LOG.type == activityType.MOVE)
                {
                    Movement log = (Movement)executedStack.Pop();
                    nodes[log.index].Move(-log.x_steps, -log.y_steps);
                }
                else if (LOG.type == activityType.TURN_COLOR)
                {
                    TurnColor log = (TurnColor)executedStack.Pop();
                    nodes[log.index].SetColor(log.initialColor);
                }
                else if (LOG.type == activityType.DISABLE)
                {
                    Disable log = (Disable)executedStack.Pop();
                    nodes[log.index].SetColor(MainControl.ColorSetting["normal"]);
                }
                MyTools.QueuePushFront(demoQueue, LOG);
                IsRun = false;
                return (executedStack.Count > 0 && executedStack.Peek().hasNext) && flag ; 
            }
            return false;
        }
    }


    class LinearSearchModel : SearchBasicModel
    {
        public LinearSearchModel(int x, List<ArrayNode> Nodes) : base(x,Nodes)
        {
        }

        private void LinearSearch()
        {
            bool flag = false;//是否查找成功
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].num != x)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                }
                else
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["failed"], i != arr.Count - 1 ? true : false));
                }
                for (int i = 0; i < arr.Count; i++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["failed"], MainControl.ColorSetting["normal"], i != arr.Count - 1 ? true : false));
                }
            }
        }

        public override void RecordProce(int pass=0)
        {
            LinearSearch();
        }
    }

    class LinearSearchStudyModel : LinearSearchModel
    {
        public LinearSearchStudyModel(int x, List<ArrayNode> Nodes) : base(x, Nodes)
        {
            this.annotation = GameObject.Find("ToolsMan").GetComponent<SearchView>().Text_Annotation;

        }

        private void LinearSearch()
        {
            string str = "";
            bool flag = false;//是否查找成功
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i].num != x)
                {
                    str = "第" + MyTools.ColorText("(" + i + ")", "blue") + "个元素" + MyTools.ColorText(arr[i].num.ToString(), "red") + "不等于目标" + MyTools.ColorText(x.ToString(), "red") + "，继续比较下一个元素";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                }
                else
                {
                    str = "第" + MyTools.ColorText("(" + i + ")", "blue") + "个元素" + MyTools.ColorText(arr[i].num.ToString(), "red") + "等于目标" + MyTools.ColorText(x.ToString(), "red") + "，查找成功";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                for (int i = 0; i < arr.Count; i++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["failed"], i == arr.Count - 1 ? false : true));
                }
                str = "数组中不存在目标元素，查找失败";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                for (int i = 0; i < arr.Count; i++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["failed"], MainControl.ColorSetting["normal"], i == arr.Count - 1 ? false : true));
                }

            }
        }

        public override void RecordProce(int pass=0)
        {
            LinearSearch();
        }
    }
    class BinarySearchModel : SearchBasicModel
    {
        public BinarySearchModel(int x, List<ArrayNode> Nodes) : base(x,Nodes)
        {
        }

        private void BinarySearch()
        {
            bool[] DisableFlag = new bool[arr.Count];
            for (int i = 0; i < arr.Count; i++) DisableFlag[i] = false;
            int left = 0, right = arr.Count - 1;
            while (left <= right)
            {
                for (int i = 0; i < left; i++)
                {
                    if (!DisableFlag[i])
                    {
                        demoQueue.Enqueue(new Disable(activityType.DISABLE, arr[i].index, true));
                        DisableFlag[i] = true;
                    }
                }
                for (int i = right + 1; i < arr.Count; i++)
                {
                    if (!DisableFlag[i])
                    {
                        demoQueue.Enqueue(new Disable(activityType.DISABLE, arr[i].index, true));
                        DisableFlag[i] = true;
                    }
                }
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[right].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[right].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                int mid = (left + right) >> 1;
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], false));

                if (arr[mid].num == x)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false));
                    break;
                }
                else
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["failed"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["failed"], MainControl.ColorSetting["normal"], false));
                    if (arr[mid].num > x)
                    {
                        right = mid - 1;
                    }
                    else if (arr[mid].num < x)
                    {
                        left = mid + 1;
                    }
                }
            }
        }
        public override void RecordProce(int pass=0)
        {
            BinarySearch();
        }
    }

    class BinarySearchStudyModel : BinarySearchModel
    {
        public BinarySearchStudyModel(int x, List<ArrayNode> Nodes) : base(x, Nodes)
        {
            this.annotation = GameObject.Find("ToolsMan").GetComponent<SearchView>().Text_Annotation;
        }

        private void BinarySearch()
        {
            string str = "";
            bool[] DisableFlag = new bool[arr.Count];
            for (int i = 0; i < arr.Count; i++) DisableFlag[i] = false;
            int left = 0, right = arr.Count - 1;
            while (left <= right)
            {
                bool flag = false;
                for (int i = 0; i < left; i++)
                {
                    if (!DisableFlag[i])
                    {
                        flag = true;
                        demoQueue.Enqueue(new Disable(activityType.DISABLE, arr[i].index, true));
                        DisableFlag[i] = true;
                    }
                }
                for (int i = right + 1; i < arr.Count; i++)
                {
                    if (!DisableFlag[i])
                    {
                        flag = true;
                        demoQueue.Enqueue(new Disable(activityType.DISABLE, arr[i].index, true)); 
                        DisableFlag[i] = true;
                    }
                }
                if (flag)
                {
                    Activity T = MyTools.GetQueueTail(demoQueue);
                    T.hasNext = false;
                    demoQueue.Enqueue(T);
                }
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[right].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[right].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                int mid = (left + right) >> 1;

                if (arr[mid].num == x)
                {
                    str = "当前查找范围中间元素" + MyTools.ColorText(x.ToString(), "red") + "等于目标" + MyTools.ColorText(x.ToString(), "red") + "，查找成功";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));

                    //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["successed"], false));
                    //DemoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false));
                    break;
                }
                else
                {
                    if (left == right)
                    {
                        str = "数组中不存在目标元素，查找失败";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                        demoQueue.Enqueue(new Disable(activityType.DISABLE, arr[left].index, false));
                        break;
                    }
                    else
                    {
                        if (arr[mid].num > x)
                        {
                            str = "当前查找范围中间元素" + MyTools.ColorText(arr[mid].num.ToString(), "red") + "大于目标" + MyTools.ColorText(x.ToString(), "red") + "，目标值只可能存在当前范围的左半部分（如果存在的话），在左半部分继续查找";
                            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                            right = mid - 1;
                        }
                        else if (arr[mid].num < x)
                        {
                            str = "当前查找范围中间元素" + MyTools.ColorText(arr[mid].num.ToString(), "red") + "小于目标" + MyTools.ColorText(x.ToString(), "red") + "，目标值只可能存在当前范围的右半部分（如果存在的话），在右半部分继续查找";
                            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                            left = mid + 1;
                        }
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[mid].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                    }
                    
                }
            }
        }
        public override void RecordProce(int pass=0)
        {
            BinarySearch();
        }
    }
}
