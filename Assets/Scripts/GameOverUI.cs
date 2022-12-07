using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace TAR
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text t;
        
        public void SetText(int gameOverPlayer)
        {
            if (gameOverPlayer == -1)
                t.text = "Game Over";
            else 
                t.text = $"Player {2-(1*gameOverPlayer)} win!";            
        }
        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
