using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgrammingView : MonoBehaviour
{
    public GameObject DemoArea;
    public GameObject SampleArea;
    public Button Btn_Conceal;
    public Text Text_ConcealBtn;
    public RectTransform Rect_DemoArea;
    public RectTransform Rect_SymbolTable;


    public GameObject NODE;
    public GameObject DemoArea_Content;
    public GameObject GO_SymbolTable;

    #region prefab
    public GameObject VARIABLE;
    public GameObject[] COMMAND;
    public GameObject END_IF, END_WHILE, EDN_ELSE;
    public GameObject ADD_SUB;
    #endregion

    public GameObject GO_Exception;
    [HideInInspector]
    public Text Text_Exception;
    public GameObject ExecuteQueueGridLayout;
    [HideInInspector]
    public RectTransform Rect_ExecuteQueueGridLayout;

    public GameObject AddMenu;
    public Scrollbar ScrollBar_ExecuteQueue_Vertical;
    public Button Btn_Execute;
    public Button Btn_Step;
    public Button Btn_ReExecute;
    public Button Btn_Clear;

    [HideInInspector]
    public RectTransform Rect_ExecuteBtn;
    [HideInInspector]
    public RectTransform Rect_StepBtn;

    void Start()
    {
        Rect_ExecuteBtn = Btn_Execute.GetComponent<RectTransform>();
        Rect_StepBtn = Btn_Step.GetComponent<RectTransform>();
        Text_Exception = GO_Exception.GetComponentInChildren<Text>();
        Rect_ExecuteQueueGridLayout = ExecuteQueueGridLayout.GetComponent<RectTransform>();
    }


}
