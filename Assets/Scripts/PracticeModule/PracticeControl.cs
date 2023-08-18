using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class PracticeControl : MonoBehaviour
    {
        public PracticeView view;

        private List<ArrayNode> nodes = new List<ArrayNode>();
        private List<ArrayNode> Answernodes = new List<ArrayNode>();

        private List<bool> posSign = new List<bool>();//位置标记
        private List<Vector2> OridinalPos = new List<Vector2>();
        private List<Vector2> distinctPos = new List<Vector2>();
        private string answerText = "";
        private Stack<int> AnswerStack = new Stack<int>();
        private PracticeModel p = new PracticeModel();
        public static bool JMPFlag = false;
        // Start is called before the first frame update

        #region 生成问题相关属性
        private int[] FontSize = { 30, 30, 30, 26, 28, 28 };
        #endregion
        void Start()
        {
            MoveTool.duration = 0.35f;
            GenerateQuestion();
        }

        public void GenerateQuestion()
        {
            #region 清理工作
            if (p.QType != questionType.CHOICEQUESTION)
            {
                StopCoroutine("PlayIEnumerator");
                for (int i = 0; i < nodes.Count; i++)
                {
                    Destroy(nodes[i].gameObject);
                }
                for (int i = 0; i < Answernodes.Count; i++)
                {
                    Destroy(Answernodes[i].gameObject);
                }
                nodes.Clear();
                AnswerStack.Clear();
                answerText = "";
                Answernodes.Clear();
                view.Input_Respond.image.color = new Color(1, 1, 1);
                view.Input_Respond.text = "";
                view.Btn_Submit.interactable = true;
                view.Btn_Undo.interactable = true;
                view.Btn_Demo.interactable = false;
                view.Btn_LastStep.interactable = false;
                view.Btn_NextStep.interactable = false;
                view.Btn_Restart.interactable = false;
            }
            #endregion

            p.GererateQuestion();//生成问题
            view.QuestionText.text = p.QuestionText;
            //设置字体大小
            view.CodeText.fontSize = FontSize[(int)p.SortType];
            //生成结点

            //ArrayNode.CreatArryNodes(p.length, 0, view.NODE, view.DemoArea, view.Rect_DemoArea.sizeDelta, null);
            CreatDemoNodes(p.length);
            //CreatDemoNodes(p.length);
            //List<int> arr = MyTools.GetArr(nodes);
            //设置代码
            view.CodeArea.GetComponentInChildren<Text>().text = MyTools.ColourKeyWord(view.Sorttxt[(int)p.SortType].text);
            p.RecordProce(nodes);//记录演示过程

            if (p.QType == questionType.PASS)
            {
                view.Btn_Undo.gameObject.SetActive(true);
                view.Btn_Submit.interactable = false;
                view.Btn_Submit.gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-20f, 0f), 0.5f);
                view.Input_Respond.gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(10f, 0f), 0.5f);

                #region 生成答案区域
                view.Text_AnswerTip.text = "在此作答，按顺序依次点击下列结点";
                view.Input_Respond.interactable = false;
                CreatAnswerNodes();
                for (int i = 0; i < Answernodes.Count; i++)
                {
                    int temp = Answernodes[i].index;
                    Answernodes[i].gameObject.GetComponent<Button>().enabled = true;
                    Answernodes[i].gameObject.GetComponent<Button>().onClick.AddListener(() => ClickEvent(temp));
                }
                #endregion

            }
            else if (p.QType == questionType.EXCHANGECOUNT)
            {
                #region 准备工作
                view.Btn_Undo.gameObject.SetActive(false);
                view.Input_Respond.interactable = true;
                view.Btn_Submit.gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-20f, -20f), 0.5f);
                view.Input_Respond.gameObject.GetComponent<RectTransform>().DOAnchorPos(new Vector2(10f, -20f), 0.5f);
                #endregion

                #region 答案区域
                view.Text_AnswerTip.text = "在此作答";
                #endregion
            }
        }

        public void CheckAnswer()
        {
            view.Btn_Submit.interactable = false;
            view.Btn_Undo.interactable = false;
            view.Btn_Restart.interactable = true;

            string respond = view.Input_Respond.text;
            if (p.CheckAnswer(respond))
            {
                view.Input_Respond.GetComponent<Image>().DOColor(new Color(0, 1, 0), 0.2f);
                view.Input_Respond.text += " 作答正确";
                //GetComponent<PracticeLogControl>().SetLogs(p.SortType, true);
            }
            else
            {
                view.Input_Respond.GetComponent<Image>().DOColor(new Color(1, 0, 0), 0.2f);
                if (p.QType != questionType.PASS)
                    view.Input_Respond.text += " 正确答案：" + p.AnswerText;
                //GetComponent<PracticeLogControl>().SetLogs(p.SortType, false);
            }
            //Play();
            Auto_Btn();
        }

        public void ClickEvent(int index)
        {
            AnswerStack.Push(index);
            if (AnswerStack.Count == nodes.Count)
            {
                view.Btn_Submit.interactable = true;
            }
            answerText += Answernodes[index].num.ToString() + "  ";
            view.Input_Respond.text = answerText;
            Answernodes[index].image.DOColor(new Color(1, 1, 1, 0), 0.2f);
            Answernodes[index].gameObject.GetComponent<Button>().interactable = false;
        }
        public void Undo()
        {
            if (AnswerStack.Count > 0)
            {
                view.Btn_Submit.interactable = false;
                int index = AnswerStack.Pop();
                answerText = answerText.Remove(answerText.Length - 3);
                view.Input_Respond.text = answerText;
                Answernodes[index].image.DOColor(new Color(1, 1, 1, 1), 0.2f);
                Answernodes[index].gameObject.GetComponent<Button>().interactable = true;
            }
        }

        private void CreatDemoNodes(int length)
        {
            List<int> list = MyTools.GetNoRepeatList(length);
            #region 计算生成位置
            float sideMargin = 50f;//侧边距
            float width = (view.Rect_DemoArea.sizeDelta.x - sideMargin * 2) / length;
            float pos = view.Rect_DemoArea.sizeDelta.x / 2 - sideMargin - width / 2;//起始生成位置    
            pos = -pos;
            ArrayNode.width = width;
            ArrayNode.verticalStandard = 50f;
            int mmax = MyTools.GetListMax(list);

            #endregion
            float offset = (150f - 35f) / mmax;
            for (int i = 0; i < length; i++)
            {

                ArrayNode n = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<ArrayNode>();
                n.index = i;
                n.num = list[i];
                n.gameObject.GetComponentInChildren<Text>().text = list[i].ToString();
                nodes.Add(n);
                //nodes.Add(new Node(Instantiate(view.NODE, view.DemoArea.transform), list[i], i));

                nodes[i].rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 35f + list[i] * offset);
                nodes[i].rect.sizeDelta = new Vector2(width, nodes[i].rect.sizeDelta.y);
                nodes[i].rect.pivot = new Vector2(0.5f, 0f);
                nodes[i].rect.anchoredPosition = new Vector2(pos, 10f);
                pos += width;
            }
        }
        private void CreatAnswerNodes()
        {
            #region 计算生成位置
            //
            float sideMargin = 20f;//侧边距
            float width = 30f;
            float pos = view.Rect_DemoArea.sizeDelta.x / 2 - sideMargin - width / 2;//起始生成位置
            pos = -pos;

            #endregion
            for (int i = 0; i < nodes.Count; i++)
            {
                ArrayNode n = Instantiate(view.NODE, view.AnswerArea.transform).GetComponent<ArrayNode>();
                n.index = i;
                n.num = nodes[i].num;
                n.gameObject.GetComponentInChildren<Text>().text = nodes[i].num.ToString();
                //Debug.Log(nodes[i].num);
                Answernodes.Add(n);
                Answernodes[i].rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 35f);
                Answernodes[i].rect.sizeDelta = new Vector2(width, Answernodes[i].rect.sizeDelta.y);
                Answernodes[i].rect.pivot = new Vector2(0.5f, 0f);
                Answernodes[i].rect.anchoredPosition = new Vector2(pos, 10f);
                posSign.Add(true);
                OridinalPos.Add(new Vector2(pos, 10f));
                distinctPos.Add(new Vector2(pos, 45f));
                pos += width + width / 2;
            }

        }

        public void ReStart()
        {
            //StopCoroutine("PlayIEnumerator");
            StopAllCoroutines();
            view.Btn_Demo.interactable = true;
            view.Btn_NextStep.interactable = true;
            view.Btn_LastStep.interactable = false;
            float sideMargin = 50f;//侧边距
            float width = ArrayNode.width;
            float pos = view.Rect_DemoArea.sizeDelta.x / 2 - sideMargin - width / 2;//起始生成位置
            pos = -pos;
            List<int> arr = new List<int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                arr.Add(nodes[i].num);
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                Destroy(nodes[i].gameObject);
            }
            nodes.Clear();
            int mmax = MyTools.GetListMax(arr);
            float offset = (150f - 35f) / mmax;
            for (int i = 0; i < arr.Count; i++)
            {
                ArrayNode n = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<ArrayNode>();
                n.index = i;
                n.num = arr[i];
                n.gameObject.GetComponentInChildren<Text>().text = arr[i].ToString();
                nodes.Add(n);
                //nodes.Add(new Node(Instantiate(view.NODE, view.DemoArea.transform), arr[i], i));
                nodes[i].rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 35f + arr[i] * offset);
                nodes[i].rect.sizeDelta = new Vector2(width, nodes[i].rect.sizeDelta.y);
                nodes[i].rect.pivot = new Vector2(0.5f, 0f);
                nodes[i].rect.anchoredPosition = new Vector2(pos, 10f);
                pos += width;
            }
            p.RecordProce(nodes);
        }
        //public void Play()
        //{
        //    StartCoroutine("PlayIEnumerator");
        //}
        //private IEnumerator PlayIEnumerator()
        //{
        //    view.Btn_Restart.interactable = true;
        //    view.Btn_Demo.interactable = false;
        //    view.Btn_NextStep.interactable = false;
        //    view.Btn_LastStep.interactable = false;

        //    yield return new WaitForSeconds(MoveTool.duration);
        //    while (p.demo.DemoQueue.Count > 0)
        //    {
        //        StartCoroutine(p.demo.PlayForward());
        //        yield return new WaitForSeconds(MoveTool.duration);
        //        if (p.demo.DemoQueue.Count == 0)
        //        {
        //            view.Btn_Demo.interactable = false;
        //            view.Btn_NextStep.interactable = false;
        //            view.Btn_LastStep.interactable = true;
        //        }
        //    }
        //}
        //public void NextStep()
        //{
        //    view.Btn_LastStep.interactable = true;
        //    if (p.demo.DemoQueue.Count > 0)
        //    {
        //        StartCoroutine(p.demo.PlayForward());
        //        if (p.demo.DemoQueue.Count == 0)
        //        {
        //            view.Btn_NextStep.interactable = false;
        //            view.Btn_Demo.interactable = false;
        //        }
        //    }
        //}
        //public void LastStep()
        //{
        //    view.Btn_NextStep.interactable = true;
        //    view.Btn_Demo.interactable = true;

        //    if (p.demo.ExecutedStack.Count > 0)
        //    {
        //        StartCoroutine(p.demo.PlayBackward());
        //        if (p.demo.ExecutedStack.Count == 0)
        //        {
        //            view.Btn_LastStep.interactable = false;
        //        }
        //    }
        //}

       

        public void LastStep_Btn()
        {
            StartCoroutine(LastStep());
        }

        private IEnumerator LastStep()
        {
            view.Btn_NextStep.interactable = true;
            view.Btn_Demo.interactable = true;

            if (p.demo.executedStack.Count > 0)
            {
                view.Btn_LastStep.interactable = false;
                bool flag;
                do
                {
                    flag = p.demo.PlayBackward();
                    if (!flag)
                    {
                        yield return new WaitForSeconds(MoveTool.duration);
                    }
                } while (flag);
                view.Btn_LastStep.interactable = true;
                if (p.demo.executedStack.Count == 0)
                    view.Btn_LastStep.interactable = false;
            }
        }
        public void NextStep_Btn()
        {
            StartCoroutine(NextStep());
        }

        private IEnumerator NextStep()
        {
            if (p.demo.demoQueue.Count > 0)
            {
                view.Btn_NextStep.interactable = false;
                bool flag;
                do
                {
                    flag = p.demo.PlayForward();
                    if (!flag)
                        yield return new WaitForSeconds(MoveTool.duration);
                } while (flag);
                view.Btn_NextStep.interactable = true;
                if (p.demo.demoQueue.Count == 0)
                {
                    view.Btn_NextStep.interactable = false;
                    //view.Btn_StartButton.interactable = false;
                    view.Btn_Demo.interactable = false;
                }
            }
            view.Btn_LastStep.interactable = true;
        }
        public void Auto_Btn()
        {
            //view.Text_Annotation.alignment = TextAnchor.MiddleCenter;
            view.Btn_LastStep.interactable = false;
            view.Btn_NextStep.interactable = false;
            view.Btn_Demo.interactable = false;
            
            //view.Btn_Restart = false;
            StartCoroutine("AutoPlay");

        }
        private IEnumerator AutoPlay()
        {
            while (p.demo.demoQueue.Count > 0)
            {
                bool flag;
                do
                {
                    flag = p.demo.PlayForward();
                    if (!flag)
                        yield return new WaitForSeconds(MoveTool.duration);
                } while (flag);
            }

            view.Btn_Demo.interactable = false;

            view.Btn_NextStep.interactable = false;
            view.Btn_LastStep.interactable = true;
        }

    }
}