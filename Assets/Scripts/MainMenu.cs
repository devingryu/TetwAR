using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TAR
{
    public class MainMenu : MonoBehaviour
    {
        public void SinglePlay()
        {
            SceneManager.LoadScene("ARScene");
        }

        public void MultyPlay()
        {
            SceneManager.LoadScene("ARSceneMulti");
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
