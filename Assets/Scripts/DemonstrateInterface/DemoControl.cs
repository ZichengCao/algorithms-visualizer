using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyAlgorithm
{
    public class DemoControl : MonoBehaviour
    {
        public DemoView view;

        private SortDemoModel demo;

        private bool IsStart = false;//是否开始
        private bool PlayOrPause = true;//开始、暂停
        private float DemoArea_length;
        private int DataLength = 15;
        private int DataType = 0;
        private List<int> ValueToNums = new List<int>();//数量 DropDown 的 Value 索引对应的元素个数


        private List<Node> nodes = new List<Node>();
        private List<GameObject> GO_nodes = new List<GameObject>();

        void Start()
        {
            //数组元素数量
            ValueToNums.Add(5);
            ValueToNums.Add(10);
            ValueToNums.Add(15);
            ValueToNums.Add(20);
            ValueToNums.Add(30);
            ValueToNums.Add(50);

            DemoArea_length = view.DemoArea.GetComponent<RectTransform>().sizeDelta.x;
            view.Drop_SortTypeSelect.value = MainControl.SORTTYPE;
            MoveTool.duration = 0.5f;
            CreatSortNodes(DataLength);
        }
        private void Initialize(int length)
        {
            switch ( MainControl.SORTTYPE )
            {
                case 0:
                    demo = new InsertSortModel(nodes);
                    break;
                case 1:
                    demo = new SelectSortModel(nodes);
                    break;
                case 2:
                    demo = new BubbleSortModel(nodes);
                    break;
                case 3:
                    demo = new ShellSortModel(nodes);
                    break;
                case 4:
                    demo = new MergeSortModel(nodes);
                    break;
                case 5:
                    demo = new QuickSortModel(nodes);
                    break;
                default:
                    break;
            }
            demo.RecordProce();
        }
        private void CreatSortNodes(int length)
        {
            float width, interval, pos;//结点宽度，间距，起始生成位置
            List<int> list = MyTools.GetList(DataType, length);
            
            //计算合理宽高
            if ( length < 10 )
            {
                width = 100f;
                interval = width;
                pos = -( width * length ) / 2;
            }
            else
            {
                width = DemoArea_length / ( length + 2 );
                interval = width;
                pos = -( 1100 / 2 - width - width / 2 );
            }

            Node.width = width;
            Node.verticalStandard = 100f;
            //计算合适的起始生成位置（锚点在中下）

            //生成结点
            bool ShowNumFlag = true;
            if ( length > 30 ) ShowNumFlag = false;
            for ( int i = 0; i < length; i++ )
            {
                GO_nodes.Add(Instantiate(view.NODE, view.DemoArea.transform));
                nodes.Add(new Node(GO_nodes[i], list[i], i, ShowNumFlag));
                //设置锚点，第三个参数为沿插入方向的宽度，在这里也就是高度
                //设置高度,0-- > 高度100,15-- > 高度295
                nodes[i].rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 200);//100 + 195 / 15 * nodes[i].num
                nodes[i].rect.sizeDelta = new Vector2(width, nodes[i].rect.sizeDelta.y);
                nodes[i].rect.pivot = new Vector2(0.5f, 0f);
                nodes[i].rect.anchoredPosition = new Vector2(pos, 0);
                //生成位置移动到下一个
                pos += interval;
            }
            //形变动画
            float height = 35;//最短高度规定为35
            int mmax = MyTools.GetListMax(list);
            float HeightOffset = ( 200 - height ) / mmax;

            for ( int i = 0; i < nodes.Count; i++ )
            {
                nodes[i].Reshape(direction.VERTICAL, height + HeightOffset * nodes[i].num);
            }
        }
        private void DestoryNodes(int length)
        {
            for ( int i = 0; i < view.DemoArea.transform.childCount; i++ )
            {
                if ( view.DemoArea.transform.GetChild(i).tag != "MENU" )
                    Destroy(view.DemoArea.transform.GetChild(i).gameObject);
            }
            GO_nodes.Clear();
            nodes.Clear();
        }

        private IEnumerator AutoPlay()
        {
            while ( demo.DemoQueue.Count > 0 )
            {
                StartCoroutine(demo.PlayForward());
                yield return new WaitForSeconds(MoveTool.duration);

                if ( demo.DemoQueue.Count == 0 )
                {
                    SortDemoModel.IsFinish = true;
                    if ( MainControl.SORTTYPE == 0 || MainControl.SORTTYPE == 3 || MainControl.SORTTYPE == 4 )
                        StartCoroutine(demo.FinishSort());
                }
            }
            view.Text_StartButton.text = "结束";
            view.Btn_StartButton.interactable = false;
            view.Btn_NextStep.interactable = false;
            view.Btn_LastStep.interactable = true;
        }

        #region demonstrate control Button
        public void Restart()
        {
            SortDemoModel.IsRun = false;
            PlayOrPause = true;
            IsStart = false;
            view.Btn_LastStep.interactable = false;
            view.Btn_NextStep.interactable = true;
            view.Btn_StartButton.interactable = true;
            view.Text_StartButton.text = "自动";
            StopAllCoroutines();
            DestoryNodes(DataLength);
            CreatSortNodes(DataLength);
        }
        public void LastStep()
        {
            view.Btn_NextStep.interactable = true;
            view.Btn_StartButton.interactable = true;
            if ( SortDemoModel.IsFinish == true )
            {
                if ( MainControl.SORTTYPE == 0 || MainControl.SORTTYPE == 3 || MainControl.SORTTYPE == 4 )
                {
                    for ( int i = 0; i < nodes.Count; i++ )
                    {
                        nodes[i].SetColor(MainControl.ColorSetting["normal"]);
                    }
                }
                PlayOrPause = true;
                view.Text_StartButton.text = "自动";
                SortDemoModel.IsFinish = false;
            }

            if ( demo.ExecutedStack.Count > 0 )
            {
                StartCoroutine(demo.PlayBackward());
                if ( demo.ExecutedStack.Count == 0 )
                {
                    view.Btn_LastStep.interactable = false;
                }
            }
        }
        public void NextStep()
        {
            if ( !IsStart )
            {
                Initialize(DataLength);
                IsStart = true;
            }

            if ( demo.DemoQueue.Count > 0 )
            {
                StartCoroutine(demo.PlayForward());
                if ( demo.DemoQueue.Count == 0 )
                {
                    view.Btn_NextStep.interactable = false;
                    view.Btn_StartButton.interactable = false;
                    view.Text_StartButton.text = "结束";
                    SortDemoModel.IsFinish = true;
                    if ( MainControl.SORTTYPE == 0 || MainControl.SORTTYPE == 3 || MainControl.SORTTYPE == 4 )
                        StartCoroutine(demo.FinishSort());
                }
            }

            if ( demo.ExecutedStack.Count > 0 )
                view.Btn_LastStep.interactable = true;
        }
        public void Auto()
        {
            if ( !IsStart )
            {
                Initialize(DataLength);
                IsStart = true;
            }
            if ( PlayOrPause )
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
            PlayOrPause = !PlayOrPause;

        }

        #endregion


        #region topTabs
        public void SetChoose()
        {
            MainControl.SORTTYPE = view.Drop_SortTypeSelect.value;
            if ( IsStart )
            {
                Restart();
                IsStart = false;
            }
        }
        public void SetDataNums()
        {
            DataLength = ValueToNums[view.Drop_DataNums.value];
            Restart();
        }

        public void SetDelay()
        {
            view.Text_Dealy.text = view.Slider_Delay.value.ToString() + " ms";
            if ( view.Slider_Delay.value == 0f )
            {
                MoveTool.duration = 0f;
            }
            else
            {
                MoveTool.duration = view.Slider_Delay.value / 100f;
            }
        }

        public void SetDataType()
        {
            DataType = view.Drop_DataType.value;
            Restart();
        }

        #endregion

    }
}
