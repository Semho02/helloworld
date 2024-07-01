using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public string menuSceneName = "MainMenu";

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        LoadMainMenu();
    //        Cursor.visible = true;
    //    }
    //}

    //public void LoadMainMenu()
    //{
    //    SceneManager.LoadScene(menuSceneName);
    //}
    public string sceneName;
    public GameObject pauseMenu;
    public bool isPaused = false;
    public GameObject inventoryMenu;
    public GameObject equipmentMenu;
    public AudioSource gameAudioSource;
    public AudioClip pauseAudioClip;
    public AudioClip gameAudioClip;
    private InventoryManager inventoryManager;
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inventoryMenu.activeSelf || equipmentMenu.activeSelf)
            {
                inventoryManager.isMenuOpen = false;
                CloseMenu();
            }
            else if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GoToScene(sceneName);
        //}
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // ������������� �����
        isPaused = true;
        pauseMenu.SetActive(true); // ���������� ���� �����
        Cursor.visible = true;
        inventoryManager.isMenuOpen=true;
        gameAudioSource.clip = pauseAudioClip;
        gameAudioSource.Play();
        Debug.Log("Game paused");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // ������������ �����
        isPaused = false;
        pauseMenu.SetActive(false); // �������� ���� �����
        inventoryManager.isMenuOpen = false;
        if (!inventoryMenu.activeSelf && !equipmentMenu.activeSelf) // ���������, ��� �� �������� ������ ����
        {
            Cursor.visible = false; // �������� ������ ������ ���� ��� �������� ����
        }
        gameAudioSource.clip = gameAudioClip;
        gameAudioSource.Play();
        Debug.Log("Game resumed");
    }

    public void CloseMenu()
    {
        if (inventoryMenu.activeSelf)
        {
            // ��������� ���� ������
            inventoryMenu.SetActive(false);
        }
        else if (equipmentMenu.activeSelf)
        {
            // ��������� ���� ����������
            equipmentMenu.SetActive(false);

            Debug.Log("Equipment menu closed");
        }
        if (isPaused)
        {
            Time.timeScale = 1f; // ������������ �����
            isPaused = false;
            pauseMenu.SetActive(false); // �������� ���� �����
            Debug.Log("Game resumed after closing equipment menu");
        }

        Debug.Log("isPaused in GameManager: " + isPaused);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("App has quit");
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
