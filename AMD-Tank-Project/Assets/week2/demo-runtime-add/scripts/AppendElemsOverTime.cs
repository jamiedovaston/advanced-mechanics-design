using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class AppendElemsOverTime : MonoBehaviour
{
    [SerializeField] private string m_containerName = "container";

    private VisualElement m_rootElement;
    private ScrollView m_containerElement;

    private static uint buttonCount = 0;

    private void Awake()
    {
        //Get root element
        m_rootElement = GetComponent<UIDocument>().rootVisualElement;

        //Get container element
        m_containerElement = m_rootElement.Q<ScrollView>(name: m_containerName);

        //Couldn't find container? Throw error
        if (m_containerElement == null)
        {
            throw new UnityException($"Couldn't find container element with identifier #{m_containerName}");
        }

        //Otherwise start up coroutine for adding elements
        StartCoroutine(AddButtonUnderContainer());
    }


    private IEnumerator AddButtonUnderContainer()
    {
        while(true)
        {
            //Wait a bit
            yield return new WaitForSecondsRealtime(0.5f);

            //Make the button
            Button newButton = new Button();
            newButton.name = $"button-{++buttonCount}";
            newButton.text = $"I am button {buttonCount}";

            //Add it under the container element
            m_containerElement.Add(newButton);
        }
    }
}
