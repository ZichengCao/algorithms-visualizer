using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunnyAlgorithm
{
    class BubbleSortModel : SortDemoModel
    {
        public BubbleSortModel(List<Node> nodes) : base(nodes)
        {
        }
        private void BubbleSort()
        {
            for ( int i = 0; i < arr.Count ; i++ )
            {
                for ( int j = 0; j < arr.Count - i - 1; j++ )
                {
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, 2, true)); ;
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, 2, false)); ;
                    if ( arr[j].num > arr[j + 1].num )
                    {
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 1, 0, true));
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, -1, 0, false));
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, true));
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, -2, false));
                        arr[j].Swap(arr[j + 1]);
                    }
                    else
                    {
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, 0, -2, true)); ;
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j + 1].index, 0, -2, false));
                    }
                }
                DemoQueue.Enqueue(new TurnColor(activityType.TURNCOLOR, arr[arr.Count - i - 1].index, MainControl.ColorSetting["normal"], MainControl.ColorSetting["successed"], false, false));
            }
        }
        public override void RecordProce()
        {
            BubbleSort();
        }
    }
}
