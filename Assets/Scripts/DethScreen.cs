using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DethScreen : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button restart;
    [SerializeField] private UnityEngine.UI.Button quit;
    [SerializeField] private Joystick joystick;
    [SerializeField] private GraphicRaycaster graphicRaycaster;
    
    void Start()
    {
        restart.onClick.AddListener(() =>Restart());
        quit.onClick.AddListener(() =>Quit());
        joystick.gameObject.SetActive(false);
        graphicRaycaster.enabled = true;
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
