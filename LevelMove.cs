using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public string sceneName;
    public Vector2 newPlayerPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("switch scene to " + sceneName);
            SaveAndSwitchScene(sceneName, newPlayerPosition);
        }
    }
    public void SaveAndSwitchScene(string sceneName, Vector2 newPosition)
    {
        // Сохраняем текущую игру и новую позицию игрока
        GameData gameData = DataPersistenceManager.instance.GetCurrentGameData();
        if (gameData != null)
        {
            gameData.newPlayerPosition = newPosition;
            gameData.useNewPlayerPosition = true;
            DataPersistenceManager.instance.SaveGame();
            Debug.Log("Saved new player position: " + newPosition);
        }
        SceneManager.LoadScene(sceneName);
    }

}
