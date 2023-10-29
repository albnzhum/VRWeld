using UnityEngine;
using UnityEngine.UI;

namespace ProfTestium
{
    public class AuthMenu : MonoBehaviour
    {
        [SerializeField] private Button authButton;
        
        [SerializeField] private Button viewPassBtn;
        
        [SerializeField] private InputField login;
        
        [SerializeField] private InputField password;
        
        [SerializeField] private Sprite viewPasSpt;
        
        [SerializeField] private Sprite disabledPass;
        
        [SerializeField] private AuthManager AuthManager;
        [SerializeField] private ModulesManager ModulesManager ;
        
        private void Awake()
        {
            authButton.onClick.AddListener(OnClick);
            viewPassBtn.onClick.AddListener(OnViewPassBtnClick);
        }

        private void OnViewPassBtnClick()
        {
            if (password.contentType == InputField.ContentType.Password)
            {
                password.contentType = InputField.ContentType.Standard;
            }
            else
            {
                password.contentType = InputField.ContentType.Password;
            }

            password.ForceLabelUpdate();
            
            switch (password.contentType)
            {
                case InputField.ContentType.Password:
                    viewPassBtn.image.sprite = viewPasSpt;
                    break;
                case InputField.ContentType.Standard:
                    viewPassBtn.image.sprite = disabledPass;
                    break;
            }
        }

        private void OnClick()
        {
            StartCoroutine(AuthManager.LoginUser(login.text, password.text));
            
        }
    }
}