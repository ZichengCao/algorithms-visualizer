using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FunnyAlgorithm
{
    class SelectSortModel : SortDemoModel
    {
        public SelectSortModel(List<Node> nodes):base(nodes)
        {
        }
        private void SelectSort()
        {
            for ( int i = 0; i < arr.Count; i++ ) 
            {
                int minIndex = i;
                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[i].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, true));
                for ( int j = i + 1; j < arr.Count; j++ )
                {
                    DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true, true));
                    if ( arr[j].num < arr[minIndex].num )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, true));
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[minIndex].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false, true));
                        minIndex = j;
                    }
                }
                Log T = MyTools.GetQueueTail(DemoQueue);T.hasNext = false;DemoQueue.Enqueue(T);
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -( minIndex - i ), 0, true));
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, minIndex - i, 0, false));
                arr[i].Swap(arr[minIndex]);
            }
        }

        #region 双向选择排序
        /// <summary>
        /// 双向选择排序
        /// </summary>
        /// <param name="pass"></param>
        //public void D_SelectSort(int pass)
        //{
        //    if ( pass == 0 ) pass = arr.Count;
        //    int left = 0;
        //    int right = arr.Count - 1;
        //    while ( left < right )
        //    {
        //        int maxIndex = left, minIndex = left;
        //        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[left].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, true));
        //        for (int j = left + 1; j <= right; j++ )
        //        {
        //            DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], true, true));
        //            if ( arr[j].num < arr[minIndex].num )
        //            {
        //                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, true));
        //                if(maxIndex!=minIndex)
        //                    DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[minIndex].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false, true));
        //                minIndex = j;
        //            }
        //            if ( arr[j].num > arr[maxIndex].num )
        //            {
        //                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, true));
        //                if ( maxIndex != minIndex )
        //                    DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[maxIndex].index, MainControl.ColorSetting["successed"], MainControl.ColorSetting["normal"], false, true));
        //                maxIndex = j;
        //            }
        //        }
        //        SortLog T = MyTools.GetQueueTail(DemoQueue); T.hasNext = false; DemoQueue.Enqueue(T);
        //        if ( maxIndex != left )
        //        {
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, minIndex - left, 0, true));
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -(minIndex - left), 0, false));
        //            arr[left].Swap(arr[minIndex]);

        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[right].index, -(right-maxIndex), 0, true)); 
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[maxIndex].index, right - maxIndex, 0, false)) ;
        //            arr[right].Swap(arr[maxIndex]);
        //        }
        //        else if ( minIndex != right )
        //        {
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[right].index, -( right - maxIndex ), 0, true));
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[maxIndex].index, right - maxIndex, 0, false));
        //            arr[right].Swap(arr[maxIndex]);

        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[left].index, minIndex - left, 0, true));
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -( minIndex - left ), 0, false));
        //            arr[left].Swap(arr[minIndex]);
        //        }
        //        else
        //        {
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[maxIndex].index, right - maxIndex, 0, true));
        //            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[minIndex].index, -( minIndex - left ), 0, false));
        //            arr[minIndex].Swap(arr[maxIndex]);
        //        }
        //        left++; right--;
        //    }
        //}
        #endregion

        public override void RecordProce()
        {
            //if ( type == 0 )
            //{
                SelectSort();
            //}else if ( type == 1 )
            //{
            //    D_SelectSort(pass);
            //}
        }
    }
}
