using UnityEngine;

public class Inputs : MonoBehaviour
{
    #region Touch State
    public enum TouchState {
        Touched,
        HoldStart,
        HoldEnded,
        NotTouching
    };

    private static TouchState state = TouchState.NotTouching;
    public static TouchState State {
        get => state;
        set {
            state = value;

            switch (value)
            {
                case TouchState.Touched:
                    OnTouch();
                    break;
                case TouchState.HoldStart:
                    OnHoldStart();
                    break;
                case TouchState.HoldEnded:
                    OnHoldEnd();
                    break;
                case TouchState.NotTouching:
                    break;
            }
        }
    }
    #endregion

    [SerializeField] float minHoldTime = .1f;

    public delegate void TouchEvent();
    public static event TouchEvent OnTouch;
    public static event TouchEvent OnHoldStart;
    public static event TouchEvent OnHoldEnd;

    private float pressTime;
    private bool isEditor;

    #region Awake
    void Awake()
    {
        isEditor = (Application.platform == RuntimePlatform.WindowsEditor);
    }
    #endregion

    #region Update
    void Update()
    {
        if (state == TouchState.Touched || state == TouchState.HoldEnded)
            state =  TouchState.NotTouching;

        if (isEditor) UpdateStateEditor();
        else          UpdateState();
    }
    #endregion

    #region Update State (Editor)
    void UpdateStateEditor()
    {
        if (Input.GetMouseButtonDown(0))
            pressTime = Time.time;

        float holdTime = (Time.time - pressTime);

        if (Input.GetMouseButton(0))
        {
            if (holdTime >= minHoldTime && State == TouchState.NotTouching)
                State = TouchState.HoldStart;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (holdTime <= minHoldTime)
            {
                OnTouch();
                State = TouchState.Touched;
            }
            else
            {
                OnHoldEnd();
                State = TouchState.HoldEnded;
            }
        }
    }
    #endregion

    #region Update State
    void UpdateState()
    {
        var phase = Input.GetTouch(0).phase;

        if (phase == TouchPhase.Began)
            pressTime = Time.time;

        float holdTime = (Time.time - pressTime);

        if (phase == TouchPhase.Stationary || phase == TouchPhase.Moved)
        {
            if (holdTime >= minHoldTime && State == TouchState.NotTouching)
                State = TouchState.HoldStart;
        }

        if (phase == TouchPhase.Ended)
        {
            if (holdTime <= minHoldTime)
            {
                OnTouch();
                State = TouchState.Touched;
            }
            else
            {
                OnHoldEnd();
                State = TouchState.HoldEnded;
            }
        }
    }
    #endregion
}