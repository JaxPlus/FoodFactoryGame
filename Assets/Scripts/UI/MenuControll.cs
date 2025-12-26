using UnityEngine;
using UnityEngine.UIElements;

public class MenuControll : MonoBehaviour
{
    // [SerializeField] private GameObject hotbar;
    public GameObject menuCanvas;
    // public GameObject recipeTip;
    private QuestUI questUI;

    void Start()
    {
        questUI = FindFirstObjectByType<QuestUI>();
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            questUI.HideQuestDetails();
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            // hotbar.GetComponent<UIDocument>().rootVisualElement.visible = !menuCanvas.activeSelf;
            
            if (!menuCanvas.activeSelf)
            {
                HoverTipManager.OnMouseLoseFocus();
            }
        }
    }
}
