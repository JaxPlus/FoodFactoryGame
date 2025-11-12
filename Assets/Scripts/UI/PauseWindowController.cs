using UnityEngine;

public class PauseWindowController : MonoBehaviour
{
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject background;

    void Start()
    {
        background.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            background.SetActive(!background.activeSelf);

            if (background.activeSelf)
            {
                Time.timeScale = 0.0f;
                hotbar.SetActive(false);
            }
            else
            {
                Time.timeScale = 1.0f;
                hotbar.SetActive(true);
            }
        }
    }

    public void Reasume()
    {
        background.SetActive(false);
        hotbar.SetActive(true);
        Time.timeScale = 1.0f;
    }
}
