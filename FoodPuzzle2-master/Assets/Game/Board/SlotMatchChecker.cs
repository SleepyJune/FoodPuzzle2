using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Game.Board
{
    public class SlotMatchChecker : MonoBehaviour
    {
        BoardManager boardManager;
        BoardGameManager boardGameManager;
        RecipeController recipeController;
        void Start()
        {
            boardManager = GameManager.instance.boardManager;
            boardGameManager = GameManager.instance.boardGameManager;
            recipeController = boardGameManager.recipeController;
        }

        public void CheckAndDestroy(Slot slot)
        {
            var matches = CheckMatch(slot);

            if(matches.Count > 1)
            {
                foreach(var match in matches)
                {
                    boardManager.DestroySlot(match);
                    recipeController.AddIngredient(match.food, 1);
                }
            }
        }

        public List<Slot> CheckMatch(Slot slot)
        {
            int matches = 0;

            HashSet<Slot> openSet = new HashSet<Slot>();
            HashSet<Slot> closedSet = new HashSet<Slot>();

            FoodObject food = slot.food;

            List<Slot> matchedSlots = new List<Slot>();
            openSet.Add(slot);
            closedSet.Add(slot);

            while (openSet.Count > 0)
            {
                var current = openSet.FirstOrDefault();
                openSet.Remove(current);                

                matchedSlots.Add(current);
                matches += 1;

                foreach (var neighbour in current.neighbours)
                {
                    if (!closedSet.Contains(neighbour))
                    {
                        closedSet.Add(neighbour);

                        if(neighbour.isInteractable && neighbour.food.id == food.id)
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return matchedSlots;
        }

    }
}
