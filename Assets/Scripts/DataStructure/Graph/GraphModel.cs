using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyAlgorithm;

public class GraphModel : DemoModel<GraphNode>
{
    private GraphView view;
    private LineTools linetool;

    public List<List<int>> AdjMatrix = new List<List<int>>();
    private List<List<Vector2>> list_Position = new List<List<Vector2>>();
    private List<List<Vector2>> list_graphNodePos = new List<List<Vector2>>();
    private int length;

    public GraphModel()
    {
        view = GameObject.Find("ToolsMan").GetComponent<GraphView>();
        linetool = GameObject.Find("ToolsMan").GetComponent<LineTools>();
        length = Random.Range(2, 9);
        CreateNodes();
    }


    #region 生成结点
    
    public void CreateNodes()
    {
        CalcgraphNodePos();
        CreateRandomAdjMatrix();
        Stack<char> S = new Stack<char>(MyTools.GetRandomAlphabet(length));
        List<char> random_alphabet = MyTools.GetRandomAlphabet(length);
        MyTools.disorganizeList(list_Position);
        for(int i = 0; i < length; i++)
        {
            GameObject g = Instantiate(view.NODE, view.DemoArea.transform);
            g.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            g.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            g.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            g.GetComponent<RectTransform>().anchoredPosition = list_graphNodePos[length][i];
            nodes.Add(g.GetComponent<GraphNode>());
            char c = S.Pop() ;
            nodes[i].SetValue(c);
        }
        for(int i = 0; i < AdjMatrix.Count; i++)
        {
            for(int j = 0; j < AdjMatrix[i].Count; j++)
            {
                if (i < j && AdjMatrix[i][j] != 0) 
                {
                    //生成边
                    //linetool.DrawLine(nodes[i].gameObject, nodes[j].gameObject);
                }
            }
        }
        
    }
    private void CreateRandomAdjMatrix()
    {
        for (int i = 0; i < length; i++)
        {
            List<int> temp = new List<int>();
            for (int j = 0; j < length; j++)
            {
                temp.Add(0);
            }
            AdjMatrix.Add(temp);
        }
        for(int i = 0; i < length; i++)
        {
            int i1;
            do
            {
                i1 = Random.Range(0, length);
            } while (i1 == i);
            AdjMatrix[i][i1] = AdjMatrix[i1][i] = 1;
            
        }
    }
    private void CalcgraphNodePos()
    {
        Vector2 size = view.DemoArea.GetComponent<RectTransform>().sizeDelta;
        float standrad_x = size.x / 9;
        float standrad_y = size.y / 9;
        float start_x = -(size.x / 2 - standrad_x / 2);
        float start_y = -(size.y / 2 - standrad_y / 2) ;
        for(int i = 0; i < 9; i++)
        {

            List<Vector2> temp = new List<Vector2>();
            for(int j = 0; j < 9; j++)
            {
                temp.Add(new Vector2(start_x + j * standrad_x, start_y));
                //list_graphNodePosition.Add();
            }
            start_y += standrad_y;
            list_Position.Add(temp);
        }
        //0
        list_graphNodePos.Add(null);
        //1
        list_graphNodePos.Add(new List<Vector2>() { list_Position[4][4] });
        //2
        list_graphNodePos.Add(new List<Vector2>() { list_Position[4][2], list_Position[4][6] });
        //3
        list_graphNodePos.Add(new List<Vector2>() { list_Position[2][4], list_Position[6][2], list_Position[6][6] });
        //4
        list_graphNodePos.Add(new List<Vector2>() { list_Position[2][4], list_Position[4][2], list_Position[4][6], list_Position[6][4] });
        //5
        list_graphNodePos.Add(new List<Vector2>() { list_Position[2][4], list_Position[4][2], list_Position[4][6], list_Position[6][2], list_Position[6][6] });
        //6 
        list_graphNodePos.Add(new List<Vector2>() { list_Position[0][4], list_Position[3][2], list_Position[3][6], list_Position[5][2], list_Position[5][6], list_Position[8][4] });
        //7
        list_graphNodePos.Add(new List<Vector2>() { list_Position[0][4], list_Position[3][2], list_Position[3][6], list_Position[5][2], list_Position[5][6], list_Position[8][4], list_Position[4][0] });
        //8
        list_graphNodePos.Add(new List<Vector2>() { list_Position[0][4], list_Position[3][2], list_Position[3][6], list_Position[5][2], list_Position[5][6], list_Position[8][4], list_Position[4][0], list_Position[4][8] });
    }

    public void Clean()
    {
        //LineTools.DestroyLines();
        for(int i = 0; i < view.DemoArea.transform.childCount; i++)
        {
            Destroy(view.DemoArea.transform.GetChild(i).gameObject);
        }
    }
    #endregion
    public override bool PlayBackward()
    {
        return base.PlayBackward();
    }

    public override bool PlayForward()
    {
        return base.PlayForward();
    }

    public override void RecordProce(int pass=0)
    {
        base.RecordProce();
    }

}
