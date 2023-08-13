using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Основной скрипт, контролирующий процесс проведения экзамена
public class ExaminationScript : MonoBehaviour
{
    // Задержки
    public float AnsSpawnDelay = 1f;        // Задержка появления кнопок
                                            // ответов
    public float DarkScreenDelay = 2f;      // Задержка исчезновения тёмного
                                            // экрана
    public float NewStageDelay = 1.5f;      // Задержка перехода к следующему
                                            // вопросу
    private float newSectionDelay;          // Задержка перехода к следующему
                                            // блоку вопросов

    // Переменные контроля состояния экзамена
    public int QuestionCntInSection = 10;   // Количество вопросов, которое
                                            // берётся из каждого блока
    private int currentSection = -1;        // Текущий номер блока
    private int currentSectLen;             // Длина текущего блока
    private int currentElemInSection;       // ID текущего вопроса в блоке
    private int sectionStep;                // Шаг выбора новых вопросов
                                            // в блоке
    private int ElemsUsedInSect = 0;        // Количество использованных
                                            // вопросов из блока

    private int currentStage = -1;          // Номер текущего вопроса
    private int lastStage;                  // Номер последнего вопроса

    private int correctAnswer;              // Номер текущего правильного
                                            // ответа

    private bool isHardmode = false;        // Переменная-флаг для определения
                                            // включения сложного режима
    public float timer = 30f;               // Таймер для вопросов
    private bool paused = false;            // Переменная-флаг для приостановки
                                            // таймера
    private bool chanceUsed = false;        // Переменная-флг наличия у игрока
                                            // права на ошибку
    private int realRes = 0;                // Реальное количество пройденных
                                            // вопросов

    // Наполнение экзамена
    private List<AnswerField> answersFields;// Список объектов полей ответов
    private Question[][] questions;         // Вопросы экзамена
    private string[] levels;                // Название уровней экзамена

    // Объекты для взаимодействия
    public DialogueManager Manager;         // Менеджер диалога с магом
    public GameObject AnswersObject;        // Объект для хранения кнопок
                                            // ответов
    public PlayerMovement Player;           // Скрипт контроля персонажа игрока
    public GameObject RestartButton;        // Кнопка перезапуска сцены
    public Slider TimerSlider;              // Слайдер таймера
    public Animator[] InviteButtons;        // Анимационные компоненты
                                            // начальных кнопок диалога с магом
    public Animator DarkScreen;             // Анимационный компонент
                                            // тёмного экрана
    public Animator Wizard;                 // Анимационный компонент
                                            // объекта мага
    public CloudNoteScript CloudNote;       // Скрипт облачной заметки
    public HealthButtonScript HealthIcon;   // Значок наличия права на ошибку

    // Контроль аудио
    public AudioManager MusicBox;           // Скрипт игрового аудио-менеджера
    private int curMelody = 0;              // Номер текущей меузыкальной темы
    private readonly int melodyCnt = 7;     // Количество музыкальных тем
                                            // на экзамене

    // Инструменты разработчика
    public bool DevMode = false;            // Переменная флаг для режима
                                            // разработчика
    public List<GameObject> DevButtons;     // Список кнопок разработчика

    // Функция, запускаемая перед первым кадром
    private void Start()
    {
        try
        {
            // Загрузка и упорядочивание объектов диалогов
            IEnumerable<Dialogue> dlg = 
                Resources.LoadAll<Dialogue>("")
                         .Where(dlg => dlg.name.IndexOf("Question #") != -1)
                         .Select(dlg => new {st = dlg.name.IndexOf("#") + 1, 
                                             end = dlg.name.IndexOf("-"), dlg})
                         .Select(dict => new {ind = int.Parse(dict.dlg.name.Substring(dict.st, dict.end - dict.st)), 
                                              dict.dlg })
                         .OrderBy(dict => dict.ind)
                         .Select(dict => dict.dlg);

            // Получение вопросов из диалогов
            questions = dlg.Select(dlg => dlg.questions).ToArray();
            // Получение названий уровней из диалогов
            levels = dlg.Select(dlg => dlg.level).ToArray();
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex);
            questions = null;
        }

        // Определение номера последнего вопроса
        lastStage = questions.Length * QuestionCntInSection - 1;
        // Определение задержки при переходе между блоками вопросов
        newSectionDelay = CloudNote.RegularDelay + 1.2f;

        // Получение кнопок ответов и создание объектов полей ответов из них
        answersFields = new List<AnswerField>();
        foreach (Transform t in AnswersObject.
            GetComponentsInChildren<Transform>())
        {
            if (t.name != AnswersObject.name && t.name != "Text")
                answersFields.Add(new AnswerField(t.gameObject));
        }

        // Отключение слайдера таймера
        TimerSlider.gameObject.SetActive(false);
        // Сокрытие тёмного экрана
        DarkScreen.SetBool("appeared", false);
    }

    // Функция вызова начала обычной игры
    public void StartGameFromBeg()
    {
        // Вызов запроса на начало движения объекта храма для
        // всех объектов пола
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().SpawnShrine();
        }
    }

    // Функция вызова начала обычной игры с чекпоинта
    public void StartGameFromCheck(int firstSection)
    {
        // Вызов тёмного экрана
        DarkScreen.SetBool("appeared", true);
        // Вызов запроса на прекращение генерации декораций
        // всех объектов пола
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().TeleportShrine();
        }

        // Определение текущего блока вопросов
        currentSection = firstSection - 1;
        // Определение текущего номера вопроса
        currentStage = firstSection * QuestionCntInSection - 1;
        // Вызов после задержки функции начала экзамена с чекпоинта
        Invoke(nameof(StartExaminationFromCheck), DarkScreenDelay);
    }

    // Функция начала экзамена с чекпоинта
    public void StartExaminationFromCheck()
    {
        // Удаление всех облаков разговора
        foreach (GameObject obj in GameObject.
            FindGameObjectsWithTag("SoldierCloud"))
        {
            Destroy(obj);
        }
        // Удаление у храма первого остановочного триггера
        FindObjectOfType<MoveShrine>().RemoveFirstTrig();
        // Телепортация игрока на текущее положение храма
        Player.TeleportForward(162.1f);
        // Сокрытие тёмного экрана
        DarkScreen.SetBool("appeared", false);
        // Включение текстового поля экзамена
        Manager.TextField.enabled = true;

        // Вызов функции начала экзамена
        StartExamination();
    }

    // Функция вызова начала сложной игры
    public void StartHardGame()
    {
        // Установка флага сложной игры
        isHardmode = true;
        // Вызов запроса на начало движения объекта храма для
        // всех объектов пола
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Floor"))
        {
            obj.GetComponent<SpawnGround>().SpawnShrine();
        }
    }

    // Функция вызова начала игры с разговора с магом
    public void StartGame()
    {
        // Включение текстового поля экзамена
        Manager.TextField.enabled = true;
        // Вывод диалога в зависимости от выбранной сложности
        if (isHardmode == true)
            Manager.StartDialogue(
                (Resources.Load("ExaminationHardBeginning") as Dialogue).
                    text.Split('\n').ToList<string>());
        else
            Manager.StartDialogue(
                (Resources.Load("ExaminationBeginning") as Dialogue)
                    .text.Split('\n').ToList<string>());
        // Вызов вступительной музыкальной темы
        MusicBox.Play("Invitation");
        // Вызов кнопок общения с магом
        foreach (Animator anim in InviteButtons)
        {
            anim.SetBool("appeared", true);
        }
    }

    // Функция начала экзамена
    public void StartExamination()
    {
        // Вызов кнопок разработчка в случае режима разработчика
        if (DevMode == true)
        {
            foreach (GameObject obj in DevButtons)
                obj.SetActive(true);
        }
        
        // Перевод персонажа игрока в состояние ожидания
        Player.ChangeIdle();
        // Сокрытие кнопок общения с магом
        foreach (Animator anim in InviteButtons)
        {
            anim.SetBool("appeared", false);
        }

        // При сложном режиме вызов слайдера таймера
        if (isHardmode == true)
        {
            TimerSlider.gameObject.SetActive(true);
        }

        // Вызов значка наличия права на ошибку
        HealthIcon.ButtonOn();

        // Вызов первой музыкальной темы экзамена
        MusicBox.Play("Exam #" + curMelody.ToString());

        // Вызов функции начала нового блока вопросов
        if (questions != null)
        {
            NewSection();
        }
    }

    // Функция начала нового блока вопросов экзамена
    public void NewSection()
    {
        // Обновление информации о текущем блоке вопросов
        currentSection++;
        ElemsUsedInSect = 0;
        currentSectLen = questions[currentSection].Length;

        // Выбор случайного первого вопроса из блока и
        // случайного шага для взятия вопросов
        currentElemInSection = Random.Range(0, currentSectLen);
        sectionStep = Random.Range(1, currentSectLen / QuestionCntInSection + 1);

        // Приостановка игры
        paused = true;
        // Выключение текстового поля экзамена
        Manager.TextField.enabled = false;
        // Вывод облачной заметки с названием текущего блока вопросов
        CloudNote.TurnCloudNote(levels[currentSection]);
        // Вызов функции получения нового вопроса
        Invoke(nameof(NewStage), newSectionDelay);
    }

    // Функция выбора нового вопроса в текущем блоке вопросов экзамена
    public void NewElemInSection()
    {
        currentElemInSection += sectionStep;
        if (currentElemInSection >= currentSectLen)
        {
            currentElemInSection -= currentSectLen;
        }
        Invoke(nameof(NewStage), NewStageDelay);
    }
    
    // Функция получения нового вопроса экзамена
    public void NewStage()
    {
        // Возобновление таймера игры
        paused = false;
        // Включение текстового поля экзамена
        Manager.TextField.enabled = true;
        // Получение и вывод нового вопроса
        currentStage++;
        Question currentQuestion = 
            questions[currentSection][currentElemInSection];
        Manager.StartDialogue(new List<string> { currentQuestion.question });

        // Вызов после задержки кнопок ответов
        StopAllCoroutines();
        for (int i = 0; i < currentQuestion.options.Length; i++)
        {
            StartCoroutine(CallButtons(i, currentQuestion.options[i]));
        }

        // Сохранение номера текущего правильного ответа
        correctAnswer = currentQuestion.correctIs;
        // Увеличения счётчика использованных в рамках данного блока вопросов
        ElemsUsedInSect++;
    }

    // Функция вызова кнопки ответа
    IEnumerator CallButtons(int id, string text)
    {
        yield return new WaitForSeconds(AnsSpawnDelay);
        answersFields[id].Text.text = text;
        answersFields[id].Anim.SetInteger("disNum", 0);
        answersFields[id].Anim.SetBool("appeared", true);
    }

    // Функция перехода к финалу для режима разработчика
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

    // Функция обработки полученного ответа на вопрос
    public void SetAnswer(int id)
    {
        // Поставлено условие правильности ответа
        if (id == correctAnswer)
        {
            // Обновление переменной пройденных вопросов
            realRes++;

            // Поставлено условие, что сейчас последний вопрос
            if (currentStage == lastStage)
            {
                // Сокрытие всех кнопок ответов
                HideAnswersButtons();

                // Вызов музыкальной темы последнего вопроса
                MusicBox.Play("Last step");

                // Вывод текста диалога мага относительно последнего вопроса
                Manager.StartDialogue(new List<string> { 
                    (Resources.Load("ExaminationLast") as Dialogue).text });

                // Приостановка таймера игры и выключение слайдера таймера
                // в случае сложного режима
                if (isHardmode == true)
                {
                    TimerSlider.gameObject.SetActive(false);
                    paused = true;
                }

                // Вызов с задержкой функции хорошего финала экзамена
                Invoke(nameof(GoodFinal), 25f);
            }
            else
            {
                // Установка цвета кнопки с правильным ответом
                answersFields[id].Anim.SetInteger("disNum", 1);
                // Сокрытие всех кнопок ответов
                HideAnswersButtons();

                // Обновление таймера при сложном режиме
                if (isHardmode == true)
                {
                    timer = 30f;
                }
                // В зависимости от количества уже использованных вопросов
                // в блоке - вызов нового блока или выбор нового вопроса
                // в рамках текущего
                if (ElemsUsedInSect >= QuestionCntInSection)
                    NewSection();
                else
                    NewElemInSection();
            }
        }
        else
        {
            // Поставлено условие, что сейчас последний вопрос
            if (currentStage == lastStage)
            {
                // Сокрытие всех кнопок ответов
                HideAnswersButtons();

                // Вызов музыкальной темы последнего вопроса
                MusicBox.Play("Last step");

                // Вывод текста диалога мага относительно последнего вопроса
                Manager.StartDialogue(new List<string> { 
                    (Resources.Load("ExaminationLast") as Dialogue).text });

                // Приостановка таймера игры и выключение слайдера таймера
                // в случае сложного режима
                if (isHardmode == true)
                {
                    TimerSlider.gameObject.SetActive(false);
                    paused = true;
                }

                // В зависимости от наличия права на ошибку, вызов
                // наихудшего финала экзамена или почти хорошего
                if (chanceUsed)
                    Invoke(nameof(WorstFinal), 25f);
                else
                    Invoke(nameof(AlmostGoodFinal), 25f);
            }
            else
            {
                // Установка цвета кнопки с неправильным ответом
                answersFields[id].Anim.SetInteger("disNum", -1);
                // Сокрытие всех кнопок ответов
                HideAnswersButtons();
                // Поставлено условие, что право на ошибку уже использовано
                if (chanceUsed == true)
                {
                    // Скрытие слайдера таймера и приостановка таймера
                    // в случае сложного режима
                    if (isHardmode == true)
                    {
                        TimerSlider.gameObject.SetActive(false);
                        paused = true;
                    }

                    // Вызов функции плохого финала экзамена
                    BadFinal();
                }
                else
                {
                    // Изменение флага права на ошибку
                    chanceUsed = true;
                    // Приостановка и сброс таймера в случае сложного режима
                    if (isHardmode == true)
                    {
                        timer = 30f;
                        paused = true;
                    }
                    // Вывод текста диалога мага относительно
                    // потери права на ошибку
                    Manager.StartDialogue(new List<string> {
                        (Resources.Load("ExaminationChanceUsed") as Dialogue).text });
                    // Проигрыш звука ошибки
                    MusicBox.PlayTransit("Problem sound");
                    // Скрытие значка наличия права на ошибку
                    HealthIcon.ButtonOff();
                    // В зависимости от количества уже использованных вопросов
                    // в блоке - вызов нового блока или выбор нового вопроса
                    // в рамках текущего, всё после задержки
                    if (ElemsUsedInSect >= QuestionCntInSection)
                        Invoke(nameof(NewSection), 8f);
                    else
                        Invoke(nameof(NewElemInSection), 8f);
                }
            }
        }
    }

    // Функция сокрытия всех кнопок ответов
    private void HideAnswersButtons()
    {
        foreach (AnswerField field in answersFields)
        {
            field.Anim.SetBool("appeared", false);
        }
    }

    // Функция наихудшего финала экзамена
    public void WorstFinal()
    {
        // Вызов соответствующей музыкальной темы
        MusicBox.Play("Worst final");
        // Вызов соответствующего текста диалога от мага
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationWorstResult") as Dialogue).text });
        // Вызов анимации магии взрыва у мага
        Wizard.SetTrigger("CastTrigger");
        // Вызов функции взрыва у персонажа игрока
        #pragma warning disable UNT0016
        Player.Invoke("Explode", 1f);
        #pragma warning restore UNT0016

        // Вызов финального меню
        StartCoroutine(ActivateAfterGameMenu(delay: 5f, showStat: true));

        // Сохранение итогов захода
        SaveCheckpoint();
    }

    // Функция плохого финала экзамена
    public void BadFinal()
    {
        // Вызов соответствующей музыкальной темы
        MusicBox.Play("Bad final");
        // Вызов соответствующего текста диалога от мага
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationFailed") as Dialogue).text});
        // Вызов анимации магии взрыва у мага
        Wizard.SetTrigger("CastTrigger");
        // Вызов функции взрыва у персонажа игрока
        #pragma warning disable UNT0016
        Player.Invoke("Explode", 1f);
        #pragma warning restore UNT0016
        // Вызов после задержки финального меню
        StartCoroutine(ActivateAfterGameMenu(delay: 5f, showStat: true));

        // Сохранение итогов захода
        SaveCheckpoint();
    }

    // Функция почти хорошего финала экзамена
    public void AlmostGoodFinal()
    {
        // Скрытие значка наличия права на ошибку
        HealthIcon.ButtonOff();
        // Приостановка музыки
        MusicBox.Play("Invitation");
        // Вызов соответствующего текста диалога от мага
        Manager.StartDialogue(new List<string> {
            (Resources.Load("ExaminationAlmostGood") as Dialogue).text });
        // Вызов хорошего финала экзамена
        Invoke(nameof(GoodFinal), 8f);
    }

    // Функция хорошего финала экзамена
    public void GoodFinal()
    {
        // Скрытие значка наличия права на ошибку
        HealthIcon.ButtonOff();
        // Вызов соответствующей музыкальной темы
        MusicBox.Play("Good final");
        // Вызов соответствующего текста диалога от мага
        Manager.StartDialogue(new List<string> { 
            (Resources.Load("ExaminationSuccess") as Dialogue).text });
        // Вызов анимации проявления радости
        Player.Celebrate();
        // Смена типа анимации бездействия
        Player.ChangeIdle();
        // Вызов после задержки функции финальной проходки персонажа
        Invoke(nameof(FinalWalk), 15f);
    }

    // Функция финальной проходки персонажа
    public void FinalWalk()
    {
        // Отключение текстового поля экзамена
        Manager.TextField.enabled = false;
        // Вызов анимации ожидания мага
        Wizard.SetTrigger("NextTrigger");
        // Запуск движения объекта храма
        FindObjectOfType<MoveShrine>().Restart();
        // Вызов анимации ходьбы персонажа игроки
        Player.StartWalk();
        // Вызов облачной заметки
        CloudNote.TurnCloudNote("Advanced", withoutSound: true);
    }

    // Функция сохранения текущего достигнутого блока вопросов
    public void SaveCheckpoint()
    {
        int best = PlayerPrefs.GetInt("bestSection", 0);
        if (currentSection > best)
        {
            PlayerPrefs.SetInt("bestSection", currentSection);
            PlayerPrefs.Save();
        }
    }

    // Функция вызова хорошей концовки игры
    public void HappyEnd()
    {
        // Вызов анимации проявления радости персонажа игрока
        Player.Celebrate();
        // Вывод облачной заметки об открытых режимах и бонусах
        // в случае если они ранее не были открыты
        if (isHardmode == true)
        {
            if (PlayerPrefs.GetInt("hardmodeCompleted", 0) == 0)
            {
                PlayerPrefs.SetInt("hardmodeCompleted", 1);
                CloudNote.TurnCloudNote("Теперь вам открыт новый фон!\nВы " +
                                        "полностью прошли игру, поздравляем!", 
                                        withoutSound: true);
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("regularCompleted", 0) == 0)
            {
                PlayerPrefs.SetInt("regularCompleted", 1);
                CloudNote.TurnCloudNote("Теперь вам открыт новый фон и " +
                                        "режим игры на время", 
                                        withoutSound: true);
            }
        }

        // Сохранение итогов захода
        SaveCheckpoint();

        // Выключение тени текстового поля экзамена
        Manager.gameObject.GetComponent<Shadow>().enabled = false;
        // Изменение положения текста в тестовом поле экзамена
        Manager.TextField.alignment = TextAnchor.MiddleCenter;
        // Включение текстового поля экзамена
        Manager.TextField.enabled = true;
        // Вывод соответствующего текста диалога
        if (isHardmode == true)
            Manager.StartDialogue(new List<string> 
                {"Англия будет нашей, товарищ главнокомандующий!"});
        else
            Manager.StartDialogue(new List<string> {"Англия будет нашей!"});
        // Включение игрового меню конца игры
        StartCoroutine(ActivateAfterGameMenu(delay: 0f));
    }

    // Функция вывода меню конца игры
    IEnumerator ActivateAfterGameMenu(float delay, bool showStat = false)
    {
        yield return new WaitForSeconds(delay);
        // Активация и включение кнопки перезагрузки сцены
        RestartButton.GetComponent<CircleCollider2D>().enabled = true;
        RestartButton.GetComponent<SpriteRenderer>().enabled = true;
        RestartButton.transform.Find("Icon").gameObject.
            GetComponent<SpriteRenderer>().enabled = true;
        RestartButton.transform.Find("Diod").gameObject.
            GetComponent<SpriteRenderer>().enabled = true;

        // Если необходимо вывод статистики об игре
        if (showStat == true)
        {
            yield return new WaitForSeconds(2f);
            // Вызов соответствующего текста диалога от мага
            Manager.StartDialogue(new List<string> {
            "Текущий уровень: " + levels[currentSection] +
            "\nВсего правильных ответов: " + realRes.ToString()});
        }
    }

    // Функция, запускаемая раз в некоторое фиксированное количество кадров
    private void FixedUpdate()
    {
        // Поставлены условия, что включён сложный режим, взят существующий
        // блок вопросов и таймер не стоит на паузе
        if (isHardmode == true && currentStage > -1 && paused == false)
        {
            // Обновление таймера
            timer -= Time.deltaTime;
            // Обновление слайдера таймера
            TimerSlider.value = 1 - timer / 30f;
            // Поставлено условие срабатывания таймера
            if (timer <= 0)
            {
                // Выключение таймера слайдера
                TimerSlider.gameObject.SetActive(false);
                // Остановка таймера
                paused = true;
                // Выключение всех полей ответов
                foreach (AnswerField field in answersFields)
                {
                    field.Anim.SetBool("appeared", false);
                }
                // Вызов функции плого завершения экзамена
                BadFinal();
            }
        }

        // Поставлено условие остановки текущей музыкальной темы
        if (!MusicBox.IsPlaying())
        {
            // Получение номера новой музыкальной темы
            curMelody++;
            if (curMelody == melodyCnt)
                curMelody = 0;
            // Запуск новой темы
            MusicBox.Play("Exam #" + curMelody.ToString(), false);
        }
            
    }
}

// Объект для хранения поля ответа
public class AnswerField
{
    private readonly string name;   // Название поля ответа
    private readonly Text text;     // Текущий текст ответа
    private readonly Animator anim; // Анимационный компонент поля ответа
    // Переменные для публичного доступа к полям поля ответа
    public string Name => this.name;
    public Text Text => this.text;
    public Animator Anim => this.anim;

    // Функция инициализации поля ответа
    public AnswerField(GameObject obj)
    {
        name = obj.name;
        text = obj.GetComponentInChildren<Text>();
        anim = obj.GetComponent<Animator>();
    }
}