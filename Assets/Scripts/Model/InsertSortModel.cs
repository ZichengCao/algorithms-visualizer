using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FunnyAlgorithm
{
    class InsertSortModel : SortDemoModel
    {
        public InsertSortModel(List<Node> nodes) : base(nodes)
        {
        }
        private void InsertSort()
        {
            for ( int i = 1; i < arr.Count; i++ )
            {
                int j = i;
                Part temp = new Part(arr[i]);
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, 0, 2, false));
                while ( j > 0 )
                {
                    if ( temp.num < arr[j - 1].num )
                    {
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, -1, 0, true)); ;
                        DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j - 1].index, 1, 0, false));
                        arr[j].Assign(arr[j - 1]);
                    }
                    else
                        break;
                    j--;
                }
                DemoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, 0, -2, false));
                arr[j].Assign(temp);
            }
        }

        public override void RecordProce()
        {
            InsertSort();
        }
    }

}
