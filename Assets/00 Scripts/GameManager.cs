using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    public PlayerBall playerBall;
    public TutorialManager tutorialManager;
    public bool isLose;
    public bool isGamePlaying = true;
    public List<GameObject> levelPrefabs;
    [SerializeField] Transform levelRoot;
    private GameObject currentLevel;
    [SerializeField] private FloatingTextManager floatingTextManager;
    [SerializeField] private BallManager ballManager;
    [SerializeField] private MeshRenderer quadMeshRenderer;
    [SerializeField] private List<Material> bgrMaterial;
    [SerializeField] private RectTransform startPlayRangeRect;

    private bool _isPause = false;

    public bool isPause { get => _isPause; private set => _isPause = value; }
    string urlData = "https://utilities-realtimedb-default-rtdb.asia-southeast1.firebasedatabase.app/.json";

    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener(EventID.Pause, SetPause);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventID.Pause, SetPause);
    }

    private void Start()
    {
        isPause = false;
        isLose = false;
        SetFPS(Constant.FPS);
        LoadLevel();
    }

    private void Update()
    {
        if (isGamePlaying) return;

        if (MouseOnElement(startPlayRangeRect))
        {
            if (Input.GetMouseButtonDown(0))
            {
                isGamePlaying = true;
                tutorialManager.EnableTutorial(false);
                playerBall.StartPlay();
            }
        }
    }

    private bool MouseOnElement(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float xMin = corners[0].x;
        float xMax = corners[2].x;

        float yMin = corners[0].y;
        float yMax = corners[2].y;

        if ((Input.mousePosition.x > xMax || Input.mousePosition.x < xMin)
            ||
            (Input.mousePosition.y > yMax || Input.mousePosition.y < yMin))
        {
            return false;
        }

        return true;
    }

    public void BallMoveLeftRight(float xPos)
    {
        playerBall.DragLeftRight(-xPos);
    }

    public async void LoadLevel()
    {
        tutorialManager.EnableTutorial(true);
        isLose = false;
        isGamePlaying = false;
        if (levelRoot.childCount > 0)
            Destroy(levelRoot.GetChild(0).gameObject);

        if (UserData.LevelNumber < levelPrefabs.Count)
            currentLevel = Instantiate<GameObject>(levelPrefabs[UserData.LevelNumber - 1], levelRoot);
        else
        {
            currentLevel = Instantiate<GameObject>(levelPrefabs[UnityEngine.Random.Range(0, levelPrefabs.Count)], levelRoot);
        }

        #region  TEST LOAD DATA

        // await UniTask.Delay(1000);
        // _ = new RequestBase(urlData)
        //     .Send((res) =>
        //     {
        //         Debug.LogWarning(res.response);
        //         JSONArray dataParse = JSONArray.Parse(res.response).AsArray;
        //         int index = dataParse[0]["ID"].AsInt;
        //         Debug.LogWarning(index);
        //         index++;
        //         Debug.LogWarning(index);
        //         Destroy(levelRoot.transform.GetChild(0).gameObject);
        //         currentLevel = Instantiate<GameObject>(levelPrefabs[index], levelRoot);
        //     });

        #endregion

        currentLevel.transform.SetParent(levelRoot);

        LevelInfo levelInfo = currentLevel.GetComponent<LevelInfo>();
        playerBall.Init(levelInfo);
        ballManager.Init(levelInfo.ballMapContainer);

        EventDispatcher.Instance.PostEvent(EventID.LoadLevel, UserData.LevelNumber);

        UIManager.instance.HidePopup();

        ChangeBackGroundColor(bgrMaterial[UnityEngine.Random.Range(0, bgrMaterial.Count)]);
    }

    private void ChangeBackGroundColor(Material material)
    {
        quadMeshRenderer.material = material;
    }

    public void EndLevel(bool isWin)
    {
        EventDispatcher.Instance.PostEvent(EventID.EndLevel, isWin);
        playerBall.StopMove();
        currentLevel = null;

        if (isWin)
        {
            AudioManager.instance.PlayAudio(AudioName.congratulation);
            Vibrator.Vibrate(Constant.STRONG_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.WIN);
            StartCoroutine(WaitBallsExplodeThenPopup());
        }
        else
        {
            Vibrator.Vibrate(Constant.WEAK_VIBRATE);
            EventDispatcher.Instance.PostEvent(EventID.ChangeCharacterState, Constant.LOSE);
            UIManager.instance.ShowPopup(isWin);
        }
    }

    private IEnumerator WaitBallsExplodeThenPopup()
    {
        yield return new WaitForSeconds(Constant.DELAY_TO_DESTROY_BALL - 1.5f);
        playerBall.DestroyBallWhenWinAndShowPopup();
    }

    public void NextLevel()
    {
        UserData.LevelNumber++;
        LoadLevel();
        AdsManager.instance.ShowInterstitialAds();
    }

    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
    }

    public void ShowFloatingText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    private void SetPause(object param = null)
    {
        isPause = (bool)param;
    }

}
