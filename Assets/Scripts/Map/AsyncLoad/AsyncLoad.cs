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
    /// ����Ƽ���� �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// ���� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// �ε� �Ϸ� ǥ��(true�� �Ϸ�, false �̿�)
    /// </summary>
    bool loadingDone = false;

    /// <summary>
    /// slider�� value�� ������ �� ��
    /// </summary>
    float loadRatio;

    /// <summary>
    /// slider�� value�� �����ϴ� �ӵ�(�ʴ�)
    /// </summary>
    public float loadingBarSpeed = 1.0f;

    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;

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
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();

        loadingTextCoroutine = LoadingTextProgress();

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
        // �����̴��� value�� loadRatio�� �� ������ ��� ����
        if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }
    }

    /// <summary>
    /// ���콺�� Ű�� �������� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="_"></param>
    private void Press(InputAction.CallbackContext _)
    {
        async.allowSceneActivation = loadingDone; // loadingDone�� true�� allowSceneActivation�� true�� �����
    }

    /// <summary>
    /// ������ ����� ��� �����ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadingTextProgress()
    {
        // 0.2�� �������� .�� ������.
        // .�� �ִ� 5�������� ��´�.
        // "Loading" ~ "Loading . . . . ."

        WaitForSeconds wait = new WaitForSeconds(0.2f);
        string[] texts =
        {
            "Loading",
            "Loading .",
            "Loading . .",
            "Loading . . .",
            "Loading . . . .",
            "Loading . . . . .",
        };

        int index = 0;
        while (true)
        {
            loadingText.text = texts[index];
            index++;
            index %= texts.Length;
            yield return wait;
        }
    }

    /// <summary>
    /// �񵿱�� ���� �ε��ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator AsyncLoadScene()
    {
        loadRatio = 0.0f;
        loadingSlider.value = loadRatio;

        // �񵿱� �ε� ����
        async = SceneManager.LoadSceneAsync(nextStageMap.name);
        async.allowSceneActivation = false; // �ڵ����� ����ȯ���� �ʵ��� �ϱ�

        while (loadRatio < 1.0f)
        {
            loadRatio = async.progress + 0.1f;  // �ε� �������� ���� loadRatio ����
            yield return null;
        }

        // �����ִ� �����̴��� �� �� ������ ��ٸ���
        yield return new WaitForSeconds((1 - loadingSlider.value) / loadingBarSpeed);

        StopCoroutine(loadingTextCoroutine);        // ���� ���� �ȵǰ� �����
        loadingText.text = "Loading\nComplete!";    // �Ϸ�Ǿ��ٰ� ���� ���
        loadingDone = true;                         // �ε� �Ϸ�Ǿ��ٰ� ǥ��
    }

    public void LoadNextStage()
    {
        StartCoroutine(AsyncLoadScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���ο� ������ "PlayerLocation" �±׸� ���� ������Ʈ ã��, ������ �װ����� �̵�.
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }
    }
}