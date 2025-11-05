using UnityEngine;

public class MenuControll : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject recipeTip;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);

            if (!menuCanvas.activeSelf)
            {
                HoverTipManager.OnMouseLoseFocus();
            }
        }
    }
}
