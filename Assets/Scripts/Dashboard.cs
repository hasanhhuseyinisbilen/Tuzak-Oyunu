using UnityEngine;
using UnityEngine.SceneManagement;

public class DashBoard : MonoBehaviour
{
   
    public void level()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }


    void Update()
    {
        
    }
}
