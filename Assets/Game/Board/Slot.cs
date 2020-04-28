using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Board
{
    public class Slot : MonoBehaviour
    {
        public static float slotWidth;
        public static float slotHeight;

        public Image image;
        public CanvasGroup canvasGroup;

        public HashSet<Slot> neighbours = new HashSet<Slot>();

        public int x;
        public int y;

        BoardManager boardManager;
        RectTransform rectTransform;

        [NonSerialized]
        public FoodObject food;

        public void Initialize(int x, int y)
        {
            boardManager = GameManager.instance.boardManager;            

            this.x = x;
            this.y = y;

            rectTransform = GetComponent<RectTransform>();
            
            var randomFood = GameManager.instance.foodManager.GenerateRandomFood();

            SetSlotType(randomFood);
        }
        public void SetSlotType(FoodObject food)
        {
            this.food = food;
            image.sprite = food.icon;
        }
    }
}
