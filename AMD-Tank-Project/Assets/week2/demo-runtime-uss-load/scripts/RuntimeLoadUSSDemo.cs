using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UIDocument))]
public class RuntimeLoadUSSDemo : MonoBehaviour
{
    private enum Theme
    {
        DarkMode, LightMode
    };

    //Chosen theme & two USS files
    [SerializeField] private Theme m_theme;
    [SerializeField] private StyleSheet m_darkmodeUSS;
    [SerializeField] private StyleSheet m_lightmodeUSS;

    //UI document, root element, and debug label
    private UIDocument m_uiDocument;
    private VisualElement m_rootElement;
    private Label m_debugLabel;

    private void Awake()
    {
        m_uiDocument = GetComponent<UIDocument>();
        m_rootElement = m_uiDocument.rootVisualElement;
        m_debugLabel = m_rootElement.Q<Label>(name: "debug-label");
    }

    private void RemoveExistingStylesheets(VisualElement root) => root.styleSheets.Clear();

    private void LoadStylesheet(Theme theme)
    {
        //Dark mode? Add to stylesheets
        if (theme == Theme.DarkMode)
            m_rootElement.styleSheets.Add(m_darkmodeUSS);

        //Light mode? Add to stylesheets
        else if (theme == Theme.LightMode)
            m_rootElement.styleSheets.Add(m_lightmodeUSS);
    }
    private void UpdateDebugLabel() => m_debugLabel.text = $"m_theme = ${m_theme}";

    private void Start()
    {
        RemoveExistingStylesheets(m_rootElement);
        LoadStylesheet(m_theme);
        UpdateDebugLabel();        
    }

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Space pressed? Remove stylesheets
            RemoveExistingStylesheets(m_rootElement);

            //Toggle theme dark/light
            m_theme = (m_theme == Theme.DarkMode) ? (Theme.LightMode) : (Theme.DarkMode);

            //Load new stylesheet and update label
            LoadStylesheet(m_theme);
            UpdateDebugLabel();
        }
    }
}
