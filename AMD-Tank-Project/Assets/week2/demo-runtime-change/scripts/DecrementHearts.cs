using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UIDocument))]
public class DecrementHearts : MonoBehaviour
{
    //UI document & root element
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;

    //Containers for hearts example
    private VisualElement m_heartsContainer;
    private VisualElement m_deadLabel;
    private List<VisualElement> m_hearts;

    //Internal heart count
    private int m_heartCount;

    private void Awake()
    {
        //Set up stuff
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;

        //Get containers, references, etc
        m_heartsContainer = m_rootElement.Q<VisualElement>(name: "hearts");
        m_deadLabel = m_rootElement.Q<VisualElement>(name: "dead-label");
        m_hearts = m_heartsContainer.Query<VisualElement>(className: "heart").ToList();

        //Set num of hearts
        m_heartCount = m_hearts.Count;
    }

    private void DecreaseHeartsCount(int offset = 1) => m_heartCount -= offset;

    private void ShowDeadLabel() => m_deadLabel.EnableInClassList("active", true);

    private void HideHeart(int index) => m_hearts[index].EnableInClassList("removed", true);

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame && m_heartCount > 0)
        {
            //Space pressed and lives left? Hide the current
            //heart and decrease num hearts
            HideHeart(m_heartCount - 1);
            DecreaseHeartsCount();

            //Dead? Show dead label by adding active class
            if(m_heartCount == 0)
            {
                ShowDeadLabel();
            }
        }
    }
}
