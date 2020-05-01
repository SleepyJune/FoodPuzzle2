using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace Assets.Game.Board
{
    public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static float slotWidth;
        public static float slotHeight;

        public Image image;
        public CanvasGroup canvasGroup;
        public Animator anim;

        public Text text;
        public Image textPanel;

        public HashSet<Slot> neighbours = new HashSet<Slot>();

        public Slot prevSlot;
        public Slot nextSlot;

        public int x;
        public int y;

        BoardManager boardManager;
        RectTransform rectTransform;

        [NonSerialized]
        public FoodObject food;

        public bool isDead;
        public bool isFalling;

        float acceleration = 9.8f;

        const float startFallingSpeed = 1000f;
        float fallingSpeed = 0f;

        Vector2 targetPosition;

        public bool isInteractable
        {
            get { return !isDead && !isFalling; }
        }

        void Start()
        {
            boardManager = GameManager.instance.boardManager;

            textPanel.gameObject.SetActive(false);            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            text.text = this.ToString();
            textPanel.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //text.text = "";
            textPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            if (isFalling)
            {                
                var currentPos = rectTransform.anchoredPosition;               

                if (currentPos.y - fallingSpeed * Time.deltaTime <= targetPosition.y) //reached target pos
                {
                    SetPosition();
                    isFalling = false;

                    boardManager.SlotSetPosition(this);
                    //set neighbour
                }
                else
                {
                    rectTransform.anchoredPosition = new Vector2(currentPos.x, currentPos.y - fallingSpeed * Time.deltaTime);
                    fallingSpeed += acceleration * Time.deltaTime;
                }
            }
        }

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
            GameManager.instance.boardGameManager.OnSlotPressed(this);
        }

        public void Kill()
        {
            isDead = true;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            anim.SetTrigger("death");

            DeleteNeighbourConnection();
        }

        void DeleteNeighbourConnection()
        {
            //destroy neighbour connections
            foreach (var neighbour in neighbours)
            {
                if (neighbour != null)
                {
                    neighbour.neighbours.Remove(this);
                }
            }
        }

        public void Destroy()
        {
            boardManager.SpawnNewFallingSlot(x);
            DestroyLinkedSlots();

            Destroy(gameObject);
        }

        void DestroyLinkedSlots()
        {
            if (nextSlot != null)
            {
                if (prevSlot != null)
                {
                    nextSlot.prevSlot = prevSlot;
                    prevSlot.nextSlot = nextSlot;                    
                }
                else
                {
                    nextSlot.prevSlot = null;
                }
            }
            else
            {
                if (prevSlot != null)
                {
                    prevSlot.nextSlot = null;                    
                }
            }

            /*if(prevSlot != null)
            {
                prevSlot.SetFalling(x, y);
            }*/

            //loop through all above slots
            Slot current = prevSlot;
            Slot highestSlot = current;

            int numLoops = 0;
            while(current != null)
            {
                current.SetFalling(x, y - numLoops);

                highestSlot = current;
                current = current.prevSlot;
                numLoops += 1;

                if(numLoops > 50)
                {
                    Debug.LogError("inf loop");
                    break;
                }
            }

            boardManager.TrySetHighestSlot(highestSlot);
        }

        public void SetFalling(int x, int y)
        {
            if (!isFalling)
            {
                fallingSpeed = startFallingSpeed;
                isFalling = true;

                boardManager.SlotRemovePosition(this);
                DeleteNeighbourConnection();
            }

            this.x = x;
            this.y = y;            

            targetPosition = toSlotPosition();
        }

        public void SetSlotType(FoodObject food)
        {
            this.food = food;
            image.sprite = food.icon;
        }

        public void SetSlotAbove(Slot slot)
        {
            var pos = slot.rectTransform.anchoredPosition;
            rectTransform.anchoredPosition = new Vector2(pos.x, pos.y + slotHeight);
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
            float yPos = -y * slotHeight;// + extraMaskHeight;
            float xPos = x * slotWidth + 25;

            return new Vector2(xPos, yPos);
        }

        public static Vector2 toSlotPosition(int x, int y)
        {
            float yPos = -y * slotWidth;// + extraMaskHeight;
            float xPos = x * slotHeight + 25;

            return new Vector2(xPos, yPos);
        }

        public override string ToString()
        {
            return "[" + x + " " + y + "]";
        }
    }
}
