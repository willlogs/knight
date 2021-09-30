using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PT.Utils
{
    // scene 0 should be init scene, this scene manager circulates
    public class LevelManager : MonoBehaviour
    {
        public void Next()
        {
            int idx = SceneManager.GetActiveScene().buildIndex + 1;
            if(idx == SceneManager.sceneCountInBuildSettings)
            {
                idx = 1;
            }
            print("loading next : " + idx);

            SceneManager.LoadScene(idx);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
        }
    }
}