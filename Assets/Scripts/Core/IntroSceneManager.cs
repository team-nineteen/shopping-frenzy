using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{

    public Transform cardStack;
    public TextMeshProUGUI subtitles;
    public GameObject spaceText;

    private int currentActionIndex = 0;
    private List<CutsceneAction> actions;

    void Start()
    {
        actions = new List<CutsceneAction>() {
            new Transition(cardStack, 3f, true),
            new Dialog(subtitles, "Mom", "Hey, Charlie", 10f),
            new Dialog(subtitles, "Charlie", "Momma?", 10f),
            new Dialog(subtitles, "Mom", "I am feeling a bit sick..."),
            new Dialog(subtitles, "Mom", "Could you do the groceries today?"),
            new Dialog(subtitles, "Charlie", "Okay!", 5f),
            new Dialog(subtitles, "Mom", "Just be back before dinner!"),
            new Dialog(subtitles, "Mom", "And only buy the necessary!"),
            new Dialog(subtitles, "Charlie", "But...", 5f),
            new Transition(cardStack, 1f, true),
            new Dialog(subtitles, "Mom", "Come on. It's easy!"),
            new Dialog(subtitles, "Mom", "Just put everything in your cart!"),
            new Dialog(subtitles, "Mom", "Then pass through the check out!"),
            new Dialog(subtitles, "Charlie", "...", 3f),
            new Transition(cardStack, 1f, true),
            new Dialog(subtitles, "Mom", "Good luck sweetheart!"),
            new Transition(cardStack, 3f, true),
        };
        spaceText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentActionIndex >= actions.Count) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        bool done = actions[currentActionIndex].Update();
        bool button = Input.GetButtonDown(GameConstants.k_ButtonNameSubmit);
        bool autoNext = actions[currentActionIndex].autoTriggerNext;

        if (button || autoNext)
        {
            if (done) {
                actions[currentActionIndex].Finish();
                currentActionIndex++;
                spaceText.SetActive(false);
            }
            else if (button) {
                actions[currentActionIndex].Finish();
                spaceText.SetActive(true);
            }
        } else if (done) {
            spaceText.SetActive(true);
        }
    }

    public abstract class CutsceneAction
    {
        public bool autoTriggerNext { get; protected set; }

        public CutsceneAction(bool autoTrigger)
        {
            this.autoTriggerNext = autoTrigger;
        }

        public abstract bool Update();
        public abstract void Finish();
    };

    public class Transition : CutsceneAction
    {
        private float transitionTime;
        private float t;
        private Transform cardStack;
        private Image card;
        public Transition(Transform cardStack, float transitionTime, bool autoTriggerNext = false) : base(autoTriggerNext)
        {
            this.cardStack = cardStack;
            this.transitionTime = transitionTime;
            t = transitionTime;
        }

        public override bool Update()
        {
            if (t <= 0) return true;
            if (!card) card = cardStack.GetChild(cardStack.childCount - 1).GetComponent<Image>();
            t -= Time.deltaTime;
            card.color = new Color(card.color.r, card.color.g, card.color.b, t / transitionTime);
            return t <= 0;
        }

        public override void Finish()
        {
            t = 0;
            if (card && card.gameObject) Destroy(card.gameObject);
        }
    }
    public class Dialog : CutsceneAction
    {
        private string text;
        private float textSpeed;
        private TextMeshProUGUI subtitles;
        private float t;

        public Dialog(TextMeshProUGUI subtitles, string author, string text, float textSpeed = 15f, bool autoTriggerNext = false) : base(autoTriggerNext)
        {
            this.subtitles = subtitles;
            this.text = author + ": " + text;
            this.textSpeed = textSpeed;
            t = author.Length + 2;
        }

        public override bool Update()
        {
            if (t >= text.Length) return true;
            t += Time.deltaTime * textSpeed;
            subtitles.text = text.Substring(0, Mathf.Min((int)t, text.Length));
            return t >= text.Length;
        }

        public override void Finish()
        {
            t = text.Length;
            subtitles.text = text;
        }
    }
}
