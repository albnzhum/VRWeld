using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ProfTestium;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public enum CriterionName
{
    WeldDress,
    LegDress,
    Gloves,
    Mask,
    Rug,
    WireChecked,
    Grounding, 
    WireLengthSetted,
    SwitchOn,
    ElectrodeInHolder,
    SwitchOff,
    CindersRemoved,
    DetailOnTable,
    TableIsClear
}

public class WeldProcess : MonoBehaviour
{
    [SerializeField] private List<Criterion> _criteria = new List<Criterion> { 
        new Criterion(CriterionName.WeldDress, "Сварочный костюм"),
        new Criterion(CriterionName.LegDress, "Спец. обувь"),
        new Criterion(CriterionName.Gloves, "Перчатки"),
        new Criterion(CriterionName.Mask, "Маска"),
        new Criterion(CriterionName.Rug, "Коврик"),
        new Criterion(CriterionName.WireChecked, "Целостность провода"),
        new Criterion(CriterionName.Grounding, "Заземление"),
        new Criterion(CriterionName.WireLengthSetted, "Длина шнура"),
        new Criterion(CriterionName.SwitchOn, "Включение сварочного аппарата"),
        new Criterion(CriterionName.ElectrodeInHolder, "Установка электрода в держак"),
        new Criterion(CriterionName.SwitchOff, "Выключение сварочного аппарата"),
        new Criterion(CriterionName.CindersRemoved, "Огарки выброшены"),
        new Criterion(CriterionName.DetailOnTable, "Готовая деталь"),
        new Criterion(CriterionName.TableIsClear, "Соблюдение пожарной безопасности")
    };

    public List<Criterion> Criteria => _criteria;

    public bool DetailIsComplete;

    private DateTime sceneStartTime;
    private TimeSpan duration;

    public static WeldProcess Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SceneTimeTracker timeTracker = FindObjectOfType<SceneTimeTracker>();
        if (timeTracker != null)
        {
            sceneStartTime = timeTracker.GetSceneStartTime();
            duration = timeTracker.GetSceneDuration();
        }
        else
        {
            Debug.LogWarning("SceneTimeTracker is not found in scene");
        }
    }

    public DateTime GetSceneStartTime() => sceneStartTime;
    public TimeSpan GetSceneDuration() => duration;

    public Criterion GetCriterion(CriterionName name)
    {
        foreach (var criterion in _criteria)
        {
            if (criterion.Name == name)
            {
                return criterion;
            }
        }

        return null;
    }

    public void SetCriterion(CriterionName criterionName, bool complete)
    {
        foreach (var criterion in _criteria)
        {
            if(criterion.Name == criterionName)
            {
                criterion.Complete = complete;
                break;
            }
        }
    }

    public int GetCompletedCriteriaCount()
    {
        int count = 0;

        foreach (var criterion in _criteria)
        {
            if (criterion.Complete)
            {
                count++;
            }
        }

        return count;
    }
    
    public string GetIncompletedCriteriaCount()
    {
        foreach (var criterion in _criteria)
        {
            if (criterion.Complete == false)
            {
                return criterion.Name.ToString();
            }
        }

        return null;
    }

    public void DropResults()
    {
        foreach (var criterion in _criteria)
        {
            criterion.Complete = false;
        }
    }
}

[Serializable]
public class Criterion
{
    public CriterionName Name;
    public string Description;
    public bool Complete;

    public Criterion(CriterionName name, string description)
    {
        Name = name;
        Complete = false;
        Description = description;
    }
}
