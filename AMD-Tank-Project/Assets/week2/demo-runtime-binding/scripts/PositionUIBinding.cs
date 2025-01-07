using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class PositionUIBinding : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;
    private Vector3Field m_positionControl;

    //The health script
    [SerializeField] private PositionScript m_positionScript;

    private void Awake()
    {
        //Boilerplate
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_positionControl = m_rootElement.Q<Vector3Field>(name: "position-field");

        //Make the binding
        DataBinding binding = new DataBinding
        {
            dataSource = m_positionScript,
            dataSourcePath = new Unity.Properties.PropertyPath("m_position"),
            bindingMode = BindingMode.TwoWay
        };

        //Set update triggers
        binding.updateTrigger = BindingUpdateTrigger.OnSourceChanged;

        //Set binding for field
        m_positionControl.SetBinding("value", binding);

    }
}
