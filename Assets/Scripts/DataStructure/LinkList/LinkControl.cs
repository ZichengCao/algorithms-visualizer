using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FunnyAlgorithm;
using System;

public class LinkControl : MonoBehaviour
{
    [HideInInspector]
    public LinkModel demo;
    public linkListType link_type = linkListType.SINGLE;
    public LinkView view;

    private void Start()
    {
        view.input_list.onValueChanged.AddListener(delegate { InputLinkList(); });
        initializeList();
    }

    private void OnEnable()
    {
        MoveTool.duration = 1f;
    }

    private void ShowMenu(int n)
    {
        bool flag = view.selectArea.activeSelf;
        if (!flag)
        {
            view.selectArea.SetActive(true);
            for (int i = 0; i < 10; i++)
            {
                view.selectAreaGroups[i].onClick.RemoveAllListeners();
                // delegate 会绑定这个变量的值，也就是说用的是调用这个函数时的值，如果用i的话，调用函数时全部都是10
                int temp = view.selectAreaGroups[i].name[view.selectAreaGroups[i].name.Length - 2] - '0';
                if (n == 0)
                    //view.selectAreaGroups[i].onClick.AddListener(delegate () { push_front(temp); });
                    view.selectAreaGroups[i].onClick.AddListener(push_front);
                else if (n == 1)
                    view.selectAreaGroups[i].onClick.AddListener(push_back);
                //else if (n == 2)
                //    view.selectAreaGroups[i].onClick.AddListener(delegate() { sepcify_insert_value(temp); });
                else if (n == 2)
                    view.selectAreaGroups[i].onClick.AddListener(search);
            }
        }
        else
        {
            view.selectArea.SetActive(false);
        }
    }

    public void push_front_btn()
    {
        if (demo.nodes.Count == 9)
        {
            view.warning.showWarning("便于演示，请不要继续添加结点");
            return;
        }
        else
        {
            ShowMenu(0);
        }
    }
    private void push_front()
    {
        int value = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        if (demo.nodes.Count == 1)
        {
            CleanView();
            //demo = link_type == linkListType.SINGLE ? new SingleLinkList(view.input_list) : new DoubleLinkList(view.input_list);
            demo.CreateLinkNodes(new List<int>() { value }, link_type == linkListType.SINGLE ? false : true) ;
            UpdateInputText();

        }
        else
        {
            StartCoroutine(demo.PushFront(value));
        }
        view.selectArea.SetActive(false);
    }

    public void push_back_btn()
    {

        if (demo.nodes.Count == 9)
        {
            view.warning.showWarning("便于演示，请不要继续添加结点");
            return;
        }
        else
        {
            ShowMenu(1);
        }
    }
    private void push_back()
    {
        int value = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);

        if (demo.nodes.Count == 1)
        {
            CleanView();
            demo.CreateLinkNodes(new List<int>() { value }, link_type == linkListType.SINGLE ? false : true);
            UpdateInputText();

        }
        else
        {
            StartCoroutine(demo.PushBack(value));
            view.selectArea.SetActive(false);

        }
    }

    public void search_btn()
    {
        ShowMenu(2);
    }

    public void remove_btn()
    {
        EnableRemoveFunction();
    }

    private void search()
    {
        int value = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text);
        StartCoroutine(demo.Search(value));
        view.selectArea.SetActive(false);
    }

    private void remove()
    {
        int value = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<LinkNode>().num;
        DisableRemoveFunction();
        StartCoroutine(demo.Remove(value));
    }
    public void initializeList()
    {
        if (link_type == linkListType.SINGLE)
        {
            demo = new SingleLinkList();
            view.text_code.text = MyTools.ColourKeyWord(view.textAssetSingleListSummary.text);

        }
        else
        {
            demo = new DoubleLinkList();
            //view.text_code.text = MyTools.ColourKeyWord(view.textAssetDoubleListSummary.text);
        }
        //view.text_code.text= MyTools.ColourKeyWord(view..text);
        //if (link_type == linkListType.INSERT)
        //{
        //    view.text_code.text = MyTools.ColourKeyWord(view.textAsset_push.text);
        //}else if (link_type == linkListType.REMOVE)
        //{
        //    view.text_code.text = MyTools.ColourKeyWord(view.textAsset_remove.text);
        //}
        //else if (link_type == linkListType.SEARCH)
        //{
        //    view.text_code.text = MyTools.ColourKeyWord(view.textAsset_search.text);
        //}
        UpdateInputText();
    }
    public void RandomLinkList()
    {
        StopAllCoroutines();
        CleanView();
        initializeList();
    }

    #region process control
    public void CleanView()
    {
        for (int i = 0; i < view.DemoArea.transform.childCount; i++)
        {
            if (view.DemoArea.transform.GetChild(i).tag == "NODE")
                Destroy(view.DemoArea.transform.GetChild(i).gameObject);
        }
        view.selectArea.gameObject.SetActive(false);

        for (int i = 0; i < view.PointArea.transform.childCount; i++)
        {
            Destroy(view.PointArea.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < view.SymbolTableArea.transform.childCount; i++)
        {
            Destroy(view.SymbolTableArea.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < view.SelectableGroups.Length; i++)
        {
            view.SelectableGroups[i].interactable = true;
        }
        demo.nodes.Clear();
    }
    public void UpdateInputText()
    {
        view.input_list.onValueChanged.RemoveAllListeners();
        // 链表长度调整为一个时，需要把头结点的指针隐藏，随机出来的长度不会是一个，界面的四种操作结果一定会更新到 inputtext 中
        // 也就一定会执行该函数，在此处做更新最合适
        if (demo.nodes.Count == 1)
        {
            demo.Head.HidePoint(1);
        }
        else
        {
            demo.Head.ShowPoint();
        }
        string str = "";
        for (int i = 1; i < demo.nodes.Count; i++)
        {
            str += demo.nodes[i].num.ToString();
            if (i != demo.nodes.Count - 1) str += ",";
        }
        view.input_list.text = str;
        view.input_list.onValueChanged.AddListener(delegate { InputLinkList(); });
    }
    public void InputLinkList()
    {
        StopAllCoroutines();
        CleanView();

        if (link_type == linkListType.SINGLE)
        {
            demo = new SingleLinkList(view.input_list);
        }
        else
        {
            demo = new DoubleLinkList(view.input_list);
        }
    }

    public void SelectTopButton(int index)
    {
        if (index != (int)link_type)
        {
            view.TopButtonGroups[(int)link_type].image.color = Color.white;
            link_type = (linkListType)index;
            view.TopButtonGroups[(int)link_type].image.color = ColorSetting.orangeButton;
            CleanView();
            initializeList();
        }
    }

    #endregion

    #region tools
    private void EnableRemoveFunction()
    {
        view.Shade.SetActive(true);
        List<int> list = new List<int>();
        //  从1开始，头结点不需要
        for (int i = 1; i < demo.nodes.Count; i++)
        {
            demo.nodes[i].gameObject.GetComponent<Button>().enabled = true;
            demo.nodes[i].gameObject.GetComponent<Button>().onClick.AddListener(remove);
        }
    }
    private void DisableRemoveFunction()
    {
        view.Shade.SetActive(false);
        for (int i = 1; i < demo.nodes.Count; i++)
        {
            demo.nodes[i].gameObject.GetComponent<Button>().enabled = false;
            demo.nodes[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
    #endregion
}
