using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FunnyAlgorithm;

public class LinkView : MonoBehaviour
{
    public Button next_btn;
    public Selectable[] SelectableGroups;
    public TextAsset[] textAssets_StructDefinition;
    public TextAsset[] textAssetsSingleList;
    public TextAsset textAssetSingleListSummary;
    public TextAsset[] textAssetsDoubleList;
    public TextAsset textAssetDoubleListSummary;


    public Text text_StructDefinition;
    public Text text_code;

    
    public Warning warning;
    public LinkControl Control;
    public GameObject NODE;
    public GameObject PONINTER;
    public GameObject VARIABLE;
    public GameObject DemoArea;
    public GameObject PointArea;
    public GameObject SymbolTableArea;
    public GameObject Shade;

    public Button btn_RandomLinkList;

    public Button[] TopButtonGroups;
    public Button[] InsertButtonGroups;
    
    public Button Search_Remove_Button;

    //public GameObject selectArea_push_front;
    //public GameObject selectArea_push_back;
    //public GameObject selectValue_specify_insert;
    //public GameObject selectValue_search;
    
    public GameObject selectArea;
    public Button[] selectAreaGroups;
    public GameObject ArrowArea_specify_insert;
    public GameObject ArrowArea_remove;
    public InputField input_list;
    //public InputField input_push_front;
    //public InputField input_push_f;

    // Start is called before the first frame update
    void Start()
    {
        text_StructDefinition.text = MyTools.ColourKeyWord(textAssets_StructDefinition[0].text);
    }
}
