using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class QuickRestarter : MonoBehaviour
{
    void Update()
    {
        // Directly checks the current keyboard layout for the R key press
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}