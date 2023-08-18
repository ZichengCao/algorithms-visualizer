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
   public abstract class Activity
    {
        public activityType type; //活动类型
        public bool inStack;//是否需要进入Back栈
        public bool hasNext;//下一个activity是否需要连续进行

        public Activity(activityType type, bool hasNext, bool inStack = true)
        {
            this.type = type;
            this.hasNext = hasNext;
            this.inStack = inStack;
        }
    }

    public abstract class BasicModel : MonoBehaviour
    {
        public Queue<Activity> demoQueue = new Queue<Activity>();//执行队列
        public Stack<Activity> executedStack = new Stack<Activity>();//已经执行过的步骤
        public bool IsRun = false;//当前是否处于执行状态
        public virtual void RecordProce(int pass =0) { }
        //正向播放
        //返回值表示表示下一个动作是否需要继续执行
        public virtual bool PlayForward() { return false; }
        //逆向播放
        public virtual bool PlayBackward() { return false; }
    }
    public abstract class DemoModel <NodeType>: BasicModel
    {
        public List<NodeType> nodes = new List<NodeType>();//结点数组
    }
}
