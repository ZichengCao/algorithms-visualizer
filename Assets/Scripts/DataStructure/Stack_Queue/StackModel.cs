using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class StackModel_LinkNode : DemoModel<LinkNode>
    {
        private Pointer top;
        private StackView view;
        public LinkNode Head;
        private float original_x = 100, original_y = 0, standard_x = 115;

        public StackModel_LinkNode()
        {
            view = GameObject.Find("ToolsMan").GetComponent<StackView>();
            int data_length = Random.Range(2, 5);
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateLinkNodes(list);
            //List<int> list = new List<int>();
            //CreateLinkNodes(list);
        }

        public StackModel_LinkNode(InputField input)
        {
            view = GameObject.Find("ToolsMan").GetComponent<StackView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            if (list.Count > 9)
            {
                view.warning.showWarning("栈满");
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
                    GameObject g = Instantiate(view.POINT, view.PointArea.transform);
                    top = g.GetComponent<Pointer>();
                    top.initialize(new Vector2(original_x, original_y + 60), "top", false);
                }
                else
                {
                    GameObject g = Instantiate(view.LINKNODE, view.DemoArea.transform);
                    LinkNode N = g.GetComponent<LinkNode>();
                    N.initializePosition(OriginalPos);
                    OriginalPos = new Vector2(OriginalPos.x + standard_x, original_y);
                    N.SetValue(list[i]);
                    if (i == list.Count - 1)
                        N.HidePoint();
                    nodes.Add(N);
                }
            }
        }


        public IEnumerator Push(int val)
        {
            string originalStr = view.PreTreat(view.textAsset_LinkStack[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("val", val);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            LinkNode N = Instantiate(view.LINKNODE, view.DemoArea.transform).GetComponent<LinkNode>();
            N.initializePosition(new Vector2(original_x, original_y - 60));
            N.SetValue(val);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            N.AdjustPointTo(direction.UP);
            yield return DS_processControl.Wait(MoveTool.duration);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Move(standard_x, direction.RIGHT, MoveTool.duration);
            }
            top.RightShift(standard_x, MoveTool.duration);
            N.Move(60, direction.UP, MoveTool.duration);
            N.AdjustPointTo(direction.RIGHT);
            nodes.Insert(0, N);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
            top.LeftShit(standard_x, MoveTool.duration);
            yield return DS_processControl.Wait(MoveTool.duration);
            data.Fade();
            view.FinalTreat(view.textAsset_LinkStack[0].text);
        }

        public IEnumerator Pop()
        {
            string originalStr = view.PreTreat(view.textAsset_LinkStack[2].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            MyVariable temp = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            temp.setValue("temp", nodes[0].num);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            top.RightShift(standard_x);
            yield return DS_processControl.Wait(MoveTool.duration);

            view.text_code.text = MyTools.BoldCode_normal(originalStr, 3);
            nodes[0].Fade();
            nodes.RemoveAt(0);
            top.LeftShit(standard_x, MoveTool.duration);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Move(standard_x, direction.LEFT, MoveTool.duration);
            }
            temp.Fade();
            yield return DS_processControl.Wait(MoveTool.duration);
            view.FinalTreat(view.textAsset_LinkStack[0].text);
        }

    }


    public class StackModel_ArrayNode : DemoModel<ArrayNode>
    {
        private Pointer top;
        int top_num = 0;
        private StackView view;
        private float original_x = 1010, original_y = 0, standard_x = 65;

        public StackModel_ArrayNode()
        {
            view = GameObject.Find("ToolsMan").GetComponent<StackView>();
            //int data_length = Random.Range(2, 5);
            int data_length = Random.Range(2, 5);
            List<int> list = MyTools.GetNoRepeatList(data_length);
            CreateArrayNodes(list);
        }


        public StackModel_ArrayNode(InputField input)
        {
            view = GameObject.Find("ToolsMan").GetComponent<StackView>();
            string str = input.text;
            List<int> list = MyTools.getInputIntegerStringList(str);
            if (list.Count > 15)
            {
                view.warning.showWarning("栈满");
                return;
            }
            CreateArrayNodes(list);
        }
        public void CreateArrayNodes(List<int> list)
        {
            Vector2 GeneratePos = new Vector2(original_x, original_y);

            for (int i = 0; i <= list.Count; i++)
            {
                if (i == list.Count)
                {
                    GameObject g = Instantiate(view.POINT, view.PointArea.transform);
                    top = g.GetComponent<Pointer>();
                    top.initialize(new Vector2(GeneratePos.x, original_y - 60f), "top", false);
                    top.Reverse();
                }
                else
                {
                    ArrayNode N = Instantiate(view.ARRAYNODE, view.DemoArea.transform).GetComponent<ArrayNode>();
                    N.initialize(GeneratePos);
                    GeneratePos = new Vector2(GeneratePos.x - standard_x, original_y);
                    N.SetValue(list[i]);
                    nodes.Add(N);
                }
            }
        }

        public IEnumerator Push(int val)
        {
            string originalStr = view.PreTreat(view.textAsset_ArrayStack[1].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable top_variable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            top_variable.setValue("top", nodes.Count);


            MyVariable data = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            data.setValue("val", val);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            ArrayNode N = Instantiate(view.ARRAYNODE, view.DemoArea.transform).GetComponent<ArrayNode>();
            N.initialize(new Vector2(original_x - nodes.Count * standard_x, original_y));
            N.SetValue(val);
            nodes.Add(N);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            top.LeftShit(standard_x, MoveTool.duration);
            top_variable.setValue(nodes.Count);
            yield return DS_processControl.Wait(MoveTool.duration);
            data.Fade();
            top_variable.Fade();
            view.FinalTreat(view.textAsset_ArrayStack[0].text);
        }

        public IEnumerator Pop()
        {
            string originalStr = view.PreTreat(view.textAsset_ArrayStack[2].text);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 0);
            MyVariable top_variable = Instantiate(view.VARIABLE, view.SymbolTableArea.transform).GetComponent<MyVariable>();
            top_variable.setValue("top", nodes.Count);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 1);
            top.RightShift(standard_x, MoveTool.duration);
            top_variable.setValue(nodes.Count - 1);
            yield return DS_processControl.Wait(MoveTool.duration);
            view.text_code.text = MyTools.BoldCode_normal(originalStr, 2);
            nodes[nodes.Count - 1].Fade();

            yield return DS_processControl.Wait(MoveTool.duration);
            nodes.RemoveAt(nodes.Count - 1);
            view.FinalTreat(view.textAsset_ArrayStack[0].text);
        }

    }
}

