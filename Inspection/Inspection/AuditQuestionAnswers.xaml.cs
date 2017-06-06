using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Inspection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuditQuestionAnswers : ContentPage
    {
        InspectionDatabase db = new InspectionDatabase();
        int auditID;
        public AuditQuestionAnswers(int auditId, bool isNewAudit)
        {
            InitializeComponent();
            BindingContext = new AuditQuestionAnswersViewModel();
         
            auditID = auditId;            
            ddlAnswer4.Items.Add("Always");
            ddlAnswer4.Items.Add("Never");
            ddlAnswer4.Items.Add("Sometimes");

            List<TemplateQuestions> questions = new List<TemplateQuestions>();
            questions = db.GetTemplateQuestions();
            lblQuestion1.Text = questions[0].QuestionDescription;
            lblQuestion2.Text = questions[1].QuestionDescription;
            lblQuestion3.Text = questions[2].QuestionDescription;
            lblQuestion4.Text = questions[3].QuestionDescription;

            if(!isNewAudit)
            {
                List<AuditAnswers> answers = new List<AuditAnswers>();
                answers = db.GetAuditAnswersByID(auditID);
                if(answers.Count >0)
                {
                    txtEntry1.Text = answers[0].Answer;
                    datePicker.Date = DateTime.Parse(answers[1].Answer);
                    chkFlag.IsToggled = bool.Parse(answers[2].Answer);
                    List<AuditTemplate> templates = new List<AuditTemplate>();
                    //templates = db.GetAllTemplates();
                    //AuditTemplate templateselected= templates.Where(i => i.Name.ToLower() == answers[3].Answer.ToLower()).FirstOrDefault();
                    //ddlAnswer4.SelectedItem = templateselected;    
                    ddlAnswer4.SelectedItem = answers[3].Answer;
                    btnSave.IsVisible = false;
                    
                }
            }
        }

        private void OnSaveClick(object sender, EventArgs args)
        {

            List<AuditAnswers> userAnswers = new List<AuditAnswers>();
            AuditAnswers answer1 = new AuditAnswers();
            answer1.AuditId = Convert.ToInt32(auditID);
            answer1.Answer = txtEntry1.Text;
            answer1.QuestionId = 1;
            answer1.ImagePath = "";
            userAnswers.Add(answer1);

            AuditAnswers answer2 = new AuditAnswers();
            answer2.AuditId = Convert.ToInt32(auditID);
            answer2.Answer = datePicker.Date.ToString();
            answer2.QuestionId = 2;
            answer2.ImagePath = string.Empty;
            userAnswers.Add(answer2);

            AuditAnswers answer3 = new AuditAnswers();
            answer3.AuditId = Convert.ToInt32(auditID);
            answer3.Answer = chkFlag.IsToggled.ToString();
            answer3.QuestionId = 3;
            answer3.ImagePath = string.Empty;
            userAnswers.Add(answer3);

            AuditAnswers answer4 = new AuditAnswers();
            answer4.AuditId = Convert.ToInt32(auditID);
            answer4.Answer = ddlAnswer4.SelectedItem.ToString();
            answer4.QuestionId = 4;
            answer4.ImagePath = string.Empty;
            userAnswers.Add(answer4);
            db.SaveAnswers(userAnswers);

            Navigation.PushModalAsync(new MainPage());
        }
    }

   
    class AuditQuestionAnswersViewModel : INotifyPropertyChanged
    {

        public AuditQuestionAnswersViewModel()
        {
            IncreaseCountCommand = new Command(IncreaseCount);
        }

        int count;

        string countDisplay = "You clicked 0 times.";
        public string CountDisplay
        {
            get { return countDisplay; }
            set { countDisplay = value; OnPropertyChanged(); }
        }

        public ICommand IncreaseCountCommand { get; }

        void IncreaseCount() =>
            CountDisplay = $"You clicked {++count} times";


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}