using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // 游戏结束
    public void GameOver()
    {
        Debug.Log("Game Over");

        gameOverPanel.SetActive(true);

        // 暂停游戏
        Time.timeScale = 0f;
    }

    //  重新开始
    public void RestartGame()
    {
        Time.timeScale = 1f; // 恢复时间

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
