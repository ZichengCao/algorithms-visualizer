using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


namespace FunnyAlgorithm
{

    public class TreeModel : DemoModel<TreeNode>
    {
        private TreeView view;
        private float node_radius;
        private HashSet<int> num_set = new HashSet<int>();
        public TreeNode Head;
        public int height;
        private List<Vector2> list_treeNodePosition = new List<Vector2>();
        private int node_count = 0;
        private Vector2 PointOffset;
        //public List<BTreeNode> list_treeNodes = new List<BTreeNode>();
        public TreeModel()
        {
            view = GameObject.Find("ToolsMan").GetComponent<TreeView>();
            CalcTreeNodePos();
            BuildArray(0, 0);
            BuildTree();
            //SetBTreeValue();
            create_bstree_low = 0;
            SetBSTreeValue(Head);
            UpdateHeight();
            DrawLine(Head);
        }

        public int serial_number = 1;
        private void HideSerialNumber(TreeNode Head)
        {
            if (Head == null) return;
            Head.hideSerialNumber();
            HideSerialNumber(Head.left);
            HideSerialNumber(Head.right);
        }
        public string PreTreat(string str)
        {
            DS_processControl.passport = false;
            HideSerialNumber(Head);
            view.next_btn.interactable = true;
            for (int i = 0; i < view.SelectableGroups.Length; i++)
            {
                view.SelectableGroups[i].interactable = false;
            }
            return view.text_code.text = MyTools.ColourKeyWord(str);
        }
        public void FinalTreat(string originalStr)
        {
            view.next_btn.interactable = false;

            for (int i = 0; i < view.SelectableGroups.Length; i++)
            {
                view.SelectableGroups[i].interactable = true;
            }
            view.text_code.text = MyTools.ColourKeyWord(originalStr);
            if (view.Input_Insert.gameObject.activeSelf)
            {
                view.Input_Insert.gameObject.SetActive(false);
            }
            view.Input_Search.gameObject.SetActive(false);
        }
        public IEnumerator Preorder()
        {
            string OriginalStr = PreTreat(view.textAsset_Traverse[1].text);
            Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
            P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
            P.SetDirection(direction.RIGHT);
            yield return Preorder(Head, OriginalStr, P);
            P.Fade();
            FinalTreat(OriginalStr);
        }
        private IEnumerator Preorder(TreeNode T, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 1);
            T.SetColor(ColorSetting.successed);
            yield return DS_processControl.Wait(MoveTool.duration);
            T.SetColor(ColorSetting.normal);
            T.SetSerialNumber(serial_number++);
            yield return DS_processControl.Wait(MoveTool.duration);

            if (T.left != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 3);
                P.MoveTo(T.left.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Preorder(T.left, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);

            }
            P.MoveTo(T.rect.anchoredPosition + PointOffset);
            if (T.right != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 4);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 5);
                P.MoveTo(T.right.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Preorder(T.right, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 4);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
        }
        public IEnumerator Inorder()
        {
            string OriginalStr = PreTreat(view.textAsset_Traverse[2].text);
            Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
            P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
            P.SetDirection(direction.RIGHT);
            yield return Inorder(Head, OriginalStr, P);
            P.Fade();
            FinalTreat(OriginalStr);
        }
        private IEnumerator Inorder(TreeNode T, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);
            if (T.left != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 2);
                P.MoveTo(T.left.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Inorder(T.left, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);

            }
            P.MoveTo(T.rect.anchoredPosition + PointOffset);

            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 3);
            T.SetColor(ColorSetting.successed);
            yield return DS_processControl.Wait(MoveTool.duration);
            T.SetColor(ColorSetting.normal);
            T.SetSerialNumber(serial_number++);
            yield return DS_processControl.Wait(MoveTool.duration);


            if (T.right != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 4);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 5);
                P.MoveTo(T.right.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Inorder(T.right, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 4);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
        }
        public IEnumerator Postorder()
        {
            string OriginalStr = PreTreat(view.textAsset_Traverse[3].text);
            Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
            P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
            P.SetDirection(direction.RIGHT);
            yield return Postorder(Head, OriginalStr, P);
            P.Fade();
            FinalTreat(OriginalStr);
        }
        private IEnumerator Postorder(TreeNode T, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);

            if (T.left != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 2);
                P.MoveTo(T.left.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Postorder(T.left, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);

            }
            P.MoveTo(T.rect.anchoredPosition + PointOffset);

            if (T.right != null)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 4);
                P.MoveTo(T.right.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Postorder(T.right, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
            P.MoveTo(T.rect.anchoredPosition + PointOffset);
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 5);
            T.SetColor(ColorSetting.successed);
            yield return DS_processControl.Wait(MoveTool.duration);
            T.SetColor(ColorSetting.normal);
            T.SetSerialNumber(serial_number++);
            yield return DS_processControl.Wait(MoveTool.duration);
        }
        public IEnumerator Search(int x)
        {
            string OriginalStr = PreTreat(view.textAsset_BSTree[1].text);
            Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
            P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
            P.SetDirection(direction.RIGHT);
            yield return Search(Head, x, OriginalStr, P);
            FinalTreat(OriginalStr);
        }
        private IEnumerator Search(TreeNode T, int x, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);

            if (T.num == x)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 1);
                T.SetColor(ColorSetting.successed);
                yield return DS_processControl.Wait(MoveTool.duration);
                T.SetColor(ColorSetting.normal);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 2);
                P.Fade();
                yield return DS_processControl.Wait(MoveTool.duration);
                yield break;
            }
            else if (x < T.num && T.left != null)
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 4);
                P.MoveTo(T.left.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Search(T.left, x, OriginalStr, P);
            }
            else if (x > T.num && T.right != null)
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 5);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 6);
                P.MoveTo(T.right.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Search(T.right, x, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 5);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 7);
                yield return DS_processControl.Wait(MoveTool.duration);
                P.Fade();
            }
        }
        private bool TryInsert(TreeNode T, TreeNode Pre, int x)
        {
            if (Pre == null) return true;
            if (T == null)
            {
                int index = x < Pre.num ? Pre.index * 2 + 1 : Pre.index * 2 + 2;
                if (index < 31) return true;
                else return false;
            }
            if (x < T.num)
                return TryInsert(T.left, T, x);
            else
                return TryInsert(T.right, T, x);
        }
        public IEnumerator Insert(int x)
        {
            if (num_set.Contains(x))
            {
                string str = MyTools.ColorText(x.ToString(), "#0000FF") + "已存在，请不要插入重复值";
                view.warning.showWarning(str);
                yield break;
            }
            else if (Head && !TryInsert(Head, Head, x))
            {
                string str = "插入" + MyTools.ColorText(x.ToString(), "#0000FF") + "后树过高，便于演示请更改输入值";
                view.warning.showWarning(str);
                yield break;
            }
            else
            {
                num_set.Add(x);
                string OriginalStr = PreTreat(view.textAsset_BSTree[2].text);
                Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
                P.SetDirection(direction.RIGHT);
                yield return Insert(Head, null, x, OriginalStr, P);
                P.Fade();
                UpdateHeight();
                FinalTreat(OriginalStr);
            }
        }
        private IEnumerator Insert(TreeNode T, TreeNode Pre, int x, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);
            if (T == null)
            {
                //树为空时，插入首个节点
                if (Pre == null)
                {
                    Head = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<TreeNode>();
                    Head.initialize(list_treeNodePosition[0], node_radius, 0);
                    Head.SetValue(x);
                }
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 2);
                int index = x < Pre.num ? Pre.index * 2 + 1 : Pre.index * 2 + 2;
                Vector2 GeneratePos = list_treeNodePosition[index];
                T = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<TreeNode>();
                T.initialize(GeneratePos, node_radius, index);
                T.SetValue(x);
                if (x < Pre.num)
                    Pre.left = T;
                else
                    Pre.right = T;
                Pre.DrawLine();
                yield return DS_processControl.Wait(MoveTool.duration);

                yield break;
            }
            view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
            yield return DS_processControl.Wait(MoveTool.duration);
            if (x < T.num)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 4);
                Vector2 endPos = T.left != null ? T.left.rect.anchoredPosition + PointOffset : list_treeNodePosition[T.index * 2 + 1] + PointOffset;
                P.MoveTo(endPos);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Insert(T.left, T, x, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 3);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 6);
                Vector2 endPos = T.right != null ? T.right.rect.anchoredPosition + PointOffset : list_treeNodePosition[T.index * 2 + 2] + PointOffset;
                P.MoveTo(endPos);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Insert(T.right, T, x, OriginalStr, P);
            }
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 7);
            P.MoveTo(T.rect.anchoredPosition + PointOffset);
            yield return DS_processControl.Wait(MoveTool.duration);
        }
        public IEnumerator Remove(int x)
        {

            string OriginalStr = PreTreat(view.textAsset_BSTree[3].text);
            Pointer P = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
            P.initialize(Head.rect.anchoredPosition + PointOffset, "T", false);
            P.SetDirection(direction.RIGHT);
            yield return Remove(Head, null, x, OriginalStr, P);
            num_set.Remove(x);
            P.Fade();
            UpdateHeight();
            FinalTreat(OriginalStr);

        }
        private IEnumerator Remove(TreeNode T, TreeNode Pre, int x, string OriginalStr, Pointer P)
        {
            view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 0, view.sb);
            yield return DS_processControl.Wait(MoveTool.duration);
            if (T.num == x)
            {
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                if (!T.left && !T.right)
                {
                    view.text_code.text = MyTools.BoldCode_success(OriginalStr, 2);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 3);
                    T.Fade();
                    if (Pre == null)//删除的是头结点
                    {
                        Head = null;
                        yield break;
                    }
                    //等于为了处理第三种情况中先把 p.data赋值给了T.data
                    if (x <= Pre.num)
                    {
                        Pre.left = null;
                        Pre.DestroyLine(childTree.LEFT);
                    }
                    else
                    {
                        Pre.right = null;
                        Pre.DestroyLine(childTree.RIGHT);
                    }
                    //nodes[T.index] = null;
                    yield break;
                }
                else if (!T.left || !T.right)
                {
                    view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 2);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_success(OriginalStr, 4);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 5);
                    T.Fade();//让T褪色，1s后才会销毁
                    if (Pre == null) //删除的是头结点
                    {
                        LineTools.DestroyLines(Head);
                        Head = Head.left ? Head.left : Head.right;
                        Head.index = 0;
                        updateIndex(Head);
                        yield return new WaitForSeconds(MoveTool.duration);
                        DrawLine(Head);
                        yield break;
                    }
                    if (x <= Pre.num)
                    {
                        Pre.left = T.left ? T.left : T.right;
                    }
                    else
                    {
                        Pre.right = T.left ? T.left : T.right;
                    }
                    LineTools.DestroyLines(T);
                    //linetool.DestroyLine(T);
                    updateIndex(Head);
                    yield return new WaitForSeconds(MoveTool.duration);
                    //yield return DS_processControl.Wait(MoveTool.duration);
                    DrawLine(x <= Pre.num ? Pre.left : Pre.right);
                    yield break;
                }
                else
                {
                    view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 2);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 4);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 7, view.sb);
                    Pointer temp = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                    temp.initialize(T.left.rect.anchoredPosition + PointOffset, "p", false);
                    temp.SetDirection(direction.RIGHT);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    TreeNode pp = T.left, ppre = T;
                    while (pp.right)
                    {
                        view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 8);
                        yield return DS_processControl.Wait(MoveTool.duration);
                        view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 9);
                        temp.MoveTo(pp.right.rect.anchoredPosition + PointOffset);
                        yield return DS_processControl.Wait(MoveTool.duration);
                        ppre = pp;
                        pp = pp.right;
                    }
                    view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 8);
                    yield return DS_processControl.Wait(MoveTool.duration);

                    view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 10);
                    T.SetValue(pp.num);

                    yield return DS_processControl.Wait(MoveTool.duration);

                    view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 11);
                    temp.Fade();
                    P.MoveTo(T.left.rect.anchoredPosition + PointOffset);

                    yield return DS_processControl.Wait(MoveTool.duration);
                    yield return Remove(T.left, T, pp.num, OriginalStr, P);
                }
            }
            else if (x > T.num)
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_success(OriginalStr, 14, view.sb);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 15);
                P.MoveTo(T.right.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Remove(T.right, T, x, OriginalStr, P);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_fail(OriginalStr, 14, view.sb);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(OriginalStr, 17, view.sb);
                P.MoveTo(T.left.rect.anchoredPosition + PointOffset);
                yield return DS_processControl.Wait(MoveTool.duration);
                yield return Remove(T.left, T, x, OriginalStr, P);
            }
        }

        #region build the tree

        private void DrawLine(TreeNode Head)
        {
            if (Head == null) return;
            Head.DrawLine();
            DrawLine(Head.left);
            DrawLine(Head.right);
        }
        private void CalcTreeNodePos()
        {
            Vector2 size = view.DemoArea.GetComponent<RectTransform>().sizeDelta;
            float level_height = size.y / 5;
            //第一层
            list_treeNodePosition.Add(new Vector2(0, 2 * level_height));
            //第二层
            float standard = size.x / 2, originalPos = 0f;
            list_treeNodePosition.Add(new Vector2(-standard / 2, level_height));
            list_treeNodePosition.Add(new Vector2(standard / 2, level_height));
            //第三层
            standard = size.x / 4;
            originalPos = -(size.x / 2 - standard / 2);
            for (int i = 0; i < 4; i++)
            {
                list_treeNodePosition.Add(new Vector2(originalPos, 0));
                originalPos += standard;
            }
            //第四层
            standard = size.x / 8;
            originalPos = -(size.x / 2 - standard / 2);
            for (int i = 0; i < 8; i++)
            {
                list_treeNodePosition.Add(new Vector2(originalPos, -level_height));
                originalPos += standard;
            }
            //第五层
            standard = size.x / 16;
            node_radius = standard * 0.95f;
            originalPos = -(size.x / 2 - standard / 2);
            for (int i = 0; i < 16; i++)
            {
                list_treeNodePosition.Add(new Vector2(originalPos, -level_height * 2));
                originalPos += standard;
            }
        }

        private bool[] build_Tree_Flag = new bool[32];

        private void BuildArray(int index, int level)
        {

            if (level > 4) return;
            if (MyTools.TossCoin(level) && node_count < 26)
            {
                build_Tree_Flag[index] = true;
                BuildArray(index * 2 + 1, level + 1);
                BuildArray(index * 2 + 2, level + 1);
                node_count++;
            }
            else
            {
                build_Tree_Flag[index] = false;
            }

        }
        private void BuildTree()
        {
            for (int i = 0; i < 31; i++)
            {
                int level = (int)Mathf.Ceil((Mathf.Log(i + 2, 2))) - 1;
                if (!build_Tree_Flag[i])
                {
                    nodes.Add(null);
                    //nodes[i].gameObject.SetActive(false);
                }
                else
                {
                    nodes.Add(Instantiate(view.NODE, view.DemoArea.transform).GetComponent<TreeNode>());
                    nodes[i].initialize(list_treeNodePosition[i], node_radius, i);
                }
            }
            Head = nodes[0];
            for (int i = 0; i < 31; i++)
            {
                if (build_Tree_Flag[i])
                {
                    TreeNode N = (TreeNode)(nodes[i]);
                    int left = i * 2 + 1, right = i * 2 + 2;
                    if (left < 31 && build_Tree_Flag[left])
                    {
                        N.left = nodes[left];
                    }
                    else
                    {
                        N.left = null;

                    }
                    if (right < 31 && build_Tree_Flag[right])
                    {
                        N.right = nodes[right];
                    }
                    else
                    {
                        N.right = null;
                    }
                }
            }
        }

        private void SetBTreeValue(TreeNode T, int low, int high)
        {
            if (T == null) return;
            T.SetValue(Random.Range(low, high + 1));
            SetBTreeValue(T.left, low, high);
            SetBTreeValue(T.right, low, high);
        }

        private int create_bstree_low = 0;

        private void SetBSTreeValue(TreeNode T)
        {
            if (T == null) return;
            SetBSTreeValue(T.left);
            create_bstree_low += Random.Range(3, 7);
            num_set.Add(create_bstree_low);
            T.SetValue(create_bstree_low);
            SetBSTreeValue(T.right);
        }

        private int CalcHeight(TreeNode T)
        {
            if (T == null) return 0;
            return Mathf.Max(CalcHeight(T.left), CalcHeight(T.right)) + 1;
        }

        public void UpdateHeight()
        {
            int temp = CalcHeight(Head);
            if (height != temp)
            {
                height = temp;
                PointOffset = new Vector2(view.DemoArea.GetComponent<RectTransform>().sizeDelta.x / 2 - node_radius * 1.25f, 5);
                if (height == 1)
                {
                    node_radius = 60;
                }
                else if (height == 2)
                {
                    node_radius = 55f;
                }
                else if (height == 3)
                {
                    node_radius = 50f;

                }
                else if (height == 4)
                {
                    node_radius = 45f;
                }
                else if (height == 5)
                {
                    node_radius = 40f;
                }
                UpdateHeight(Head);
            }
        }
        private void UpdateHeight(TreeNode Head)
        {
            if (!Head) return;
            Head.SetRadius(node_radius);
            UpdateHeight(Head.left);
            UpdateHeight(Head.right);
        }

        private void updateIndex(TreeNode Head)
        {
            if (Head == null) return;
            Head.rect.DOAnchorPos(list_treeNodePosition[Head.index], MoveTool.duration);
            if (Head.left)
            {
                Head.left.index = Head.index * 2 + 1;
                updateIndex(Head.left);
            }
            if (Head.right)
            {
                Head.right.index = Head.index * 2 + 2;
                updateIndex(Head.right);
            }
        }

        #endregion


    }


}

