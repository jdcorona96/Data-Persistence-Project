using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static string SAVEFILE;

    public static DataManager Instance;

    public string currentName = "";
    public string highscoreName = "Jeff";
    public int highScore = 0;


    private void Awake() {

        SAVEFILE = Application.persistentDataPath +  "/savefile.json";

        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScore();
    }

    public void SaveScore() {

        SaveData data = new SaveData();
        data.highScore = highScore;
        data.highScoreName = highscoreName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SAVEFILE, json);
        Debug.Log("High score recorded: " + data.highScore + "\nname: " + data.highScoreName);
    }

    public void LoadScore() {

        string path = SAVEFILE;

        if(File.Exists(path)) {

            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            highscoreName = data.highScoreName;
        }
    }

    public void DeleteScore() {

        SaveData data = new SaveData();
        data.highScore = 0;
        data.highScoreName = "n/a";

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SAVEFILE, json);

        highScore = 0;
        highscoreName = "n/a";
    }

    [System.Serializable]
    class SaveData {

        public int highScore;
        public string highScoreName;
    }
}
