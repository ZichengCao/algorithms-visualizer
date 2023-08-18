using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FunnyAlgorithm
{
    public class StartInterfaceControl : MonoBehaviour
    {
        public RectTransform row1, row2, row3;
        public RectTransform[] rects_block;
        public Image[] images_block;
        public Text[] texts_detail;
        public RectTransform[] rects_text;

        private float duration = 0.2f;
        private void Awake()
        {
        }
        public void SecondModeMouseIn(int index)
        {
            rects_block[index].DOSizeDelta(new Vector2(400f, 200f), duration);
            images_block[index].DOColor(MyTools.Color_HexToRgb("#FFBC8E"), duration);
            rects_text[index].DOScale(new Vector3(1.2f, 1.2f, 1f), duration);
            texts_detail[index].gameObject.SetActive(true);
            texts_detail[index].GetComponent<Text>().DOFade(1, duration * 2);
            if (index >= 6)
            {
                rects_block[index].DOSizeDelta(new Vector2(400f, 200f), duration);
            }
            //if (index < 3)
            //{
                
            //    row2.DOAnchorPosY(row2.anchoredPosition.y - 10, 0);
            //    row3.DOAnchorPosY(row3.anchoredPosition.y - 10, 0);
            //}
            //else if (index < 6)
            //{
            //    row1.DOAnchorPosY(row1.anchoredPosition.y + 10, 0);
            //    row3.DOAnchorPosY(row3.anchoredPosition.y - 10, 0);
            //}
            //else
            //{
            //    row1.DOAnchorPosY(row1.anchoredPosition.y + 10, 0);
            //    row2.DOAnchorPosY(row2.anchoredPosition.y + 10, 0);
            //}
        }
        public void SecondModeMouseOut(int index)
        {
            rects_block[index].DOSizeDelta(new Vector2(300f, 100f), duration);
            images_block[index].DOColor(MyTools.Color_HexToRgb("#A1DBFF"), duration);
            rects_text[index].DOScale(new Vector3(1f, 1f, 1f), duration);
            texts_detail[index].GetComponent<Text>().DOFade(0, duration * 2);
            texts_detail[index].gameObject.SetActive(false);

            //if (index < 3)
            //{
            //    row2.DOAnchorPosY(row2.anchoredPosition.y + 10, 0);
            //    row3.DOAnchorPosY(row3.anchoredPosition.y + 10, 0);
            //}
            //else if (index < 6)
            //{
            //    row1.DOAnchorPosY(row1.anchoredPosition.y - 10, 0);
            //    row3.DOAnchorPosY(row3.anchoredPosition.y + 10, 0);
            //}
            //else
            //{
            //    row1.DOAnchorPosY(row1.anchoredPosition.y - 10, 0);
            //    row2.DOAnchorPosY(row2.anchoredPosition.y - 10, 0);
            //}
        }
    }
}
