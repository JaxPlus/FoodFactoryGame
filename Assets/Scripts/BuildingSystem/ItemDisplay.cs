using UnityEngine;

public class ItemDisplay : MonoBehaviour
{
    void Update()
    {
        var parent = GetComponentInParent<Belt>();

        if (parent != null)
        {
            if (!parent.IsItemDisplaySet())
            {
                parent.SetItemDisplay(gameObject);
            }
        }
    }
}
