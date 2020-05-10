using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Game.Board
{
    public class BoardGameManager : MonoBehaviour
    {
        public RecipeController recipeController;
        public SlotMatchChecker matchChecker;

        FoodRecipeObject currentRecipe;

        [NonSerialized]
        public List<FoodObject> foodList;

        public void Initialize()
        {
            GenerateNewRandomDish();
        }

        void GenerateNewRandomDish()
        {
            currentRecipe = GameManager.instance.foodManager.GenerateRandomRecipe();

            recipeController.SetRecipe(currentRecipe);

            foodList = new List<FoodObject>();

            foreach(var food in currentRecipe.ingredients)
            {
                foodList.Add(food);
            }
        }

        public void OnSlotPressed(Slot slot)
        {
            matchChecker.CheckAndDestroy(slot);
        }

        public FoodObject GenerateRandomFood()
        {
            return foodList.GenerateRandomElement();
        }

        FoodRecipeObject GetRecipe()
        {
            return currentRecipe;
        }
    }
}
