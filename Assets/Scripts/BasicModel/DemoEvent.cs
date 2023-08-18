using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyAlgorithm
{
    abstract class SortEvent : Activity
    {
        public int index;//操作索引
        public SortEvent(activityType type, bool hasNext, bool inStack = true) : base(type, hasNext, inStack)
        {

        }
    }
    class Disable : SortEvent
    {
        public Disable(activityType type,int index, bool hasNext, bool inStack = true) : base(type, hasNext, inStack)
        {
            this.index = index;
        }
    }
    class TurnColor : SortEvent
    {
        //改变颜色操作
        public string targetColor;//改变的颜色
        public string initialColor;//原来的颜色
        public TurnColor(activityType type, int index, string initialColor, string targetColor, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
            this.targetColor = targetColor;
            this.initialColor = initialColor;
        }
    }
    public class ShowTraverseSerialNumber:Activity
    {
        public Node node;
        public int serial_number;
        public ShowTraverseSerialNumber(activityType type, Node node,int serial_number,  bool hasNext) : base(type, hasNext)
        {
            this.node = node;
            this.serial_number = serial_number;
        }
    }
    class Movement : SortEvent
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

        public Movement(activityType type, int index, direction dir,int steps, bool hasNext) : base(type, hasNext)
        {

        }
    }
    class NoteEvent : SortEvent
    {
        public string note;//学习模式下的注解
        public NoteEvent(activityType type, bool hasNext, string note) : base(type, hasNext)
        {
            this.note = note;
        }
    }
    class BorderEvent : SortEvent
    {
        public bool flag;//砖头，哪里需要往哪搬
        public int left, right;//起始终止索引
        public int HasSign;//0全都不要，1要LR，2全都要
        public BorderEvent(activityType type, bool hasNext) : base(type, hasNext)
        {

        }
    }
    class DestoryBorder : BorderEvent
    {
        /// <summary>
        /// 销毁Border。left,right用于Last操作,index一般不用，可以用于传输别的参数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="hasNext"></param>
        /// <param name="index"></param>
        public DestoryBorder(activityType type, int left, int right, int HasSign, bool hasNext, int index, bool flag = true) : base(type, hasNext)
        {
            this.left = left;
            this.flag = flag;
            this.HasSign = HasSign;
            this.right = right;
            this.index = index;
        }
    }
    class DrawBorder : BorderEvent
    {
        /// <summary>
        /// 用于创建一个Border,falg标记是否进入Executed队列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="hasNext"></param>
        public DrawBorder(activityType type, int left, int right, bool hasNext, int CreatWithSign, bool flag = true) : base(type, hasNext)
        {
            this.left = left;
            this.right = right;
            this.flag = flag;
            this.HasSign = CreatWithSign;
        }
    }
    class ReShape : BorderEvent
    {
        //边框
        public int OriginLeft, OriginRight;
        /// <summary>
        /// 对border进行形变,flag是否加入Executed队列
        /// </summary>
        /// <param name="type"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public ReShape(activityType type, int OriginalLeft, int OriginalRight, int left, int right, bool hasNext, bool flag = true) : base(type, hasNext)
        {
            this.left = left;
            this.right = right;
            this.flag = flag;
            OriginLeft = OriginalLeft;
            OriginRight = OriginalRight;
        }
    }
    class UpdateNodeHeadnote : SortEvent
    {
        public bool flag;//砖头，哪里需要往哪搬

        public string note;
        /// <summary>
        /// flag true表示显示Pivot，false表示消失
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="flag"></param>
        /// <param name="hasNext"></param>
        public UpdateNodeHeadnote(activityType type, int index, bool flag, bool hasNext, string Text = "") : base(type, hasNext)
        {
            this.index = index;
            this.flag = flag;
            this.hasNext = hasNext;
        }
        public UpdateNodeHeadnote(activityType type, int index, string text, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
            this.hasNext = hasNext;
            this.note = text;
        }
    }
}

