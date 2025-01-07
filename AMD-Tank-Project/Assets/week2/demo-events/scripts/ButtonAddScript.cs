using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ButtonAddScript : MonoBehaviour
{
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;

    private VisualElement m_containerElement;
    private Button m_addButton;

    private static uint buttonCount = 0;

    private void Awake()
    {
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_containerElement = m_rootElement.Q<VisualElement>(name: "container");
        m_addButton = m_rootElement.Q<Button>(name: "add-button");

        //Alternative <ClickEvent,VisualElement> version to
        //additionally pass VisualElement as target
        m_addButton.RegisterCallback<ClickEvent, VisualElement>(HandleButtonClick, m_addButton);
    }

    private void HandleButtonClick(ClickEvent eventData, VisualElement elem)
    {
        Debug.Log($"Clicked on {elem.name}");
        AddButton();
    }

    private void AddButton()
    {
        Button newButton = new Button();
        newButton.name = $"container-button-{++buttonCount}";
        newButton.text = $"I am button {buttonCount}";

        newButton.RegisterCallback<ClickEvent, VisualElement>(HandleButtonClick, newButton);

        m_containerElement.Add(newButton);
    }
}
