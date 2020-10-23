using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace FunnyAlgorithm
{
    class QuickSortModel : SortDemoModel
    {
        public QuickSortModel(List<Node> nodes):base(nodes)
        {
        }
        private void QuickSort(int left, int right)
        {
            if ( left == right  ) 
            {
                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[left].index, MainControl.ColorSetting["normal"]
                    , MainControl.ColorSetting["successed"], false, false));
                return;
            }
            if ( left <= right )
            {
                int temp = UnityEngine.Random.Range(left, right + 1);
                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[temp].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, false));
                if ( temp != left )
                {
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[temp].index, -( temp - left ), 0, true));
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, temp - left, 0, false));
                    arr[temp].Swap(arr[left]);
                }

                int x = arr[left].num;
                int i = left, j = right;
                while ( i < j )
                {
                    while ( i < j && arr[j].num >= x )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true, true));
                        j--;
                    }

                    if ( i < j )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], false, false));
                    }

                    while ( i < j && arr[i].num <= x )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[i].index, i == left ? MainControl.ColorSetting["successed"] : MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true, true));
                        i++;
                    }

                    bool flag = true;
                    if ( i < j )
                    {
                        flag = false;
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["signed"], false, false));
                    }

                    if ( flag )
                    {
                        Log T = MyTools.GetQueueTail(DemoQueue); T.hasNext = false; DemoQueue.Enqueue(T);
                    }

                    if ( i < j )
                    {
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, j - i, 0, true));
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -( j - i ), 0, true));
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false, true));
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false, false));
                        arr[i].Swap(arr[j]);
                    }
                }
                if ( i != left )
                {
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -( i - left ), 0, true));
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, i - left, 0, true));
                    DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[i].index, MainControl.ColorSetting["signed"], MainControl.ColorSetting["normal"], false, false));
                    arr[left].Swap(arr[i]);
                }
                QuickSort(left, i - 1);
                QuickSort(i + 1, right);
            }
        }
        public override void RecordProce()
        {
            QuickSort(0, arr.Count - 1) ;
        }
    }
  
}
