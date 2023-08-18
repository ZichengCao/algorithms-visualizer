using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeView : MonoBehaviour
{

    public Warning warning;
    public GameObject NODE;
    public GameObject POINT;
    public GameObject LINE;
    public GameObject DemoArea;
    public GameObject PointArea;
    public GameObject Shade;
    public Selectable[] SelectableGroups;


    public TextAsset textAsset_StructDefinition;
    public TextAsset[] textAsset_Traverse;
    public TextAsset[] textAsset_BSTree;
    public Button random_btn, next_btn;

    public Button[] TopButtonGroups;
    public Button[] Traverse_ButtonGroups;
    public Button[] BSTree_ButtonGroups;

    public Text text_code;
    public Text text_structDefinition;

    public InputField Input_Search;
    public InputField Input_Insert;

    public Scrollbar sb;
    // Start is called before the first frame update

}
