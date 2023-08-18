using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FunnyAlgorithm;

public class StackView : MonoBehaviour
{
    public StackControl Control;
    public Selectable[] SelectableGroups;

    public Button next_btn, btn_RandomLinkList, btn_push, btn_pop;

    public TextAsset[] textAsset_StructDefinition;
    public TextAsset[] textAsset_LinkStack;
    public TextAsset[] textAsset_ArrayStack;

    public Text text_StructDefinition;
    public Text text_code;

    public Warning warning;
    public GameObject VARIABLE;
    public GameObject LINKNODE;
    public GameObject POINT;
    public GameObject ARRAYNODE;
    public GameObject DemoArea;
    public GameObject PointArea;
    public GameObject selectArea;
    public GameObject SymbolTableArea;
    public Button[] selectAreaGroups;
    public Button[] topButtonGroups;


    public InputField input_list;
    // Start is called before the first frame update
    public string PreTreat(string str)
    {
        DS_processControl.passport = false;
        next_btn.interactable = true;
        for (int i = 0; i < SelectableGroups.Length; i++)
        {
            if (SelectableGroups[i].gameObject.activeSelf)
            {
                SelectableGroups[i].interactable = false;
            }
        }
        return text_code.text = MyTools.ColourKeyWord(str);
    }

    public void FinalTreat(string originalStr)
    {
        text_code.text = MyTools.ColourKeyWord(originalStr) ;
        next_btn.interactable = false;
        for (int i = 0; i < SelectableGroups.Length; i++)
        {
            if (SelectableGroups[i].gameObject.activeSelf)
            {
                SelectableGroups[i].interactable = true;
            }
        }
        Control.UpdateInputText();
    }
}
