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

public class GameManager : MonoBehaviour
{
    public static GameManager manager; //singleton inst
    public GameObject sceneObject; // passed gameObject singleton (bad solution to moving the same object from scene to scene)

    private static int managers;

    public float health;
    public float eXP;
    public int score;
    public float shield;
    public float mana;
    public int life;

    public Text saveText;
    public Text loadText;
    private bool fadeSave;
    private bool fadeLoad;
    private float textFadeWaitTime = 1.5f;

    void Awake()
    {
        managers += 1; // adds to the manager count upon being instanced

        if (manager == null)
        {
            DontDestroyOnLoad(sceneObject);
            DontDestroyOnLoad(this.gameObject);
            manager = this; // setting this object to be THE singleton
        }
        else if (manager != this) // already exist's? DESTROY
        {
            Destroy(this.gameObject);
        }

        // make fading text invisible
        saveText.CrossFadeAlpha(0, .1f, true);
        loadText.CrossFadeAlpha(0, .1f, true);
    }
    private void OnDestroy()
    {
        managers -= 1; // removes from manager count upon Destruction
    }

    void Update() 
    {
        Controls();

        if (fadeSave)
        {
            saveText.CrossFadeAlpha(0, 3, false); fadeSave = false;
        }
        if (fadeLoad)
        {
            loadText.CrossFadeAlpha(0, 3, false); fadeLoad = false;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), $"Health: {health}");
        GUI.Label(new Rect(10, 40, 100, 30), $"# of Managers: {managers}");
        GUI.Label(new Rect(110, 10, 100, 30), $"EXP: {eXP}");
        GUI.Label(new Rect(220, 10, 100, 30), $"Score: {score}");
        GUI.Label(new Rect(330, 10, 100, 30), $"Shield: {shield}");
        GUI.Label(new Rect(440, 10, 100, 30), $"Mana: {mana}");
        GUI.Label(new Rect(550, 10, 100, 30), $"Life: {life}");
    }
    private void Controls() // Global Controls
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(4);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
        /*else if (Input.GetKeyDown(KeyCode.E)) /// NOT WORKING :(
        {

            if (SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1) != null)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {

            if (SceneManager.GetActiveScene().buildIndex - 1 >= 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            else
            {
                SceneManager.LoadScene(SceneManager.sceneCount);
            }
        }*/
    }
    public void Save() // canned file save method
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedInfo.dat");

        SaveInfo savedInfo = new SaveInfo();
        savedInfo.health = GameManager.manager.health;
        savedInfo.eXP = GameManager.manager.eXP;
        savedInfo.score = GameManager.manager.score;
        savedInfo.shield = GameManager.manager.shield;
        savedInfo.mana = GameManager.manager.mana;
        savedInfo.life = GameManager.manager.life;

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

            GameManager.manager.health = loadedInfo.health;
            GameManager.manager.eXP = loadedInfo.eXP;
            GameManager.manager.score = loadedInfo.score;
            GameManager.manager.shield = loadedInfo.shield;
            GameManager.manager.mana = loadedInfo.mana;
            GameManager.manager.life = loadedInfo.life;

            loadText.CrossFadeAlpha(1, .1f, true);
            StartCoroutine(WaitToFadeText("load"));
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
    public float health;
    public float eXP;
    public int score;
    public float shield;
    public float mana;
    public int life;
}
