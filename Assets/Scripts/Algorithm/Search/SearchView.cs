using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyAlgorithm
{
    public class SearchView : MonoBehaviour
    {
        public SearchControl Control;
        public GameObject NODE;//预制体 
        public GameObject TargetNode;
        public GameObject SelectArea;
        public Button Btn_Restart;
        public Button Btn_NextStep;
        public Button Btn_StartButton;
        public Button Btn_LastStep;
        public Text Text_StartButton;
        public Text Text_Annotation;
        public Dropdown Drop_DataNums;
        public GameObject[] SearchButtonGroups;
        [HideInInspector] public List<Image> images_SearchButton = new List<Image>();


        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < SearchButtonGroups.Length; i++)
            {
                images_SearchButton.Add(SearchButtonGroups[i].GetComponent<Image>());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetSearchType_Button(int index)
        {
                if (index != (int)Control.search_type)
                {
                    images_SearchButton[(int)Control.search_type].color = new Color(1f, 1f, 1f);
                    Control.search_type = (searchType)index;
                    images_SearchButton[(int)Control.search_type].color = new Color(0.96078f, 0.56470f, 0.51764f);
                    ResetCompareDemoNums();
                    Control.Restart_Btn();
                }
        }
        public void SetDataNums_Btn()
        {
            int[] value_to_nums = { 5, 10, 15, 30 } ;
            SearchControl.data_length = value_to_nums[Drop_DataNums.value];
            //SearchControl.nums = MyTools.GetRandomList(SearchControl.data_length);
            ResetCompareDemoNums();
            Control.Restart_Btn();
        }

        public void SelectTargetNode()
        {
            for (int i = 0; i < SearchControl.data_length; i++) 
            {
                SelectArea.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public void SelectTargetNode(int index)
        {
            SearchControl.x = index;
            if (SearchControl.IsStart)
            {
                Control.Restart_Btn();
            }
            else
            {
                Control.Reinitialize();
            }
            for (int i = 0; i < SearchControl.data_length; i++)
            {
                SelectArea.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        public void ResetCompareDemoNums()
        {
            if (Control.search_type == searchType.LINEARSEARCH)
            {
                SearchControl.nums = MyTools.GetRandomList(SearchControl.data_length);
            }
            else
            {
                SearchControl.nums = MyTools.GetAscendList(SearchControl.data_length);
            }
            SearchControl.x = Random.Range(0, SearchControl.data_length); 
        }
    }

}
