using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZumpaReader.Model;
using ZumpaReader.WebService;
using ZWS = ZumpaReader.WebService.WebService;

namespace ZumpaReader.Commands
{
    public class SurveyCommand : BaseLoadCommand
    {   
        private IWebService _service;
        private Action<Survey> _callback;

        public SurveyCommand(IWebService service, Action<Survey> callback) :base(service)
        {
            _service = service;
            _callback = callback;

        }
        public async override void Execute(object parameter)
        {
            CanExecuteIt = false;
            Survey.SurveyVoteItem item = parameter as Survey.SurveyVoteItem;
            if (item != null)
            {
                try
                {
                    EnsureInternet();
                    EnsureLoggedIn();

                    ZWS.ContextResult<Survey> result = await _service.VoteSurvey(item.SurveyID, item.Index);
                    if (_callback != null)
                    {
                        _callback.Invoke(result.Context);
                    }
                }
                catch (Exception e)
                {
                    ShowError(e);
                }
                
            }
            CanExecuteIt = true;       
        }
    }
}
