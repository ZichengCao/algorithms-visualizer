using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using FunnyAlgorithm;
public class TreeControl : MonoBehaviour
{
    public TreeView view;
    private bool play_or_pause = true;//开始、暂停
    private TreeModel demo;
    private TreeType tree_type = TreeType.TRAVERSE;
    private static bool IsStart = false;
    // Start is called before the first frame update
    void Start()
    {
        LineTools.Line = view.LINE;
        view.Traverse_ButtonGroups[0].onClick.AddListener(delegate () { Start_Traverse(0); });
        view.Traverse_ButtonGroups[1].onClick.AddListener(delegate () { Start_Traverse(1); });
        view.Traverse_ButtonGroups[2].onClick.AddListener(delegate () { Start_Traverse(2); });
        view.random_btn.onClick.AddListener(Btn_random);
        initialize();
    }

    private void OnEnable()
    {
        MoveTool.duration = 1f;
        view.text_structDefinition.text = MyTools.ColourKeyWord(view.textAsset_StructDefinition.text) ;

    }
    private void OnDisable()
    {
        CleanView();
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void initialize()
    {
        demo = new TreeModel();
        if (tree_type == TreeType.TRAVERSE)
        {
            view.text_code.text = MyTools.ColourKeyWord(view.textAsset_Traverse[0].text);
        }
        else
        {

        }
    }

    public void SelectTopButton(int index)
    {
        if ((TreeType)index != tree_type)
        {
            view.TopButtonGroups[(int)tree_type].image.color = ColorSetting.normal;
            tree_type = (TreeType)index;
            view.TopButtonGroups[(int)tree_type].image.color = ColorSetting.orangeButton;
            if (tree_type == TreeType.TRAVERSE)
            {
                view.text_code.text = MyTools.ColourKeyWord(view.textAsset_Traverse[0].text) ;
                MyTools.SetActive(view.BSTree_ButtonGroups, false);
                MyTools.SetActive(view.Traverse_ButtonGroups, true);
            }
            if (tree_type == TreeType.BSTREE)
            {
                view.text_code.text = MyTools.ColourKeyWord(view.textAsset_BSTree[0].text) ;
                MyTools.SetActive(view.BSTree_ButtonGroups, true);
                MyTools.SetActive(view.Traverse_ButtonGroups, false);
            }
        }
    }

    public void Start_Traverse(int index)
    {
        demo.serial_number = 1;
        if (index == 0)
        {
            StartCoroutine(demo.Preorder());
        }
        else if (index == 1)
        {
            StartCoroutine(demo.Inorder());
        }
        else if (index == 2)
        {
            StartCoroutine(demo.Postorder());
        }
        //StartCoroutine(demo.Preorder(demo.Head));
    }
    public void Btn_Search()
    {
        if (!view.Input_Search.gameObject.activeSelf)
        {
            view.Input_Search.gameObject.SetActive(true);
            view.Input_Search.text = "";
        }
        else
        {
            view.Input_Search.gameObject.SetActive(false) ;
        }
    }

    public void Search()
    {
        if (view.Input_Search.text.Length > 0)
        {
            StartCoroutine(demo.Search(int.Parse(view.Input_Search.text)));
        }
    }
    public void Btn_Insert()
    {
        if (!view.Input_Insert.gameObject.activeSelf)
        {
            view.Input_Insert.gameObject.SetActive(true);
            view.Input_Insert.text = "";
        }
        else
        {
            view.Input_Insert.gameObject.SetActive(false);
        }
    }

    public void Insert()
    {
        if (view.Input_Insert.text.Length > 0)
        {
            StartCoroutine(demo.Insert(int.Parse(view.Input_Insert.text)));
        }
    }

    public void Btn_random()
    {
        CleanView();
        demo = new TreeModel();
    }
    

    public void Btn_remove()
    {
        if (demo.Head == null)
        {
            string str = "当前树空";
            view.warning.showWarning(str);
            return;
        }
        view.Shade.SetActive(true);
        EnableRemoveFunction(demo.Head);
    }

    private void EnableRemoveFunction(TreeNode head)
    {
        if (head == null) return;
        head.gameObject.GetComponent<Button>().enabled = true;
        head.gameObject.GetComponent<Button>().onClick.AddListener(Remove);
        EnableRemoveFunction(head.left);
        EnableRemoveFunction(head.right);
    }
    private void DisableRemoveFunction(TreeNode head)
    {
        if (head == null) return;
        head.gameObject.GetComponent<Button>().enabled = false;
        head.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        DisableRemoveFunction(head.left);
        DisableRemoveFunction(head.right);
    }

    public void Remove()
    {
        view.Shade.SetActive(false);
        StartCoroutine(demo.Remove(EventSystem.current.currentSelectedGameObject.GetComponent<TreeNode>().num));
        DisableRemoveFunction(demo.Head);
    }
    #region LeftButton_BTree
    //public void SelectTraverseType(int index)
    //{
    //    if (index != (int)traverse_type)
    //    {
    //        view.LeftButtonGroups_BTree[(int)traverse_type].image.color = Color.white;
    //        traverse_type = (BTreeTraverseType)index;
    //        view.LeftButtonGroups_BTree[(int)traverse_type].image.color = MyTools.Color_HexToRgb(MainControl.ColorSetting["orangeButton"]) ;
    //        if (IsStart)
    //            Restart_Btn();
    //        else
    //            initialize();
    //    }

    //}
    #endregion

    public void CleanView()
    {
        StopAllCoroutines();
        LineTools.DestroyLines(demo.Head);
        for (int i = 0; i < view.DemoArea.transform.childCount; i++)
        {
            if (view.DemoArea.transform.GetChild(i).tag == "NODE")
                Destroy(view.DemoArea.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < view.PointArea.transform.childCount; i++)
        {
            Destroy(view.PointArea.transform.GetChild(i).gameObject);
        }
       
        for (int i = 0; i < view.SelectableGroups.Length; i++)
        {
            view.SelectableGroups[i].interactable = true;
        }

    }
}
