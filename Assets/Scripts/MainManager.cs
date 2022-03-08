using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text highScoreText;
    public Text ScoreText;
    public GameObject GameOverText;
    
    private int m_Points;
    private string m_Name;

    private bool m_Started = false;
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("calling menuManager Start");
        Debug.Log(DataManager.Instance.highScore);
        Debug.Log(DataManager.Instance.highscoreName);
        m_Name = DataManager.Instance.currentName;
        Debug.Log("game name: " + m_Name);

        highScoreText.text = "Best Score: " + DataManager.Instance.highscoreName
            + " = " + DataManager.Instance.highScore;

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
            return;
        }

        if(m_Points > DataManager.Instance.highScore) {

            DataManager.Instance.highscoreName = m_Name;
            ScoreText.text = "NEW HIGH SCORE!! " + DataManager.Instance.currentName
                + " = " + m_Points;
        }

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        
        }
        else if (m_GameOver)
        {

            if(m_Points > DataManager.Instance.highScore) {

                DataManager.Instance.highScore = m_Points;
                DataManager.Instance.highscoreName = m_Name;
                DataManager.Instance.SaveScore();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
