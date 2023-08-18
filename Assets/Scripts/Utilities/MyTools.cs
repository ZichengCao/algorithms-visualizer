using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyAlgorithm
{
    public class MyTools : MonoBehaviour
    {

        #region program module

        public static bool JudgeExpressionWithAlpha(string op1)
        {
            //for ( int i = 0; i < op1.Length; i++ ) 
            //{
            //    if ( char.IsLetter(op1[i]))
            //    {
            //        if ( i == 0 )
            //        {
            //            if ( !IsOperator(op1[i + 1]) ) return false ;
            //        }
            //        else if ( i == op1.Length - 1 )
            //        {
            //            if ( !IsOperator(op1[i - 1]) ) return false;
            //        }
            //        else
            //        {
            //            if ( !IsOperator(op1[i - 1]) || !IsOperator(op1[i + 1]) ) return false ;
            //        }
            //    }
            //}
            return true;
        }

        public static bool JudgeExpressionLegitimacy(string op1)
        {
            //第一位，最后一位不能是运算符，第一位可以是'-'
            if (IsOperator(op1[op1.Length - 1]) || op1[0] == '*' || op1[0] == '/') return false;
            //括号匹配
            Stack<char> stack = new Stack<char>();
            int left = 0, right = 0;
            for (int i = 0; i < op1.Length; i++)
            {
                if (op1[i] == '(')
                {
                    if (i != 0 && isdigit(op1[i - 1]))
                        return false;
                    if (left == 0) left = i;
                    stack.Push('(');
                }
                else if (op1[i] == ')')
                {
                    if (i != op1.Length - 1 && isdigit(op1[i + 1]))
                        return false;
                    if (right == 0) right = i;
                    if (stack.Peek() == '(') stack.Pop();
                    else return false;
                }
            }
            //运算符不得连续出现
            bool flag = false;
            for (int i = 0; i < op1.Length; i++)
            {
                if (IsOperator(op1[i]))
                {
                    if (flag) return false;
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }

            if (left == 0 && right == 0)
                return true;
            else
                //递归判断括号内是否满足要求
                return JudgeExpressionLegitimacy(op1.Substring(left + 1, right - left - 1));
        }

        public static bool IsOperator(char c)
        {
            if (c == '-' || c == '+' || c == '*' || c == '/') return true;
            return false;
        }
        /// <summary>
        /// 判断一个字符串是否是数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDigit(string str)
        {
            int offset = 0;
            if (str[0] == '-' || str[0] == '+') offset = 1;
            for (int i = offset; i < str.Length; i++)
            {
                if (!char.IsDigit(str[i])) return false;
            }
            return true;
        }
        public static bool isdigit(char c)
        {
            if (c >= '0' && c <= '9') return true;
            return false;
        }
        public static bool isdigit(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!isdigit(str[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool JudgeIdentifier(string str)
        {
            if (!char.IsLetter(str[0])) return false;
            for (int i = 1; i < str.Length; i++)
            {
                if (!char.IsLetterOrDigit(str[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个布尔表达式的真假
        /// </summary>
        /// <param name="op1"></param>
        /// <param name="symbol"></param>
        /// <param name="op2"></param>
        /// <returns></returns>
        public static bool JudgeBooleanExpression(int op1, string symbol, int op2)
        {
            if (symbol.CompareTo("<") == 0)
            {
                if (op1 < op2) return true;
                else return false;
            }
            else if (symbol.CompareTo(">") == 0)
            {
                if (op1 > op2) return true;
                else return false;
            }
            else if (symbol.CompareTo("<=") == 0)
            {
                if (op1 <= op2) return true;
                else return false;
            }
            else if (symbol.CompareTo(">=") == 0)
            {
                if (op1 >= op2) return true;
                else return false;
            }
            else if (symbol.CompareTo("==") == 0)
            {
                if (op1 == op2) return true;
                else return false;
            }
            else if (symbol.CompareTo("!=") == 0)
            {
                if (op1 != op2) return true;
                else return false;
            }
            else
            {
                throw new MyException("第" + (ProgrammingModel.OverallIndex + 1) + "行错误，预期之外的运算符" + "\"" + symbol + "\"");
            }
        }
        /// <summary>
        /// 按名字寻找一个子物体，宽搜
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject FindChildWithName(GameObject parent, string childName)
        {
            Queue<GameObject> Q = new Queue<GameObject>();
            Q.Enqueue(parent);
            while (Q.Count > 0)
            {
                GameObject obj = Q.Dequeue();
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    GameObject go = obj.transform.GetChild(i).gameObject;
                    if (go.name.CompareTo(childName) == 0) return go;
                    else Q.Enqueue(go);
                }
            }
            return null;
        }
        #endregion
        #region 获取不同类型的List
        public static List<int> GetNoRepeatList(int length)
        {
            List<int> list = new List<int>();
            for ( int i = 0; i < length; i++ ) list.Add(i);

            for ( int i = 0; i < length; i++ )
            {
                int T1 = Random.Range(0, length);
                int T2 = Random.Range(0, length);
                Swap(list, T1, T2);
            }
            return list;
        }

        public static bool TossCoin(int n)
        {
            int i = Random.Range(1, 11);
            if (n == 0 || n == 1)  
                return true;
            else if (n == 2)
            {
                return i < 9 ? true : false;
            }
            else if (n == 3)
            {
                return i < 6 ? true : false;
            }
            else if (n == 4)
            {
                return i < 4 ? true : false;
            }
            else
                return false;
        }
        public static List<int> GetRandomList(int length)
        {
            List<int> list = new List<int>();
            for ( int i = 0; i < length; i++ )
            {
                list.Add(Random.Range(0, length));
            }
            return list;
        }

        public static List<char>GetRandomAlphabet(int length)
        {
            List<char> list = new List<char>();
            char c = 'A';
            for(int i = 0; i < length; i++)
            {
                list.Add((char)(c + i)) ;
            }
            for (int i = 0; i < length; i++)
            {
                int i1 = Random.Range(0, length);
                int i2 = Random.Range(0, length);
                Swap(list, i1, i2);
            }
            return list;
        }
        public static List<int> GetAscendList(int length)
        {
            List<int> list = GetRandomList(length);
            list.Sort();
            return list;
        }

        public static List<int> GetDescendList(int length)
        {
            List<int> list = new List<int>();
            for ( int i = 0; i < length; i++ )
            {
                list.Add(length - i - 1);
            }
            return list;
        }

        public static List<int> GetAlmostOrderedList(int length)
        {
            List<int> list = GetAscendList(length);
            int cnt = length / 10;
            for ( int i = 0; i < cnt; i++ )
            {
                int T1 = Random.Range(0, length / 2);
                int T2 = Random.Range(length / 2, length);
                Swap(list, T1, T2);
            }
            return list;
        }

        public static List<int> GetEqualList(int length)
        {
            List<int> list = new List<int>();
            int k = Random.Range(1, length);
            for ( int i = 0; i < length; i++ )
            {
                list.Add(k);
            }
            return list;

        }

        #endregion

        #region ColorText
        public static string ColorText(string str,string color)
        {
            return "<color='" + color + "'> " + str + " </color>" ;
        }
        
        #endregion

        public static void disorganizeList<T>(List<T> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                int i1 = Random.Range(0, list.Count);
                int i2 = Random.Range(0, list.Count);
                Swap(list, i1, i2);
            }
        }

        public static List<int> getInputIntegerStringList(string str)
        {
            List<int> list = new List<int>();
            string[] ss = str.Split(',') ;
            for(int i = 0; i < ss.Length; i++)
            {
                if (ss[i].Length == 0) continue;
                list.Add(StringToInt(ss[i]));
            }
            return list;
        }

        public static bool Check(List<Node> nodes, InputField input)
        {
            string[] str = input.text.Split(',');
            if (nodes.Count > str.Length) return true;
            int i = 0, j = 0;
            for (; i < str.Length; i++)
            {
                if (str[i].Length == 0) continue;
                else if (j >= nodes.Count) return true;
                else if (MyTools.StringToInt(str[i]) != nodes[j++].num)
                    return true;
            }
            if (i == str.Length && j == nodes.Count) return false;
            return true;
        }

        public static int GetListMax(List<Part> list)
        {
            int maxIndex = 0;
            for ( int i = 1; i < list.Count; i++ )
            {
                if ( list[i].num > list[maxIndex].num )
                {
                    maxIndex = i;
                }
            }
            return list[maxIndex].num;
        }

        public static List<int>PartListToInt(List<Part> list)
        {
            List<int> temp=new List<int>(); 
            for(int i = 0; i < list.Count; i++)
            {
                temp.Add(list[i].num);
            }
            return temp;
        }
        public static int GetListMax(List<int> list)
        {
            int maxIndex = 0;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > list[maxIndex])
                {
                    maxIndex = i;
                }
            }
            return list[maxIndex];
        }
        /// <summary>
        /// 输入颜色码的十六进制表示，返回RGB表示
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color Color_HexToRgb(string color)
        {
            int index = 0;
            if ( color[0] == '#' ) index++;
            string R = color.Substring(index, 2);
            string G = color.Substring(index + 2, 2);
            string B = color.Substring(index + 4, 2);
            return new Color(HexToDec(R) / 255f, HexToDec(G) / 255f, HexToDec(B) / 255f);
        }

        /// <summary>
        /// 输入一个不超过int的十六进制数，返回该数的十进制表示
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int HexToDec(string s)
        {
            int res = 0;
            for ( int i = s.Length - 1, k = 0; i >= 0; i--, k++ )
            {
                if ( s[i] >= '0' && s[i] <= '9' )
                {
                    res += (int) Mathf.Pow(16, k) * ( s[i] - '0' );
                }
                else
                {
                    if ( char.IsUpper(s[i]) )
                        res += (int) Mathf.Pow(16, k) * ( s[i] - 'A' + 10 );
                    else
                        res += (int) Mathf.Pow(16, k) * ( s[i] - 'a' + 10 );
                }
            }
            return res;
        }
        public static void QueuePushFront<LogType>(Queue<LogType> Q, LogType log)
        {
            Queue<LogType> temp = new Queue<LogType>();
            while ( Q.Count > 0 )
            {
                temp.Enqueue(Q.Dequeue());
            }
            Q.Enqueue(log);
            while ( temp.Count > 0 )
            {
                Q.Enqueue(temp.Dequeue());
            }
        }
        public static T GetQueueTail<T>(Queue<T> Q)
        {
            Queue<T> temp = new Queue<T>();
            while ( Q.Count > 1 )
            {
                temp.Enqueue(Q.Dequeue());
            }
            T log = Q.Dequeue();
            while ( temp.Count > 0 )
            {
                Q.Enqueue(temp.Dequeue());
            }
            return log;
        }

        static string[] keywords = { "null", "new", "typedef", "struct", "boolean", "delete", "true", "false", "void", "int", "for", "while", "return", "if", "else", "break" };
        static string[] functinoName = { "Swap", "BubbleSort", "InsertSort", "SelectSort", "ShellSort", "Random", "Partiton", "QuickSort", "MergeSort" };
        public static string ColourKeyWord(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            for (int i = 0; i < keywords.Length; i++)
            {
                sb.Replace(keywords[i], "<color=#0033FF>" + keywords[i] + "</color>");
            }
            return sb.ToString();
        }

        public static string BoldCode_normal(string str, int line, Scrollbar sb = null)
        {
            string[] ss = str.Split('\n');
            if (sb != null) 
            {
                float val = 1.0f / (float)ss.Length;
                sb.value = 1 - line * val; 
            }
            //ss[line] = "<b>" + ss[line] + "</b>"; 
            ss[line] = ss[line].Replace("0033FF", "FF6666");
            ss[line] = "<color=#FF6666>" + ss[line] + "</color>";
            return string.Join("\n", ss);
        }

        public static string BoldCode_success(string str, int line, Scrollbar sb = null)
        {
            string[] ss = str.Split('\n');
            if (sb != null)
            {
                float val = 1.0f / (float)ss.Length;
                sb.value = 1 - line * val;
            }
            //ss[line] = "<b>" + ss[line] + "</b>"; 
            ss[line] = ss[line].Replace("0033FF", "00FF00");
            ss[line] = "<color=#00FF00>" + ss[line] + "</color>";
            return string.Join("\n", ss);
        }


        public static string BoldCode_fail(string str, int line, Scrollbar sb = null)
        {
            string[] ss = str.Split('\n');
            if (sb != null)
            {
                float val = 1.0f / (float)ss.Length;
                sb.value = 1 - line * val;
            }
            //ss[line] = "<b>" + ss[line] + "</b>"; 
            ss[line] = ss[line].Replace("0033FF", "FF0000");
            ss[line] = "<color=#FF0000>" + ss[line] + "</color>";
            return string.Join("\n", ss);
        }

        public static void SetActive(MonoBehaviour[] groups, bool flag)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].gameObject.SetActive(flag);
            }
        }

        public static void Swap<T>(List<T> list, int x, int y)
        {
            T temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }


        public static int StringToInt(string s)
        {
            bool flag = false;
            int offset = 0;
            if (s[0] == '-' || s[0] == '+')
            {
                offset = 1;
                if (s[0] == '-')
                    flag = true;
            }
            
            int res = 0;
            for (int i = offset; i < s.Length; i++)
            {
                res = (res << 3) + (res << 1) + (s[i] ^ 48);
            }
            if (flag) res = ~res + 1;
            return res;

        }


        #region practice module tools
        public static string BoldImportantWord(string str)
        {
            string temp = str;
            temp = temp.Replace("{", "<b>");
            temp = temp.Replace("}", "</b>");
            return temp;
        }

        public static List<int> GetArr(List<Node> nodes)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                list.Add(nodes[i].num);
            }
            return list;
        }

        public static int GetSwapCount(List<int> arr, sortType type)
        {
            int cnt = 0;
            switch (type)
            {
                default:
                case sortType.BUBBLESORT:
                    BubbleSort(arr, ref cnt);
                    break;
                case sortType.INSERTSORT:
                    InsertSort(arr, ref cnt);
                    break;
                case sortType.SELECTSORT:
                    SelectSort(arr, ref cnt);
                    break;
                case sortType.SHELLSORT:
                    shellSort(arr, ref cnt);
                    break;
                case sortType.QUICKSORT:
                    QuickSort(arr, 0, arr.Count - 1, ref cnt);
                    break;
                case sortType.MERGESORT:
                    break;
            }
            return cnt;
        }
        public static void BubbleSort(List<int> arr, ref int cnt)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = 0; j < arr.Count - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        Swap(arr, j, j + 1);
                        cnt++;
                    }
                }
            }
        }
        public static void InsertSort(List<int> arr, ref int cnt)
        {
            int n = arr.Count;
            for (int i = 1; i < n; i++)
            {
                int j, temp = arr[i];
                for (j = i; j > 0 && temp < arr[j - 1]; j--)
                {
                    arr[j] = arr[j - 1];
                    cnt++;
                }
                arr[j] = temp;
            }
        }
        public static void SelectSort(List<int> arr, ref int cnt)
        {
            int n = arr.Count;
            for (int i = 0; i < n; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                        minIndex = j;
                }
                if (i != minIndex)
                {
                    Swap(arr, i, minIndex);
                    cnt++;
                }
            }
        }
        public static void QuickSort(List<int> arr, int left, int right, ref int cnt)
        {
            if (left < right)
            {
                int k = partiton(arr, left, right, ref cnt);
                QuickSort(arr, left, k - 1, ref cnt);
                QuickSort(arr, k + 1, right, ref cnt);
            }
        }
        static int partiton(List<int> arr, int left, int right, ref int cnt)
        {
            //int temp = Random.Range(left, right + 1);
            int temp = left;
            int pivot = arr[temp];
            //Swap(arr, left, temp);
            int i = left, j = right;
            while (i < j)
            {
                while (i < j && arr[j] >= pivot) j--;
                while (i < j && arr[i] <= pivot) i++;
                if (i < j)
                {
                    Swap(arr, i, j);
                    cnt++;
                }
            }
            if (i != left)
            {
                Swap(arr, i, left);
                cnt++;
            }
            return i;
        }
        public static void shellSort(List<int> arr, ref int cnt)
        {
            int n = arr.Count;
            for (int div = n / 2; div >= 1; div = div / 2)
            {
                for (int i = 0; i < div; i++)
                {
                    for (int j = i + div; j < n; j += div)
                    {
                        int k = j - div;
                        while (k >= 0 && arr[k + div] < arr[k])
                        {
                            cnt++;
                            Swap(arr, k + div, k);
                            k -= div;
                        }
                    }
                }
            }
        }
        static void merge(List<int> arr, int left, int right)
        {
            int[] temp = new int[left - right + 1];
            int mid = (left + right) / 2;
            int i = left, j = mid + 1, k = 0;
            while (i <= mid && j <= right)
                temp[k++] = arr[i] <= arr[j] ? arr[i++] : arr[j++];
            while (i <= mid) temp[k++] = arr[i++];
            while (j <= right) temp[k++] = arr[j++];
            i = 0;
            while (left <= right)
            {
                arr[left++] = temp[i++];
            }
        }
        static void mergeSort(List<int> arr, int left, int right)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                mergeSort(arr, left, mid);
                mergeSort(arr, mid + 1, right);
                merge(arr, left, right);
            }
        }
        #endregion
    }
}
