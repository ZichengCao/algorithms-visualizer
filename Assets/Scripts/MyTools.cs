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
     
        #region 获取不同类型的List
        public static List<int> GetList(int DataType, int length)
        {
            switch ( DataType )
            {
                case 0:
                    return GetNoRepeatList(length);
                case 1:
                    return GetRandomList(length);
                case 2:
                    return GetAscendList(length);
                case 3:
                    return GetDescendList(length);
                case 4:
                    return GetAlmostOrderedList(length);
                case 5:
                    return GetEqualList(length);
                default:
                    return GetNoRepeatList(length);
            }
        }

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

        public static List<int> GetRandomList(int length)
        {
            List<int> list = new List<int>();
            for ( int i = 0; i < length; i++ )
            {
                list.Add(Random.Range(0, length));
            }
            return list;
        }

        public static List<int> GetAscendList(int length)
        {
            List<int> list = new List<int>();
            for ( int i = 0; i < length; i++ )
            {
                list.Add(i);
            }
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
        public static int GetListMax(List<int> list)
        {
            int maxIndex = 0;
            for ( int i = 1; i < list.Count; i++ )
            {
                if ( list[i] > list[maxIndex] )
                {
                    maxIndex = i;
                }
            }
            return list[maxIndex];
        }

        // 输入颜色码的十六进制表示，返回RGB表示
        public static Color Color_HexToRgb(string color)
        {
            int index = 0;
            if ( color[0] == '#' ) index++;
            string R = color.Substring(index, 2);
            string G = color.Substring(index + 2, 2);
            string B = color.Substring(index + 4, 2);
            return new Color(HexToDec(R) / 255f, HexToDec(G) / 255f, HexToDec(B) / 255f);
        }

       
        // 输入一个不超过int的十六进制数，返回该数的十进制表示
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
            while ( Q.Count > 0 )
            {
                GameObject obj = Q.Dequeue();
                for ( int i = 0; i < obj.transform.childCount; i++ )
                {
                    GameObject go = obj.transform.GetChild(i).gameObject;
                    if ( go.name.CompareTo(childName) == 0 ) return go;
                    else Q.Enqueue(go);
                }
            }
            return null;
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

        public static void Swap<T>(List<T> list, int x, int y)
        {
            T temp = list[x];
            list[x] = list[y];
            list[y] = temp;
        }

    }
}
