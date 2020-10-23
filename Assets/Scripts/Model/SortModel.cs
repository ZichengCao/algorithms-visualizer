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
    public class Node
    {
		
        public int index;
        public GameObject g;
        public Image image;

        public static float width, verticalStandard;
        public int num;
        public RectTransform rect;
        private MoveTool moveTool = new MoveTool();
        public Node(GameObject G, int num, int index, bool flag = true)
        {
            this.index = index;
            this.num = num;
            this.g = G;
            rect = g.GetComponent<RectTransform>();
            image = g.GetComponent<Image>();
            if ( flag )
            {
                g.GetComponentInChildren<Text>().text = num.ToString() ;
            }
         
        }

        public void Assign(Node b)
        {
            index = b.index;
            num = b.num;
            rect = b.rect;
            g = b.g;
            moveTool = b.moveTool;
        }
       
        public void MoveTo(int x_steps, int y_steps, float duration = 0)
        {
            moveTool.MoveTo(rect, x_steps, y_steps, Node.width, Node.verticalStandard, duration);
        }
    
        public void SetColor(string color)
        {
            if ( image == null ) return;
            image.color = MyTools.Color_HexToRgb(color);
        }
        /// <summary>
        /// 对node做长或宽形变
        /// </summary>
        /// <param name="dir">形变方向</param>
        /// <param name="destination">形变目标值</param>
        /// <param name="duration">变化时间</param>
        public void Reshape(direction dir, float destination, float duration = 1f)
        {
            if ( dir == direction.HORIZONTIAL )
            {
                image.rectTransform.DOSizeDelta(new Vector2(destination, image.rectTransform.sizeDelta.y), duration);
            }
            else
            {
                image.rectTransform.DOSizeDelta(new Vector2(image.rectTransform.sizeDelta.x, destination), duration);
            }
        }
    }
    
	#region LOG
    abstract class Log
    {
        public activityType type;
        public int index;//操作索引
        public bool flag;//砖头，哪里需要往哪搬
        public bool hasNext;//下一个操作是否连续进行

        public Log(activityType type,bool hasNext)
        {
            this.type = type;
            this.hasNext = hasNext;
        }
    }
    class TurnColor : Log
    {
        //改变颜色操作
        public string DestinationColor;//改变的颜色
        public string OriginColor;//原来的颜色
        public bool restore;//是否变回原来的颜色
        public TurnColor(activityType type, int index, string OriginColor, string DestinationColor, bool restore, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
            this.restore = restore;
            this.DestinationColor = DestinationColor;
            this.OriginColor = OriginColor;
        }
    }
    class Movement : Log
    {
        //移动操作
        public int x_steps, y_steps;//x移动的格数(x单位width，y方向100)
        /// <summary>
        /// </summary>
        /// <param name="index">移动的索引</param>
        /// <param name="x_steps">X方向上移动的步数</param>
        /// <param name="y_steps">Y方向上移动的步数</param>
        /// <param name="hasNext">是否有下一个操作数（交换操作）</param>
        /// <param name="annotation">学习模式下的注解</param>
        public Movement(activityType type, int index, int x_steps, int y_steps, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
            this.x_steps = x_steps;
            this.y_steps = y_steps;
        }
    }
  
    #endregion

    #region sortMode

	
    abstract class SortDemoModel : MonoBehaviour
    {
        public List<Node> Nodes = new List<Node>();//结点
        public Queue<Log> DemoQueue = new Queue<Log>();//执行队列
        public Stack<Log> ExecutedStack = new Stack<Log>();//已经执行过的步骤
        public static bool IsRun = false;//当前是否处于执行状态
        public static bool IsFinish = false;//当前是否执行完毕

        public List<Part> arr = new List<Part>();//不重要，记录结点当前位置与最开始索引的关系
        public virtual void RecordProce() { }
        public SortDemoModel(List<Node> nodes)
        {
            for ( int i = 0; i < nodes.Count; i++ )
            {
                Nodes.Add(nodes[i]);
                arr.Add(new Part(nodes[i].num, i));
            }
        }

        public  IEnumerator PlayForward()
        {
            bool flag = true;
            if ( !IsRun )
            {
                IsRun = true;
                Log LOG;
                do
                {
                    LOG = DemoQueue.Peek();
                    if ( LOG.type == activityType.MOVE )
                    {
                        Movement log = (Movement) DemoQueue.Dequeue() ;
                        Nodes[log.index].MoveTo(log.x_steps, log.y_steps); ;
                    }
                    else
                    {
                        TurnColor log = (TurnColor) DemoQueue.Dequeue();
                        Nodes[log.index].SetColor(log.DestinationColor);
                        if ( log.restore )//如果要求颜色变回去
                        {
                            flag = false;
                            yield return new WaitForSeconds(MoveTool.duration);
                            Nodes[log.index].SetColor(log.OriginColor);
                        }
                    }
                    ExecutedStack.Push(LOG);
                } while ( DemoQueue.Count > 0 && LOG.hasNext ) ;
                if ( flag )
                    yield return new WaitForSeconds(MoveTool.duration);
                IsRun = false;
            }
        }
        public  IEnumerator PlayBackward()
        {
            if ( !IsRun )
            {
                bool Flag = true;
                IsRun = true;
                Log LOG;
                do
                {
                    LOG = ExecutedStack.Peek();
                    if ( LOG.type == activityType.TURNCOLOR )
                    {
                        TurnColor log = (TurnColor) ExecutedStack.Pop();
                        if ( log.restore )
                        {
                            Flag = false;
                            Nodes[log.index].SetColor(log.DestinationColor);
                            yield return new WaitForSeconds(MoveTool.duration);
                            Nodes[log.index].SetColor(log.OriginColor);
                        }
                        else
                        {
                            Nodes[log.index].SetColor(log.OriginColor);
                        }
                    }
                    else
                    {
                        Movement log = (Movement) ExecutedStack.Pop();
                        Nodes[log.index].MoveTo(-log.x_steps, -log.y_steps) ;
                    }
                    MyTools.QueuePushFront(DemoQueue, LOG);
                } while ( ExecutedStack.Count > 0 && ExecutedStack.Peek().hasNext );
                if ( Flag )
                    yield return new WaitForSeconds(MoveTool.duration);
                IsRun = false;
            }
        }
        public IEnumerator FinishSort()
        {
            for ( int i = 0; i < Nodes.Count; i++ )
            {
                Nodes[arr[i].index].SetColor(MainControl.ColorSetting["successed"]);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
    #endregion
}
