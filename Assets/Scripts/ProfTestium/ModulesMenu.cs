
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProfTestium
{
    public class ModulesMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loading;
        [SerializeField] private Button login;
        [SerializeField] private ModulesManager ModulesManager;

        private void OnEnable()
        {
            StartCoroutine(ModulesManager.CreateModules(PlayerPrefs.GetString("AccessToken")));
            login.onClick.AddListener(OnLoginButtonClick);

            
        }
        private void OnLoginButtonClick()
        {
            loading.SetActive(true);
        }

        private UnityAction OnItemSelected(int id)
        {
            PlayerPrefs.SetInt("ModuleId", id);
            return () => Debug.Log("Module with ID " + id + " selected.");
        }
    }
}