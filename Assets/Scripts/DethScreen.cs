using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DethScreen : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button restart;
    [SerializeField] private UnityEngine.UI.Button quit;
    
    void Start()
    {
        restart.onClick.AddListener(() =>Restart());
        quit.onClick.AddListener(() =>Quit());
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void Quit()
    {
        Application.Quit();
    }
}
