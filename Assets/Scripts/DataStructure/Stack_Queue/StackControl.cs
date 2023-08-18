using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunnyAlgorithm;

public class StackControl : MonoBehaviour
{
    public StackView view;
    public StackType stack_type = StackType.LINKSTACK;
    private StackModel_ArrayNode demo_arrayNode;
    private StackModel_LinkNode demo_linkNode;
    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }
    private void OnEnable()
    {
        MoveTool.duration = 1f;
        view.btn_RandomLinkList.onClick.AddListener(RandomLinkList);
    }

    public void initialize()
    {

        if (stack_type == StackType.LINKSTACK)
        {
            demo_linkNode = new StackModel_LinkNode();
            view.text_StructDefinition.text = MyTools.ColourKeyWord(view.textAsset_StructDefinition[0].text);
            view.text_StructDefinition.fontSize = 22;
            view.text_code.text = MyTools.ColourKeyWord(view.textAsset_LinkStack[0].text);
        }
        else
        {
            demo_arrayNode = new StackModel_ArrayNode();
            view.text_StructDefinition.text = MyTools.ColourKeyWord(view.textAsset_StructDefinition[1].text);
            view.text_StructDefinition.fontSize = 30;
            view.text_code.text = MyTools.ColourKeyWord(view.textAsset_ArrayStack[0].text);
        }
        UpdateInputText();
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
                int temp = view.selectAreaGroups[i].name[view.selectAreaGroups[i].name.Length - 2] - '0';
                if (n == 0)
                {
                    view.selectAreaGroups[i].onClick.AddListener(delegate () { push(temp); });
                }
                //else if (n == 1)
                //    view.selectAreaGroups[i].onClick.AddListener(delegate () { push_back(temp); });
                //else if (n == 2)
                //    view.selectAreaGroups[i].onClick.AddListener(delegate () { sepcify_insert_value(temp); });
                //else if (n == 3)
                //    view.selectAreaGroups[i].onClick.AddListener(delegate () { search_remove(temp); });
            }
        }
        else
        {
            view.selectArea.SetActive(false);
        }
    }


    public void push_select()
    {
        if (stack_type == StackType.LINKSTACK)
        {
            if (demo_linkNode.nodes.Count == 9)
            {
                view.warning.showWarning("栈满");
                return;
            }
            else
            {
                ShowMenu(0);
            }
        }
        else
        {
            if (demo_linkNode.nodes.Count == 15)
            {
                view.warning.showWarning("栈满");
                return;
            }
            else
            {
                ShowMenu(0);
            }
        }

    }
    public void push(int index)
    {
        if (stack_type == StackType.LINKSTACK)
        {
            if (demo_linkNode.nodes.Count == 0)
            {
                CleanView();
                demo_linkNode.CreateLinkNodes(new List<int>() { index });
            }
            else
            {
                StartCoroutine(demo_linkNode.Push(index));
            }
        }
        else
        {
            if (demo_arrayNode.nodes.Count == 0)
            {
                CleanView();
                demo_arrayNode.CreateArrayNodes(new List<int>() { index });
            }
            else
            {
                StartCoroutine(demo_arrayNode.Push(index));
            }
        }

        view.selectArea.SetActive(false);

    }

    public void pop_select()
    {
        if (stack_type == StackType.LINKSTACK)
        {
            if (demo_linkNode.nodes.Count == 0)
            {
                view.warning.showWarning("栈空");
                return;
            }
            else
                StartCoroutine(demo_linkNode.Pop());
        }
        else
        {
            if (demo_arrayNode.nodes.Count == 0)
            {
                view.warning.showWarning("栈空");
                return;
            }
            else
                StartCoroutine(demo_arrayNode.Pop());
        }

    }

    public void TopButtonSelect(int index)
    {
        if ((StackType)(index) != stack_type)
        {
            CleanView();
            view.topButtonGroups[(int)stack_type].image.color = ColorSetting.normal;
            view.topButtonGroups[index].image.color = ColorSetting.orangeButton;
            stack_type = (StackType)(index);
            initialize();
        }
    }

    public void UpdateInputText()
    {
        view.input_list.onValueChanged.RemoveAllListeners();
        string str = "";
        if (stack_type == StackType.LINKSTACK)
        {
            for (int i = 0; i < demo_linkNode.nodes.Count; i++)
            {
                str += demo_linkNode.nodes[i].num.ToString();
                if (i != demo_linkNode.nodes.Count - 1) str += ",";
            }
        }
        else
        {
            for (int i = 0; i < demo_arrayNode.nodes.Count; i++)
            {
                str += demo_arrayNode.nodes[i].num.ToString();
                if (i != demo_arrayNode.nodes.Count - 1) str += ",";
            }
        }
        view.input_list.text = str;
        view.input_list.onValueChanged.AddListener(delegate { InputLinkList(); });
    }

    public void InputLinkList()
    {
        StopAllCoroutines();
        CleanView();
        if (stack_type == StackType.LINKSTACK)
        {
            demo_linkNode = new StackModel_LinkNode(view.input_list);
        }
        else
        {
            demo_arrayNode = new StackModel_ArrayNode(view.input_list);
        }
    }

    public void RandomLinkList()
    {
        StopAllCoroutines();
        CleanView();
        initialize();
    }

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

    }
}
