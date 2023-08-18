using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FunnyAlgorithm
{
    public class StartView: MonoBehaviour
    {
        public RectTransform[] rects;
        public Image[] images;
        public RectTransform[] texts;            

        #region Model1
        public RectTransform Book;
        #endregion

        #region Model2
        public GameObject[] Nodes;
        #endregion

        #region Model3
        public RectTransform Computer_Keyboard;
        public TextAsset text;
        public Text CodeText;
        #endregion

        void Start()
        {
          
        }
    }
}
