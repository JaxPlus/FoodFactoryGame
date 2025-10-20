using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeController : MonoBehaviour
{
    [SerializeField] public List<GameObject> recipeQueue;
    [SerializeField] public GameObject recipeWindow;
    private Recipe currentRecipe;
    
    public static RecipeController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentRecipe = null;
    }

    public void AddRecipeToQueue(GameObject recipe)
    {
        recipeQueue.Add(recipe);
    }
    
    private void UpdateRecipeQueue()
    {
        foreach (var recipe in recipeQueue)
        {
            Instantiate(recipe.GetComponent<Recipe>().result.GetComponent<Image>(), recipeWindow.transform.position, recipeWindow.transform.rotation);
        }
    }
}
