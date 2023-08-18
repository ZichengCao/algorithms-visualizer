using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public abstract class Node : MonoBehaviour
    {
        public int num;
        public Image image;
        public RectTransform rect;

        private void OnEnable()
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }

        public void Move(float value, direction dir, float duration)
        {
            switch (dir)
            {
                case direction.UP:
                    rect.DOAnchorPosY(rect.anchoredPosition.y + value, duration);
                    break;
                case direction.RIGHT:
                    rect.DOAnchorPosX(rect.anchoredPosition.x + value, duration);
                    break;
                case direction.LEFT:
                    rect.DOAnchorPosX(rect.anchoredPosition.x - value, duration);
                    break;
                case direction.DOWN:
                    rect.DOAnchorPosY(rect.anchoredPosition.y - value, duration);
                    break;
                default:
                    break;
            }
        }

        public void Move(int x_steps, int y_steps)
        {
            MoveTool.MoveTo(rect, x_steps, y_steps, ArrayNode.width, ArrayNode.verticalStandard, MoveTool.duration);
        }

      

        public void SetColor(string color)
        {
            if (image == null) return;
            image.color = MyTools.Color_HexToRgb(color);
        }

        public void SetColor(Color color)
        {
            image.color = color;               
        }
    }
}
