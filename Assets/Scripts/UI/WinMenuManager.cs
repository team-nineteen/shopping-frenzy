using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinMenuManager : MonoBehaviour
{
    public GameObject root;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI RankText;
    public TextMeshProUGUI MoneyText;
    public TextMeshProUGUI TimeText;
    public Image ImageGood;
    public Image ImageBadTime;
    public Image ImageBadMoney;
    public GameObject FadeOutCanvas;
    public Image FadeOutImage;
    public Image FadeInImage;
    public AudioSource WinMusic;
    public Button ContinueButton;

    [Header("Margins for bad endings")]
    public float badMoneyMargin = 1.5f;
    public float badTimeMargin = 2f;

    private EventSystem es;

    private float fadeOutAlpha = 0f;
    private float fadeInAlpha = 1f;
    private AudioRandomizer m_AudioRandomizer;

    public bool isActivated { get; private set; }
    private bool transitionEnded = false;
    void Start()
    {
        m_AudioRandomizer = FindObjectOfType<AudioRandomizer>();
        DebugUtility.HandleErrorIfNullFindObject<AudioRandomizer, WinMenuManager>(m_AudioRandomizer, this);

        ContinueButton.onClick.AddListener(OnContinue);
        es = EventSystem.current;
        root.SetActive(false);
        isActivated = false;
        ImageBadMoney.color = new Color(1, 1, 1, 0);
        ImageBadTime.color = new Color(1, 1, 1, 0);
        ImageGood.color = new Color(1, 1, 1, 0);
    }

    private void SetRankText(Score.Rank rank)
    {
        RankText.text = rank.ToString();
        switch (rank)
        {
            case Score.Rank.S:
                RankText.color = new Color(1.0f, 0.85f, 0f);
                WinText.text = "EXCELLENT!";
                break;
            case Score.Rank.A:
                RankText.color = Color.green;
                WinText.text = "Wonderful!";
                break;
            case Score.Rank.B:
                RankText.color = Color.green;
                WinText.text = "Great!";
                break;
            case Score.Rank.C:
                RankText.color = Color.yellow;
                WinText.text = "Not bad!";
                break;
            case Score.Rank.D:
                RankText.color = Color.yellow;
                WinText.text = "That's OK!";
                break;
            case Score.Rank.E:
                RankText.color = Color.red;
                WinText.text = "Hmmm?!";
                break;
            case Score.Rank.F:
                RankText.color = Color.red;
                WinText.text = "Not so good!";
                break;
            default:
                RankText.color = new Color(0.4f, 0f, 0f);
                WinText.text = "AWFUL!";
                break;
        }
    }

    public void OnActivate(Score score, int moneyGoal, int timeGoal)
    {
        ScoreText.text = "Your Score: " + score.score.ToString();
        SetRankText(score.rank);
        MoneyText.text = Score.MoneyString(score.moneySpent);
        TimeText.text = Score.TimeString(score.timeInSecondsSpent);

        isActivated = true;
        if (score.moneySpent >= moneyGoal * badMoneyMargin || score.timeInSecondsSpent >= timeGoal * badTimeMargin)
        {
            if (((float)score.moneySpent) / moneyGoal > ((float)score.timeInSecondsSpent) / timeGoal)
                ImageBadMoney.color = Color.white;
            else ImageBadTime.color = Color.white;
        }
        else ImageGood.color = Color.white;
    }

    public void Update()
    {
        if (isActivated && !transitionEnded)
        {
            if (Transition())
            {
                EndTransition();
            }
        }
    }

    public bool Transition()
    {
        if (fadeOutAlpha < 1.0f)
        {
            fadeOutAlpha = Mathf.Min(fadeOutAlpha + Time.deltaTime, 1.0f);
            FadeOutImage.color = new Color(0, 0, 0, fadeOutAlpha);
            if (fadeOutAlpha == 1.0f)
            {
                root.SetActive(true);
                FadeOutCanvas.SetActive(false);
            }
        }
        else
        {
            fadeInAlpha = Mathf.Max(fadeInAlpha - Time.deltaTime, 0.0f);
            FadeInImage.color = new Color(0, 0, 0, fadeInAlpha);
        }
        return fadeInAlpha <= 0.0f;
    }

    public void EndTransition()
    {
        FadeInImage.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_AudioRandomizer.Stop();
        WinMusic.Play();
        Time.timeScale = 0f;
        es.SetSelectedGameObject(ContinueButton.gameObject);
        transitionEnded = true;
    }

    public void OnHighlightButton(GameObject obj)
    {
        es.SetSelectedGameObject(obj);
    }
    void OnContinue()
    {
        SettingsData.Instance.ClearSubscriptions();
        SceneManager.LoadScene(0); // Back to main menu
    }

}
