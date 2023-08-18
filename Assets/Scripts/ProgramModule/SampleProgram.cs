using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace FunnyAlgorithm
{

    public class SampleProgram : MonoBehaviour
    {
        // Start is called before the first frame update
        public TextAsset[] CodeBase;
        public static bool SampleFlag = false;
        private ProgrammingControl programmingControl;
        private RectTransform Rect_ExecuteLayout;
        //List<List<Command>> Sample = new List<List<Command>>();
        List<Command> Sample = new List<Command>();
        void Start()
        {
            programmingControl = GetComponent<ProgrammingControl>();
            Rect_ExecuteLayout = programmingControl.view.ExecuteQueueGridLayout.GetComponent<RectTransform>();

            #region smaple1
            //List<Command> S1 = new List<Command>();
            //S1.Add(new Command(CommandType.INT, MyTools.getStringList("i", "0")));
            //S1.Add(new Command(CommandType.INT, MyTools.getStringList("j", "9")));
            //S1.Add(new Command(CommandType.INT, MyTools.getStringList("n", "5")));
            //S1.Add(new Command(CommandType.WHILE, MyTools.getStringList("i", "<", "n")));
            //S1.Add(new Command(CommandType.ADD_SUB, null));
            //S1.Add(new Command(CommandType.SWAP, MyTools.getStringList("i", "j")));
            //S1.Add(new Command(CommandType.INC, MyTools.getStringList("i")));
            //S1.Add(new Command(CommandType.DEC, MyTools.getStringList("j")));
            //S1.Add(new Command(CommandType.ENDWHILE, null));
            //Sample.Add(S1);
            #endregion
        }

        public void LoadSample(string path)
        {
            SampleFlag = true;
            programmingControl.Clear();
            RecognizeText(path);

            for (int i = 0; i < Sample.Count; i++)
            {
                programmingControl.AddCommandMenu((int)Sample[i].type);

                if (Sample[i].OP != null && Sample[i].OP.Count != 0)
                {
                    for (int j = 0; j < Sample[i].OP.Count; j++)
                    {
                        InputField input = programmingControl.ParameterList[programmingControl.ParameterList.Count - Sample[i].OP.Count + j];
                        RectTransform Rect_input = input.GetComponent<RectTransform>();
                        input.text = Sample[i].OP[j];
                    }
                }
            }

            programmingControl.GO_CommandList[programmingControl.GO_CommandList.Count - 1].transform.SetAsLastSibling();
            Rect_ExecuteLayout.pivot = new Vector2(0.5f, 1f);
            Rect_ExecuteLayout.anchoredPosition = new Vector2(Rect_ExecuteLayout.anchoredPosition.x, 0f);
            Sample.Clear();
            SampleFlag = false;
        }

        private void RecognizeText(string path)
        {
            string text = "";
            for (int i = 0; i < CodeBase.Length; i++)
            {
                if (CodeBase[i].name.CompareTo(path) == 0)
                {
                    text = CodeBase[i].text;
                    break;
                }
            }

            List<string> line = new List<string>(text.Split('\n'));
            for (int i = 0; i < line.Count; i++)
            {
                if (line[i][line[i].Length - 1] == '\r')
                {
                    line[i] = line[i].Remove(line[i].Length - 1);
                }
                List<string> list = new List<string>(line[i].Split(' '));
                if (list.Count == 1)
                {
                    Sample.Add(new Command(stringToCommandType(list[0]), null));
                }
                else
                {
                    List<string> op = new List<string>();
                    for (int k = 1; k < list.Count; k++)
                    {
                        if (list[k].Equals("arr.length"))
                        {
                            op.Add(ProgrammingControl.length.ToString());
                        }
                        else
                        {
                            op.Add(list[k]);
                        }
                    }
                    Sample.Add(new Command(stringToCommandType(list[0]), op));
                }
            }
        }

        private static bool AlmostEqual(string s1, string s2)
        {
            int temp;
            if ((temp = s1.IndexOf('#')) != -1)
                s1 = s1.Remove(temp, 1);
            if ((temp = s1.IndexOf('_')) != -1)
                s1 = s1.Remove(temp, 1);
            s1 = s1.ToUpper(); s2 = s2.ToUpper();
            if (s1.CompareTo(s2) == 0) return true;
            return false;
        }

        public static CommandType stringToCommandType(string line1)
        {
            if (AlmostEqual(line1, "INT"))
            {
                return CommandType.INT;
            }
            else if (AlmostEqual(line1, "ASSIGN"))
            {
                return CommandType.ASSIGN;
            }
            else if (AlmostEqual(line1, "MOVE"))
            {
                return CommandType.MOVE;
            }
            else if (AlmostEqual(line1, "SWAP"))
            {
                return CommandType.SWAP;
            }
            else if (AlmostEqual(line1, "INC"))
            {
                return CommandType.INC;
            }
            else if (AlmostEqual(line1, "DEC"))
            {
                return CommandType.DEC;
            }
            else if (AlmostEqual(line1, "WHILE"))
            {
                return CommandType.WHILE;
            }
            else if (AlmostEqual(line1, "ENDWHILE") || AlmostEqual(line1, "ENDWHILE"))
            {
                return CommandType.ENDWHILE;
            }
            else if (AlmostEqual(line1, "IF"))
            {
                return CommandType.IF;
            }
            else if (AlmostEqual(line1, "ENDIF"))
            {
                return CommandType.ENDIF;
            }
            else if (AlmostEqual(line1, "ADDSUB") || AlmostEqual(line1, "ADD_SUB"))
            {
                return CommandType.ADD_SUB;
            }
            else if (AlmostEqual(line1, "ELSE"))
            {
                return CommandType.ELSE;
            }
            else if (AlmostEqual(line1, "ENDELSE"))
            {
                return CommandType.ENDELSE;
            }
            else if (AlmostEqual(line1, "BREAK"))
            {
                return CommandType.BREAK;
            }

            else
            {
                return 0;
                //出错
            }
        }
    }
}
