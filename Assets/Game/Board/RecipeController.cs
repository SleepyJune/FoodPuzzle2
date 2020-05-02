using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Board
{
    public class RecipeController : MonoBehaviour
    {
        public Image image;

        public IngredientController ingredientPrefab;
        public Transform ingredientParent;

        Dictionary<FoodObject, IngredientController> ingredients = new Dictionary<FoodObject, IngredientController>();

        public void SetRecipe(FoodRecipeObject recipe)
        {
            image.sprite = recipe.dish.icon;

            ingredientParent.DeleteChildren();

            foreach(var ingredient in recipe.ingredients)
            {
                var newIngredient = Instantiate(ingredientPrefab, ingredientParent);
                var amount = UnityEngine.Random.Range(10, 50);

                newIngredient.Initialize(ingredient, amount);

                ingredients.Add(ingredient, newIngredient);
            }
        }

        public void AddIngredient(FoodObject ingredient, int amount)
        {
            IngredientController ingredientController;
            if(ingredients.TryGetValue(ingredient, out ingredientController))
            {
                ingredientController.Add(amount);
            }
        }
    }
}
