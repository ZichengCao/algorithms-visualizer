using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyAlgorithm
{

    public class SortBasicModel : DemoModel<ArrayNode>
    {
        public List<Part> arr = new List<Part>();//index,value键值对
        //public List<ArrayNode> nodes = new List<ArrayNode>();//结点
        public SortBasicModel(List<ArrayNode> Nodes)
        {
            base.nodes = Nodes;
            for (int i = 0; i < Nodes.Count; i++)
            {
                arr.Add(new Part(Nodes[i].num, i));
            }
        }

        public override bool PlayForward()
        {
            
            if (!IsRun)
            {
                IsRun = true;
                Activity activity = demoQueue.Peek();
                if (activity.type == activityType.MOVE)
                {
                    Movement move = (Movement)demoQueue.Dequeue();
                    nodes[move.index].Move(move.x_steps, move.y_steps); ;
                }
                else if (activity.type == activityType.TURN_COLOR)
                {
                    TurnColor turn = (TurnColor)demoQueue.Dequeue();
                    nodes[turn.index].SetColor(turn.targetColor);
                }
                executedStack.Push(activity);
                IsRun = false;
                return activity.hasNext;
            }
            return false;
        }
        public override bool PlayBackward()
        {
            if (!IsRun)
            {
                IsRun = true;
                Activity LOG = executedStack.Peek();
                if (LOG.type == activityType.TURN_COLOR)
                {
                    TurnColor log = (TurnColor)executedStack.Pop();
                    nodes[log.index].SetColor(log.initialColor);
                }
                else if (LOG.type == activityType.MOVE)
                {
                    Movement log = (Movement)executedStack.Pop();
                    nodes[log.index].Move(-log.x_steps, -log.y_steps);
                }
                MyTools.QueuePushFront(demoQueue, LOG);
                IsRun = false;
                return (executedStack.Count > 0 && executedStack.Peek().hasNext);
            }
            return false;
        }

        public virtual void Finish()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], i == arr.Count - 1 ? false : true));
            }
        }


        public static void DestoryArrayNodes(GameObject DemoArea, List<ArrayNode> nodes)
        {
            for (int i = 0; i < DemoArea.transform.childCount; i++)
            {
                if (DemoArea.transform.GetChild(i).gameObject.tag == "NODE")
                    Destroy(DemoArea.transform.GetChild(i).gameObject);
            }
            nodes.Clear();
        }
    }
    abstract class SortStudyModel : SortBasicModel
    {
        public Text annotation; //上方文字讲解
        protected GameObject Demo_Area;//演示区域
        protected GameObject BORDER;//边框预制体
        protected List<Vector2> OriginalPosition = new List<Vector2>();
        protected string str = "";
        protected RectTransform L, M, R;
        protected Border B;//边框对象
        protected List<Border> BB = new List<Border>();//这是干啥的，我忘了

        public SortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
            this.annotation = GameObject.Find("ToolsMan").GetComponent<SortView>().Text_Annotation;
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
                else if (LOG.type == activityType.RESHAPE_BORDER)
                {
                    ReShape log = (ReShape)demoQueue.Dequeue();
                    if (!log.flag) executedStack.Pop();
                    if (M != null && (B.left - B.right) % 2 != (log.left - log.right) % 2)
                    {
                        if ((log.right - log.left) % 2 == 1)
                        {
                            float x = M.anchoredPosition.x - ArrayNode.width / 2;
                            M.DOAnchorPosX(x, MoveTool.duration);
                        }
                        else
                        {
                            float x = M.anchoredPosition.x + ArrayNode.width / 2;
                            M.DOAnchorPosX(x, MoveTool.duration);
                        }
                    }
                    B.ReShape(log.left, log.right);
                    flag = false;
                    //这里必须要等一下，因为调用PlayForward的地方无法判断此次Forward会进行多长时间，是根据IsRun来判断的，这里如果不等，会有下一个Reshape动作抢拍，达不到想要实现的效果
                    //yield return new WaitForSeconds(MoveTool.duration);
                }
                else if (LOG.type == activityType.DRAW_BORDER)
                {
                    DrawBorder log = (DrawBorder)demoQueue.Dequeue();
                    if (!log.flag) executedStack.Pop();
                    CreatBorder(log.left, log.right, log.HasSign);
                }
                else if (LOG.type == activityType.DESTORY_BORDER)
                {
                    DestoryBorder log = (DestoryBorder)demoQueue.Dequeue();
                    if (!log.flag) executedStack.Pop();
                    for (int i = 0; i < BB.Count; i++)
                        Destroy(BB[i].g);
                    BB.Clear();
                }
                else if (LOG.type == activityType.UPDATE_NODE_HEADNOTE)
                {
                    UpdateNodeHeadnote log = (UpdateNodeHeadnote)demoQueue.Dequeue();
                    if (log.flag)
                    {
                        nodes[log.index].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    else
                    {
                        nodes[log.index].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
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
                else if (LOG.type == activityType.DRAW_BORDER)
                {
                    executedStack.Pop();
                    Destroy(B.g);
                }
                else if (LOG.type == activityType.RESHAPE_BORDER)
                {
                    ReShape log = (ReShape)executedStack.Pop();
                    if (M != null && (B.left - B.right) % 2 != (log.OriginLeft - log.OriginRight) % 2)
                    {

                        if ((log.OriginRight - log.OriginLeft) % 2 == 1)
                        {
                            float x = M.anchoredPosition.x - ArrayNode.width / 2;
                            M.DOAnchorPosX(x, MoveTool.duration);
                        }
                        else
                        {
                            float x = M.anchoredPosition.x + ArrayNode.width / 2;
                            M.DOAnchorPosX(x, MoveTool.duration);
                        }
                    }
                    B.ReShape(log.OriginLeft, log.OriginRight);
                    flag = false;
                }
                else if (LOG.type == activityType.DESTORY_BORDER)
                {
                    DestoryBorder log = (DestoryBorder)executedStack.Pop();
                    CreatBorder(log.left, log.right, log.HasSign);
                }
                else if (LOG.type == activityType.UPDATE_NODE_HEADNOTE)
                {
                    UpdateNodeHeadnote log = (UpdateNodeHeadnote)executedStack.Pop();
                    if (log.flag)
                    {
                        nodes[log.index].gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                    else
                    {
                        nodes[log.index].gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    }
                }

                MyTools.QueuePushFront(demoQueue, LOG);
                IsRun = false;
                return (executedStack.Count > 0 && executedStack.Peek().hasNext) && flag;
            }
            return false;
        }
        private void CreatBorder(int left, int right, int flag)
        {
            int mid = left + right >> 1;
            B = new Border(Instantiate(BORDER, Demo_Area.transform), left, right);
            B.rect.sizeDelta = new Vector2((right - left + 1) * Border.width, B.rect.sizeDelta.y);
            B.rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 200);
            float x = OriginalPosition[mid].x;
            if ((right - left) % 2 == 1)
                x += Border.width / 2;
            B.rect.anchoredPosition = new Vector2(x, OriginalPosition[mid].y);
            BB.Add(B);

            L = B.g.transform.GetChild(0).GetComponent<RectTransform>();
            R = B.g.transform.GetChild(1).GetComponent<RectTransform>();
            M = B.g.transform.GetChild(2).GetComponent<RectTransform>();

            if (flag == 0)
            {
                L.gameObject.SetActive(false);
                R.gameObject.SetActive(false);
                M.gameObject.SetActive(false);
                M = null;
            }
            else
            {
                if (flag == 1)
                {
                    M.gameObject.SetActive(false);
                    M = null;
                }
                else if (flag == 2)
                {
                    if ((right - left) % 2 == 0)
                    {
                        x = M.anchoredPosition.x + ArrayNode.width / 2;
                        M.DOAnchorPosX(x, MoveTool.duration); ;
                    }
                }
                float temp_x;
                if (Border.width >= 40)
                {
                    temp_x = 40f;
                }
                else
                {
                    temp_x = Border.width >= 10 ? Border.width : 10;
                }

                //设置Border上面L和R的宽度

                L.sizeDelta = new Vector2(temp_x, 40f);
                L.anchoredPosition = new Vector2(0, 25);
                R.anchoredPosition = new Vector2(0, 25);
                R.sizeDelta = new Vector2(temp_x, 40f);
                if (M != null)
                {
                    M.GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 25);
                    M.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(temp_x, 40f);
                }
            }
        }
    }


    class BubbleSortDemoModel : SortBasicModel
    {
        public BubbleSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void BubbleSort(int pass)
        {
            if (pass == 0) pass = arr.Count;
            for (int i = 0; i < arr.Count && pass != 0; i++, pass--)
            {
                for (int j = 0; j < arr.Count - i - 1; j++)
                {
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, true)); ;
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, 2, false)); ;
                    if (arr[j].num > arr[j + 1].num)
                    {
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 1, 0, true));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, -1, 0, false));
                        arr[j].Swap(arr[j + 1]);
                    }
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, true)); ;
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, -2, false));
                }
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[arr.Count - i - 1].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
            }
        }
        public override void RecordProce(int pass = 0)
        {
            BubbleSort(pass);
        }
    }
    class BubbleSortStudyModel : SortStudyModel
    {
        public BubbleSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void BubbleSort()
        {
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = 0; j < arr.Count - i - 1; j++)
                {
                    if (arr[j].num > arr[j + 1].num)
                    {
                        str = "比较相邻的两个元素，<color='red'> " + arr[j].num + " </color>比<color='red'> " + arr[j + 1].num + " </color>大，交换两个元素";
                    }
                    else if (arr[j].num == arr[j + 1].num)
                    {
                        str = "比较相邻的两个元素，<color='red'> " + arr[j].num + " </color>等于<color='red'> " + arr[j + 1].num + " </color>，无需移动";
                    }
                    else
                    {
                        str = "比较相邻的两个元素，<color='red'> " + arr[j].num + " </color>比<color='red'> " + arr[j + 1].num + " </color>小，无需移动";
                    }
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, true)); ;
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, 2, false)); ;
                    if (arr[j].num > arr[j + 1].num)
                    {
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 1, 0, true));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, -1, 0, false));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, true));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, -2, false));
                        arr[j].Swap(arr[j + 1]);
                    }
                    else
                    {
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, true)); ;
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, -2, false));
                    }
                }

                SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = !T.hasNext; demoQueue.Enqueue(T);

                str = "无序部分的最大值<color='red'> " + arr[arr.Count - i - 1].num + " </color>到达右端，将其标记为有序。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[arr.Count - i - 1].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
            }

        }

        public override void RecordProce(int pass = 0)
        {
            BubbleSort();
            SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue);
            T.hasNext = true;
            demoQueue.Enqueue(T);
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
        }
    }


    class InsertSortDemoModel : SortBasicModel
    {
        public InsertSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void InsertSort(int pass)
        {
            if (pass == 0) pass = arr.Count;

            for (int i = 1; i < arr.Count && pass != 0; i++, pass--)
            {
                int j = i;
                Part temp = new Part(arr[i]);
                //demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, 0, 2, false));
                while (j > 0)
                {
                    if (temp.num < arr[j - 1].num)
                    {
                        demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -1, 0, true)); ;
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j - 1].index, 1, 0, false));
                        arr[j].Assign(arr[j - 1]);
                    }
                    else
                        break;
                    j--;
                }
                //demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
                arr[j].Assign(temp);
            }
            Finish();
        }

        public override void RecordProce(int pass = 0)
        {
            InsertSort(pass);

        }
    }

    class InsertSortStudyModel : SortStudyModel
    {
        public InsertSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void InsertSort()
        {
            //Debug.Log(arr.Count);
            for (int i = 1; i < arr.Count; i++)
            {
                int j = i;
                Part temp = new Part(arr[i]);
                str = "现在选中第<color=\"blue\"> (" + i + ") </color>个元素进行向前插入操作。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, 0, 2, false)); ;
                while (j > 0)
                {
                    if (temp.num < arr[j - 1].num) str = "<color=\"red\"> " + temp.num + " </color>" + "比前面的元素" + "<color=\"red\"> " + arr[j - 1].num + " </color>" + "小，继续向前进行比较。";
                    else if (temp.num > arr[j - 1].num) str = "<color=\"red\"> " + temp.num + " </color>" + "比前面的元素" + "<color=\"red\"> " + arr[j - 1].num + " </color>" + "大，插入当前位置。";
                    else str = "<color=\"red\"> " + temp.num + " </color>" + "与前面的元素相等，插入当前位置。";
                    if (temp.num < arr[j - 1].num)
                    {
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -1, 0, true)); ;
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j - 1].index, 1, 0, true)); ;
                        arr[j].Assign(arr[j - 1]);
                    }
                    else
                        break;
                    j--;
                }
                arr[j].Assign(temp);
                if (j == 0) str = "<color=\"red\"> " + arr[j].num + " </color>" + "到达了最左端，插入当前位置。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, false)); ;
            }
            SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue);
            T.hasNext = true;
            demoQueue.Enqueue(T);
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));


        }
        public override void RecordProce(int pass = 0)
        {
            InsertSort();
            if (pass == 0)
                Finish();
        }
    }

    class MergeSortDemoModel : SortBasicModel
    {
        private List<Part> temp = new List<Part>();
        protected List<Vector2> OriginalPosition = new List<Vector2>();
        private int PASS = 0;

        public MergeSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {

        }
        private void merge(int left, int right)
        {
            if (PASS <= 0) return;

            int mid = (left + right) / 2;
  
            int i = left, j = mid + 1, k = left;

            while (i <= mid && j <= right)
            {
                if (arr[i].num <= arr[j].num)
                {
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - k), 1, false));
                    temp[k++].Assign(arr[i++]);
                }
                else
                {
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - k), 1, false)); ;
                    temp[k++].Assign(arr[j++]);
                }
            }

            while (i <= mid)
            {
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - k), 1, false));
                temp[k++].Assign(arr[i++]);
            }
            while (j <= right)
            {
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - k), 1, false)); ;
                temp[k++].Assign(arr[j++]);
            }
            for (int tt = left; tt <= right; tt++)
            {
                bool flag = false;
                if (tt != right) flag = true;
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[tt].index, 0, -1, flag));
            }
            while (left <= right)
            {
                arr[left].Assign(temp[left++]);
            }
        }

        private void mergeSort(int left, int right)
        {
            if (left < right && PASS != 0)
            {
                int mid = (left + right) / 2;
                mergeSort(left, mid);
                mergeSort(mid + 1, right);
                merge(left, right);
                PASS--;
            }
        }

        public override void RecordProce(int pass = 0)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                OriginalPosition.Add(base.nodes[i].rect.anchoredPosition);
                temp.Add(new Part());
            }
            PASS = pass;
            if (PASS == 0) PASS = arr.Count;
            mergeSort(0, arr.Count - 1);
            if (pass == 0)
                Finish();
        }
    }

    class MergeSortStudyModel : SortStudyModel
    {
        private List<Part> temp = new List<Part>();
        private int originalLeft, originalRight;

        public MergeSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
            BORDER = GameObject.Find("ToolsMan").GetComponent<SortView>().GO_BORDER;
            Demo_Area = GameObject.Find("ToolsMan").GetComponent<SortView>().Control[0].gameObject;

        }

        private void merge(int left, int right)
        {

            int mid = (left + right) / 2;
            //for (int tt = left; tt <= right; tt++)
            //{
            //    bool flag = false;
            //    if (tt != right) flag = true;
            //    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[tt].index, 0, 2, flag));
            //}
            int i = left, j = mid + 1, k = left;

            while (i <= mid && j <= right)
            {
                if (arr[i].num <= arr[j].num)
                {

                    if (arr[i].num < arr[j].num)
                        str = "比较<color='red'> " + arr[i].num + " </color>和<color='red'> " + arr[j].num + " </color>，<color='red'> " + arr[i].num + " </color>小于<color='red'> " + arr[j].num + " </color>，所以移动<color='red'> " + arr[i].num + " </color>";
                    else
                        str = "比较<color='red'> " + arr[i].num + " </color>和<color='red'> " + arr[j].num + " </color>，<color='red'> " + arr[i].num + " </color>等于<color='red'> " + arr[j].num + " </color>，为了使得排序保持稳定性，移动<color='red'> " + arr[i].num + " </color>";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));

                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - k), 2, false));
                    temp[k++].Assign(arr[i++]);
                }
                else
                {

                    str = "比较<color='red'> " + arr[i].num + " </color>和<color='red'> " + arr[j].num + " </color>，<color='red'> " + arr[i].num + " </color>大于<color='red'> " + arr[j].num + " </color>，所以移动<color='red'> " + arr[j].num + " </color>";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));

                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - k), 2, false)); ;
                    temp[k++].Assign(arr[j++]);
                }
            }
            //SortLog T = MyTools.GetQueueTail(DemoQueue);
            //T.hasNext = !T.hasNext;
            //DemoQueue.Enqueue(T);
            while (i <= mid)
            {
                //if (right - left > 1)
                //{
                //    str = "[M+1,R] 区间内已经没有元素了，继续移动[L,M]区间内的元素直至为空。";
                //    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                //}

                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - k), 2, false));
                temp[k++].Assign(arr[i++]);
            }
            while (j <= right)
            {
                //if (right - left > 1)
                //{
                //    str = "[L,M]区间内已经没有元素了，继续移动[M+1,R]区间内的元素直至为空。";
                //    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                //}
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - k), 2, false)); ;
                temp[k++].Assign(arr[j++]);
            }
            for (int tt = left; tt <= right; tt++)
            {
                bool flag = false;
                if (tt != right) flag = true;
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[tt].index, 0, -2, flag));
            }
            while (left <= right)
            {
                arr[left].Assign(temp[left++]);
            }
        }

        private void mergeSort(int left, int right)
        {
            if (left < right)
            {
                str = String.Format("进行递归划分，L = <color='blue'>{0}</color>, M = <color='blue'>{1}</color>, R = <color='blue'>{2}</color> ", left, (left + right) / 2, right);
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                if (left == 0 && right == arr.Count - 1)
                    demoQueue.Enqueue(new DrawBorder(activityType.DRAW_BORDER, 0, arr.Count - 1, false, 2));
                else
                    demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originalLeft, originalRight, left, right, right == left + 1 ? false : true));
                originalLeft = left; originalRight = right;
                int mid = (left + right) / 2;
                mergeSort(left, mid);
                mergeSort(mid + 1, right);
                String baseStr = String.Format("对 <color='blue'>[{0},{1}]</color>, <color='blue'>[{2},{3}]</color> 区间", left, mid, mid + 1, right);

                str = baseStr + "进行归并，依次选择区间较小的首元素，使得归并后的数组仍然保持有序";

                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                if (left != originalLeft || right != originalRight)
                {
                    demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originalLeft, originalRight, left, right, false, true)); ;
                    originalLeft = left; originalRight = right;
                }
                merge(left, right);
            }
        }

        public override void RecordProce(int pass = 0)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                OriginalPosition.Add(base.nodes[i].rect.anchoredPosition);
                temp.Add(new Part());
            }
            Border.width = base.nodes[0].rect.sizeDelta.x;
            Border.height = 200f;
            originalLeft = 0; originalRight = arr.Count - 1;
            mergeSort(0, arr.Count - 1);
            demoQueue.Enqueue(new DestoryBorder(activityType.DESTORY_BORDER, 0, arr.Count - 1, 2, true, -1, true));
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
            Finish();
        }
    }

    class QuickSortDemoModel : SortBasicModel
    {
        private int PASS = 0;
        public QuickSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void QuickSort(int left, int right, bool flag = true)
        {
            if (left == right && PASS > 0)
            {
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["normal"]
                    , MainControl.ColorSetting["successed"], false));
                return;
            }
            if (left <= right)
            {
                if (PASS <= 0) return;
                PASS--;
                int temp;
                if (flag)
                    temp = UnityEngine.Random.Range(left, right + 1);
                else
                    temp = left;
                //int temp = UnityEngine.Random.Range(left, right + 1);

                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[temp].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));

                if (temp != left)
                {
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[temp].index, -(temp - left), 0, true));
                    demoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, temp - left, 0, false));
                    arr[temp].Swap(arr[left]);
                }

                int x = arr[left].num;
                int i = left, j = right;
                while (i < j)
                {
                    while (i < j && arr[j].num >= x)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                        j--;
                    }

                    if (i < j)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], true));
                    }

                    while (i < j && arr[i].num <= x)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, i == left ? MainControl.ColorSetting["successed"] : MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["selected"], i == left ? MainControl.ColorSetting["successed"] : MainControl.ColorSetting["normal"], true));
                        i++;
                    }

                    if (i < j)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], true));
                    }

                    if (i < j)
                    {
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, j - i, 0, true));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - i), 0, true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false));
                        arr[i].Swap(arr[j]);
                    }
                }
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - left), 0, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, i - left, 0, true));
                if (i != left)
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false));
                arr[left].Swap(arr[i]);
                QuickSort(left, i - 1);
                QuickSort(i + 1, right);
            }
        }
        public override void RecordProce(int pass = 0)
        {
            PASS = pass;
            if (PASS <= 0) PASS = arr.Count;
            QuickSort(0, arr.Count - 1);
        }
    }

    class QuickSortStudyModel : SortStudyModel
    {
        public QuickSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
            BORDER = GameObject.Find("ToolsMan").GetComponent<SortView>().GO_BORDER;
            Demo_Area = GameObject.Find("ToolsMan").GetComponent<SortView>().Control[0].gameObject;
        }
        private void QuickSort(int left, int right)
        {
            string str;

            if (left == right)
            {
                str = "<color='blue'>L</color> 与 <color='blue'>R</color> 区域内只有一个元素，将其标记为有序";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new DrawBorder(activityType.DRAW_BORDER, left, right, true, 1, true));
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[left].index, MainControl.ColorSetting["normal"]
                    , MainControl.ColorSetting["successed"], false));
                demoQueue.Enqueue(new DestoryBorder(activityType.DESTORY_BORDER, left, right, 1, false, arr[left].index));
                return;
            }
            if (left <= right)
            {
                int originLeft = left, originRight = right;
                int temp = UnityEngine.Random.Range(left, right + 1);
                str = "确定 <color='blue'>L</color> 与 <color='blue'>R</color> 的范围，从 <color='blue'>L</color> 到 <color='blue'>R</color> 中随机选取一个元素作为 <color='blue'>Pivot</color>";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new DrawBorder(activityType.DRAW_BORDER, left, right, true, 1, true));
                demoQueue.Enqueue(new UpdateNodeHeadnote(activityType.UPDATE_NODE_HEADNOTE, arr[temp].index, true, true));
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[temp].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));

                if (temp != left) str = "将 <color='blue'>Pivot</color> 与 <color='blue'>L</color> 位置上的元素交换";
                else str = "将 <color='blue'>Pivot</color> 与 <color='blue'>L</color> 位置上的元素交换，Piovt 已经在 <color='blue'>L</color> 位置上，无需交换";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[temp].index, -(temp - left), 0, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, temp - left, 0, false));
                arr[temp].Swap(arr[left]);

                int x = arr[left].num;
                int i = left, j = right;
                while (i < j)
                {
                    bool flag = true;
                    while (i < j && arr[j].num >= x)
                    {
                        flag = false;
                        str = "从 <color='blue'>R</color> 位置向左寻找比 <color='blue'>Pivot</color> 小的元素";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                        demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originLeft, originRight, i, j, true));
                        originLeft = i; originRight = j;
                        j--;
                    }

                    if (i < j)
                    {
                        if (flag) str = "从 <color='blue'>R</color> 位置向左寻找比 <color='blue'>Pivot</color> 小的元素，找到 <color='red'>" + arr[j].num + " </color>小于 <color='red'>" + x + "</color>";
                        else str = "找到 <color='red'>" + arr[j].num + " </color>小于 <color='red'>" + x + "</color>";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], true));
                    }
                    bool flag1 = true;//如果下面这个if执行了，后面的那个if(flag1)就不应该被执行
                    if (i == j)
                    {
                        flag1 = false;
                        if (i != left)
                            str = "<color='blue'>L</color> 与 <color='blue'>R</color> 相遇，与 <color='blue'>Pivot</color> 位置上的元素进行交换。";
                        else
                            str = "<color='blue'>L</color> 与 <color='blue'>R</color> 在 <color='blue'>Pivot</color> 位置相遇，无需作交换。";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    }
                    demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originLeft, originRight, i, j, false, true));
                    originLeft = i; originRight = j;

                    flag = true;
                    while (i < j && arr[i].num <= x)
                    {
                        str = "从 <color='blue'>L</color> 位置向右寻找比 <color='blue'>Pivot</color> 大的元素";
                        flag = false;
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, i == left ? MainControl.ColorSetting["successed"] : MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["selected"], i == left ? MainControl.ColorSetting["successed"] : MainControl.ColorSetting["normal"], true));
                        demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originLeft, originRight, i, j, true));
                        originLeft = i; originRight = j;
                        i++;
                    }
                    if (i < j)
                    {
                        if (flag) str = "从 <color='blue'>L</color> 位置向右寻找比 <color='blue'>Pivot</color> 大的元素" + "找到 <color='red'>" + arr[i].num + " </color>大于 <color='red'>" + x + "</color>";
                        str = "找到 <color='red'>" + arr[i].num + " </color>大于 <color='red'>" + x + "</color>";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], true));
                    }

                    if (i == j)
                    {
                        if (i != left)
                            str = "<color='blue'>L</color> 与 <color='blue'>R</color> 相遇，与 <color='blue'>Pivot</color> 位置元素交换";
                        else
                            str = "<color='blue'>L</color> 与 <color='blue'>R</color> 在 <color='blue'>Pivot</color> 位置相遇，无需作交换";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    }
                    if (flag1)
                    {
                        demoQueue.Enqueue(new ReShape(activityType.RESHAPE_BORDER, originLeft, originRight, i, j, false, true));
                        originLeft = i; originRight = j;
                    }

                    if (i < j)
                    {
                        str = "将 <color='blue'>L</color> 位置的 <color='red'> " + arr[i].num + "</color> 与 <color='blue'>R</color> 位置的<color='red'> " + arr[j].num + " </color>交换";
                        demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, j - i, 0, true));
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -(j - i), 0, true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false));
                        arr[i].Swap(arr[j]);
                    }
                }
                str = "此时，<color='blue'>Pivot</color> 左边的元素都不大于它，右边的元素都不小于它，<color='blue'>Pivot</color> 已经在有序的位置上。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -(i - left), 0, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, i - left, 0, true));
                if (i != left)
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false));
                demoQueue.Enqueue(new UpdateNodeHeadnote(activityType.UPDATE_NODE_HEADNOTE, arr[left].index, false, true));
                demoQueue.Enqueue(new DestoryBorder(activityType.DESTORY_BORDER, left, right, 1, true, -1));
                arr[left].Swap(arr[i]);
                QuickSort(left, i - 1);
                QuickSort(i + 1, right);
            }
        }
        public override void RecordProce(int pass = 0)
        {
            Border.width = nodes[0].rect.sizeDelta.x;
            Border.height = 200f;
            for (int i = 0; i < nodes.Count; i++)
            {
                OriginalPosition.Add(nodes[i].rect.anchoredPosition);
            }
            QuickSort(0, arr.Count - 1);
            SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue);
            T.hasNext = true;
            demoQueue.Enqueue(T);
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
        }
    }

    class SelectSortDemoModel : SortBasicModel
    {
        public SelectSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void SelectSort(int pass)
        {
            if (pass == 0) pass = arr.Count;
            for (int i = 0; i < arr.Count && pass != 0; i++, pass--)
            {
                int minIndex = i;
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
                for (int j = i + 1; j < arr.Count; j++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                    if (arr[j].num < arr[minIndex].num)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[minIndex].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false));
                        minIndex = j;
                    }
                }
                SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = false; demoQueue.Enqueue(T);
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -(minIndex - i), 0, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, minIndex - i, 0, false));
                arr[i].Swap(arr[minIndex]);
            }
        }


        public override void RecordProce(int pass = 0)
        {
            SelectSort(pass);
        }
    }

    class SelectSortStudyModel : SortStudyModel
    {
        public SelectSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }
        private void SelectSort()
        {
            for (int i = 0; i < arr.Count; i++)
            {
                int minIndex = i;
                str = "遍历无序部分的数组，找出最小值。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false));
                for (int j = i + 1; j < arr.Count; j++)
                {
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false));
                    demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                    if (arr[j].num < arr[minIndex].num)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], true));
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[minIndex].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false));
                        minIndex = j;
                    }
                }

                str = "无序部分的最小值为<color='red'> " + arr[minIndex].num + "</color>";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
                if (minIndex == i)
                    str = "最小值<color='red'> " + arr[minIndex].num + " </color>已经在无序部分最左端，无需移动。";
                else
                    str = "将最小值<color='red'> " + arr[minIndex].num + " </color>替换到无序部分最左端，并标记为有序。";
                demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -(minIndex - i), 0, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, minIndex - i, 0, false));
                arr[i].Swap(arr[minIndex]);
            }
        }
        public override void RecordProce(int pass = 0)
        {
            SelectSort();
            SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue);
            T.hasNext = true;
            demoQueue.Enqueue(T);
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
        }
    }

    class ShellSortDemoModel : SortBasicModel
    {
        public ShellSortDemoModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }

        private void ShellSort(int pass)
        {
            if (pass == 0) pass = arr.Count;
            int len = arr.Count;
            for (int div = len / 2; div >= 1 && pass != 0; div = div / 2, pass--)
            {
                for (int i = 0; i < div; ++i)
                {
                    int j;
                    for (j = i; j < len; j += div)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true));
                    }
                    SortEvent T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = false; demoQueue.Enqueue(T);
                    for (j = i + div; j < len; j += div)
                    {
                        int k;
                        Part temp = new Part(arr[j]);
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, false));
                        for (k = j - div; k >= 0 && temp.num < arr[k].num; k -= div)
                        {
                            demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -div, 0, true));
                            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[k].index, div, 0, false));
                            arr[k + div].Assign(arr[k]);
                        }
                        demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
                        arr[k + div].Assign(temp);
                    }
                    for (j = i; j < len; j += div)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                    }
                    T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = false; demoQueue.Enqueue(T);
                }
            }


        }

        public override void RecordProce(int pass = 0)
        {
            ShellSort(pass);
            if (pass == 0)
                Finish();
        }
    }

    class ShellSortStudyModel : SortStudyModel
    {
        public ShellSortStudyModel(List<ArrayNode> Nodes) : base(Nodes)
        {
        }

        private void ShellSort()
        {
            SortEvent T;
            int len = arr.Count;
            str = "初始增量为 <color='blue'>N / 2 = " + len / 2 + "</color>";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
            for (int div = len / 2; div >= 1; div = div / 2)
            {
                for (int i = 0; i < div; ++i)
                {
                    int t = i + 1;
                    str = "本轮增量为 <color='blue'>" + div + " </color>，" + "对第<color='blue'> (" + t + ") </color>组元素进行插入排序（关于插入排序的详细步骤参见插入排序）";
                    demoQueue.Enqueue(new NoteEvent(activityType.NOTE, true, str));
                    int j;
                    for (j = i; j < len; j += div)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true));
                    }
                    T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = false; demoQueue.Enqueue(T);
                    for (j = i + div; j < len; j += div)
                    {
                        int k;
                        Part temp = new Part(arr[j]);
                        demoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, false));
                        for (k = j - div; k >= 0 && temp.num < arr[k].num; k -= div)
                        {
                            demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -div, 0, true));
                            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[k].index, div, 0, false));
                            arr[k + div].Assign(arr[k]);
                        }
                        demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
                        arr[k + div].Assign(temp);
                    }
                    for (j = i; j < len; j += div)
                    {
                        demoQueue.Enqueue(new TurnColor(activityType.TURN_COLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], true));
                    }
                    T = (SortEvent)MyTools.GetQueueTail(demoQueue); T.hasNext = false; demoQueue.Enqueue(T);
                }
            }
            T = (SortEvent)MyTools.GetQueueTail(demoQueue);
            T.hasNext = true;
            demoQueue.Enqueue(T);
            str = "排序完成";
            demoQueue.Enqueue(new NoteEvent(activityType.NOTE, false, str));
            Finish();
        }
        public override void RecordProce(int pass = 0)
        {
            ShellSort();
        }
    }
}