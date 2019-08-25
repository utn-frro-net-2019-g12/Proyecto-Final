using Caliburn.Micro;
using DesktopPresentationWPF.EventModels;
using DesktopPresentationWPF.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopPresentationWPF.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "nico@example.com";
        // TO-DO: Hash this
        private string _password = "Example1?";
        private IApiHelper _apiHelper;
        private IEventAggregator _events;

        public LoginViewModel(IApiHelper apiHelper,IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get{ return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password;}
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;

                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public bool CanLogIn
        {
            get
            {
                // Not Working, Revise this
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = "";

                var response = await _apiHelper.Authenticate(UserName, Password);

                // Capture information about the user in apiHelper singleton instance
                await _apiHelper.GetLoggedInUserInfo(response.Access_Token);

                _events.PublishOnUIThread(new LogOnEvent());
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}
