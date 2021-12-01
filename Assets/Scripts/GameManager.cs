using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// game manager
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System;

// disappearing tex
using UnityEngine.UI;

public enum GameState
{ 
    TITLEMENU,
    GAMEPLAY,
    WIN,
    LOSE,
    PAUSE,
    OPTIONS,
    CREDITS
}

public class GameManager : MonoBehaviour
{
    public static GameManager manager; //singleton inst

    public UIManager uiManager;
    public Text saveText;
    public Text loadText;
    
    public float health;
    public float eXP;
    public int score;
    public float shield;
    public float mana;
    public int life;

    private GameState gameState;
    private GameState savedScreenState;

    private bool fadeSave;
    private bool fadeLoad;
    private float textFadeWaitTime = 1.5f;

    void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(this.gameObject);
            manager = this; // setting this object to be THE singleton
        }
        else if (manager != this) // already exist's? DESTROY
        {
            Destroy(this.gameObject);
        }

        // make fading text invisible at start
        saveText.CrossFadeAlpha(0, .1f, true);
        loadText.CrossFadeAlpha(0, .1f, true);

        gameState = GameState.TITLEMENU;
    }

    void Update() 
    {
        Controls();

        FadeText();

        switch (gameState)
        {
            case GameState.TITLEMENU:
                {
                    if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0))
                    {
                        SceneManager.LoadScene(0);
                        SaveScreenState();
                    }
                    uiManager.LoadTitleMenu();
                    return; 
                }
            case GameState.GAMEPLAY:
                {
                    if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(1))
                    {
                        SceneManager.LoadScene(1);
                        SaveScreenState();
                    }
                    uiManager.LoadGameplay();
                    return;
                }
            case GameState.WIN:
                {
                    uiManager.LoadWinScreen();
                    return;
                }
            case GameState.LOSE:
                {
                    uiManager.LoadLoseScreen();
                    return;
                }
            case GameState.PAUSE:
                {
                    uiManager.LoadPauseScreen();
                    return;
                }
            case GameState.OPTIONS:
                {
                    uiManager.LoadOptions();
                    return;
                }
            case GameState.CREDITS:
                {
                    uiManager.LoadCredits();
                    return;
                }
        }
    }

    public void ChangeState(GameState targetState)
    {
        gameState = targetState;
    }

    public void SaveScreenState()
    {
        savedScreenState = gameState;
    }

    public void ReturnToPreviousState()
    {
        gameState = savedScreenState;
    }

    private void Controls() // Global Controls
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        if (gameState != GameState.TITLEMENU)
        { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameState = GameState.PAUSE;
            }
        }
    }
    
    public void Save() // canned file save method
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedInfo.dat");

        SaveInfo savedInfo = new SaveInfo();
        savedInfo.scene = SceneManager.GetActiveScene().buildIndex; 
        savedInfo.health = health;
        savedInfo.eXP = eXP;
        savedInfo.score = score;
        savedInfo.shield = shield;
        savedInfo.mana = mana;
        savedInfo.life = life;

        saveText.CrossFadeAlpha(1, .1f, true);
        StartCoroutine(WaitToFadeText("save"));

        bf.Serialize(file, savedInfo);
        file.Close();
    }
    
    public void Load() // canned file load method
    {
        if (File.Exists(Application.persistentDataPath + "/savedInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedInfo.dat", FileMode.Open);
            SaveInfo loadedInfo = (SaveInfo)bf.Deserialize(file);
            file.Close();

            SceneManager.LoadScene(loadedInfo.scene);
            health = loadedInfo.health;
            eXP = loadedInfo.eXP;
            score = loadedInfo.score;
            shield = loadedInfo.shield;
            mana = loadedInfo.mana;
            life = loadedInfo.life;

            loadText.CrossFadeAlpha(1, .1f, true);
            StartCoroutine(WaitToFadeText("load"));
        }
    }
    
    public void NewStart()
    {
        health = 100;
        eXP = 0;
        score = 0;
        shield = 150;
        mana = 420;
        life = 3;
        SceneManager.LoadScene(1);
    }
    
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void FadeText()
    {
        if (fadeSave)
        {
            saveText.CrossFadeAlpha(0, 3, false); fadeSave = false;
        }
        if (fadeLoad)
        {
            loadText.CrossFadeAlpha(0, 3, false); fadeLoad = false;
        }
    }

    IEnumerator WaitToFadeText(string fade)
    {
        yield return new WaitForSeconds(textFadeWaitTime);
        if (fade == "save")
            fadeSave = true;
        else if (fade == "load")
            fadeLoad = true;
    }
}

[Serializable]
class SaveInfo
{
    public int scene;
    public float health;
    public float eXP;
    public int score;
    public float shield;
    public float mana;
    public int life;
}

