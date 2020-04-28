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
        Dictionary<Vector2Int, Slot> slots;

        public Transform slotParent;
        public Slot slotPrefab;

        public float extraColWidth = 50;
        public float extraRowHeight = 50;

        bool boardInitialized = false;

        void Start()
        {
            slots = new Dictionary<Vector2Int, Slot>();

            InitializeBoard();
        }
        void InitializeBoard()
        {
            slotParent.DeleteChildren();

            var rectTransform = slotPrefab.GetComponent<RectTransform>();

            Slot.slotWidth = rectTransform.rect.width + extraRowHeight;
            Slot.slotHeight = rectTransform.rect.height + extraColWidth;

            //Slot.extraMaskHeight = extraMaskHeight;

            int maxHeight = 7;
            int maxWidth = 7;

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

        void AddNeighbours(Slot slot)
        {
            var x = slot.x;
            var y = slot.y;

            AddNeighbour(slot, x, y - 1); //up
            AddNeighbour(slot, x, y + 1); //down
            AddNeighbour(slot, x - 1, y); //left
            AddNeighbour(slot, x + 1, y); //right
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
        void SpawnSlot(int x, int y)
        {
            var newSlot = Instantiate(slotPrefab, slotParent);
            newSlot.Initialize(x, y);

            slots.Add(new Vector2Int(x, y), newSlot);
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
