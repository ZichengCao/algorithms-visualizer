using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using FunnyAlgorithm;

public class test : MonoBehaviour
{
    class Node
    {
        public int index; // 结点的索引
        public int num; // 结点的值
        public Node(Node n)
        {

        }
    }

    List<Node> nodes = new List<Node>();
    Queue<Activity> demoQueue;
    public void RecordProce()
    {
        for (int i = 1; i < nodes.Count; i++)
        {
            int j = i;
            Node temp = new Node(nodes[i]);
            // 表示操作：索引为 nodes[i].index 的结点，向上移动，2格，下一个 Activity 不连续
            demoQueue.Enqueue(new Movement(activityType.MOVE, nodes[i].index, direction.UP, 2, false));
            for (; j > 0 && temp.num < nodes[j - 1].num; j--)
            {
                demoQueue.Enqueue(new Movement(activityType.MOVE,temp.index, direction.LEFT, 1, true));
                demoQueue.Enqueue(new Movement(activityType.MOVE,nodes[j - 1].index, direction.RIGHT, 1, false));
                nodes[j] = nodes[j - 1];
            }
            demoQueue.Enqueue(new Movement(activityType.MOVE, temp.index, direction.DOWN, 2, false));
            nodes[j] = temp;
        }
    }
}


