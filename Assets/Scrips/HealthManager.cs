using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class HealthManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static HealthManager Instance;

    public int health = 10;
    public TextMeshProUGUI healthTxt;
    private void Awake()
    {
        Instance = this;
        UpdateHealth(0);
    }
    public void UpdateHealth(int changeAmount)
    {
        health += changeAmount;
        healthTxt.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
