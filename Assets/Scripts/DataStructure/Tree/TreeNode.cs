using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class TreeNode : Node
    {
        public int index;//在数组中的索引，标记位置使用
        public Text serial_number;
        public TreeNode left = null, right = null;
        public GameObject left_line = null, right_line = null;
        private void OnEnable()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
        public void initialize(Vector2 Pos, float radius, int index)
        {
            GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
            GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
            GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            GetComponent<RectTransform>().anchoredPosition = Pos;
            GetComponent<RectTransform>().sizeDelta = new Vector2(radius, radius);
            this.index = index;
        }
        public void SetRadius(float radius)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(radius, radius);
        }
        public void ActiveNode()
        {
            gameObject.GetComponent<Image>().enabled = true;
            gameObject.GetComponentInChildren<Text>().enabled = true;
        }
        public void KillNode()
        {
            gameObject.GetComponent<Image>().enabled = false;
            gameObject.GetComponentInChildren<Text>().enabled = false;
        }
        public void SetValue(int val)
        {
            this.num = val;
            GetComponentInChildren<Text>().text = val.ToString();
            //num.text = val.ToString();
        }
        public void SetSerialNumber(int n)
        {
            serial_number.gameObject.SetActive(true);
            serial_number.text = n.ToString();
        }
        public void hideSerialNumber()
        {
            serial_number.gameObject.SetActive(false);
        }
        public void Fade()
        {
            image.DOFade(0, MoveTool.duration);
            GetComponentInChildren<Text>().DOFade(0, MoveTool.duration);
            Destroy(gameObject, 1f);
        }

        public void DrawLine()
        {
            if (left)
            {
                if (left_line != null)
                {
                    Destroy(left_line);
                    left_line = null;
                }
                left_line = LineTools.DrawLine(this, left) ;
            }
            if (right)
            {
                if (right_line != null)
                {
                    Destroy(right_line);
                    right_line = null;
                }
                right_line = LineTools.DrawLine(this, right) ;
            }
        }

        /// <summary>
        /// -1左，1右，0全部
        /// </summary>
        /// <param name="flag"></param>
        public void DestroyLine(childTree flag)
        {
            if (flag == childTree.LEFT || flag == childTree.BOTH) 
            {
                if (left_line != null) 
                    Destroy(left_line);
                left_line = null;
            }
            if (flag == childTree.RIGHT || flag == childTree.BOTH) 
            {
                if (right_line != null)
                    Destroy(right_line);
                right_line = null;
            }
        }
    }
}
