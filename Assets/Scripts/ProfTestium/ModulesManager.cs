using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ProfTestium
{
    public class ModulesManager : MonoBehaviour
    {
        private string baseUrl = "http://xn----7sbpbfclakh1al9a7fxc.xn--p1ai:8000/";
        private string modulesEndpoint = "tests/modules/get_all";
        
        [SerializeField] private Button moduleName;
        [SerializeField] private GameObject parent;
        
        public IEnumerator CreateModules(string accessToken)
        {
            string url = baseUrl + modulesEndpoint;
            
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", "Bearer " + accessToken);
            request.SetRequestHeader("accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
        
            yield return request.SendWebRequest();
            Debug.Log(request.responseCode);
            string responseText = request.downloadHandler.text;
            print(responseText);
            PlayerPrefs.SetString("ListKey", responseText);

            if (request.responseCode == 200)
            {
                Debug.Log("success");
                
                string json = PlayerPrefs.GetString("ListKey");
                List<Module> modules = JsonConvert.DeserializeObject< List<Module>>(json);
                foreach (var module in modules)
                {
                    Button mod = Instantiate(moduleName, parent.transform);
                    mod.GetComponentInChildren<Text>().text = module.name;
                    mod.onClick.AddListener(OnItemSelected(module.id_));
                }
            }
            else
            {
                Debug.LogError(request.responseCode);
            }
        }
        
        private UnityAction OnItemSelected(int id)
        {
            PlayerPrefs.SetInt("ModuleId", id);
            return () => Debug.Log("Module with ID " + id + " selected.");
        }
    }

    [System.Serializable]
    public class ModulesList
    {
        public List<Module> _modules;

        public ModulesList(List<Module> modulesList)
        {
            _modules = modulesList;
        }
    }

    [System.Serializable]
    public class Module
    {
        public int id_;
        public string name;
        public string portal_id;
        public string url_file;

        public Module(int id, string name, string portalID, string urlFile)
        {
            this.id_ = id;
            this.name = name;
            portal_id = portalID;
            url_file = urlFile;
        }
    }
}