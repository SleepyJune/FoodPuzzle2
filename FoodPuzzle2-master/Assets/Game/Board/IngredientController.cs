using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Board
{
    public class IngredientController : MonoBehaviour
    {
        public Image image;
        public Text text;
        public Animator anim;

        int count;
        int max;

        public void Initialize(FoodObject food, int amount)
        {
            image.sprite = food.icon;
            text.text = amount.ToString();

            count = amount;
            max = amount;
        }

        public void Add(int amount)
        {
            count = Math.Max(0, count - amount);
            text.text = count.ToString();

            anim.SetTrigger("add");
        }
    }
}
