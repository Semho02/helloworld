using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;

    [SerializeField] private bool initializeDataIfNull = false;

    [SerializeField] private bool overrideSelectedProfileId = false;

    [SerializeField] private string testSelectedProfileId = "test";

    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 300f;

    private GameData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private Coroutine autoSaveCoroutine;

    private bool isFirstLoad = true;

    public static DataPersistenceManager instance{get; private set;}

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Found more than one DataPManager in the scene. Destroying the new one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("DataPersistence is currently disabled!");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
        if (overrideSelectedProfileId)
        {
            this.selectedProfileId = testSelectedProfileId;
            Debug.LogWarning("Override selected profile id with test id: " + testSelectedProfileId);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

        if (scene.name != "Main Menu") // Игнорируем сохранение позиции при загрузке главного меню
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && gameData != null)
            {
                if (gameData.useNewPlayerPosition)
                {
                    player.transform.position = gameData.newPlayerPosition;
                    gameData.useNewPlayerPosition = false;
                    Debug.Log("Setting player position to new position: " + gameData.newPlayerPosition);
                }
                else
                {
                    player.transform.position = gameData.playerPosition;
                    Debug.Log("Setting player position to saved position: " + gameData.playerPosition);
                }
            }
        }

        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        this.gameData = dataHandler.Load(selectedProfileId);

        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }

        if(this.gameData == null)
        {
            Debug.Log("No data was found. A new Game needs to be started before data can be loaded");
            return;
        }
        if (isFirstLoad && !gameData.useNewPlayerPosition)
        {
            gameData.playerPosition = Vector2.zero; // Сброс позиции игрока, если не используется новая позиция
        }
        isFirstLoad = false;

        //if (gameData != null && SceneManager.GetActiveScene().name != "Main Menu")
        //{
        //    GameObject player = GameObject.FindGameObjectWithTag("Player");
        //    if (player != null)
        //    {
        //        player.transform.position = gameData.playerPosition;
        //    }
        //}

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            DamageableCharacter damageableCharacter = player.GetComponent<DamageableCharacter>();
            if (damageableCharacter != null)
            {
                damageableCharacter.currentHealth = gameData.currentHealth;
                damageableCharacter.healthBar.SetHealth(damageableCharacter.currentHealth);
            }
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found. A new game needs to be started before data can be saved.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        GameObject playerhp = GameObject.FindGameObjectWithTag("Player");
        if (playerhp != null)
        {
            DamageableCharacter damageableCharacter = playerhp.GetComponent<DamageableCharacter>();
            if (damageableCharacter != null)
            {
                gameData.currentHealth = damageableCharacter.currentHealth;
            }
        }

        // Сохранение текущей сцены
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.name.Equals("Main Menu"))
        {
            gameData.currentSceneName = scene.name;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                gameData.playerPosition = player.transform.position;
                Debug.Log("Saving player position: " + gameData.playerPosition);
            }
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public GameData GetCurrentGameData()
    {
        return gameData;
    }


    public bool HasGameData()
    {
        return gameData!= null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("AutoSavedGame");
        }
    }

    public string GetSavedSceneName()
    {
        if (gameData == null)
        {
            Debug.LogError("Tried to get scene name but data was null.");
            return null;
        }
        return gameData.currentSceneName;
    }
}
