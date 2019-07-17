using UnityEngine;
using System.Collections;

public class BackgroundColors : MonoBehaviour
{
    [SerializeField] Color[] colors = null;
    [SerializeField] float colorChangeTime = 5f;

    private Camera cam;

    void Awake() => cam = Camera.main;
    void Start() => InvokeRepeating("ChangeBackground", 0, 5);

    #region Change Background
    void ChangeBackground()
    {
        StartCoroutine(
            IChangeBackground(
                colors[Random.Range(0, colors.Length)],
                colorChangeTime
            )
        );
    }

    IEnumerator IChangeBackground(Color color, float duration)
    {
        float time = 0;
        Color startColor = cam.backgroundColor;

        while(time < colorChangeTime)
        {
            float percentage = (time / colorChangeTime);

            cam.backgroundColor = Color.Lerp(
                startColor,
                color,
                percentage
            );

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        cam.backgroundColor = color;
    }
    #endregion
}