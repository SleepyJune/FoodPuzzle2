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

        public void Initialize(FoodObject food, int x, int y)
        {
            boardManager = GameManager.instance.boardManager;            

            this.x = x;
            this.y = y;

            rectTransform = GetComponent<RectTransform>();
                        
            SetSlotType(food);

            SetPosition();
        }

        public void OnButtonClick()
        {

        }

        public void SetSlotType(FoodObject food)
        {
            this.food = food;
            image.sprite = food.icon;
        }
        public void SetPosition(float xPos, float yPos)
        {
            rectTransform.anchoredPosition = new Vector2(xPos, yPos);
        }

        public void SetPosition()
        {
            rectTransform.anchoredPosition = toSlotPosition();
        }

        public Vector2 toSlotPosition()
        {
            float yPos = -y * slotWidth;// + extraMaskHeight;
            float xPos = x * slotHeight + 25;

            return new Vector2(xPos, yPos);
        }

        public static Vector2 toSlotPosition(int x, int y)
        {
            float yPos = -y * slotWidth;// + extraMaskHeight;
            float xPos = x * slotHeight + 25;

            return new Vector2(xPos, yPos);
        }

        public string GetPositionString()
        {
            return "[" + x + " " + y + "]";
        }
    }
}
