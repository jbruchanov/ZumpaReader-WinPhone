using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Model
{
    public class Survey
    {
        [JsonProperty("Question")]
        public string Question { get; set; }

        [JsonProperty("Responds")]
        public int Responds { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("Answers")]
        public string[] Answers { get; set; }

        [JsonProperty("Percents")]
        public int[] Percents { get; set; }

        [JsonProperty("VotedItem")]
        public int VotedItem { get; set; }

        public Survey()
        {
            VotedItem = -1;
        }

        private string _questionResps;
        [JsonIgnore]
        public string QuestionResps
        {
            get
            {
                if (_questionResps == null)
                {
                    _questionResps = String.Format("{0} ({1})", Question, Responds);
                }
                return _questionResps;
            }
        }

        private SurveyVoteItem[] _surveyVoteItems;
        [JsonIgnore]
        public SurveyVoteItem[] SurveyVoteItems
        {
            get
            {
                if (_surveyVoteItems == null)
                {
                    _surveyVoteItems = new SurveyVoteItem[Answers.Length];
                    for (int i = 0, n = _surveyVoteItems.Length; i < n; i++)
                    {
                        _surveyVoteItems[i] = new SurveyVoteItem { Index = i, 
                                                                   SurveyID = ID,
                                                                   Answer = String.Format("{0} ({1}%)", Answers[i], Percents[i]), 
                                                                   Percentage = Percents[i],
                                                                   Enabled = VotedItem != i };
                    }
                }
                return _surveyVoteItems;
            }
        }

        public class SurveyVoteItem
        {
            public int SurveyID {get;set;}
            public int Index { get; set; }
            public int Percentage { get; set; }
            public string Answer { get; set; }
            public bool Enabled {get;set;}
        }
    }
}
