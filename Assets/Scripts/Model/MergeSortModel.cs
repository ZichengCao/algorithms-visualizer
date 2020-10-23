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
    class MergeSortModel : SortDemoModel
    {
        private List<Part> temp = new List<Part>();
        protected List<Vector2> OriginalPosition = new List<Vector2>();

        public MergeSortModel(List<Node> nodes) : base(nodes)
        {
            for ( int i = 0; i < arr.Count; i++ )
            {
                OriginalPosition.Add(Nodes[i].rect.anchoredPosition);
                temp.Add(new Part());
            }
        }
        private void merge(int left, int right)
        {
            int mid = ( left + right ) / 2;
            for ( int tt = left; tt <= right; tt++ )
            {
                bool flag = false;
                if ( tt != right ) flag = true;
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[tt].index, 0, 2, flag));
            }
            int i = left, j = mid + 1, k = left;

            while ( i <= mid && j <= right )
            {
                if ( arr[i].num <= arr[j].num )
                {
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -( i - k ), -2, false));
                    temp[k++].Assign(arr[i++]);
                }
                else
                {
                    DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -( j - k ), -2, false)); ;
                    temp[k++].Assign(arr[j++]);
                }
            }

            while ( i <= mid )
            {
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[i].index, -( i - k ), -2, false));
                temp[k++].Assign(arr[i++]);
            }
            while ( j <= right )
            {
                DemoQueue.Enqueue(new Movement(activityType.MOVE, arr[j].index, -( j - k ), -2, false)); ;
                temp[k++].Assign(arr[j++]);
            }
            while ( left <= right )
            {
                arr[left].Assign(temp[left++]);
            }
        }

        private void mergeSort(int left, int right)
        {
            if ( left < right  ) 
            {
                int mid = ( left + right ) / 2;
                mergeSort(left, mid);
                mergeSort(mid + 1, right);
                merge(left, right);
            }
        }

        public override void RecordProce()
        {
            mergeSort(0, arr.Count - 1);
        }
    }

}
