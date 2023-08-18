using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InputField))]
public class InputFieldScaler : MonoBehaviour, ILayoutElement
{
    private Text textComponent
    {
        get
        {
            return this.GetComponent<InputField>().textComponent;
        }

    }

    public TextGenerationSettings GetTextGenerationSettings(Vector2 extents)
    {

        var settings = textComponent.GetGenerationSettings(extents);
        settings.generateOutOfBounds = true;

        return settings;


    }

    private RectTransform m_Rect;

    private RectTransform rectTransform
    {
        get
        {
            if ( m_Rect == null )
                m_Rect = GetComponent<RectTransform>();
            return m_Rect;
        }
    }

    public void OnValueChanged(string v)
    {

        rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis) 0, LayoutUtility.GetPreferredSize(m_Rect, 0));
    }

    void OnEnable()
    {

        this.inputField.onValueChanged.AddListener(OnValueChanged);
    }

    void OnDisable()
    {

    }

    private Vector2 originalSize;
    private InputField _inputField;

    public InputField inputField
    {
        get
        {

            return _inputField ?? ( _inputField = this.GetComponent<InputField>() );

        }

    }

    protected void Awake()
    {
        this.originalSize = this.GetComponent<RectTransform>().sizeDelta;
    }

    private string text
    {
        get
        {
            return this.GetComponent<InputField>().text;
        }

    }

    private TextGenerator _generatorForLayout;

    public TextGenerator generatorForLayout
    {
        get
        {
            return _generatorForLayout ?? ( _generatorForLayout = new TextGenerator() );
        }
    }

    [Tooltip("测试用")]
    public float Width;
    [Tooltip("测试用")]
    public float Height;
    public void Update()
    {
        this.Width = this.preferredWidth;
        this.Height = this.preferredHeight;
    }


    public float preferredWidth
    {
        get
        {
            if ( keepMinOriginalSize )
            {
                return Mathf.Max(this.originalSize.x, generatorForLayout.GetPreferredWidth(text, GetTextGenerationSettings(Vector2.zero)) / textComponent.pixelsPerUnit + 20);
            }
            else
            {
                return generatorForLayout.GetPreferredWidth(text, GetTextGenerationSettings(Vector2.zero)) / textComponent.pixelsPerUnit + 20;
            }


        }

    }



    public virtual void CalculateLayoutInputHorizontal()
    {
    }

    public virtual void CalculateLayoutInputVertical()
    {
    }

    public virtual float minWidth
    {
        get { return -1; }
    }



    public virtual float flexibleWidth { get { return -1; } }

    public virtual float minHeight
    {
        get { return -1; }
    }

    public virtual float preferredHeight
    {
        get
        {
            if ( keepMinOriginalSize )
            {
                return Mathf.Max(this.originalSize.y, generatorForLayout.GetPreferredHeight(text, GetTextGenerationSettings(new Vector2(this.textComponent.GetPixelAdjustedRect().size.x, 0.0f))) / textComponent.pixelsPerUnit);

            }
            else
            {

                return generatorForLayout.GetPreferredHeight(text, GetTextGenerationSettings(new Vector2(this.textComponent.GetPixelAdjustedRect().size.x, 0.0f))) / textComponent.pixelsPerUnit;
            }

        }
    }

    public virtual float flexibleHeight { get { return -1; } }

    [SerializeField]
    [Tooltip("缩放的最小值为 Awake()中取得的值")]
    private bool keepMinOriginalSize = true;

    [SerializeField]
    [Tooltip("提高Layou计算优先级，要比InputField大 这里设为1")]
    private int priority = 1;

    public virtual int layoutPriority { get { return priority; } }



}