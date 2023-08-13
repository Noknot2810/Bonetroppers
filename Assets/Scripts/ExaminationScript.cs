using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// �������� ������, �������������� ������� ���������� ��������
public class ExaminationScript : MonoBehaviour
{
    // ��������
    public float AnsSpawnDelay = 1f;        // �������� ��������� ������
                                            // �������
    public float DarkScreenDelay = 2f;      // �������� ������������ ������
                                            // ������
    public float NewStageDelay = 1.5f;      // �������� �������� � ����������
                                            // �������
    private float newSectionDelay;          // �������� �������� � ����������
                                            // ����� ��������

    // ���������� �������� ��������� ��������
    public int QuestionCntInSection = 10;   // ���������� ��������, �������
                                            // ������ �� ������� �����
    private int currentSection = -1;        // ������� ����� �����
    private int currentSectLen;             // ����� �������� �����
    private int currentElemInSection;       // ID �������� ������� � �����
    private int sectionStep;                // ��� ������ ����� ��������
                                            // � �����
    private int ElemsUsedInSect = 0;        // ���������� ��������������
                                            // �������� �� �����

    private int currentStage = -1;          // ����� �������� �������
    private int lastStage;                  // ����� ���������� �������

    private int correctAnswer;              // ����� �������� �����������
                                            // ������

    private bool isHardmode = false;        // ����������-���� ��� �����������
                                            // ��������� �������� ������
    public float timer = 30f;               // ������ ��� ��������
    private bool paused = false;            // ����������-���� ��� ������������
                                            // �������
    private bool chanceUsed = false;        // ����������-��� ������� � ������
                                            // ����� �� ������
    private int realRes = 0;                // �������� ���������� ����������
                                            // ��������

    // ���������� ��������
    private List<AnswerField> answersFields;// ������ �������� ����� �������
    private Question[][] questions;         // ������� ��������
    private string[] levels;                // �������� ������� ��������

    // ������� ��� ��������������
    public DialogueManager Manager;         // �������� ������� � �����
    public GameObject AnswersObject;        // ������ ��� �������� ������
                                            // �������
    public PlayerMovement Player;           // ������ �������� ��������� ������
    public GameObject RestartButton;        // ������ ����������� �����
    public Slider TimerSlider;              // ������� �������
    public Animator[] InviteButtons;        // ������������ ����������
                                            // ��������� ������ ������� � �����
    public Animator DarkScreen;             // ������������ ���������
                                            // ������ ������
    public Animator Wizard;                 // ������������ ���������
                                            // ������� ����
    public CloudNoteScript CloudNote;       // ������ �������� �������
    public HealthButtonScript HealthIcon;   // ������ ������� ����� �� ������

    // �������� �����
    public AudioManager MusicBox;           // ������ �������� �����-���������
    private int curMelody = 0;              // ����� ������� ������������ ����
    private readonly int melodyCnt = 7;     // ���������� ����������� ���
                                            // �� ��������

    // ����������� ������������
    public bool DevMode = false;            // ���������� ���� ��� ������
                                            // ������������
    public List<GameObject> DevButtons;     // ������ ������ ������������

    // �������, ����������� ����� ������ ������
    private void Start()
    {
        try
        {
            // �������� � �������������� �������� ��������
            IEnumerable<Dialogue> dlg = 
                Resources.LoadAll<Dialogue>("")
                         .Where(dlg => dlg.name.IndexOf("Question #") != -1)
                         .Select(dlg => new {st = dlg.name.IndexOf("#") + 1, 
                                             end = dlg.name.IndexOf("-"), dlg})
                         .Select(dict => new {ind = int.Parse(dict.dlg.name.Substring(dict.st, dict.end - dict.st)), 
                                              dict.dlg })
                         .OrderBy(dict => dict.ind)
                         .Select(dict => dict.dlg);

            // ��������� �������� �� ��������
            questions = dlg.Select(dlg => dlg.questions).ToArray();
            // ��������� �������� ������� �� ��������
            levels = dlg.Select(dlg => dlg.level).ToArray();
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
            questions = null;
        }

        // ����������� ������ ���������� �������
        lastStage = questions.Length * QuestionCntInSection - 1;
        // ����������� �������� ��� �������� ����� ������� ��������
        newSectionDelay = CloudNote.RegularDelay + 1.2f;

        // ��������� ������ ������� � �������� �������� ����� ������� �� ���
        answersFields = new List<AnswerField>();
        foreach (Transform t in AnswersObject.
            GetComponentsInChildren<Transform>())
        {
            if (t.name != AnswersObject.name && t.name != "Text")
                answersFields.Add(new AnswerField(t.gameObject));
        }

        // ���������� �������� �������
        TimerSlider.gameObject.SetActive(false);
        // �������� ������ ������
        DarkScreen.SetBool("appeared", false);
    }

    // ������� ������ ������ ������� ����
    public void StartGameFromBeg()
    {
        // ����� ������� �� ������ �������� ������� ����� ���
        // ���� �������� ����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().SpawnShrine();
        }
    }

    // ������� ������ ������ ������� ���� � ���������
    public void StartGameFromCheck(int firstSection)
    {
        // ����� ������ ������
        DarkScreen.SetBool("appeared", true);
        // ����� ������� �� ����������� ��������� ���������
        // ���� �������� ����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().TeleportShrine();
        }

        // ����������� �������� ����� ��������
        currentSection = firstSection - 1;
        // ����������� �������� ������ �������
        currentStage = firstSection * QuestionCntInSection - 1;
        // ����� ����� �������� ������� ������ �������� � ���������
        Invoke(nameof(StartExaminationFromCheck), DarkScreenDelay);
    }

    // ������� ������ �������� � ���������
    public void StartExaminationFromCheck()
    {
        // �������� ���� ������� ���������
        foreach (GameObject obj in GameObject.
            FindGameObjectsWithTag("SoldierCloud"))
        {
            Destroy(obj);
        }
        // �������� � ����� ������� ������������� ��������
        FindObjectOfType<MoveShrine>().RemoveFirstTrig();
        // ������������ ������ �� ������� ��������� �����
        Player.TeleportForward(162.1f);
        // �������� ������ ������
        DarkScreen.SetBool("appeared", false);
        // ��������� ���������� ���� ��������
        Manager.TextField.enabled = true;

        // ����� ������� ������ ��������
        StartExamination();
    }

    // ������� ������ ������ ������� ����
    public void StartHardGame()
    {
        // ��������� ����� ������� ����
        isHardmode = true;
        // ����� ������� �� ������ �������� ������� ����� ���
        // ���� �������� ����
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().SpawnShrine();
        }
    }

    // ������� ������ ������ ���� � ��������� � �����
    public void StartGame()
    {
        // ��������� ���������� ���� ��������
        Manager.TextField.enabled = true;
        // ����� ������� � ����������� �� ��������� ���������
        if (isHardmode == true)
            Manager.StartDialogue(
                (Resources.Load("ExaminationHardBeginning") as Dialogue).
                    text.Split('\n').ToList<string>());
        else
            Manager.StartDialogue(
                (Resources.Load("ExaminationBeginning") as Dialogue)
                    .text.Split('\n').ToList<string>());
        // ����� ������������� ����������� ����
        MusicBox.Play("Invitation");
        // ����� ������ ������� � �����
        foreach (Animator anim in InviteButtons)
        {
            anim.SetBool("appeared", true);
        }
    }

    // ������� ������ ��������
    public void StartExamination()
    {
        // ����� ������ ����������� � ������ ������ ������������
        if (DevMode == true)
        {
            foreach (GameObject obj in DevButtons)
                obj.SetActive(true);
        }
        
        // ������� ��������� ������ � ��������� ��������
        Player.ChangeIdle();
        // �������� ������ ������� � �����
        foreach (Animator anim in InviteButtons)
        {
            anim.SetBool("appeared", false);
        }

        // ��� ������� ������ ����� �������� �������
        if (isHardmode == true)
        {
            TimerSlider.gameObject.SetActive(true);
        }

        // ����� ������ ������� ����� �� ������
        HealthIcon.ButtonOn();

        // ����� ������ ����������� ���� ��������
        MusicBox.Play("Exam #" + curMelody.ToString());

        // ����� ������� ������ ������ ����� ��������
        if (questions != null)
        {
            NewSection();
        }
    }

    // ������� ������ ������ ����� �������� ��������
    public void NewSection()
    {
        // ���������� ���������� � ������� ����� ��������
        currentSection++;
        ElemsUsedInSect = 0;
        currentSectLen = questions[currentSection].Length;

        // ����� ���������� ������� ������� �� ����� �
        // ���������� ���� ��� ������ ��������
        currentElemInSection = Random.Range(0, currentSectLen);
        sectionStep = Random.Range(1, currentSectLen / QuestionCntInSection + 1);

        // ������������ ����
        paused = true;
        // ���������� ���������� ���� ��������
        Manager.TextField.enabled = false;
        // ����� �������� ������� � ��������� �������� ����� ��������
        CloudNote.TurnCloudNote(levels[currentSection]);
        // ����� ������� ��������� ������ �������
        Invoke(nameof(NewStage), newSectionDelay);
    }

    // ������� ������ ������ ������� � ������� ����� �������� ��������
    public void NewElemInSection()
    {
        currentElemInSection += sectionStep;
        if (currentElemInSection >= currentSectLen)
        {
            currentElemInSection -= currentSectLen;
        }
        Invoke(nameof(NewStage), NewStageDelay);
    }
    
    // ������� ��������� ������ ������� ��������
    public void NewStage()
    {
        // ������������� ������� ����
        paused = false;
        // ��������� ���������� ���� ��������
        Manager.TextField.enabled = true;
        // ��������� � ����� ������ �������
        currentStage++;
        Question currentQuestion = 
            questions[currentSection][currentElemInSection];
        Manager.StartDialogue(new List<string> { currentQuestion.question });

        // ����� ����� �������� ������ �������
        StopAllCoroutines();
        for (int i = 0; i < currentQuestion.options.Length; i++)
        {
            StartCoroutine(CallButtons(i, currentQuestion.options[i]));
        }

        // ���������� ������ �������� ����������� ������
        correctAnswer = currentQuestion.correctIs;
        // ���������� �������� �������������� � ������ ������� ����� ��������
        ElemsUsedInSect++;
    }

    // ������� ������ ������ ������
    IEnumerator CallButtons(int id, string text)
    {
        yield return new WaitForSeconds(AnsSpawnDelay);
        answersFields[id].Text.text = text;
        answersFields[id].Anim.SetInteger("disNum", 0);
        answersFields[id].Anim.SetBool("appeared", true);
    }

    // ������� �������� � ������ ��� ������ ������������
    public void GetFinalDev(int num)
    {
        if (DevMode == true)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DevButton"))
            {
                obj.SetActive(false);
            }

            currentStage = lastStage;
            correctAnswer = 1;

            if (num == 0)
            {
                chanceUsed = true;
                SetAnswer(0);
                realRes = 48;
                currentSection = 4;
                HealthIcon.ButtonOff();
                return;
            }
            else if (num == 1)
                isHardmode = true;
            else if (num == 3)
            {
                chanceUsed = false;
                SetAnswer(0);
                return;
            }

            SetAnswer(1);
        }
    }

    // ������� ��������� ����������� ������ �� ������
    public void SetAnswer(int id)
    {
        // ���������� ������� ������������ ������
        if (id == correctAnswer)
        {
            // ���������� ���������� ���������� ��������
            realRes++;

            // ���������� �������, ��� ������ ��������� ������
            if (currentStage == lastStage)
            {
                // �������� ���� ������ �������
                HideAnswersButtons();

                // ����� ����������� ���� ���������� �������
                MusicBox.Play("Last step");

                // ����� ������ ������� ���� ������������ ���������� �������
                Manager.StartDialogue(new List<string> { 
                    (Resources.Load("ExaminationLast") as Dialogue).text });

                // ������������ ������� ���� � ���������� �������� �������
                // � ������ �������� ������
                if (isHardmode == true)
                {
                    TimerSlider.gameObject.SetActive(false);
                    paused = true;
                }

                // ����� � ��������� ������� �������� ������ ��������
                Invoke(nameof(GoodFinal), 25f);
            }
            else
            {
                // ��������� ����� ������ � ���������� �������
                answersFields[id].Anim.SetInteger("disNum", 1);
                // �������� ���� ������ �������
                HideAnswersButtons();

                // ���������� ������� ��� ������� ������
                if (isHardmode == true)
                {
                    timer = 30f;
                }
                // � ����������� �� ���������� ��� �������������� ��������
                // � ����� - ����� ������ ����� ��� ����� ������ �������
                // � ������ ��������
                if (ElemsUsedInSect >= QuestionCntInSection)
                    NewSection();
                else
                    NewElemInSection();
            }
        }
        else
        {
            // ���������� �������, ��� ������ ��������� ������
            if (currentStage == lastStage)
            {
                // �������� ���� ������ �������
                HideAnswersButtons();

                // ����� ����������� ���� ���������� �������
                MusicBox.Play("Last step");

                // ����� ������ ������� ���� ������������ ���������� �������
                Manager.StartDialogue(new List<string> { 
                    (Resources.Load("ExaminationLast") as Dialogue).text });

                // ������������ ������� ���� � ���������� �������� �������
                // � ������ �������� ������
                if (isHardmode == true)
                {
                    TimerSlider.gameObject.SetActive(false);
                    paused = true;
                }

                // � ����������� �� ������� ����� �� ������, �����
                // ���������� ������ �������� ��� ����� ��������
                if (chanceUsed)
                    Invoke(nameof(WorstFinal), 25f);
                else
                    Invoke(nameof(AlmostGoodFinal), 25f);
            }
            else
            {
                // ��������� ����� ������ � ������������ �������
                answersFields[id].Anim.SetInteger("disNum", -1);
                // �������� ���� ������ �������
                HideAnswersButtons();
                // ���������� �������, ��� ����� �� ������ ��� ������������
                if (chanceUsed == true)
                {
                    // ������� �������� ������� � ������������ �������
                    // � ������ �������� ������
                    if (isHardmode == true)
                    {
                        TimerSlider.gameObject.SetActive(false);
                        paused = true;
                    }

                    // ����� ������� ������� ������ ��������
                    BadFinal();
                }
                else
                {
                    // ��������� ����� ����� �� ������
                    chanceUsed = true;
                    // ������������ � ����� ������� � ������ �������� ������
                    if (isHardmode == true)
                    {
                        timer = 30f;
                        paused = true;
                    }
                    // ����� ������ ������� ���� ������������
                    // ������ ����� �� ������
                    Manager.StartDialogue(new List<string> {
                        (Resources.Load("ExaminationChanceUsed") as Dialogue).text });
                    // �������� ����� ������
                    MusicBox.PlayTransit("Problem sound");
                    // ������� ������ ������� ����� �� ������
                    HealthIcon.ButtonOff();
                    // � ����������� �� ���������� ��� �������������� ��������
                    // � ����� - ����� ������ ����� ��� ����� ������ �������
                    // � ������ ��������, �� ����� ��������
                    if (ElemsUsedInSect >= QuestionCntInSection)
                        Invoke(nameof(NewSection), 8f);
                    else
                        Invoke(nameof(NewElemInSection), 8f);
                }
            }
        }
    }

    // ������� �������� ���� ������ �������
    private void HideAnswersButtons()
    {
        foreach (AnswerField field in answersFields)
        {
            field.Anim.SetBool("appeared", false);
        }
    }

    // ������� ���������� ������ ��������
    public void WorstFinal()
    {
        // ����� ��������������� ����������� ����
        MusicBox.Play("Worst final");
        // ����� ���������������� ������ ������� �� ����
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationWorstResult") as Dialogue).text });
        // ����� �������� ����� ������ � ����
        Wizard.SetTrigger("CastTrigger");
        // ����� ������� ������ � ��������� ������
        #pragma warning disable UNT0016
        Player.Invoke("Explode", 1f);
        #pragma warning restore UNT0016

        // ����� ���������� ����
        StartCoroutine(ActivateAfterGameMenu(delay: 5f, showStat: true));

        // ���������� ������ ������
        SaveCheckpoint();
    }

    // ������� ������� ������ ��������
    public void BadFinal()
    {
        // ����� ��������������� ����������� ����
        MusicBox.Play("Bad final");
        // ����� ���������������� ������ ������� �� ����
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationFailed") as Dialogue).text});
        // ����� �������� ����� ������ � ����
        Wizard.SetTrigger("CastTrigger");
        // ����� ������� ������ � ��������� ������
        #pragma warning disable UNT0016
        Player.Invoke("Explode", 1f);
        #pragma warning restore UNT0016
        // ����� ����� �������� ���������� ����
        StartCoroutine(ActivateAfterGameMenu(delay: 5f, showStat: true));

        // ���������� ������ ������
        SaveCheckpoint();
    }

    // ������� ����� �������� ������ ��������
    public void AlmostGoodFinal()
    {
        // ������� ������ ������� ����� �� ������
        HealthIcon.ButtonOff();
        // ������������ ������
        MusicBox.Play("Invitation");
        // ����� ���������������� ������ ������� �� ����
        Manager.StartDialogue(new List<string> {
            (Resources.Load("ExaminationAlmostGood") as Dialogue).text });
        // ����� �������� ������ ��������
        Invoke(nameof(GoodFinal), 8f);
    }

    // ������� �������� ������ ��������
    public void GoodFinal()
    {
        // ������� ������ ������� ����� �� ������
        HealthIcon.ButtonOff();
        // ����� ��������������� ����������� ����
        MusicBox.Play("Good final");
        // ����� ���������������� ������ ������� �� ����
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationSuccess") as Dialogue).text });
        // ����� �������� ���������� �������
        Player.Celebrate();
        // ����� ���� �������� �����������
        Player.ChangeIdle();
        // ����� ����� �������� ������� ��������� �������� ���������
        Invoke(nameof(FinalWalk), 15f);
    }

    // ������� ��������� �������� ���������
    public void FinalWalk()
    {
        // ���������� ���������� ���� ��������
        Manager.TextField.enabled = false;
        // ����� �������� �������� ����
        Wizard.SetTrigger("NextTrigger");
        // ������ �������� ������� �����
        FindObjectOfType<MoveShrine>().Restart();
        // ����� �������� ������ ��������� ������
        Player.StartWalk();
        // ����� �������� �������
        CloudNote.TurnCloudNote("Advanced", withoutSound: true);
    }

    // ������� ���������� �������� ������������ ����� ��������
    public void SaveCheckpoint()
    {
        int best = PlayerPrefs.GetInt("bestSection", 0);
        if (currentSection > best)
        {
            PlayerPrefs.SetInt("bestSection", currentSection);
            PlayerPrefs.Save();
        }
    }

    // ������� ������ ������� �������� ����
    public void HappyEnd()
    {
        // ����� �������� ���������� ������� ��������� ������
        Player.Celebrate();
        // ����� �������� ������� �� �������� ������� � �������
        // � ������ ���� ��� ����� �� ���� �������
        if (isHardmode == true)
        {
            if (PlayerPrefs.GetInt("hardmodeCompleted", 0) == 0)
            {
                PlayerPrefs.SetInt("hardmodeCompleted", 1);
                CloudNote.TurnCloudNote("������ ��� ������ ����� ���!\n�� " +
                                        "��������� ������ ����, �����������!", 
                                        withoutSound: true);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("regularCompleted", 0) == 0)
            {
                PlayerPrefs.SetInt("regularCompleted", 1);
                CloudNote.TurnCloudNote("������ ��� ������ ����� ��� � " +
                                        "����� ���� �� �����", 
                                        withoutSound: true);
            }
        }

        // ���������� ������ ������
        SaveCheckpoint();

        // ���������� ���� ���������� ���� ��������
        Manager.gameObject.GetComponent<Shadow>().enabled = false;
        // ��������� ��������� ������ � �������� ���� ��������
        Manager.TextField.alignment = TextAnchor.MiddleCenter;
        // ��������� ���������� ���� ��������
        Manager.TextField.enabled = true;
        // ����� ���������������� ������ �������
        if (isHardmode == true)
            Manager.StartDialogue(new List<string> 
                {"������ ����� �����, ������� �����������������!"});
        else
            Manager.StartDialogue(new List<string> {"������ ����� �����!"});
        // ��������� �������� ���� ����� ����
        StartCoroutine(ActivateAfterGameMenu(delay: 0f));
    }

    // ������� ������ ���� ����� ����
    IEnumerator ActivateAfterGameMenu(float delay, bool showStat = false)
    {
        yield return new WaitForSeconds(delay);
        // ��������� � ��������� ������ ������������ �����
        RestartButton.GetComponent<CircleCollider2D>().enabled = true;
        RestartButton.GetComponent<SpriteRenderer>().enabled = true;
        RestartButton.transform.Find("Icon").gameObject.
            GetComponent<SpriteRenderer>().enabled = true;
        RestartButton.transform.Find("Diod").gameObject.
            GetComponent<SpriteRenderer>().enabled = true;

        // ���� ���������� ����� ���������� �� ����
        if (showStat == true)
        {
            yield return new WaitForSeconds(2f);
            // ����� ���������������� ������ ������� �� ����
            Manager.StartDialogue(new List<string> {
            "������� �������: " + levels[currentSection] +
            "\n����� ���������� �������: " + realRes.ToString()});
        }
    }

    // �������, ����������� ��� � ��������� ������������� ���������� ������
    private void FixedUpdate()
    {
        // ���������� �������, ��� ������� ������� �����, ���� ������������
        // ���� �������� � ������ �� ����� �� �����
        if (isHardmode == true && currentStage > -1 && paused == false)
        {
            // ���������� �������
            timer -= Time.deltaTime;
            // ���������� �������� �������
            TimerSlider.value = 1 - timer / 30f;
            // ���������� ������� ������������ �������
            if (timer <= 0)
            {
                // ���������� ������� ��������
                TimerSlider.gameObject.SetActive(false);
                // ��������� �������
                paused = true;
                // ���������� ���� ����� �������
                foreach (AnswerField field in answersFields)
                {
                    field.Anim.SetBool("appeared", false);
                }
                // ����� ������� ����� ���������� ��������
                BadFinal();
            }
        }

        // ���������� ������� ��������� ������� ����������� ����
        if (!MusicBox.IsPlaying())
        {
            // ��������� ������ ����� ����������� ����
            curMelody++;
            if (curMelody == melodyCnt)
                curMelody = 0;
            // ������ ����� ����
            MusicBox.Play("Exam #" + curMelody.ToString(), false);
        }
            
    }
}

// ������ ��� �������� ���� ������
public class AnswerField
{
    private readonly string name;   // �������� ���� ������
    private readonly Text text;     // ������� ����� ������
    private readonly Animator anim; // ������������ ��������� ���� ������
    // ���������� ��� ���������� ������� � ����� ���� ������
    public string Name => this.name;
    public Text Text => this.text;
    public Animator Anim => this.anim;

    // ������� ������������� ���� ������
    public AnswerField(GameObject obj)
    {
        name = obj.name;
        text = obj.GetComponentInChildren<Text>();
        anim = obj.GetComponent<Animator>();
    }
}