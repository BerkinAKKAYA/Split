using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] float splitSpeed = 20;
    [SerializeField] TMPro.TextMeshProUGUI playTimeText = null;
    [SerializeField] TMPro.TextMeshProUGUI highScoreText = null;

    private Transform Left, Right;
    private bool splitting;
    
    [HideInInspector] public float currentX;
    
    public float playTime;
    static Camera cam;

    #region Intouchable (property)
    private bool intouchable = false;
    public bool Intouchable {
        get => intouchable;
        set {
            intouchable = value;
            SetOpacity(intouchable ? .1f : 1);
        }
    }
    #endregion

    #region Awake
    void Awake()
    {
        Left  = transform.Find("Left");
        Right = transform.Find("Right");

        UpdateHighScoreText();
        SetOpacity(1);
        
        Inputs.OnTouch     += Split;
        Inputs.OnHoldStart += Split;
        Inputs.OnHoldEnd   += Split;

        cam = Camera.main;
    }
    #endregion

    #region Update
    void Update()
    {
        playTime += Time.deltaTime;
        playTimeText.text = (playTime * 10).ToString("f0");
    }
    #endregion

    #region Move To
    void MoveTo(float x)
    {
        if(!splitting)
        {
            StartCoroutine(IMoveTo(Left,  Vector2.left  * x));
            StartCoroutine(IMoveTo(Right, Vector2.right * x));
            currentX = x;
        }
    }

    IEnumerator IMoveTo(Transform obj, Vector3 target)
    {
        splitting = true;

        while(obj.localPosition != target)
        {
            obj.localPosition = Vector2.MoveTowards(
                obj.localPosition,
                target,
                splitSpeed * Time.deltaTime
            );

            yield return new WaitForEndOfFrame();
        }

        splitting = false;
    }
    #endregion

    #region Split
    void Split()
    {
        float vh = cam.ScreenToViewportPoint(Input.mousePosition).y;

        if (vh > 0.8f)
            return;

        MoveTo(currentX == 5 ? 0 : 5);
    }
    #endregion

    #region On Trigger Enter
    void OnTriggerEnter2D (Collider2D col)
    {
        if(!intouchable)
        {
            UpdateHighScoreText();
            FindObjectOfType<MenuSystem>().GameOver();
        }
    }
    #endregion

    #region Update High Score Text
    void UpdateHighScoreText()
    {
        HighScore = (HighScore < playTime) ? playTime : HighScore;
    }

    float HighScore {
        get => PlayerPrefs.GetFloat("HighScore", 0);
        set
        {
            string prefix = "<b>High Score:</b> <size='14'>";
            PlayerPrefs.SetFloat("HighScore", value);
            highScoreText.text = prefix + (10*HighScore).ToString("f0");
        }
    }
    #endregion

    #region Set Opacity
    void SetOpacity(float opacity)
    {
        SpriteRenderer left  =  Left.GetComponent<SpriteRenderer>();
        SpriteRenderer right = Right.GetComponent<SpriteRenderer>();

        left.color = right.color = new Color(
            left.color.r,
            left.color.g,
            left.color.b,
            opacity
        );
    }
    #endregion
}