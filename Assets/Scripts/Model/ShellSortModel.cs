using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FunnyAlgorithm
{
    class ShellSortModel : SortDemoModel
    {
        public ShellSortModel(List<Node> nodes) : base(nodes)
        {
        }

        private void ShellSort()
        {
            int len = arr.Count;
            for ( int div = len / 2; div >= 1; div = div / 2) 
            {
                for ( int i = 0; i < div; ++i )
                {
                    int j;
                    for ( j = i; j < len; j += div )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false, true)) ;
                    }
                    Log T = MyTools.GetQueueTail(DemoQueue);T.hasNext = false;DemoQueue.Enqueue(T);
                    for ( j = i + div; j < len; j += div )
                    {
                        int k;
                        Part temp = new Part(arr[j]);
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, false));
                        for ( k = j - div; k >= 0 && temp.num < arr[k].num; k -= div )
                        {
                            DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -div, 0, true));
                            DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[k].index, div, 0, false)) ;
                            arr[k + div].Assign(arr[k]);
                        }
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
                        arr[k + div].Assign(temp);
                    }
                    for ( j = i; j < len; j += div )
                    {
                        DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR,  arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], false, true));
                    }
                    T = MyTools.GetQueueTail(DemoQueue);T.hasNext = false;DemoQueue.Enqueue(T);
                }
            }
        }

        //public void ShellSort2(int pass)
        //{
        //    if ( pass == 0 ) pass = arr.Count;
        //    int len = arr.Count;
        //    List<int> prime = MyTools.GetPrime(arr.Count / 2) ;
        //    prime.Reverse();
        //    prime.Add(1);//1不是素数，最后加上一个
        //    for ( int t = 0; t < prime.Count; t++ ) 
        //    {
        //        int div = prime[t];
        //        for ( int i = 0; i < div; ++i )
        //        {
        //            int j;
        //            for ( j = i; j < len; j += div )
        //            {
        //                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["selected"], false, true));
        //            }
        //            SortLog T = MyTools.GetQueueTail(DemoQueue); T.hasNext = false; DemoQueue.Enqueue(T);
        //            for ( j = i + div; j < len; j += div )
        //            {
        //                int k;
        //                Part temp = new Part(arr[j]);
        //                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, false));
        //                for ( k = j - div; k >= 0 && temp.num < arr[k].num; k -= div )
        //                {
        //                    DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -div, 0, true));
        //                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[k].index, div, 0, false));
        //                    arr[k + div].Assign(arr[k]);
        //                }
        //                DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
        //                arr[k + div].Assign(temp);
        //            }
        //            for ( j = i; j < len; j += div )
        //            {
        //                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[j].index, MainControl.ColorSetting["selected"], MainControl.ColorSetting["normal"], false, true));
        //            }
        //            T = MyTools.GetQueueTail(DemoQueue); T.hasNext = false; DemoQueue.Enqueue(T);
        //        }
        //    }
        //}
        public override void RecordProce()
        {
            //if ( type == 0 )
            //{
                ShellSort();

            //}else if ( type == 1 )
            //{
            //    ShellSort2(pass);
            //}
        }
    }
}
