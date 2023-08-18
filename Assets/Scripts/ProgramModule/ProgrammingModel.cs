using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace FunnyAlgorithm {

    class ProgrammingModel : SortBasicModel
    {
        public static bool EXCEPTIONFLAG = false;
        public static int OverallIndex;

        #region SwapEvent
        private GameObject NODE;
        private GameObject DemoArea;
        #endregion

        #region AssignEvent
        private GameObject GO_symbolTable;
        private GameObject VARIABLE;
        #endregion

        #region HighLight
        private int LastHighLightIndex = -1;
        private List<GameObject> GO_CommandList;
        #endregion

        #region 异常处理
        #endregion

        Dictionary<string, GameObject> GO_Variable = new Dictionary<string, GameObject>();
        //命令队列
        List<Command> InstructionQueue;
        //符号表，存储标识符
        Dictionary<string, int> SymbolTable = new Dictionary<string, int>();

        public ProgrammingModel(List<ArrayNode> list, List<Command> Q):base(list)
        {
            InstructionQueue = Q;
            GameObject g = GameObject.Find("ToolsMan");
            ProgrammingControl control = g.GetComponent<ProgrammingControl>();
            ProgrammingView view = g.GetComponent<ProgrammingView>();
            NODE = view.NODE;
            VARIABLE = view.VARIABLE;
            GO_symbolTable = view.GO_SymbolTable;
            DemoArea = view.DemoArea;
            GO_CommandList = control.GO_CommandList;
        }

        public override void RecordProce(int pass = 0)
        {
            #region 预检查，while endwhile,if endif是否匹配，else endelse，if和else是否匹配
            int W = 0, I = 0, E = 0;
            int lastWhileIndex = 0, lastIfIndex = 0, lastElseIndex = 0;
            for (int i = 0; i < InstructionQueue.Count; i++)
            {
                Command T = InstructionQueue[i];
                if (T.type == CommandType.WHILE)
                {
                    lastWhileIndex = i;
                    //W.Push(true);
                    W++;
                }
                else if (T.type == CommandType.IF)
                {
                    lastIfIndex = i;
                    //I.Push(true);
                    I++;
                }
                else if (T.type == CommandType.ELSE)
                {
                    if (i == 0 || InstructionQueue[i - 1].type != CommandType.ENDIF)
                        throw new MyException("第" + (i + 1) + "行错误，ELSE没有与之匹配的\nIF/ENDIF");
                    lastElseIndex = i;
                    E++;
                }
                else if (T.type == CommandType.ENDIF)
                {
                    if (I == 0)
                        throw new MyException("第" + (i + 1) + "行错误，ENDIF没有与之匹配的IF");
                    I--;
                }
                else if (T.type == CommandType.ENDWHILE)
                {
                    if (W == 0)
                        throw new MyException("第" + (i + 1) + "行错误，ENDWHILE没有与之匹配的WHILE");
                    W--;
                }
                else if (T.type == CommandType.ENDELSE)
                {
                    if (E == 0)
                        throw new MyException("第" + (i + 1) + "行错误，ENDELSE没有与之匹配的ELSE");
                    E--;
                }
                else if (T.type == CommandType.BREAK)
                {
                    if (W == 0)
                        throw new MyException("第" + (i + 1) + "行错误，请在循环体内使用BREAK");
                }
            }
            if (W > 0)
            {
                throw new MyException("第" + (lastWhileIndex + 1) + "行错误，WHILE没有与之匹配的ENDWHILE");
            }
            else if (I > 0)
            {
                throw new MyException("第" + (lastWhileIndex + 1) + "行错误，IF没有与之匹配的ENDIF");
            }
            else if (E > 0)
            {
                throw new MyException("第" + (lastWhileIndex + 1) + "行错误，ELSE没有与之匹配的ENDELSE");
            }

            #endregion

            int Pointer = 0;
            while (Pointer < InstructionQueue.Count)
            {
                OverallIndex = Pointer;
                BreakFLag = false;

                Analyze(ref Pointer);
            }
        }
        private int STEPS = 0;
        private Stack<bool> ElseFlag = new Stack<bool>();
        private bool BreakFLag = false;
        public void Analyze(ref int index)
        {
            OverallIndex = index;
            if (STEPS > 20000) throw new StackOverflowException();
            STEPS++;
            if (BreakFLag) return;
            Command cmd = InstructionQueue[index];
            if (cmd.type == CommandType.SWAP)
            {
                int op1, op2;
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, true));
                parseType t1 = parseParameterString(cmd.OP[0], out op1);
                parseType t2 = parseParameterString(cmd.OP[1], out op2);
                if (t1 == parseType.num || t1 == parseType.expression ||
                    t2 == parseType.num || t2 == parseType.expression)
                {
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，不接受的参数输入");
                }
                //assign 结点 结点
                if (t1 == parseType.arr && t2 == parseType.arr)
                {
                    AddSwapEvent(op1, op2);
                    arr[op1].Swap(arr[op2]);
                }
                //assign 变量 变量
                else if (t1 == parseType.var && t2 == parseType.var)
                {
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], op2, true));
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[1], op1, false));
                    SymbolTable[cmd.OP[0]] = op2;
                    SymbolTable[cmd.OP[1]] = op1;
                }
                //assign 变量 结点
                else if (t1 == parseType.var && t2 == parseType.arr)
                {
                    demoQueue.Enqueue(new MovementStatement(activityType.MOVESTATEMENT, arr[op2].index, op1, true, false));
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], arr[op2].num, false));
                    int temp = arr[op2].num;
                    arr[op2].num = op1;
                    SymbolTable[cmd.OP[0]] = temp;
                }
                //assign 结点 变量
                else if (t1 == parseType.arr && t2 == parseType.var)
                {
                    demoQueue.Enqueue(new MovementStatement(activityType.MOVESTATEMENT, arr[op1].index, op2, true, false));
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[1], arr[op1].num, false));
                    int temp = arr[op1].num;
                    arr[op1].num = op2;
                    SymbolTable[cmd.OP[1]] = temp;
                }
            }
            else if (cmd.type == CommandType.ASSIGN)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, true));
                int op1, op2;
                parseType t1 = parseParameterString(cmd.OP[0], out op1);
                parseType t2 = parseParameterString(cmd.OP[1], out op2);
                //如果第一个参数是常数或者表达式 报错
                if (t1 == parseType.num || t1 == parseType.expression)
                {
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，不接受的参数输入");
                }
                //assign 变量 变量/常数/表达式
                if (t1 == parseType.var && (t2 == parseType.var || t2 == parseType.num || t2 == parseType.expression))
                {
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], op2, false));
                    SymbolTable[cmd.OP[0]] = op2;
                }
                //assign 变量 结点
                else if (t1 == parseType.var && t2 == parseType.arr)
                {
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], arr[op2].num, false));
                    SymbolTable[cmd.OP[0]] = arr[op2].num;
                }
                //assign 结点 变量/常数/表达式
                else if (t1 == parseType.arr && (t2 == parseType.var || t2 == parseType.num || t2 == parseType.expression))
                {
                    demoQueue.Enqueue(new MovementStatement(activityType.MOVESTATEMENT, arr[op1].index, op2, false, false));
                    arr[op1].num = op2;

                }
                //assign 结点 结点
                else if (t1 == parseType.arr && t2 == parseType.arr)
                {
                    demoQueue.Enqueue(new MovementStatement(activityType.MOVESTATEMENT, arr[op1].index, arr[op2].index, false));
                    arr[op1].num = arr[op2].num;
                }
            }
            else if (cmd.type == CommandType.IF)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
                int op1, op2;
                string sb;

                parseType t1 = parseParameterString(cmd.OP[0], out op1);
                sb = cmd.OP[1];
                parseType t2 = parseParameterString(cmd.OP[2], out op2);

                if (t1 == parseType.arr) op1 = arr[op1].num;
                if (t2 == parseType.arr) op2 = arr[op2].num;
                //如果为真，正常往下做，正常执行中碰到#end_if不需要管他，碰到else全部弹出值Endelse
                //如果为假，就将弹出CommandQueue中的命令一直到#end_if为止，如果if里面还有if，里面的if 不管是真假都不会执行，就多找一个#end_if即可

                if (!MyTools.JudgeBooleanExpression(op1, sb, op2))
                {
                    int cnt = 1;
                    while (index < InstructionQueue.Count && cnt > 0)
                    {
                        index++;
                        Command T = InstructionQueue[index];
                        if (T.type == CommandType.ENDIF) cnt--;
                        else if (T.type == CommandType.IF) cnt++;
                    }
                    index--;
                    ElseFlag.Push(false);
                }
                else
                {
                    ElseFlag.Push(true);
                }
            }
            else if (cmd.type == CommandType.ELSE)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
                //如果if做过了，这个else就不做了
                if (ElseFlag.Pop())
                {
                    int cnt = 1;
                    while (index < InstructionQueue.Count && cnt > 0)
                    {
                        index++;
                        Command T = InstructionQueue[index];
                        if (T.type == CommandType.ENDELSE) cnt--;
                        else if (T.type == CommandType.ELSE) cnt++;
                    }
                    index--;
                }
            }
            else if (cmd.type == CommandType.BREAK)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
                //遇到break，向后找到第一个endwhile
                BreakFLag = true;
                while (index < InstructionQueue.Count)
                {
                    index++;
                    Command T = InstructionQueue[index];
                    if (T.type == CommandType.ENDWHILE || index == InstructionQueue.Count)
                        return;
                }
            }
            else if (cmd.type == CommandType.INT)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, true));
                string name = cmd.OP[0];
                if (!MyTools.JudgeIdentifier(name))
                {
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，标识符格式不合法");
                }
                int value;
                parseType t2 = parseParameterString(cmd.OP[1], out value);
                if (t2 == parseType.arr)
                    value = arr[value].num;
                if (!SymbolTable.ContainsKey(name))
                {
                    SymbolTable.Add(name, value);
                    demoQueue.Enqueue(new DefineStatement(activityType.DEFINESTATEMENT, name, value, false));
                }
                else
                {
                    //重定义
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，变量重定义（请使用符合C语言的标准，将所有变量都定义在程序最前面）");
                }
            }
            else if (cmd.type == CommandType.INC)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, true));
                int value;
                parseType t = parseParameterString(cmd.OP[0], out value);
                if (t == parseType.var)
                {
                    SymbolTable[cmd.OP[0]]++;
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], SymbolTable[cmd.OP[0]], false));
                }
                else
                {
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，不接受的参数输入");
                }
            }
            else if (cmd.type == CommandType.DEC)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, true));
                int value;
                parseType t = parseParameterString(cmd.OP[0], out value);
                if (t == parseType.var)
                {
                    SymbolTable[cmd.OP[0]]--;
                    demoQueue.Enqueue(new AssignStatement(activityType.ASSIGNFORANVARIABLE, cmd.OP[0], SymbolTable[cmd.OP[0]], false));
                }
                else
                {
                    throw new MyException("第" + (OverallIndex + 1) + "行错误，不接受的参数输入");
                }
            }
            else if (cmd.type == CommandType.WHILE)
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
                string sb = cmd.OP[1];
                //当遇到while时，只要while内的逻辑表达式仍然为true就继续往下做，那么这里左右两个操作数只能实时从符号表里取

                int op1, op2;

                parseType t1 = parseParameterString(cmd.OP[0], out op1);
                parseType t2 = parseParameterString(cmd.OP[2], out op2);
                if (t1 == parseType.arr) op1 = arr[op1].num;
                if (t2 == parseType.arr) op2 = arr[op2].num;
                int steps = 0, temp = index;
                bool WHILEFLAG = false;
                while (!BreakFLag && MyTools.JudgeBooleanExpression(op1, sb, op2))
                {
                    WHILEFLAG = true;
                    index = temp + 1;
                    if (steps != 0)
                        demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, temp, false));
                    while (InstructionQueue[index].type != CommandType.ENDWHILE)
                    {
                        //递归分析循环体内的命令
                        Analyze(ref index);
                    }
                    steps++;

                    t1 = parseParameterString(cmd.OP[0], out op1);
                    t2 = parseParameterString(cmd.OP[2], out op2);
                    if (t1 == parseType.arr) op1 = arr[op1].num;
                    if (t2 == parseType.arr) op2 = arr[op2].num;
                }
                BreakFLag = false;
                //如果没有执行这个WHILE
                if (!WHILEFLAG)
                {
                    int cnt = 1;
                    while (index < InstructionQueue.Count && cnt > 0)
                    {
                        index++;
                        Command T = InstructionQueue[index];
                        if (T.type == CommandType.ENDWHILE) cnt--;
                        else if (T.type == CommandType.WHILE) cnt++;
                    }
                }
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
            }
            //不用管的
            else
            {
                demoQueue.Enqueue(new HighLightEvent(activityType.HIGHLIGHT, index, false));
                //正常执行过程中碰到end_if就不需要去管他
            }
            index++;
        }
        public new IEnumerator PlayForward()
        {
            if (!IsRun)
            {
                IsRun = true;
                bool Flag = true;
                Activity LOG;
                do
                {
                    LOG = demoQueue.Dequeue();
                    if (LOG.type == activityType.HIGHLIGHT)
                    {
                        HighLightEvent log = (HighLightEvent)LOG;
                        if (LastHighLightIndex != -1)
                        {
                            InstructionQueue[LastHighLightIndex].mask.enabled = false;
                        }
                        InstructionQueue[log.index].mask.enabled = true;
                        LastHighLightIndex = log.index;
                    }
                    else if (LOG.type == activityType.MOVE)
                    {
                        Movement log = (Movement)LOG;
                        nodes[log.index].Move(log.x_steps, log.y_steps);
                    }
                    else if (LOG.type == activityType.MOVESTATEMENT)
                    {
                        MovementStatement log = (MovementStatement)LOG;
                        if (log.flag)
                        {
                            Flag = false;
                            int source = log.op2, destination = log.op1;
                            #region 复制一个 source 结点
                            GameObject g = Instantiate(NODE, DemoArea.transform);
                            ArrayNode node = g.GetComponent<ArrayNode>();
                            node.num = nodes[source].num;
                            node.GetComponentInChildren<Text>().text = node.num.ToString();
                            node.index = destination;
                            //Node node = new ArrayNode(g, Nodes[source].num, destination);
                            node.rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, nodes[source].rect.sizeDelta.y);
                            node.rect.sizeDelta = new Vector2(ArrayNode.width, node.rect.sizeDelta.y);
                            node.rect.pivot = new Vector2(0.5f, 0f);
                            node.rect.anchoredPosition = nodes[source].rect.anchoredPosition;
                            #endregion

                            #region 移动到destination位置
                            node.Move(0, 2);
                            yield return new WaitForSeconds(MoveTool.duration);
                            node.Move(destination - source, 0);
                            yield return new WaitForSeconds(MoveTool.duration);
                            node.Move(0, -2);
                            yield return new WaitForSeconds(MoveTool.duration);
                            #endregion

                            //销毁原来的结点
                            Destroy(nodes[destination].g);
                            g.transform.GetChild(2).GetComponent<Text>().text = destination.ToString();
                            g.transform.GetChild(2).gameObject.SetActive(true);
                            nodes[destination].Assign(node);
                        }
                        //变量给结点赋值
                        else
                        {
                            int mmax = MyTools.GetListMax(arr);
                            float offset = (150f - 35f) / mmax;
                            nodes[log.op1].g.GetComponentInChildren<Text>().text = log.op2.ToString();
                            nodes[log.op1].rect.DOSizeDelta(new Vector2(nodes[log.op1].rect.sizeDelta.x, 35f + log.op2 * offset), MoveTool.duration);
                            nodes[log.op1].num = log.op2;
                        }
                    }
                    else if (LOG.type == activityType.ASSIGNFORANVARIABLE)
                    {
                        Flag = false;
                        AssignStatement log = (AssignStatement)LOG;
                        GameObject g = GO_Variable[log.name];
                        g.GetComponent<Image>().color = MyTools.Color_HexToRgb("FF7B2C");
                        g.GetComponentInChildren<Text>().text = log.name + " = " + log.value;
                        yield return new WaitForSeconds(MoveTool.duration);
                        if (g != null)
                            g.GetComponent<Image>().color = new Color(1, 1, 1);
                    }
                    else if (LOG.type == activityType.DEFINESTATEMENT)
                    {
                        DefineStatement log = (DefineStatement)LOG;
                        GameObject g;
                        GO_Variable.Add(log.name, g = Instantiate(VARIABLE, GO_symbolTable.transform));
                        g.GetComponentInChildren<Text>().text = log.name + " = " + log.value;
                    }
                    else if (LOG.type == activityType.UPWARDTEXTEVENT)
                    {
                        NodeUpwardText log = (NodeUpwardText)LOG;
                        nodes[log.index].g.transform.GetChild(2).GetComponent<Text>().text = log.note;
                    }
                    executedStack.Push(LOG);
                } while (demoQueue.Count > 0 && LOG.hasNext);
                if (Flag)
                    yield return new WaitForSeconds(MoveTool.duration);
                IsRun = false;
            }
            yield return 0;
        }


        enum parseType { num, var, arr, expression };
        /// <summary>
        /// 1数字，2标识符，3数组，4表达式
        /// </summary>
        /// <param name="op"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        private parseType parseParameterString(string op, out int value)
        {

            //如果op是数字 字符串
            if (MyTools.IsDigit(op))
            {
                value = MyTools.StringToInt(op); ;
                return parseType.num;
            }
            //是变量
            else if (MyTools.JudgeIdentifier(op))
            {
                value = TryGetValue(op); ;
                return parseType.var;
            }
            ////一个结点
            else if (op.StartsWith("arr[") && op.EndsWith("]"))
            {
                int startIndex = op.IndexOf('[') + 1;
                int len = op.IndexOf(']') - startIndex;
                parseParameterString(op.Substring(startIndex, len), out value);
                return parseType.arr;
            }
            //是一个表达式
            else if (MyTools.JudgeExpressionLegitimacy(VariableReplace(op)) && MyTools.JudgeExpressionWithAlpha(VariableReplace(op)))
            {
                //识别表达式中的arr[]
                while (op.Contains("arr[") && op.Contains("]"))
                {
                    int startIndex = op.IndexOf('[') + 1;
                    int len = op.IndexOf(']') - startIndex;
                    string temp = op.Substring(startIndex, len);
                    parseParameterString(temp, out value);
                    temp = "arr[" + temp + "]";
                    op = op.Replace(temp, arr[value].num.ToString());
                }

                op = VariableReplace(op);
                value = CalculateTools.expressionEvalutaion.calc(op);
                return parseType.expression;
            }
            else
            {
                throw new MyException("第" + (OverallIndex + 1) + "行出现错误，请检查" + "\"" + op + "\"");
            }
        }
        /// <summary>
        /// 将表达式中的变量替换成值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string VariableReplace(string str)
        {
            //符号表中的变量
            foreach (KeyValuePair<string, int> p in SymbolTable)
            {
                if (str.Contains(p.Key))
                {
                    str = str.Replace(p.Key, p.Value.ToString());
                }
            }

            return str;
        }

        /// <summary>
        /// 尝试从符号表中取值，如果变量未定义，报错
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>

        private int TryGetValue(string c)
        {
            int value;
            if (SymbolTable.TryGetValue(c, out value))
            {
                return value;
            }
            else
            {
                throw (new MyException("第" + (OverallIndex + 1) + "行错误，未定义的标识符"));
            }
        }
        private void AddSwapEvent(int op1, int op2)
        {
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op1].index, 0, 2, true));
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op2].index, 0, 2, false)); ;
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op1].index, op2 - op1, 0, true));
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op2].index, op1 - op2, 0, false));
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op1].index, 0, -2, true));
            demoQueue.Enqueue(new Movement(activityType.MOVE, arr[op2].index, 0, -2, true));
            demoQueue.Enqueue(new NodeUpwardText(activityType.UPWARDTEXTEVENT, arr[op1].index, op2.ToString(), true));
            demoQueue.Enqueue(new NodeUpwardText(activityType.UPWARDTEXTEVENT, arr[op2].index, op1.ToString(), false));
        }

        public new IEnumerator PlayBackward()
        {
            throw new NotImplementedException();
        }
    }

    #region ProgramEvent


    class NodeUpwardText : SortEvent
    {
        public string note;
        public bool flag;
        /// <summary>
        /// flag true表示显示Pivot，false表示消失
        /// </summary>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <param name="flag"></param>
        /// <param name="hasNext"></param>
        public NodeUpwardText(activityType type, int index, bool flag, bool hasNext, string Text = "") : base(type, hasNext)
        {
            this.index = index;
            this.flag = flag;
            this.hasNext = hasNext;
        }
        public NodeUpwardText(activityType type, int index, string text, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
            this.hasNext = hasNext;
            this.note = text;
        }
    }


    class HighLightEvent : SortEvent
    {
        public HighLightEvent(activityType type, int index, bool hasNext) : base(type, hasNext)
        {
            this.index = index;
        }
    }
    class DefineStatement : SortEvent
    {
        public string name;
        public int value;

        public DefineStatement(activityType type, string name, int value, bool hasNext) : base(type, hasNext)
        {
            this.name = name;
            this.value = value;
        }

    }
    class AssignStatement : SortEvent
    {
        public string name;
        public int value;

        public AssignStatement(activityType type, string name, int value, bool hasNext) : base(type, hasNext)
        {
            this.name = name;
            this.value = value;
        }
    }
    class MovementStatement : SortEvent
    {
        public int op1, op2;
        public bool flag;
        public MovementStatement(activityType type, int op1, int op2, bool hasNext, bool flag = true) : base(type, hasNext)
        {
            this.flag = flag;
            this.op1 = op1;
            this.op2 = op2;
        }
    }
    class BranchStatement : SortEvent
    {
        public int l, r;
        public string m;

        public BranchStatement(activityType type, int op1, string m, int op2, bool hasNext) : base(type, hasNext)
        {
            this.type = type;
            this.l = op1;
            this.m = m;
            this.r = op2;
        }
    }
    class LoopStatement : SortEvent
    {
        public LoopStatement(activityType type, bool hasNext) : base(type, hasNext)
        {

        }
    }
    enum PointerMoveSign { Down, Up, Idle };
    class MovePointer : SortEvent
    {
        PointerMoveSign sign;
        public MovePointer(activityType type, PointerMoveSign sign, bool hasNext) : base(type, hasNext)
        {
            this.sign = sign;
        }
    }

    #endregion

    #region CommandClass
    public enum CommandType
    {
        INT, ASSIGN, SWAP, IF, WHILE, INC, DEC, ELSE, BREAK, ENDIF, ENDWHILE, ENDELSE, ADD_SUB, ADDMENU, MOVE,
    }
    class Command
    {
        public CommandType type;
        public GameObject g;
        public Image mask;
        public List<string> OP;

        public Command(CommandType type, List<string> OP)
        {
            this.type = type;
            this.OP = OP;
        }
        public Command(CommandType type, GameObject g, List<string> OP)
        {
            this.g = g;
            //mask = g.GetComponent<Image>();
            mask = g.GetComponentInChildren<Image>();
            this.type = type;
            this.OP = OP;
        }
    }
    #endregion


    #region 异常类
    public class MyException : ApplicationException
    {
        private string error;
        public MyException()
        {

        }
        public MyException(string msg) : base(msg)
        {
            this.error = msg;
        }

        public string GetError()
        {
            return error;
        }
    }
    #endregion


}

