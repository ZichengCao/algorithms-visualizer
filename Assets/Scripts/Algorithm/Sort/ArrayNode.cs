using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;



namespace FunnyAlgorithm
{
    public class ArrayNode : Node
    {
        public GameObject g;
        public Text text;
        public int index;
        //Node的宽度，Node在垂直方向上移动的距离
        public static float width, verticalStandard = 100f;

        public ArrayNode(GameObject G, int num, int index, bool flag = true)
        {
            this.index = index;
            this.num = num;
            this.g = G;
            rect = g.GetComponent<RectTransform>();
            image = g.GetComponent<Image>();
            if (flag)
            {
                g.GetComponentInChildren<Text>().text = num.ToString();
            }
        }
        private void OnEnable()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            g = gameObject;
        }

        /// <summary>
        /// 对Image做长或宽形变
        /// </summary>
        /// <param name="dir">形变方向</param>
        /// <param name="destination">形变目标值</param>
        /// <param name="duration">变化时间</param>
        public void Reshape(direction dir, float destination, float duration = 0.5f)
        {
            if (dir == direction.HORIZONTIAL)
            {
                image.rectTransform.DOSizeDelta(new Vector2(destination, image.rectTransform.sizeDelta.y), duration);
            }
            else if (dir == direction.VERTICAL)
            {
                image.rectTransform.DOSizeDelta(new Vector2(image.rectTransform.sizeDelta.x, destination), duration);
            }
            else
            {
                throw new Exception("unexpected direction");
            }
        }

        /// <summary>
        /// 生成ArrayNode
        /// </summary>
        /// <param name="NODE">需要生成的预制体</param>
        /// <param name="parent">母体</param>
        /// <param name="viewSize">母体面板的尺寸</param>
        /// <param name="arr">数组</param>
        /// <param name="y_pos">水平生成位置自动计算，垂直位置由y_pos给出，默认贴着面板底部</param>
        /// <returns></returns>
        public static List<ArrayNode> CreatArryNodes(int length,int data_type,GameObject NODE, GameObject parent, Vector2 viewSize, List<int> arr, float y_pos = 0)
        {
            List<int> list;
            if (arr != null)
                list = arr;
            else
            {
                switch (data_type)
                {
                    case 0:
                        list = MyTools.GetNoRepeatList(length);
                        break;
                    case 1:
                        list = MyTools.GetRandomList(length);
                        break;
                    case 2:
                        list = MyTools.GetAscendList(length);
                        break;
                    case 3:
                        list = MyTools.GetDescendList(length);
                        break;
                    case 4:
                        list = MyTools.GetAlmostOrderedList(length);
                        break;
                    case 5:
                        list = MyTools.GetEqualList(length);
                        break;
                    default:
                        list = MyTools.GetNoRepeatList(length);
                        break;
                }
            }

            List<ArrayNode> nodes = new List<ArrayNode>();
            int count = list.Count;
            float DemoArea_width = viewSize.x, DemoArea_height = viewSize.y;
            //结点宽度，x方向上起始生成位置
            float width, maxHeight, x_pos;
            maxHeight = viewSize.y * 0.4f;
            width = DemoArea_width / (count + 2);
            x_pos = -(DemoArea_width / 2 - width - width / 2);
            //避免结点过宽
            if (width > 100f)
            {
                width = 100f;
                x_pos = -(width * count) / 2;
            }

            ArrayNode.width = width;

            bool ShowNumFlag = true;
            if (width < 30)
                ShowNumFlag = false;
            else
            {
                NODE.GetComponentInChildren<Text>().fontSize = 20 + (int)((width - 30) / 10);
                NODE.transform.GetChild(1).GetComponent<Text>().fontSize = 18 + (int)((width - 30) / 10);

            }
            bool showSerialFlag = false;
            if (SceneManager.GetActiveScene().name == "Search")
            {
                showSerialFlag = true;
            }
            //计算合适的起始生成位置（锚点在中下）
            for (int i = 0; i < count; i++)
            {
                //生成
                GameObject g = Instantiate(NODE, parent.transform);
                if (showSerialFlag)
                {
                    g.transform.GetChild(1).GetComponent<Text>().text = "(" + i.ToString() + ")";
                }
                ArrayNode temp = g.GetComponent<ArrayNode>();
                temp.num = list[i];
                temp.index = i;
                if (ShowNumFlag)
                {
                    g.GetComponentInChildren<Text>().text = list[i].ToString();
                }
                nodes.Add(temp);
                nodes[i].setPosition(x_pos, y_pos, maxHeight) ;
                //坐标移动到下一个位置
                x_pos += width;
            }
            //最短高度规定为35
            float height = 38;
            int mmax = MyTools.GetListMax(list);
            float HeightOffset = (maxHeight - height) / mmax;
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Reshape(direction.VERTICAL, height + HeightOffset * list[i]);
            }
            return nodes;
        }

        public void setPosition(float x_pos, float y_pos, float maxHeight)
        {
            //nodes[i].UpdatePos(RectTransform.Edge.Bottom, 0, 200, width, nodes[i].rect.sizeDelta.y, 0.5f, 0f, pos, 0) ;
            //设置锚点，第三个参数为沿插入方向的宽度，在这里也就是高度
            //设置高度,0-- > 高度100,15-- > 高度295
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, maxHeight);//100 + 195 / 15 * nodes[i].num
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchoredPosition = new Vector2(x_pos, y_pos);
        }
        public static void DestoryArrayNodes(GameObject DemoArea, List<ArrayNode> nodes)
        {
            for (int i = 0; i < DemoArea.transform.childCount; i++)
            {
                if (DemoArea.transform.GetChild(i).gameObject.tag == "NODE")
                    Destroy(DemoArea.transform.GetChild(i).gameObject);
            }
            nodes.Clear();
        }

        public void initialize(Vector2 pos)
        {
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = pos;
        }

        public void SetValue(int value)
        {
            num = value;
            text.text = value.ToString();
        }

        public void Fade()
        {
            text.DOFade(0, MoveTool.duration);
            image.DOFade(0, MoveTool.duration);
            Destroy(gameObject, 1f);
        }

        public void Assign(ArrayNode b)
        {
            index = b.index;
            num = b.num;
            rect = b.rect;
            g = b.g;
        }
    }
}
