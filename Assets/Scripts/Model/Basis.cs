using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace FunnyAlgorithm
{
    class Basis {
        public static string[] SortName = { "插入排序", "选择排序", "冒泡排序",  "希尔排序", "归并排序", "快速排序" };
    }

    public enum activityType { MOVE, TURNCOLOR};
    public enum direction { HORIZONTIAL, VERTICAL };

    class MoveTool
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
        /// <param name="MoveWidth">X方向上移动的单步距离</param>
        /// <param name="MoveHeight">Y方向移动的单步距离</param>
        public void MoveTo(RectTransform rect, int x_steps, int y_steps, float MoveWidth, float MoveHeight, float duration = 0)
        {
            if ( rect == null ) return;
            if ( duration == 0 )
                duration = MoveTool.duration;
            float tox = rect.anchoredPosition.x + x_steps * MoveWidth, toy = rect.anchoredPosition.y + y_steps * MoveHeight;
            rect.DOAnchorPos(new Vector2(tox, toy), duration);
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

}
