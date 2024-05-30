using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoad : MonoBehaviour
{
    [SerializeField] private GameObject nextStageMap;
    public bool isInsideTrigger;
    Player player;

    /// <summary>
    /// 유니티에서 비동기 명령 처리를 위해 필요한 클래스
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// 글자 변경용 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// 로딩 완료 표시(true면 완료, false 미완)
    /// </summary>
    bool loadingDone = false;

    /// <summary>
    /// slider의 value에 영향을 줄 값
    /// </summary>
    float loadRatio;

    /// <summary>
    /// slider의 value가 증가하는 속도(초당)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    // UI

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;

    PlayerAction inputActions;
    MapManager mapManager;

    private void Awake()
    {
        inputActions = new PlayerAction();
        mapManager = GetComponent<MapManager>();
        //Transform child = transform.GetChild(0);
        //text = child.gameObject;
        //text.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameManager.Instance.Player;

    }

    void Start()
    {
        loadingTextCoroutine = LoadingTextProgress();
        Debug.Log("NextStageMap: " + nextStageMap.name);

        StartCoroutine(loadingTextCoroutine);
        StartCoroutine(AsyncLoadScene());
    }

    private void OnEnable()
    {
        inputActions.UI.Enable();
        inputActions.UI.Click.performed += Press;
        inputActions.UI.AnyKey.performed += Press;
    }

    private void OnDisable()
    {
        if (inputActions != null)
        {
            inputActions.UI.AnyKey.performed -= Press;
            inputActions.UI.Click.performed -= Press;
            inputActions.UI.Disable();
        }
    }

    private void Update()
    {
        // 슬라이더의 value는 loadRatio가 될 때까지 계속 증가
        if (loadingSlider != null &&  loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    /// <summary>
    /// 마우스나 키가 눌러지면 실행되는 함수
    /// </summary>
    /// <param name="_"></param>
    private void Press(InputAction.CallbackContext _)
    {
        if (loadingDone)
            async.allowSceneActivation = true;
            async.allowSceneActivation = loadingDone; // loadingDone이 true면 allowSceneActivation을 true로 만들기
    }

    /// <summary>
    /// 글자의 모양을 계속 변경하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        WaitForSeconds wait = new WaitForSeconds(2.0f);
        string[] texts =
        {
            "Tip : 무기를 스위칭하는 키는 Q입니다",
            "Tip : 미니맵 키는 M입니다."
        };

        System.Random random = new System.Random();

        while (true)
        {

            int index = random.Next(texts.Length);
            loadingText.text = texts[index];
            //index++;
            //index %= texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// 비동기로 씬을 로딩하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        // 비동기 로딩 시작
        async = SceneManager.LoadSceneAsync(nextStageMap.name);
        async.allowSceneActivation = false; // 자동으로 씬전환되지 않도록 하기

        while (loadRatio < 0.9f)
        {
            loadRatio = async.progress + 0.1f;  // 로딩 진행율에 따라 loadRatio 설정
            yield return null;
        }

        // 남아있는 슬라이더가 다 찰 때까지 기다리기
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);        // 글자 변경 안되게 만들기
        loadingText.text = "로딩 완료 아무키나 입력해주세요!";     // 완료되었다고 글자 출력
        loadingDone = true;                         // 로딩 완료되었다고 표시
    }

    public void LoadNextStage()
    {
        StartCoroutine(AsyncLoadScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 "PlayerLocation" 태그를 가진 오브젝트 찾아, 있으면 그곳으로 이동.
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }
    }
}