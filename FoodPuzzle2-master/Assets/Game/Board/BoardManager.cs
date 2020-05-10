using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Game.Board
{
    public class BoardManager : MonoBehaviour
    {
        public const int maxHeight = 9;
        public const int maxWidth = 7;

        Dictionary<Vector2Int, Slot> slots;

        public Transform slotParent;
        public Slot slotPrefab;

        [NonSerialized]
        public float extraColWidth = 25;
        [NonSerialized]
        public float extraRowHeight = 25;

        bool boardInitialized = false;

        int[] spawnQueue = new int[maxWidth];

        Slot[] highestSlot = new Slot[maxWidth];

        void Start()
        {
            slots = new Dictionary<Vector2Int, Slot>();

            InitializeBoard();
        }
        void Update()
        {

        }
        void InitializeBoard()
        {
            slotParent.DeleteChildren();

            var rectTransform = slotPrefab.GetComponent<RectTransform>();

            Slot.slotWidth = rectTransform.rect.width + extraRowHeight;
            Slot.slotHeight = rectTransform.rect.height + extraColWidth;

            //Slot.extraMaskHeight = extraMaskHeight;

            for (int y = 0; y < maxHeight; y++)
            {
                for (int x = 0; x < maxWidth; x++)
                {
                    SpawnSlot(x, y);
                }
            }

            InitializeNeighbours();

            boardInitialized = true;
        }

        void InitializeNeighbours()
        {
            foreach(var slot in slots.Values)
            {
                AddNeighbours(slot);
            }
        }

        public void AddNeighbours(Slot slot)
        {
            var x = slot.x;
            var y = slot.y;

            slot.neighbours = new HashSet<Slot>();

            AddNeighbour(slot, x, y - 1); //up
            AddNeighbour(slot, x, y + 1); //down
            AddNeighbour(slot, x - 1, y); //left
            AddNeighbour(slot, x + 1, y); //right

            var previous = GetSlot(x, y - 1);
            var next = GetSlot(x, y + 1);

            slot.prevSlot = previous;
            slot.nextSlot = next;

            if(previous != null)
            {
                previous.nextSlot = slot;
            }

            if(next != null)
            {
                next.prevSlot = slot;
            }
        }

        void ResetNeighbours(Slot slot)
        {
            foreach (var neighbour in slot.neighbours)
            {
                if (neighbour != null)
                {
                    neighbour.neighbours.Remove(slot);
                }
            }

            slot.neighbours = new HashSet<Slot>();

            AddNeighbours(slot);
        }

        void AddNeighbour(Slot slot, int x, int y)
        {
            var neighbour = GetSlot(x, y);

            if (neighbour != null && !slot.neighbours.Contains(neighbour))
            {
                slot.neighbours.Add(neighbour);
                neighbour.neighbours.Add(slot);
            }
        }

        public void DestroySlot(Slot slot)
        {
            SlotRemovePosition(slot);
            slot.Kill();
        }

        public void SlotRemovePosition(Slot slot)
        {
            slots.Remove(new Vector2Int(slot.x, slot.y));
        }
        public void SlotSetPosition(Slot slot)
        {
            var position = new Vector2Int(slot.x, slot.y);
            if (slots.ContainsKey(position))
            {
                Debug.LogError("Same position: " + slot);
                Debug.LogError(slots[position]);
            }
            else
            {
                slots.Add(position, slot);
                AddNeighbours(slot);
            }
        }

        Slot SpawnSlot(int x, int y)
        {
            var newSlot = Instantiate(slotPrefab, slotParent);
            var randomFood = GameManager.instance.boardGameManager.GenerateRandomFood();

            newSlot.Initialize(randomFood, x, y);

            slots.Add(new Vector2Int(x, y), newSlot);

            TrySetHighestSlot(newSlot);

            return newSlot;
        }

        public void SpawnNewFallingSlot(int x)
        {
            var highest = highestSlot[x];
            Slot newSlot;

            if (highest != null)
            {
                var height = highest.y - 1;
                
                if(height < 0)
                {
                    newSlot = SpawnSlot(x, height);
                }
                else
                {
                    newSlot = SpawnSlot(x, -1);
                }

                newSlot.SetSlotAbove(highest);

                newSlot.SetFalling(x, height);
                highest.prevSlot = newSlot;
            }
            else
            {
                newSlot = SpawnSlot(x, -1);
                newSlot.SetFalling(x, maxHeight - 1);
            }            
        }

        public void TrySetHighestSlot(Slot newSlot)
        {
            if (newSlot == null) return;

            var x = newSlot.x;
            var y = newSlot.y;

            if (highestSlot[x] != null)
            {
                if (highestSlot[x].y >= y)
                {
                    highestSlot[x] = newSlot;                    
                }
            }
            else
            {
                highestSlot[x] = newSlot;
            }
        }

        public Slot GetSlot(int x, int y)
        {
            Slot slot;
            if(slots.TryGetValue(new Vector2Int(x, y), out slot))
            {
                return slot;
            }

            return null;
        }
    }
}
