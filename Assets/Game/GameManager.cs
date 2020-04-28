using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Game.Board;
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public GameDatabaseManager databaseManager;

    public BoardManager boardManager;
    public FoodManager foodManager;
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && this != _instance)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Initialize()
    {
        foodManager.Initialize();
    }
}
