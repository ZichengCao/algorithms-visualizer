using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyAlgorithm;

public class LineTools : MonoBehaviour
{
    public static GameObject Line;
    public List<GameObject> LineContainer = new List<GameObject>();
    //public Dictionary<string, GameObject> LineContainer = new Dictionary<string, GameObject>();

    public static void DestroyLines(TreeNode head)
    {
        if (head == null) return;
        head.DestroyLine(childTree.BOTH);
        DestroyLines(head.left);
        DestroyLines(head.right);
    }
    public static GameObject DrawLine(TreeNode parent, TreeNode child)
    {
        Vector2 Pos1, Pos2;
        GameObject g1 = parent.gameObject, g2 = child.gameObject;
        Pos1 = g1.GetComponent<RectTransform>().anchoredPosition;
        Pos2 = g2.GetComponent<RectTransform>().anchoredPosition;
        GameObject g = Instantiate(Line, g1.transform.parent);
        Vector2 Pos3 = new Vector2((Pos1.x + Pos2.x) / 2, (Pos1.y + Pos2.y) / 2);
        g.GetComponent<RectTransform>().anchoredPosition = Pos3;
        g.GetComponent<RectTransform>().sizeDelta = new Vector2(5, Vector2.Distance(Pos1, Pos2));
        double angle = Mathf.Atan2(Pos1.y - Pos2.y, Pos1.x - Pos2.x) * 180 / Mathf.PI;
        g.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, (float)angle + 270);
        g.transform.SetAsFirstSibling();
        return g;
    }

    //public void updateName(TreeNode Head)
    //{
    //    LineContainer.Clear();
    //    update(Head);
    //}
    //private void update(TreeNode head) 
    //{
    //    if (head.left)
    //    {
    //        string name = head.data.ToString() + "_" + head.left.data.ToString();
    //        GameObject g = LineContainer[name];
    //        LineContainer.Add(name);
    //        update(head.left);
    //    }
    //    if (head.right)
    //    {
    //        string name = head.data.ToString() + "_" + head.right.data.ToString();
    //        GameObject g = LineContainer[name];
    //        LineContainer.Remove(name);
    //        update(head.right);
    //    }
    //}
}
