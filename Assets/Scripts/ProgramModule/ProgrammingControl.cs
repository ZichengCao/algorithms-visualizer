using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;


namespace FunnyAlgorithm {
    public class ProgrammingControl : MonoBehaviour
    {
        public ProgrammingView view;

        public Dropdown dataNumDropdown;
        private List<ArrayNode> nodes = new List<ArrayNode>();
        private ProgrammingModel demo;
        private GameObject Current_Add_Sub;
        public static int length = 10;
        private int add_sub_index = 0;
        [HideInInspector]
        public List<InputField> ParameterList = new List<InputField>();
        private int NowInputIndex = -1;
        [HideInInspector]
        public List<CommandType> CommandTypeList = new List<CommandType>();
        [HideInInspector]
        public List<GameObject> GO_CommandList = new List<GameObject>();
        void Start()
        {
            MoveTool.duration = 0.5f;
            CommandTypeList.Add(CommandType.ADD_SUB);
            GO_CommandList.Add(view.ExecuteQueueGridLayout.transform.GetChild(0).gameObject);
            CreatDemoNodes(length);
        }

        // Update is called once per frame
        void Update()
        {
            if (ParameterList.Count > 0 && Input.GetKeyDown(KeyCode.Tab)/* || Input.GetKeyDown(KeyCode.Backspace) */)
            {
                NowInputIndex = (NowInputIndex + 1) % ParameterList.Count;
                ParameterList[NowInputIndex].ActivateInputField();
            }
            else if (ParameterList.Count > 0 && Input.GetKeyDown(KeyCode.LeftAlt))
            {
                NowInputIndex = (NowInputIndex - 1 + ParameterList.Count) % ParameterList.Count;
                ParameterList[NowInputIndex].ActivateInputField();
            }
        }
        /// <summary>
        /// 添加执行队列
        /// </summary>
        public void Add()
        {
            Current_Add_Sub = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
            add_sub_index = GetAdd_SubIndex(Current_Add_Sub);
            view.AddMenu.SetActive(true);
            view.AddMenu.transform.SetSiblingIndex(add_sub_index + 1);
            if (GO_CommandList.Count > 5)
            {
                view.Rect_ExecuteQueueGridLayout.pivot = new Vector2(0.5f, 0f);
            }
            else
            {
                view.Rect_ExecuteQueueGridLayout.pivot = new Vector2(0.5f, 1f);
            }
        }
        /// <summary>
        /// 减少执行队列
        /// </summary>
        public void Sub()
        {
            CancelWithUpadteView();
            if (GO_CommandList.Count > 1)
            {
                Current_Add_Sub = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject;
                add_sub_index = GetAdd_SubIndex(Current_Add_Sub);
                if (CommandTypeList[add_sub_index - 1] == CommandType.ENDIF)
                {
                    int cnt = 1, k = 1;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - k] == CommandType.IF) cnt--;
                        if (CommandTypeList[add_sub_index - k] == CommandType.ENDIF && k != 1) cnt++;
                        Destroy(GO_CommandList[add_sub_index - k]);
                        GO_CommandList.RemoveAt(add_sub_index - k);
                        CommandTypeList.RemoveAt(add_sub_index - k);
                        k++;//删了第k个元素，是后面的往前补，不影响前面的索引
                    }
                }
                else if (CommandTypeList[add_sub_index - 1] == CommandType.ENDWHILE)
                {
                    int cnt = 1, k = 1;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - k] == CommandType.WHILE) cnt--;
                        if (CommandTypeList[add_sub_index - k] == CommandType.ENDWHILE && k != 1) cnt++;
                        Destroy(GO_CommandList[add_sub_index - k]);
                        GO_CommandList.RemoveAt(add_sub_index - k);
                        CommandTypeList.RemoveAt(add_sub_index - k);
                        k++;
                    }
                }
                else if (CommandTypeList[add_sub_index - 1] == CommandType.IF)
                {
                    int cnt = 1;
                    bool flag = false;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - 1] == CommandType.ENDIF) cnt--;
                        if (CommandTypeList[add_sub_index - 1] == CommandType.IF && flag) cnt++;
                        Destroy(GO_CommandList[add_sub_index - 1]);
                        GO_CommandList.RemoveAt(add_sub_index - 1);
                        CommandTypeList.RemoveAt(add_sub_index - 1);
                        flag = true;
                    }
                }
                else if (CommandTypeList[add_sub_index - 1] == CommandType.WHILE)
                {
                    int cnt = 1;
                    bool flag = false;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - 1] == CommandType.ENDWHILE) cnt--;
                        if (CommandTypeList[add_sub_index - 1] == CommandType.WHILE && flag) cnt++;
                        Destroy(GO_CommandList[add_sub_index - 1]);
                        GO_CommandList.RemoveAt(add_sub_index - 1);
                        CommandTypeList.RemoveAt(add_sub_index - 1);
                        flag = true;
                    }
                }
                else if (CommandTypeList[add_sub_index - 1] == CommandType.ELSE)
                {
                    int cnt = 1;
                    bool flag = false;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - 1] == CommandType.ENDELSE) cnt--;
                        if (CommandTypeList[add_sub_index - 1] == CommandType.ELSE && flag) cnt++;
                        Destroy(GO_CommandList[add_sub_index - 1]);
                        GO_CommandList.RemoveAt(add_sub_index - 1);
                        CommandTypeList.RemoveAt(add_sub_index - 1);
                        flag = true;
                    }
                }
                else if (CommandTypeList[add_sub_index - 1] == CommandType.ENDELSE)
                {
                    int cnt = 1, k = 1;
                    while (cnt != 0)
                    {
                        if (CommandTypeList[add_sub_index - k] == CommandType.ELSE) cnt--;
                        if (CommandTypeList[add_sub_index - k] == CommandType.ENDELSE && k != 1) cnt++;
                        Destroy(GO_CommandList[add_sub_index - k]);
                        GO_CommandList.RemoveAt(add_sub_index - k);
                        CommandTypeList.RemoveAt(add_sub_index - k);
                        k++;
                    }
                }
                else
                {
                    Destroy(GO_CommandList[add_sub_index - 1]);
                    GO_CommandList.RemoveAt(add_sub_index - 1);
                    CommandTypeList.RemoveAt(add_sub_index - 1);
                }

                #region 清理 ParameterList 中被删除的Command的InputField
                int i, j;
                for (i = 0, j = 0; i < GO_CommandList.Count; i++)
                {
                    GameObject g;
                    string name = "Input"; int kk = 1;
                    while ((g = MyTools.FindChildWithName(GO_CommandList[i], name + kk.ToString())) != null)
                    {
                        InputField input = g.GetComponent<InputField>();
                        while (!InputField.Equals(input, ParameterList[j]))
                        {
                            ParameterList.RemoveAt(j);
                        }
                        j++;
                        kk++;
                    }
                }
                while (j < ParameterList.Count)
                {
                    ParameterList.RemoveAt(j);
                }
                #endregion
            }
            if (GO_CommandList.Count == 1) view.Btn_Execute.interactable = false;
            UpdateExecuteQueueView();
        }

        private bool hasStart = false;
        // 很耻辱，一开始很棒的设计最后还是写成了只敢加flag来打补丁的代码
        public void Clear(bool dataNumFlag = false)
        {
            StopCoroutine("ExecuteCoroutine");
            Cancel();
            if (hasStart || dataNumFlag)
            {
                DestoryDemoNodes();
                CreatDemoNodes(length);
            }

            #region 清空符号表

            for (int i = 1; i < view.GO_SymbolTable.transform.childCount; i++)
            {
                Destroy(view.GO_SymbolTable.transform.GetChild(i).gameObject);
            }

            #endregion

            //清空只留最后一个Add_Sub
            while (GO_CommandList.Count > 1)
            {
                Destroy(GO_CommandList[0]);
                GO_CommandList.RemoveAt(0);
                CommandTypeList.RemoveAt(0);
            }

            GO_CommandList[GO_CommandList.Count - 1].SetActive(true);
            ParameterList.Clear();
            if (view.Btn_Step.gameObject.activeSelf)
            {
                HasAnalyzed = false;
                view.Btn_Execute.interactable = false;
                view.Rect_ExecuteBtn.GetComponentInChildren<Text>().text = "分析";
                view.Rect_ExecuteBtn.DOAnchorPosX(view.Rect_ExecuteBtn.anchoredPosition.x + 100f, 0.5f);
                view.Btn_Step.gameObject.SetActive(false);

                view.Rect_StepBtn.anchoredPosition = new Vector2(view.Rect_StepBtn.anchoredPosition.x - 100f, view.Rect_StepBtn.anchoredPosition.y);
            }
            UpdateExecuteQueueView();
            hasStart = false;
        }

        public void CancelWithUpadteView()
        {
            if (view.AddMenu.activeSelf)
            {
                Cancel();
                UpdateExecuteQueueView();
            }
            else
            {
                Cancel();
            }

        }

        public void ShutDownException()
        {
            view.Btn_Execute.interactable = true;
            ProgrammingModel.EXCEPTIONFLAG = false;
            view.GO_Exception.SetActive(false);
            if (GO_CommandList[ProgrammingModel.OverallIndex] != null)
            {
                GO_CommandList[ProgrammingModel.OverallIndex].GetComponentInChildren<Image>().enabled = false;
            }
        }


        public void setDataNum()
        {
            int[] value_to_nums = { 5, 10, 15, 20, 30, 50 };
            length = value_to_nums[dataNumDropdown.value];
            Clear(true);
        }

        public void Cancel()
        {
            if (view.AddMenu != null && view.AddMenu.activeSelf)
            {
                view.AddMenu.SetActive(false);
            }
        }
        /// <summary>
        /// 添加命令
        /// </summary>
        public void AddCommandMenu(int sign)
        {
            view.Btn_Execute.interactable = true;
            GameObject g;
            if (SampleProgram.SampleFlag)
            {
                int index = GO_CommandList.Count - 1;
                if (sign <= 8)
                {
                    GO_CommandList.Insert(index, g = Instantiate(view.COMMAND[sign], view.ExecuteQueueGridLayout.transform));
                    CommandTypeList.Insert(index, (CommandType)sign);
                    if (sign <= 6)
                    {
                        string name = "Input"; int kk = 1;
                        while (MyTools.FindChildWithName(g, name + kk.ToString()) != null)
                        {
                            InputField t;
                            ParameterList.Add(t = MyTools.FindChildWithName(g, name + kk.ToString()).GetComponent<InputField>());
                            AddTriggerListener(t);
                            kk++;
                        }
                    }
                }
                else
                {
                    if ((CommandType)sign == CommandType.ADD_SUB)
                    {
                        GameObject gg;
                        //直接往里面加，删除的时候，碰到#end_while，就一直向上删除，直到删除到while
                        GO_CommandList.Insert(index, gg = Instantiate(view.ADD_SUB, view.ExecuteQueueGridLayout.transform));
                        //绑定OnClick时间
                        gg.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Add);
                        gg.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Sub);
                        CommandTypeList.Insert(index, CommandType.ADD_SUB);
                    }
                    if ((CommandType)sign == CommandType.ENDWHILE)
                    {
                        GO_CommandList.Insert(index, Instantiate(view.END_WHILE, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(index, CommandType.ENDWHILE);
                    }
                    else if ((CommandType)sign == CommandType.ENDIF)
                    {
                        GO_CommandList.Insert(index, Instantiate(view.END_IF, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(index, CommandType.ENDIF);
                    }
                    else if ((CommandType)sign == CommandType.ENDELSE)
                    {
                        GO_CommandList.Insert(index, Instantiate(view.EDN_ELSE, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(index, CommandType.ENDELSE);
                    }
                }
            }
            else
            {
                GO_CommandList.Insert(add_sub_index, g = Instantiate(view.COMMAND[sign], view.ExecuteQueueGridLayout.transform));
                CommandTypeList.Insert(add_sub_index, (CommandType)sign);
                GO_CommandList[add_sub_index].transform.SetSiblingIndex(add_sub_index);

                //给InputField添加点击触发事件
                string name = "Input"; int kk = 1;
                while (MyTools.FindChildWithName(g, name + kk.ToString()) != null)
                {
                    InputField t;
                    ParameterList.Add(t = MyTools.FindChildWithName(g, name + kk.ToString()).GetComponent<InputField>());
                    AddTriggerListener(t);
                    kk++;
                }

                if ((CommandType)sign == CommandType.WHILE || (CommandType)sign == CommandType.IF || (CommandType)sign == CommandType.ELSE)
                {
                    //直接往里面加，删除的时候，碰到#end_while，就一直向上删除，直到删除到while
                    GO_CommandList.Insert(add_sub_index + 1, Instantiate(view.ADD_SUB, view.ExecuteQueueGridLayout.transform));
                    //绑定OnClick时间
                    GO_CommandList[add_sub_index + 1].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(Add);
                    GO_CommandList[add_sub_index + 1].transform.GetChild(1).GetComponent<Button>().onClick.AddListener(Sub);
                    CommandTypeList.Insert(add_sub_index + 1, CommandType.ADD_SUB);
                    GO_CommandList[add_sub_index + 1].transform.SetSiblingIndex(add_sub_index + 1);
                    if ((CommandType)sign == CommandType.WHILE)
                    {
                        GO_CommandList.Insert(add_sub_index + 2, Instantiate(view.END_WHILE, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(add_sub_index + 2, CommandType.ENDWHILE);
                    }
                    else if ((CommandType)sign == CommandType.IF)
                    {
                        GO_CommandList.Insert(add_sub_index + 2, Instantiate(view.END_IF, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(add_sub_index + 2, CommandType.ENDIF);
                    }
                    else if ((CommandType)sign == CommandType.ELSE)
                    {
                        GO_CommandList.Insert(add_sub_index + 2, Instantiate(view.EDN_ELSE, view.ExecuteQueueGridLayout.transform));
                        CommandTypeList.Insert(add_sub_index + 2, CommandType.ENDELSE);
                    }
                    GO_CommandList[add_sub_index + 2].transform.SetSiblingIndex(add_sub_index + 2);
                }
            }
            UpdateExecuteQueueView();
            CancelWithUpadteView();
        }

        public void ExecuteQueueAnalyze()
        {
            ProgrammingModel.EXCEPTIONFLAG = false;
            int len = GO_CommandList.Count;
            List<Command> CQ = new List<Command>();
            for (int i = 0; i < len; i++)
            {
                GameObject g = GO_CommandList[i];
                if (g.tag == "COMMAND")
                {
                    string name = MyTools.FindChildWithName(g, "Name").GetComponent<Text>().text;
                    List<string> op = new List<string>();
                    int cnt = getParameters(g);
                    if (cnt == 0)
                    {
                        CQ.Add(new Command(SampleProgram.stringToCommandType(name), g, null));
                    }
                    else if (cnt == 1)
                    {
                        string s1 = MyTools.FindChildWithName(g, "param1").GetComponent<Text>().text;
                        op.Add(s1);
                        CQ.Add(new Command(SampleProgram.stringToCommandType(name), g, op));
                    }
                    else if (cnt == 2)
                    {
                        string s1 = MyTools.FindChildWithName(g, "param1").GetComponent<Text>().text;
                        string s2 = MyTools.FindChildWithName(g, "param2").GetComponent<Text>().text;
                        op.Add(s1); op.Add(s2);
                        CQ.Add(new Command(SampleProgram.stringToCommandType(name), g, op));
                     }
                    else if (cnt == 3)
                    {
                        string s1 = MyTools.FindChildWithName(g, "param1").GetComponent<Text>().text;
                        string s2 = MyTools.FindChildWithName(g, "param2").GetComponent<Text>().text;
                        string s3 = MyTools.FindChildWithName(g, "param3").GetComponent<Text>().text;
                        op.Add(s1); op.Add(s2); op.Add(s3);
                        CQ.Add(new Command(SampleProgram.stringToCommandType(name), g, op));
                    }
                }
            }
            demo = new ProgrammingModel(nodes, CQ);
            try
            {
                demo.RecordProce();
            }
            catch (ArgumentOutOfRangeException)
            {
                ProgrammingModel.EXCEPTIONFLAG = true;
                view.GO_Exception.SetActive(true);
                view.Text_Exception.text = "第" + (ProgrammingModel.OverallIndex + 1) + "行错误，索引超出范围";
            }
            catch (StackOverflowException e)
            {
                ProgrammingModel.EXCEPTIONFLAG = true;
                view.GO_Exception.SetActive(true);
                view.Text_Exception.text = "栈溢出，请检查第" + (ProgrammingModel.OverallIndex + 1) + "行附近是否有死循环或格式不规范";
            }
            catch (MyException e)
            {
                ProgrammingModel.EXCEPTIONFLAG = true;
                view.GO_Exception.SetActive(true);
                view.Text_Exception.text = e.GetError();
            }
            //catch ( Exception )
            //{
            //    ProgrammingMode.EXCEPTIONFLAG = true;
            //    GO_Exception.SetActive(true);
            //    Text_Exception.text = "第" + ( ProgrammingMode.OverallIndex + 1 ) + "出现错误，请检查";
            //}
            finally
            {
                if (ProgrammingModel.EXCEPTIONFLAG)
                {
                    CQ.Clear();
                    //红色标记
                }
            }
        }

        private bool HasAnalyzed = false;

        public void Analyze()
        {
            if (GO_CommandList.Count > 1)
            {
                if (!HasAnalyzed)
                {
                    ExecuteQueueAnalyze();
                    if (!ProgrammingModel.EXCEPTIONFLAG)
                    {
                        HasAnalyzed = true;

                        view.Btn_Execute.GetComponentInChildren<Text>().text = "执行";
                        view.Rect_ExecuteBtn.DOAnchorPosX(view.Rect_ExecuteBtn.anchoredPosition.x - 100f, 0.5f);
                        view.Btn_Step.gameObject.SetActive(true);
                        view.Btn_Step.interactable = true;
                        view.Rect_StepBtn.DOAnchorPosX(view.Rect_StepBtn.anchoredPosition.x + 100f, 0.5f);

                        #region 如果没有出错，将所有InputField interactable false
                        if (!ProgrammingModel.EXCEPTIONFLAG)
                        {
                            for (int i = 0; i < ParameterList.Count; i++)
                            {
                                ParameterList[i].interactable = false;
                            }
                        }
                        #endregion

                        #region 能到这里说明分析阶段没有出错，将队列中所有 ADD_SUB 按钮删除掉
                        //最后一个设置成false

                        for (int i = 0; i < GO_CommandList.Count - 1; i++)
                        {
                            if (CommandTypeList[i] == CommandType.ADD_SUB)
                            {
                                Destroy(GO_CommandList[i]);
                                GO_CommandList.RemoveAt(i);
                                CommandTypeList.RemoveAt(i);
                                i--;
                            }
                        }
                        GO_CommandList[GO_CommandList.Count - 1].SetActive(false);

                        #endregion
                    }
                }
                else
                {
                    hasStart = true;
                    StartCoroutine("ExecuteCoroutine");
                }
            }
        }
        private IEnumerator ExecuteCoroutine()
        {
            view.Btn_Execute.interactable = false;
            view.Btn_Step.interactable = false;
            view.Rect_ExecuteQueueGridLayout.pivot = new Vector2(0.5f, 1f);
            view.ScrollBar_ExecuteQueue_Vertical.value = 1;
            while (demo.demoQueue.Count > 0)
            {
                StartCoroutine(demo.PlayForward());
                yield return new WaitForSeconds(MoveTool.duration);
            }
            view.Btn_Execute.interactable = false;
            view.Btn_Step.interactable = false;
        }

        public void NextStep()
        {
            hasStart = true;
            if (demo.demoQueue.Count > 0)
            {
                StartCoroutine(demo.PlayForward());
                if (demo.demoQueue.Count == 0)
                {
                    view.Btn_Execute.interactable = false;
                    view.Btn_Step.interactable = false;
                }
            }
        }
        #region 工具性

        private int getParameters(GameObject g)
        {
            int num = 0;
            string ss = "Input";
            int kk = 1;
            string str = ss + kk.ToString();
            while (MyTools.FindChildWithName(g, str) != null)
            {
                kk++;
                num++;
                str = ss + kk.ToString();
            }
            return num;
        }
        private int GetAdd_SubIndex(GameObject add_sub)
        {
            if (add_sub.tag != "ADD_SUB") return -1;
            int len = view.ExecuteQueueGridLayout.transform.childCount;
            bool flag = false;
            for (int i = 0; i < len; i++)
            {
                if (view.ExecuteQueueGridLayout.transform.GetChild(i).tag == "ADDMENU") flag = true;
                if (GameObject.Equals(add_sub, view.ExecuteQueueGridLayout.transform.GetChild(i).gameObject))
                {
                    return flag ? i - 1 : i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 更新命令面板视图
        /// </summary>
        private void UpdateExecuteQueueView()
        {
            if (GO_CommandList.Count > 8)
            {
                view.Rect_ExecuteQueueGridLayout.pivot = new Vector2(0.5f, 0f);
            }
            else
            {
                view.Rect_ExecuteQueueGridLayout.pivot = new Vector2(0.5f, 1f);
            }
        }

        public void updateInputIndex(BaseEventData data)
        {
            GameObject g = EventSystem.current.currentSelectedGameObject;
            InputField input;
            if (g != null)
            {
                input = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
                NowInputIndex = getInputIndex(input);
                ParameterList[NowInputIndex].ActivateInputField();
            }
        }

        private void AddTriggerListener(InputField input)
        {
            EventTrigger trigger = input.gameObject.GetComponent<EventTrigger>();
            if (trigger == null)
            {
                trigger = input.gameObject.AddComponent<EventTrigger>();
            }
            EventTrigger.Entry entry = new EventTrigger.Entry();
            //设置事件类型  
            entry.eventID = EventTriggerType.PointerDown;
            //初始化回调函数
            entry.callback = new EventTrigger.TriggerEvent();
            //定义回调函数  
            UnityEngine.Events.UnityAction<BaseEventData> callback = new UnityEngine.Events.UnityAction<BaseEventData>(updateInputIndex);
            //设置回调函数  
            entry.callback.AddListener(callback);
            trigger.triggers.Add(entry);
        }
        private int getInputIndex(InputField input)
        {
            for (int i = 0; i < ParameterList.Count; i++)
            {
                if (InputField.Equals(input, ParameterList[i]))
                {
                    return i;
                }
            }
            return 0;
        }

        private void DestoryDemoNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                Destroy(nodes[i].g);
            }
            nodes.Clear();

            List<GameObject> Temp = new List<GameObject>();
            for (int i = 0; i < view.DemoArea_Content.transform.childCount; i++)
            {
                GameObject g = view.DemoArea_Content.transform.GetChild(i).gameObject;
                if (g.tag == "NODE")
                {
                    Temp.Add(g);
                }
            }
            for (int i = 0; i < Temp.Count; i++)
            {
                Destroy(Temp[i]);
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
            float offset = (150f - 35f) / mmax;
            #endregion

            for (int i = 0; i < length; i++)
            {
                ArrayNode n = Instantiate(view.NODE, view.DemoArea.transform).GetComponent<ArrayNode>();
                n.num = list[i];
                n.index = i;
                nodes.Add(n);
                //nodes.Add(new Node(g = Instantiate(view.NODE, view.DemoArea.transform), list[i], i));
                n.g.transform.GetChild(0).GetComponent<Text>().text = list[i].ToString();
                n.g.transform.GetChild(2).GetComponent<Text>().text = i.ToString();
                n.g.transform.GetChild(2).gameObject.SetActive(true);
                nodes[i].rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 35f + list[i] * offset);
                nodes[i].rect.sizeDelta = new Vector2(width, nodes[i].rect.sizeDelta.y);
                nodes[i].rect.pivot = new Vector2(0.5f, 0f);
                nodes[i].rect.anchoredPosition = new Vector2(pos, 10f);
                pos += width;
            }
        }
        #endregion



        #region 界面控制
        private bool flag = true;
        public void Conceal_Reveal()
        {
            if (flag)
            {
                view.Text_ConcealBtn.text = "显示";
                view.Rect_DemoArea.DOSizeDelta(new Vector2(760f, 620f), 0.5f);
                view.Rect_SymbolTable.DOSizeDelta(new Vector2(760f, 200f), 0.5f);
            }
            else
            {
                view.Text_ConcealBtn.text = "隐藏";
                view.Rect_DemoArea.DOSizeDelta(new Vector2(760f, 480f), 0.5f);
                view.Rect_SymbolTable.DOSizeDelta(new Vector2(760f, 130f), 0.5f);
            }
            flag = !flag;
        }
        #endregion
    }
}


