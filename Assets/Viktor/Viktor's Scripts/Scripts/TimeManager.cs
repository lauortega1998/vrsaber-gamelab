using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    public float slowdownLenght = 2f;
    private bool isSlowingDown = false;
    private void Update()
    {
        if (isSlowingDown)
        {
            Time.timeScale += (1f / slowdownLenght) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, slowdownFactor, 1f);
            Time.fixedDeltaTime = Time.timeScale * 0.02f;

            if (Time.timeScale >= 0.99f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                isSlowingDown = false;
            }
        }
    }

    public void SlowDown()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        isSlowingDown = true;
    }

}
