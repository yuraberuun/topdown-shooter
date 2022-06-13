using UnityEngine;

public enum GameState {None, InMenu, Play, OnPause, UpgradeScreen, OpenChest, GameOver}

public class GameStatus : SingletonComponent<GameStatus>
{
    public GameState status;
}
