using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class eventsam : MonoBehaviour
{
    private Dictionary<string, object> levelstart = new Dictionary<string, object>() { }
         ;
    public Dictionary<string, object> levelfinish = new Dictionary<string, object>() { }
    ;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        levelstart.Add("level_number", SceneManager.GetActiveScene().buildIndex + 1);
        levelstart.Add("level_name", "pyramids");
        levelstart.Add("level_count", SceneManager.GetActiveScene().buildIndex + 1);
        levelstart.Add("level_diff", "easy");
        levelstart.Add("level_loop", 1);
        levelstart.Add("level_random", 0);
        levelstart.Add("level_type", "normal");
        levelstart.Add("game_mode", "normal");

        levelfinish.Add("level_number", SceneManager.GetActiveScene().buildIndex + 1);
        levelfinish.Add("level_name", "pyramids");
        levelfinish.Add("level_count", SceneManager.GetActiveScene().buildIndex + 1);
        levelfinish.Add("level_diff", "easy");
        levelfinish.Add("level_loop", 1);
        levelfinish.Add("level_random", 0);
        levelfinish.Add("level_type", "normal");
        levelfinish.Add("game_mode", "normal");
        levelfinish.Add("result", "win");
        levelfinish.Add("time", 1);
        levelfinish.Add("progress", SceneManager.GetActiveScene().buildIndex);
        levelfinish.Add("continue", 0);

        AppMetrica.Instance.ReportEvent("level_start", levelstart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
