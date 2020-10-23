using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyAlgorithm
{
    public class DemoView : MonoBehaviour
    {
        public Button Btn_NextStep;
        public Button Btn_StartButton;
        public Button Btn_LastStep;
        public Text Text_StartButton;
        public Text Text_Dealy;

        public GameObject NODE;//预制体 
        public GameObject DemoArea;

        public Dropdown Drop_SortTypeSelect;
        public Dropdown Drop_DataNums;
        public Dropdown Drop_DataType;

        public Slider Slider_Delay;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
