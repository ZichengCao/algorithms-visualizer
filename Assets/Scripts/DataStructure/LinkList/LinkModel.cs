using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using DG.Tweening;

namespace FunnyAlgorithm
{
    public abstract class LinkModel : DemoModel<LinkNode>
    {
        protected LinkView view;
        public float original_x = 100, standard_x = 115;
        public LinkNode Head;

        //public LinkModel()
        //{
        //    view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
        //    //int data_length = Random.Range(2, 5);
        //    int data_length = 5;
        //    List<int> list = MyTools.GetNoRepeatList(data_length);
        //    CreateLinkNodes(list);
        //}

        //public LinkModel(InputField input)
        //{
        //    view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
        //    string str = input.text;
        //    List<int> list = MyTools.getInputIntegerStringList(str);
        //    CreateLinkNodes(list);
        //}

        public string PreTreat(string str)
        {
            IsRun = true;
            DS_processControl.passport = false;
            for(int i = 0; i < view.SelectableGroups.Length; i++)
            {
                if (view.SelectableGroups[i].gameObject.activeSelf)
                {
                    view.SelectableGroups[i].interactable = false;
                }
            }
            view.next_btn.interactable = true;
            return view.text_code.text = MyTools.ColourKeyWord(str);
        }

        public void FinalTreat(string originalStr)
        {
            IsRun = false;
            view.text_code.text = MyTools.ColourKeyWord(originalStr);
            for (int i = 0; i < view.SelectableGroups.Length; i++)
            {
                if (view.SelectableGroups[i].gameObject.activeSelf)
                {
                    view.SelectableGroups[i].interactable = true;
                }
            }
            view.next_btn.interactable = false;
            view.Control.UpdateInputText();
        }

        public abstract IEnumerator PushFront(int val);
        public abstract IEnumerator PushBack(int val);
        //public IEnumerator SpecifyInsert(int val, int index)
        //{
        //    string originalStr = PreTreat(view.textAsset_pushGroups[2].text);
        //    view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
        //    MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
        //    data.setValue("val", val);
        //    MyVariable k = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
        //    k.setValue("k", index + 1);
        //    yield return DS_processControl.Wait(MoveTool.duration);
        //    view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
        //    Pointer p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
        //    p.initialize(new Vector2(original_x, 50f), "p", false);
        //    for (int i = 0; i < index + 1; i++)
        //    {
        //        if (i != index)
        //        {
        //            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
        //            yield return DS_processControl.Wait(MoveTool.duration);

        //            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
        //            p.RightShift(standard_x, MoveTool.duration);
        //            yield return DS_processControl.Wait(MoveTool.duration);

        //            view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
        //            k.setValue(index - i);
        //            yield return DS_processControl.Wait(MoveTool.duration);
        //        }
        //        else
        //        {
        //            view.text_code.text = MyTools.BoldCode_fail(originalStr, 2);
        //            yield return DS_processControl.Wait(MoveTool.duration);
        //        }
        //    }
        //    view.text_code.text = MyTools.BoldCode_normal(originalStr, 6);

        //    LinkNode N = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<LinkNode>();
        //    N.initialize(new Vector2(original_x + standard_x * (index + 1) - 40, -50f));
        //    N.SetValue(val);

        //    yield return DS_processControl.Wait(MoveTool.duration);
        //    view.text_code.text = MyTools.BoldCode_normal(originalStr, 7);
        //    N.AdjustPointTo(direction.UP);
        //    yield return DS_processControl.Wait(MoveTool.duration);
        //    view.text_code.text = MyTools.BoldCode_normal(originalStr, 8);
        //    nodes[index].AdjustPointTo(direction.DOWN);
        //    yield return DS_processControl.Wait(MoveTool.duration);
        //    for (int i = index + 1; i < nodes.Count; i++)
        //    {
        //        nodes[i].rect.DOAnchorPosX(nodes[i].rect.anchoredPosition.x + standard_x, MoveTool.duration);
        //    }
        //    N.rect.DOAnchorPos(new Vector2(N.rect.anchoredPosition.x + 40, 0), MoveTool.duration);
        //    nodes[index].AdjustPointTo(direction.RIGHT);
        //    N.AdjustPointTo(direction.RIGHT);
        //    k.Fade();
        //    p.Fade();
        //    data.Fade();
        //    yield return DS_processControl.Wait(MoveTool.duration);
        //    nodes.Insert(index + 1, N);
        //    FinalTreat(view.textAsset_push.text);
        //}
        public IEnumerator Search(int val)
        {
            string originalStr = PreTreat(view.textAssetsSingleList[2].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable value = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            value.setValue("value", val);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            Pointer p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            p.initialize(new Vector2(original_x + standard_x, 50), "p", false);
            yield return DS_processControl.Wait(MoveTool.duration);

            bool flag = false;
            int i = 0;
            for (; i < nodes.Count - 1; i++)
            {
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                if (val == nodes[i + 1].num)
                {
                    view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                    nodes[i + 1].SetInnerColor(ColorSetting.successed);
                    flag = true;
                    break;
                }
                else
                {
                    view.text_code.text = MyTools.BoldCode_fail(originalStr, 3);
                    nodes[i + 1].SetInnerColor(ColorSetting.failed);
                    yield return DS_processControl.Wait(MoveTool.duration * 0.5f);
                    nodes[i + 1].SetInnerColor(ColorSetting.normal);
                }
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
                p.RightShift(standard_x, MoveTool.duration);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
            if (flag)
            {
                yield return DS_processControl.Wait(MoveTool.duration * 0.5f);
                nodes[i + 1].SetInnerColor(ColorSetting.normal);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 7);
            }
            yield return DS_processControl.Wait(MoveTool.duration);
            value.Fade();
            p.Fade();
            FinalTreat(view.textAssetSingleListSummary.text);
        }
        public abstract IEnumerator Remove(int value);
        public void CreateLinkNodes(List<int> list, bool doubleListNodeFlag=false)
        {
            Vector2 OriginalPos = new Vector2(original_x, 0);

            for (int i = -1; i < list.Count; i++)
            {
                LinkNode N = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<LinkNode>();
                N.initializePosition(OriginalPos);
                if (doubleListNodeFlag)
                {
                    transformIntoDoubleListNode(N);
                }
                OriginalPos = new Vector2(OriginalPos.x + standard_x, 0);
                // 头结点
                if (i == -1)
                {
                    N.num_text.text = "head";
                    N.num_text.fontSize -= 5;
                    N.HidePoint(1); // 隐藏prior指针
                    if (list.Count == 0)
                    {
                        N.HidePoint();//隐藏 next指针
                    }
                    Head = N;
                    nodes.Add(N);
                }
                else
                {
                    N.SetValue(list[i]);
                    // 最后一个结点隐藏next指针
                    if (i == list.Count - 1)
                    {
                        N.HidePoint();
                    }
                    nodes.Add(N);
                }

                // 如果是双链表，转换成双链表结点，头结点不转
                //if (doubleListNodeFlag && i != -1)
                //{
                //    transformIntoDoubleListNode(N);
                //    nodes[i].nextPointShift();
                //    if (i != nodes.Count - 1)
                //    {
                //        nodes[i + 1].priorPointShift();
                //    }
                //}
            }
        }

        /// <summary>
        /// 转换成单链表结点
        /// </summary>
        public static void transformIntoSingleListNode(LinkNode node)
        {
            node.inner.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 50);
            node.prior.SetActive(false);
        }

        public static void transformIntoDoubleListNode(LinkNode node)
        {
            node.inner.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 50);
            node.prior.SetActive(true);
            node.nextPointShift();
            node.priorPointShift();
        }
    }

    public class SingleLinkList : LinkModel
    {
        public SingleLinkList()
        {
            //Debug.Log("single link list constructer");
            view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
            int data_length = 5;
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateLinkNodes(list);
        }
        public SingleLinkList(InputField input)
        {
            //Debug.Log("single link list constructer by inputlist");
            view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            CreateLinkNodes(list);
        }

        public override IEnumerator PushFront(int val)
        {
            string originalStr = PreTreat(view.textAssetsSingleList[0].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("value", val);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);

            LinkNode N = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<LinkNode>();
            N.initializePosition(new Vector2(original_x + standard_x, -50f));
            N.SetValue(val);
            
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);

            N.AdjustPointTo(direction.UP);

            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
            for (int i = 1; i < nodes.Count; i++)
            {
                //nodes[i].rect.DOAnchorPosX(nodes[i].rect.anchoredPosition.x + standard_x, MoveTool.duration);
                nodes[i].Move(standard_x, direction.RIGHT, MoveTool.duration);
            }
            nodes.Insert(1, N);
            nodes[1].rect.DOAnchorPosY(0, MoveTool.duration);
            nodes[1].AdjustPointTo(direction.RIGHT);
            data.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            FinalTreat(view.textAssetSingleListSummary.text);
        }
        public override IEnumerator PushBack(int val)
        {
            string originalStr = PreTreat(view.textAssetsSingleList[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("value", val);

            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);

            GameObject g = Instantiate(view.NODE, view.DemoArea.transform);
            LinkNode N = g.GetComponent<LinkNode>();
            N.initializePosition(new Vector2(original_x + standard_x * (nodes.Count), -50f));
            N.SetValue(val);
            N.HidePoint();
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            Pointer p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            p.initialize(new Vector2(original_x, 50f), "p", false) ;
            yield return DS_processControl.Wait(MoveTool.duration);

            //Head.SetColor(MainControl.ColorSetting["selected"]);
            //Head.SetInnerColor(ColorSetting.selected);
            //yield return DS_processControl.Wait(MoveTool.duration);
            //Head.SetInnerColor(ColorSetting.normal);   //Head.image.color = new Color(1, 1, 1, 0.4156f);

            for (int i = 0; i < nodes.Count; i++)
            {
                if (i != nodes.Count - 1)
                {
                    view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
                    p.RightShift(standard_x, MoveTool.duration);
                    yield return DS_processControl.Wait(MoveTool.duration);
                }
                else
                {
                    view.text_code.text = MyTools.BoldCode_fail(originalStr, 3);
                    yield return DS_processControl.Wait(MoveTool.duration);
                }
            }

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 6);
            nodes[nodes.Count - 1].ShowPoint();
            nodes.Add(N);
            N.Move(50, direction.UP, MoveTool.duration);
            p.Fade();
            data.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            FinalTreat(view.textAssetSingleListSummary.text);
        }
       
      
        public override IEnumerator Remove(int value)
        {
            string originalStr = PreTreat(view.textAssetsSingleList[3].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable variableValue = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            variableValue.setValue("value", value);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            Pointer pointer_p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            pointer_p.initialize(new Vector2(original_x, 50f), "p", false);
            yield return DS_processControl.Wait(MoveTool.duration);

            int i = 0;
            for (; i + 1 < nodes.Count && nodes[i + 1].num != value; i++) 
            {
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                pointer_p.RightShift(standard_x, MoveTool.duration);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
            view.text_code.text = MyTools.BoldCode_fail(originalStr, 2);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
            nodes[i + 1].rect.DOAnchorPosY(-60, MoveTool.duration);
            Pointer pointer_temp = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            pointer_temp.initialize(new Vector2(original_x + standard_x * (i + 1), 50f), "temp", false) ;
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
            for (int j = i + 2; j < nodes.Count; j++)
            {
                nodes[j].rect.DOAnchorPosX(nodes[j].rect.anchoredPosition.x - standard_x, MoveTool.duration);
            }
            nodes[i + 1].Fade();
            nodes.RemoveAt(i + 1);
            yield return DS_processControl.Wait(MoveTool.duration);
            nodes[nodes.Count - 1].HidePoint();
            pointer_p.Fade();
            pointer_temp.Fade();
            variableValue.Fade();
            FinalTreat(view.textAssetSingleListSummary.text);
        }
        //public void CreateLinkNodes(List<int> list)
        //{
        //    Vector2 OriginalPos = new Vector2(original_x, 0);

        //    for (int i = -1; i < list.Count; i++)
        //    {
        //        LinkNode N = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<LinkNode>();
        //        N.initializePosition(OriginalPos);
        //        OriginalPos = new Vector2(OriginalPos.x + standard_x, 0);
        //        if (i == -1)
        //        {
        //            N.num_text.text = "head";
        //            N.num_text.fontSize -= 5;
        //            nodes.Add(N);
        //        }
        //        else
        //        {
        //            N.SetValue(list[i]);
        //            if (i == list.Count - 1)
        //                N.HidePoint();
        //            nodes.Add(N);
        //        }
        //    }
        //}

        
    }

    public class DoubleLinkList : LinkModel
    {
        public DoubleLinkList()
        {
            view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
            int data_length = 5;
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateLinkNodes(list, true) ;
        }

        public DoubleLinkList(InputField input)
        {
            view = GameObject.Find("ToolsMan").GetComponent<LinkView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            CreateLinkNodes(list, true) ;
        }

        /// <summary>
        /// 双链表，指针箭头不能位于中间，否则 next prior 会重合
        /// </summary>
        public void adjustArrowHeight()
        {

        }

        public override IEnumerator PushBack(int val)
        {
            string originalStr = PreTreat(view.textAssetsDoubleList[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("value", val);

            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            LinkNode N = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<LinkNode>(); ;
            N.initializePosition(new Vector2(original_x + standard_x * (nodes.Count) - standard_x / 2, -50f)) ;
            N.SetValue(val);
            transformIntoDoubleListNode(N);
            N.HidePoint();
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            Pointer p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            p.initialize(new Vector2(original_x, 50f), "p", false);
            yield return DS_processControl.Wait(MoveTool.duration);

            //Head.SetColor(MainControl.ColorSetting["selected"]);
            //Head.SetInnerColor(ColorSetting.selected);
            //yield return DS_processControl.Wait(MoveTool.duration);
            //Head.SetInnerColor(ColorSetting.normal);   //Head.image.color = new Color(1, 1, 1, 0.4156f);

            for (int i = 0; i < nodes.Count; i++)
            {
                if (i != nodes.Count - 1)
                {
                    view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                    yield return DS_processControl.Wait(MoveTool.duration);
                    view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
                    p.RightShift(standard_x, MoveTool.duration);
                    yield return DS_processControl.Wait(MoveTool.duration);
                }
                else
                {
                    view.text_code.text = MyTools.BoldCode_fail(originalStr, 3);
                    yield return DS_processControl.Wait(MoveTool.duration);
                }
            }
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
            N.AdjustPointTo(direction.UP, 1);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 6);
            nodes[nodes.Count - 1].ShowPoint();
            N.rect.DOAnchorPos(new Vector2(original_x + standard_x * (nodes.Count), 0f), MoveTool.duration);
            nodes.Add(N);
            N.AdjustPointTo(direction.LEFT, 1);
            //N.Move(50, direction.UP, MoveTool.duration);
            p.Fade();
            data.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            FinalTreat(view.textAssetDoubleListSummary.text);
        }

        public override IEnumerator PushFront(int val)
        {
            string originalStr = PreTreat(view.textAssetsDoubleList[0].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("value", val);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);

            GameObject g = Instantiate(view.NODE, view.DemoArea.transform);
            LinkNode N = g.GetComponent<LinkNode>();
            N.initializePosition(new Vector2(original_x + standard_x / 2, -50f)) ;
            N.SetValue(val);
            transformIntoDoubleListNode(N);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            N.AdjustPointTo(direction.UP);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
            N.AdjustPointTo(direction.UP, 1);
            yield return DS_processControl.Wait(MoveTool.duration);

            if (nodes.Count > 1)
            {
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(originalStr, 4);
            }
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
            view.text_code.text = MyTools.BoldCode_normal(view.text_code.text, 6);
            for (int i = 1; i < nodes.Count; i++)
            {
                //nodes[i].rect.DOAnchorPosX(nodes[i].rect.anchoredPosition.x + standard_x, MoveTool.duration);
                nodes[i].Move(standard_x, direction.RIGHT, MoveTool.duration);
            }
            nodes.Insert(1, N);
            nodes[1].rect.DOAnchorPos(new Vector2(original_x + standard_x, 0f), MoveTool.duration) ;
            //nodes[1].node_rect.DOAnchorPosY(0, MoveTool.duration);
            nodes[1].AdjustPointTo(direction.RIGHT);
            nodes[1].AdjustPointTo(direction.LEFT, 1);
            data.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            FinalTreat(view.textAssetDoubleListSummary.text);
        }

        public override IEnumerator Remove(int value)
        {
            string originalStr = PreTreat(view.textAssetsDoubleList[3].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable variableValue = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            variableValue.setValue("value", value);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            Pointer pointer_p = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            pointer_p.initialize(new Vector2(original_x, 50f), "p", false);
            yield return DS_processControl.Wait(MoveTool.duration);

            int i = 0;
            for (; i + 1 < nodes.Count && nodes[i + 1].num != value; i++)
            {
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                pointer_p.RightShift(standard_x, MoveTool.duration);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
            view.text_code.text = MyTools.BoldCode_fail(originalStr, 2);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
            nodes[i + 1].rect.DOAnchorPosY(-60, MoveTool.duration);
            Pointer pointer_temp = Instantiate(view.PONINTER, view.PointArea.transform).GetComponent<Pointer>();
            pointer_temp.initialize(new Vector2(original_x + standard_x * (i + 1), 50f), "temp", false);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
            view.text_code.text = MyTools.BoldCode_normal(view.text_code.text, 6);
            for (int j = i + 2; j < nodes.Count; j++)
            {
                nodes[j].rect.DOAnchorPosX(nodes[j].rect.anchoredPosition.x - standard_x, MoveTool.duration);
            }
            nodes[i + 1].Fade();
            nodes.RemoveAt(i + 1);
            yield return DS_processControl.Wait(MoveTool.duration);
            nodes[nodes.Count - 1].HidePoint();
            pointer_p.Fade();
            pointer_temp.Fade();
            variableValue.Fade();
            FinalTreat(view.textAssetDoubleListSummary.text);
        }
    }
}

