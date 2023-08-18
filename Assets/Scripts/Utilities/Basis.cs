using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



namespace FunnyAlgorithm
{
    class Basis
    {
        public static string[] SortName = { "插入排序", "选择排序", "冒泡排序", "希尔排序", "归并排序", "快速排序" };
    }
    public enum SecondMenuType { ALG, DS,UNINITIALIZE };
    public enum activityType { MOVE, TURN_COLOR, NOTE, DRAW_BORDER, RESHAPE_BORDER, DESTORY_BORDER, UPDATE_NODE_HEADNOTE, DISABLE ,CREATE_LINKNODE,SHOW_TRAVERSE_SERIAL_NUMBER, MOVESTATEMENT, HIGHLIGHT, ASSIGNFORANVARIABLE, DEFINESTATEMENT , UPWARDTEXTEVENT };
    public enum StackType { LINKSTACK,ARRAYSTACK};
    public enum QueueType { LINKQUEUE,ARRAYQUEUE};
    public enum model { DEMOMODEL, STUDYMODEL };
    public enum searchType { LINEARSEARCH, BINARYSEARCH };
    public enum linkListType { SINGLE,DOUBLE};
    public enum direction { HORIZONTIAL, VERTICAL,UP,DOWN,LEFT,RIGHT };
    public enum childTree { LEFT,RIGHT,BOTH};
    public enum TreeType { TRAVERSE,BSTREE}
    public enum BTreeTraverseType{ PREORDER, INORDER,POSTORDER};
    public enum sortType { INSERTSORT, SELECTSORT, BUBBLESORT, SHELLSORT, MERGESORT, QUICKSORT };

    public enum questionType { EXCHANGECOUNT, PASS, CHOICEQUESTION };

    public enum LinkNodeFlag { INNER,OUTER,ALL};
    public class ColorSetting
    {
        public static Color normal = new Color(1, 1, 1);
        public static Color selected = new Color(1, 0.52156f, 0.4f);
        public static Color successed = new Color(0.4f, 0.8666f, 0.4f);
        public static Color failed = new Color(1, 0, 0);
        public static Color signed = new Color(0, 0.6f, 1);
        public static Color orangeButton = new Color(0.9607f, 0.5647f, 0.5176f);
        public static Color blueButton = new Color(0.44313f, 0.72156f, 1f);
        public static Color disable = new Color(0.7843f, 0.7843f, 0.7843f);
        public static Color normal_translucence= new Color(1, 1, 1, 0.4156f);
    }
    public static class MoveTool
    {
        /// <summary>
        /// 移动操作的时间
        /// </summary>
        public static float duration = 0.5f;
        /// <summary>
        /// x,y表示移动方向，目标位置Vector2(originpos.x+x*MoveWidth*steps,originpos.y+y*MoveHeight*steps,);
        /// </summary>
        /// <param name="x_steps">X方向移动的步数</param>
        /// <param name="y_steps">Y方向移动的步数</param>
        /// <param name="x_stride">X方向上移动的单步距离</param>
        /// <param name="y_stride">Y方向移动的单步距离</param>
        public static void MoveTo(RectTransform rect, int x_steps, int y_steps, float x_stride, float y_stride, float duration = 0)
        {
            if (rect == null)
                throw new System.NullReferenceException("rect is null");
            if (duration == 0)
                duration = MoveTool.duration;
            float to_x = rect.anchoredPosition.x + x_steps * x_stride, to_y = rect.anchoredPosition.y + y_steps * y_stride;
            rect.DOAnchorPos(new Vector2(to_x, to_y), duration);
        }
        /// <summary>
        /// destination表示目标位置
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="destination"></param>
        /// <param name="duration"></param>
        public static void MoveTo(RectTransform rect, Vector2 destination, float duration = 0)
        {
            if (rect == null)
            {
                throw new System.NullReferenceException("rect is null");
            }
            rect.DOAnchorPos(destination, duration);
        }
    }

    /// <summary>
    /// 用于快排和归并排序演示的外框
    /// </summary>
    /// 
    public class Border
    {
        public GameObject g;
        public RectTransform rect;
        public int left, right;
        public static float width, height;
        public Border(GameObject g, int left, int right)
        {
            this.g = g;
            rect = g.GetComponent<RectTransform>();
            this.left = left;
            this.right = right;
        }

        //左边定死，以左边为基准调整宽度
        private void LeftClip(int newRight)
        {
            Vector2 temp = rect.pivot;
            if (temp.x != 0f || temp.y != 0.5f)
            {
                rect.pivot = new Vector2(0f, 0.5f);
                //如果pivot之前在中下
                if (temp.x == 0.5f && temp.y == 0)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - rect.sizeDelta.x / 2, rect.anchoredPosition.y + rect.sizeDelta.y / 2);
                }
                //pivot之前在右中
                else if (temp.x == 1f && temp.y == 0.5f)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - rect.sizeDelta.x, rect.anchoredPosition.y);
                }
            }
            float x = rect.sizeDelta.x - (right - newRight) * width;
            float y = rect.sizeDelta.y;
            rect.DOSizeDelta(new Vector2(x, y), MoveTool.duration);
        }
        private void RightClip(int newLeft)
        {
            Vector2 temp = rect.pivot;
            if (temp.x != 1f || temp.y != 0.5f)
            {
                rect.pivot = new Vector2(1f, 0.5f);
                //如果pivot之前在中下
                if (temp.x == 0.5f && temp.y == 0)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + rect.sizeDelta.x / 2, rect.anchoredPosition.y + rect.sizeDelta.y / 2);
                }
                //如果pivot之前在左中
                else if (temp.x == 0f && temp.y == 0.5f)
                {
                    rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + rect.sizeDelta.x, rect.anchoredPosition.y);
                }
            }

            float x = rect.sizeDelta.x + (left - newLeft) * width;
            float y = rect.sizeDelta.y;
            rect.DOSizeDelta(new Vector2(x, y), MoveTool.duration);
        }
        public void ReShape(int left, int right)
        {
            if (left == this.left && right == this.right)
            {
                RightClip(left);//没有任何用，只是让它等一段时间，后面会改的
            }
            if (left != this.left && right != this.right)
            {
                Move(left, right);
                //耻辱，记下来，这一段忘记写了，调试了半个多小时，没找出来错误在哪里
                this.left = left;
                this.right = right;
            }
            else if (left != this.left)
            {
                RightClip(left);
                this.left = left;
            }
            else if (right != this.right)
            {
                LeftClip(right);
                this.right = right;
            }
        }
        private void Move(int left, int right)
        {
            if (right - left == this.right - this.left)
            {
                float x = rect.anchoredPosition.x + (left - this.left) * width;
                rect.DOAnchorPosX(x, MoveTool.duration);
            }
            if (right - left != this.right - this.left)
            {
                LeftClip(this.right + ((right - left) - (this.right - this.left)));
                float x = rect.anchoredPosition.x + (left - this.left) * width;
                rect.DOAnchorPosX(x, MoveTool.duration);
            }
        }
    }
    public class Part
    {
        public int num;
        public int index;
        public Part()
        {
        }
        public Part(int n, int i)
        {
            num = n;
            index = i;
        }
        public Part(Part p)
        {
            num = p.num;
            index = p.index;
        }
        public void Assign(Part p)
        {
            num = p.num;
            index = p.index;
        }
        public void Swap(Part p1)
        {
            Part temp = new Part(this);
            Assign(p1);
            p1.Assign(temp);
        }
    }

    public class Coord {
        int x, y;
        public Coord(int x,int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

