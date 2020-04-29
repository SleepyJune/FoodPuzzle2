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

        public void SetRecipe(FoodRecipeObject recipe)
        {
            image.sprite = recipe.dish.icon;
        }
    }
}
