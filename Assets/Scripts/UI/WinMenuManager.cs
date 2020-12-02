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
    public Button ContinueButton;
    public AudioSource WinMusic;
    private EventSystem es;

    private AudioRandomizer m_AudioRandomizer;

    public bool isActivated { get; private set; }
    void Start()
    {
        m_AudioRandomizer = FindObjectOfType<AudioRandomizer>();
        DebugUtility.HandleErrorIfNullFindObject<AudioRandomizer, WinMenuManager>(m_AudioRandomizer, this);

        ContinueButton.onClick.AddListener(OnContinue);
        es = EventSystem.current;
        root.SetActive(false);
        isActivated = false;
    }

    private void SetRankText(Score.Rank rank) {
        RankText.text = rank.ToString();
        switch (rank)
        {
            case Score.Rank.S:
                RankText.color = new Color (1.0f, 0.85f, 0f);
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

    public void OnActivate(Score score)
    {
        ScoreText.text = "Your Score: " + score.score.ToString();
        SetRankText(score.rank);
        MoneyText.text = Score.MoneyString(score.moneySpent);
        TimeText.text = Score.TimeString(score.timeInSecondsSpent);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        es.SetSelectedGameObject(ContinueButton.gameObject);
        isActivated = true;
        root.SetActive(true);

        m_AudioRandomizer.Stop();
        WinMusic.Play();
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
