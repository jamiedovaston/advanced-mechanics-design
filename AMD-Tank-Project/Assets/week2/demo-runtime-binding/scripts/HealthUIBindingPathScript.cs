using UnityEngine;
using UnityEngine.UIElements;
using Unity.Properties;
using UnityEditor;
using UnityEditor.UIElements;

[RequireComponent(typeof(UIDocument))]
public class HealthUIBindingPathScript : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;
    private Label m_healthLabel;

    //The health script
    [SerializeField] private HealthScript m_playerHealthScript;

    void Awake()
    {
        //Boilerplate
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_healthLabel = m_rootElement.Q<Label>(name: "health-value");

        //Set the binding path via string
        m_healthLabel.bindingPath = "m_health";

        //Bind the object to track changes
        m_healthLabel.Bind(new SerializedObject(m_playerHealthScript));
    }
}
