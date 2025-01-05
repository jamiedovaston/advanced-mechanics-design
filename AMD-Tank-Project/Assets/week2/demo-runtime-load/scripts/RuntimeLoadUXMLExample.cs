using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UIDocument))]
public class RuntimeLoadUXMLExample : MonoBehaviour
{
    //The UXML asset to load (USS will be auto linked!)
    [SerializeField] private VisualTreeAsset m_uxmlAssetToLoad;
    
    //Reference elements
    private VisualElement m_rootElement;
    private VisualElement m_containerElement;
    private UIDocument m_uiDocument;

    private void Awake()
    {
        //Set up references
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_containerElement = m_rootElement.Q<VisualElement>(name: "container");
    }


    private void LoadAndAppendUXML()
    {
        //Build a tree from the uxml asset at run-time, return the root
        VisualElement tempContainer = m_uxmlAssetToLoad.Instantiate();

        //And add this to the container
        m_containerElement.Add(tempContainer);
    }


    void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Space pressed? Load the UXML
            LoadAndAppendUXML();

            //Keep in mind that you should probably
            //be using input action events instead of this horrendous
            //polled input solution
        }
    }
}
