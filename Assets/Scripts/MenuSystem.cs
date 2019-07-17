using UnityEngine;
using System.Collections;

public class MenuSystem : MonoBehaviour
{
    #region Menus
    private Menus openMenu = 0;
    public Menus OpenMenu {
        get => openMenu;
        set {
            openMenu = value;
            Time.timeScale = (value == Menus.None) ? 1 : 0;
            isPlaying = (value == Menus.None);

            switch (value)
            {
                case Menus.None:
                    mainMenu.SetActive(false);
                    gameOverMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    break;
                case Menus.Main:
                    mainMenu.SetActive(true);
                    gameOverMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    break;
                case Menus.GameOver:
                    mainMenu.SetActive(false);
                    gameOverMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                    gameOverScoreIndicator.text = (player.playTime*10).ToString("f0");
                    break;
                case Menus.Paused:
                    mainMenu.SetActive(false);
                    gameOverMenu.SetActive(false);
                    pauseMenu.SetActive(true);
                    break;
                default: break;
            }
        }
    }
    public enum Menus {None, Main, GameOver, Paused}
    #endregion

    [SerializeField, Range(0, 100)] float adShowChance = 20;

    [Header("References")]
    [SerializeField] GameObject mainMenu = null;
    [SerializeField] GameObject gameOverMenu = null;
    [SerializeField] GameObject pauseMenu = null;
    [SerializeField] TMPro.TextMeshProUGUI gameOverScoreIndicator = null;

    private Camera cam;
    private Player player;
    public static bool isPlaying;

    #region Awake & Start
    void Awake()
    {
        player = FindObjectOfType<Player>();
        Inputs.OnTouch += OnTouch;
        cam = Camera.main;
    }

    void Start() => OpenMenu = Menus.Main;
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (OpenMenu != Menus.Main)
                OpenMenu  = Menus.Main;
            else
                Application.Quit();
        }
    }
    #endregion

    #region On Touch
    void OnTouch()
    {
        if (OpenMenu == Menus.None)
            return;

        if (OpenMenu == Menus.GameOver)
        {
            var mouseHeight = cam.ScreenToViewportPoint(Input.mousePosition).y;

            if (mouseHeight > 0.4f) Restart();
            else                    return;
        }

        if (OpenMenu == Menus.Main)
        {
            ShowAd();
            Restart();
        }

        OpenMenu =  Menus.None;
    }
    #endregion

    #region Show Ad
    void ShowAd()
    {
        if (Random.value < adShowChance/100f)
            FindObjectOfType<AdManager>().ShowAd();
    }
    #endregion

    #region Buttons
    public void Restart()
    {
        Obstacles.Reset();
        player.playTime = 0;
    }
    
    public void Continue()
    {
        OpenMenu = Menus.None;
        Time.timeScale = .1f;
        StartCoroutine("StartTimeAfter", .5f);
    }

    public void Pause()    => OpenMenu = Menus.Paused;
    public void GameOver() => OpenMenu = Menus.GameOver;
    public void MainMenu() => OpenMenu = Menus.Main;
    #endregion

    #region Start Time After
    IEnumerator StartTimeAfter(float time)
    {
        StartCoroutine("PlayerIntouchableFor", 2f);
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1;
    }
    #endregion

    #region Player Intouchable For
    IEnumerator PlayerIntouchableFor(float time)
    {
        player.Intouchable = true;
        yield return new WaitForSeconds(time);
        player.Intouchable = false;
    }
    #endregion
}