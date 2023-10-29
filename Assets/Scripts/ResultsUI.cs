using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _criteriaCount;
    [SerializeField] private TextMeshProUGUI _dropReason;

    [SerializeField] private CriterionElement _criteriaPrefab;

    [SerializeField] private Transform _criteriaList;
    [SerializeField] private ScoreManager ScoreManager;

    private void OnEnable()
    {
        if (!WeldProcess.Instance.GetCriterion(CriterionName.TableIsClear).Complete)
        {
            WeldProcess.Instance.DropResults();
            _dropReason.text = "Не соблюдена пожарная безопасность!";
        }

        foreach (var criterion in WeldProcess.Instance.Criteria)
        {
            var c = Instantiate(_criteriaPrefab, _criteriaList);
            c.Text.text = criterion.Description;
            c.Toggle.isOn = criterion.Complete;
        }

        _criteriaCount.text = $"Набрано баллов: {WeldProcess.Instance.GetCompletedCriteriaCount()}";
    }

    public void Exit()
    {
        string loadedAccessToken = PlayerPrefs.GetString("AccessToken");
        string loadedRefreshToken = PlayerPrefs.GetString("RefreshToken");
        StartCoroutine(ScoreManager.CreateSession(
            WeldProcess.Instance.GetSceneStartTime(), WeldProcess.Instance.GetSceneDuration(),
            14, 14, WeldProcess.Instance.GetIncompletedCriteriaCount(), loadedAccessToken, loadedRefreshToken));
        
        PlayerPrefs.DeleteAll();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}
