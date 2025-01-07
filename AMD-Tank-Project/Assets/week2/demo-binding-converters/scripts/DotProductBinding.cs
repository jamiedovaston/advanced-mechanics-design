using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(DotProductBinding))]
public class DotProductBinding : MonoBehaviour
{
    //The document & root
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;

    //Things in the UI
    private Label m_vecALabel;
    private Label m_vecBLabel;
    private Label m_dotProductLabel;

    //Reference to the script
    [SerializeField] private DotProductTest m_dotProductTestScript;

    private void Awake()
    {
        //Get all elements
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_vecALabel = m_rootElement.Q<Label>(name: "vector_a");
        m_vecBLabel = m_rootElement.Q<Label>(name: "vector_b");
        m_dotProductLabel = m_rootElement.Q<Label>(name: "dot-prod-result");
    }

    void Start()
    {
        //This is just shorthand for producing a DataBinding
        System.Func<string, DataBinding> dataBinder = (string path) => new DataBinding
        {
            dataSource = m_dotProductTestScript,
            dataSourcePath = new Unity.Properties.PropertyPath(path),
            bindingMode = BindingMode.ToTarget
        };

        //Bind 
        DataBinding vecABinding = dataBinder("vectorA");
        DataBinding vecBBinding = dataBinder("vectorB");
        DataBinding dotBinding = dataBinder("dotProduct");

        //--

        //Provide formatting with rounded brackets
        vecABinding.sourceToUiConverters.AddConverter(
            (ref Vector3 value) => $"VectorA is ({value.x}, {value.y}, {value.z})"
        );

        //Provide formatting with square brackets
        vecBBinding.sourceToUiConverters.AddConverter(
            (ref Vector3 value) => $"VectorB is [{value.x}, {value.y}, {value.z}]"
        );

        dotBinding.sourceToUiConverters.AddConverter((ref float value) =>
        {
            //dot(a, b) = 0? Orthogonal
            if (Mathf.Approximately(value, 0.0f))
                return $"Dot (Perpendicular): {value}";

            //dot(a, b) = 1? Correlated
            else if (Mathf.Approximately(value, 1.0f))
                return $"Dot (Identical): {value}";

            //dot(a, b) = -1? Anti correlated
            else if (Mathf.Approximately(value, -1.0f))
                return $"Dot (Opposing): {value}";
            
            //Otherwise show dot product 
            else
                return $"Dot: {value}";
        });

        //--

        m_vecALabel.SetBinding("text", vecABinding);
        m_vecBLabel.SetBinding("text", vecBBinding);
        m_dotProductLabel.SetBinding("text", dotBinding);
    }
}
