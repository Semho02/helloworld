using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public TMP_Text questText;
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap=CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventsManager.instance.questEvents.onFinishQuest -= FinishQuest;
    }
    
    private void Start()
    {
        DataPersistenceManager.instance.LoadGame();
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventsManager.instance.questEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        //bool meetsRequirements = true;

        //// ѕровер€ем выполнение требований квеста в момент загрузки игры
        //foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        //{
        //    // ≈сли состо€ние квеста не соответствует требуемому, мен€ем флаг на false
        //    if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
        //    {
        //        meetsRequirements = false;
        //        break;
        //    }
        //}

        //// ≈сли требовани€ выполнены, выводим отладочное сообщение
        //if (meetsRequirements)
        //{
        //    Debug.Log($"All requirements met for quest {quest.info.displayName}");
        //}
        //else
        //{
        //    Debug.Log($"Not all requirements met for quest {quest.info.displayName}");
        //}

        //// ≈сли квест находитс€ в состо€нии REQUIREMENTS_NOT_MET и все требовани€ выполнены, измен€ем его состо€ние на CAN_START
        //if (quest.state == QuestState.REQUIREMENTS_NOT_MET && meetsRequirements)
        //{
        //    ChangeQuestState(quest.info.id, QuestState.CAN_START);
        //}

        //return meetsRequirements;

        bool meetsRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }
        return meetsRequirements;
    }

    private void Update()
    {
        foreach(Quest quest in questMap.Values)
        {
            if(quest.state==QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
        questText.text = "Quest: " + quest.info.displayName;
    }
    private void AdvanceQuest(string id)
    {
        //Quest quest = GetQuestById(id);
        //Debug.Log($"Advancing quest {quest.info.displayName}");

        //// ѕровер€ем, выполнены ли требовани€ квеста
        //bool requirementsMet = CheckRequirementsMet(quest);

        //// ѕровер€ем, есть ли ещЄ шаги в квесте
        //if (quest.CurrentStepExists())
        //{
        //    // ѕровер€ем наличие текущего шага перед его использованием
        //    if (quest.CurrentStepExists())
        //    {
        //        quest.InstantiateCurrentQuestStep(this.transform);
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"Tried to advance quest {quest.info.displayName}, but no current step exists.");
        //    }

        //    // ≈сли требовани€ выполнены, переходим к следующему шагу
        //    if (requirementsMet)
        //    {
        //        quest.MoveToNextStep();
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"Cannot advance quest {quest.info.displayName}: requirements are not met.");
        //    }
        //}
        //else
        //{
        //    // ≈сли больше нет шагов, мы можем закончить квест
        //    if (!quest.CurrentStepExists() && requirementsMet)
        //    {
        //        ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        //        questText.text = "Quest: " + quest.info.displayName + " Done!";
        //    }
        //    else
        //    {
        //        // если требовани€ не выполнены, квест остаетс€ в текущем состо€нии
        //        questText.text = "Quest: " + quest.info.displayName;
        //    }
        //}
        Quest quest = GetQuestById(id);
        //Debug.Log($"Advancing quest {quest.info.displayName}");
        //двигаемс€ в следущий шаг квеста
        quest.MoveToNextStep();
        //если есть ещЄ какие-то шаги. создаЄм экземпл€р следующего
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        //если больше нет шагов, мы можем закончить квест
        else
        {
            // если больше нет шагов, мы провер€ем, выполнены ли все требовани€ дл€ завершени€ квеста
            if (CheckRequirementsMet(quest))
            {
                //Debug.Log($"All requirements met for quest {quest.info.displayName}");
                ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
                questText.text = "Quest: " + quest.info.displayName + " Done!";
            }
            else
            {
                // если требовани€ не выполнены, квест остаетс€ в текущем состо€нии
                //Debug.Log($"Requirements not met for quest {quest.info.displayName}");
                questText.text = "Quest: " + quest.info.displayName;
            }
        }
    }
    private void FinishQuest(string id)
    {
        Quest quest=GetQuestById(id);
        ClaimRewards(quest, quest.info.moneyReward);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
        questText.text = "";
        //Debug.Log($"Finished quest {id} with final state {quest.state}");
        //Quest quest = GetQuestById(id);

        //// ѕровер€ем, выполнены ли требовани€ квеста
        //bool requirementsMet = CheckRequirementsMet(quest);

        //if (requirementsMet)
        //{
        //    ClaimRewards(quest, quest.info.moneyReward);
        //    ChangeQuestState(quest.info.id, QuestState.FINISHED);
        //    questText.text = "";
        //}
    }

    private void ClaimRewards(Quest quest, int moneyReward)
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.AddMoney(moneyReward);
    }


    private Dictionary<string, Quest> CreateQuestMap()
    {
        //«агружаем всЄ в папку Assets/Resources/Quests
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.LogWarning("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }
    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if(quest == null)
        {
            Debug.LogError("ID not found in the QuestMap: " + id);
        }
        return quest;
    }

    public void LoadData(GameData data)
    {
        //Debug.Log("Loading quest states...");
        foreach (var questState in data.questStates)
        {
            if (questMap.ContainsKey(questState.Key))
            {
                // ”станавливаем состо€ние квеста из сохраненных данных
                questMap[questState.Key].state = questState.Value;
                //Debug.Log($"Loaded quest {questState.Key} with state {questState.Value}");
                GameEventsManager.instance.questEvents.QuestStateChange(questMap[questState.Key]);
            }
            else
            {
                Debug.LogWarning($"Quest ID {questState.Key} not found in questMap during load.");
            }
        }
        foreach (var quest in questMap.Values)
        {
            if (quest.state == QuestState.IN_PROGRESS)
            {
                quest.InstantiateCurrentQuestStep(this.transform);
                questText.text = "Quest: " + quest.info.displayName;
            }
            else if (quest.state == QuestState.CAN_FINISH)
            {
                questText.text = "Quest: " + quest.info.displayName + " Done!";
            }
        }
    }

    public void SaveData(GameData data)
    {
        //Debug.Log("Saving quest states...");
        foreach (var quest in questMap)
        {
            if (data.questStates.ContainsKey(quest.Key))
            {
                data.questStates[quest.Key] = quest.Value.state;
            }
            else
            {
                data.questStates.Add(quest.Key, quest.Value.state);
            }
            //Debug.Log($"Saved quest {quest.Key} with state {quest.Value.state}");
        }
    }
}
