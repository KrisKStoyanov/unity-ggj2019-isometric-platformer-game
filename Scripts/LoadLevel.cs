using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void LoadLevelOfName(string _level)
    {
        SceneManager.LoadScene(_level);
    }
}
