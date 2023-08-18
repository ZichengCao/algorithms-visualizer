using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class QueueModel_LinkNode : DemoModel<LinkNode>
    {
        private Pointer front, rear;
        private QueueView view;
        private float original_x = 100, original_y = 0, standard_x = 115;

        public QueueModel_LinkNode()
        {
            view = GameObject.Find("ToolsMan").GetComponent<QueueView>();
            int data_length = Random.Range(2, 5);
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateLinkNodes(list);
            //List<int> list = new List<int>();
            //CreateLinkNodes(list);
        }

        public QueueModel_LinkNode(InputField input)
        {
            view = GameObject.Find("ToolsMan").GetComponent<QueueView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            if (list.Count > 9)
            {
                view.warning.showWarning("队满");
                return;
            }
            CreateLinkNodes(list);
        }


        public void CreateLinkNodes(List<int> list)
        {
            Vector2 OriginalPos = new Vector2(original_x, original_y);

            for (int i = -1; i < list.Count; i++)
            {
                if (i == -1)
                {
                    if (list.Count != 0)
                    {
                        front = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                        front.initialize(new Vector2(original_x, original_y + 60), "front", false);
                    }
                }
                else
                {
                    LinkNode N = Instantiate(view.LINKNODE, view.DemoArea.transform).GetComponent<LinkNode>();
                    N.initializePosition(OriginalPos);
                    N.SetValue(list[i]);
                    if (i == list.Count - 1)
                    {
                        N.HidePoint();
                        rear = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                        rear.initialize(new Vector2(OriginalPos.x, original_y - 60), "rear", true);
                    }
                    OriginalPos = new Vector2(OriginalPos.x + standard_x, original_y);
                    nodes.Add(N);
                }
            }
        }


        public IEnumerator Enqueue(int val)
        {
            string originalStr = view.PreTreat(view.textAsset_LinkQueue[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("val", val);
            yield return DS_processControl.Wait(MoveTool.duration);
            if (nodes.Count == 0)
            {
                view.text_code.text = MyTools.BoldCode_success(originalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
                nodes.Add(Instantiate(view.LINKNODE, view.DemoArea.transform).GetComponent<LinkNode>());
                nodes[0].initializePosition(new Vector2(original_x, original_y));
                nodes[0].SetValue(val);
                nodes[0].HidePoint();
                front.Show();
                rear.Show();
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(originalStr, 1);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 5);
                LinkNode N = Instantiate(view.LINKNODE, view.DemoArea.transform).GetComponent<LinkNode>();
                N.initializePosition(new Vector2(original_x + nodes.Count * standard_x, original_y));
                N.SetValue(val);
                N.HidePoint();
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 6);
                nodes[nodes.Count - 1].ShowPoint();
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 7);
                rear.RightShift(standard_x, MoveTool.duration);
                nodes.Add(N);
            }
            data.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            view.FinalTreat(view.textAsset_LinkQueue[0].text);
        }

        public IEnumerator Dequeue()
        {
            string originalStr = view.PreTreat(view.textAsset_LinkQueue[2].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            MyVariable temp = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            temp.setValue("temp", nodes[0].num);
            yield return DS_processControl.Wait(MoveTool.duration);

            if (nodes.Count == 1)
            {
                view.text_code.text = MyTools.BoldCode_success(originalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
                nodes[0].Fade();
                nodes.RemoveAt(0);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 4);
                front.Fade(false);
                rear.Fade(false);
                yield return DS_processControl.Wait(MoveTool.duration);
            }
            else
            {
                view.text_code.text = MyTools.BoldCode_fail(originalStr, 2);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 7, view.sb) ;
                Pointer p = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                p.initialize(new Vector2(original_x, original_y - 60f), "p", true);
                yield return DS_processControl.Wait(MoveTool.duration);
                view.text_code.text = MyTools.BoldCode_normal(originalStr, 8);
                front.RightShift(standard_x, MoveTool.duration);
                nodes[0].Fade();
                p.Fade();
                yield return DS_processControl.Wait(MoveTool.duration);
                nodes.RemoveAt(0);

                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Move(standard_x, direction.LEFT, MoveTool.duration);
                }
                front.LeftShit(standard_x, MoveTool.duration); rear.LeftShit(standard_x, MoveTool.duration);
            }
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 11);
            temp.Fade();

            yield return DS_processControl.Wait(MoveTool.duration);
            view.FinalTreat(view.textAsset_LinkQueue[0].text);
        }

    }


    public class QueueModel_ArrayNode : DemoModel<ArrayNode>
    {
        public int maxSize = 15;
        public int head = 0, tail = 0;
        private Pointer front, rear;
        private QueueView view;
        private float original_x = 100, original_y = 0, standard_x = 65;

        public QueueModel_ArrayNode()
        {
            view = GameObject.Find("ToolsMan").GetComponent<QueueView>();
            //int data_length = Random.Range(2, 5);
            int data_length = Random.Range(2, 5);
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateArrayNodes(list);
        }

        public QueueModel_ArrayNode(InputField input)
        {
            view = GameObject.Find("ToolsMan").GetComponent<QueueView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            if (list.Count > 14)
            {
                view.warning.showWarning("队满，循环队列需要牺牲一个位置来区分队满与队空");
                return;
            }
            CreateArrayNodes(list);
        }
        public void CreateArrayNodes(List<int> list)
        {
            Vector2 OriginalPos = new Vector2(original_x, original_y);

            for (int i = -1; i <= list.Count; i++)
            {
                if (i == -1)
                {
                    head = 0;

                    if (list.Count != 0)
                    {
                        front = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                        front.initialize(new Vector2(original_x, original_y + 60), "front", false);
                    }
                }
                else if (i == list.Count)
                {
                    tail = list.Count;
                    rear = Instantiate(view.POINT, view.PointArea.transform).GetComponent<Pointer>();
                    rear.initialize(new Vector2(OriginalPos.x, original_y - 60), "rear", true);
                }
                else
                {
                    ArrayNode N = Instantiate(view.ARRAYNODE, view.DemoArea.transform).GetComponent<ArrayNode>();
                    N.initialize(OriginalPos);
                    N.SetValue(list[i]);
                    OriginalPos = new Vector2(OriginalPos.x + standard_x, original_y);
                    nodes.Add(N);
                }
            }
        }

        public IEnumerator Enqueue(int val)
        {
            string originalStr = view.PreTreat(view.textAsset_ArrayQueue[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable head_vairable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            head_vairable.setValue("front", head);
            MyVariable tail_variable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            tail_variable.setValue("rear", tail);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("val", val);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            ArrayNode N = Instantiate(view.ARRAYNODE, view.DemoArea.transform).GetComponent<ArrayNode>();

            N.initialize(new Vector2(original_x + tail * standard_x, original_y));

            N.SetValue(val);
            nodes.Add(N);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            tail = (tail + 1) % maxSize;
            tail_variable.setValue(tail);
            if (tail == 0)
                rear.rect.DOAnchorPosX(original_x, MoveTool.duration);
            else
                rear.RightShift(standard_x, MoveTool.duration);
            yield return DS_processControl.Wait(MoveTool.duration);
            head_vairable.Fade();
            tail_variable.Fade();
            data.Fade();
            view.FinalTreat(view.textAsset_ArrayQueue[0].text);
        }

        public IEnumerator Dequeue()
        {
            string originalStr = view.PreTreat(view.textAsset_ArrayQueue[2].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable head_vairable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            head_vairable.setValue("front", head);
            MyVariable tail_variable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            tail_variable.setValue("rear", tail);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            MyVariable temp = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            temp.setValue("temp", nodes[0].num);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            head = (head + 1) % maxSize;
            head_vairable.setValue(head);
            if (head == 0)
                front.rect.DOAnchorPosX(original_x, MoveTool.duration);
            else
                front.RightShift(standard_x, MoveTool.duration);

            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
            temp.Fade();
            nodes[0].Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            nodes.RemoveAt(0);
            head_vairable.Fade();
            tail_variable.Fade();
            view.FinalTreat(view.textAsset_ArrayQueue[0].text);
        }

    }
}
