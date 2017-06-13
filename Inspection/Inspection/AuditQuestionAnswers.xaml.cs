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
		List<AuditData> auditDatas;
		List<AuditAnswers> answers;
		public AuditQuestionAnswers(int auditId, bool isNewAudit)
        {
			App.Current.Properties["IsNewAudit"] = isNewAudit;
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
                answers = new List<AuditAnswers>();
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

					//Camera functionality
					if (answers[0].ImagePath != string.Empty)
						btnView1.IsVisible = true;
					if (answers[1].ImagePath != string.Empty)
						btnView2.IsVisible = true;
					if (answers[2].ImagePath != string.Empty)
						btnView3.IsVisible = true;
					if (answers[3].ImagePath != string.Empty)
						btnView4.IsVisible = true;

					btnCamera1.IsVisible = false;
					btnCamera2.IsVisible = false;
					btnCamera3.IsVisible = false;
					btnCamera4.IsVisible = false;

				}
            }

			if (!App.Current.Properties.ContainsKey("AuditData"))
			{
				auditDatas = new List<AuditData>();
				FillElements(auditDatas, answers);
			}
			else
			{
				auditDatas = (List<AuditData>)App.Current.Properties["AuditData"];
				if (auditDatas[0].ImageUrl != string.Empty)
				{
					btnView1.IsVisible = true;
					btnCamera1.IsVisible = false;
				}
				if (auditDatas[1].ImageUrl != string.Empty)
				{
					btnView2.IsVisible = true;
					btnCamera2.IsVisible = false;
				}
				if (auditDatas[2].ImageUrl != string.Empty)
				{
					btnView3.IsVisible = true;
					btnCamera3.IsVisible = false;
				}
				if (auditDatas[3].ImageUrl != string.Empty)
				{
					btnView4.IsVisible = true;
					btnCamera4.IsVisible = false;
				}

				txtEntry1.Text = auditDatas[0].Answer;
				datePicker.Date = DateTime.Parse(auditDatas[1].Answer);
				chkFlag.IsToggled = bool.Parse(auditDatas[2].Answer);
				ddlAnswer4.SelectedItem = auditDatas[3].Answer;
			}			

			btnCamera1.Clicked += (sender, e) => { BtnCamera_Clicked(sender, e, "Question1"); };
			btnCamera2.Clicked += (sender, e) => { BtnCamera_Clicked(sender, e, "Question2"); };
			btnCamera3.Clicked += (sender, e) => { BtnCamera_Clicked(sender, e, "Question3"); };
			btnCamera4.Clicked += (sender, e) => { BtnCamera_Clicked(sender, e, "Question4"); };

			btnView1.Clicked += (sender, e) => { BtnView_Clicked(sender, e, auditDatas[0].ImageUrl); };
			btnView2.Clicked += (sender, e) => { BtnView_Clicked(sender, e, auditDatas[1].ImageUrl); };
			btnView3.Clicked += (sender, e) => { BtnView_Clicked(sender, e, auditDatas[2].ImageUrl); };
			btnView4.Clicked += (sender, e) => { BtnView_Clicked(sender, e, auditDatas[3].ImageUrl); };
		}

		void BtnCamera_Clicked(object sender, EventArgs e, string cameraClicked)
		{
			auditDatas[0].Answer = txtEntry1.Text;
			auditDatas[1].Answer = datePicker.Date.ToString();
			auditDatas[2].Answer = chkFlag.IsToggled.ToString();
			auditDatas[3].Answer = (string)ddlAnswer4.SelectedItem;
			App.Current.Properties["AuditData"] = auditDatas;
			App.Current.Properties["cameraClicked"] = cameraClicked;
			Navigation.PushModalAsync(new Camera(false, string.Empty));
		}

		void BtnView_Clicked(object sender, EventArgs e, string imgUrl)
		{
			Navigation.PushModalAsync(new Camera(true, imgUrl));
		}

		void FillElements(List<AuditData> auditData, List<AuditAnswers> answers)
		{
			string imgUrl1 = string.Empty;
			string imgUrl2 = string.Empty;
			string imgUrl3 = string.Empty;
			string imgUrl4 = string.Empty;
			if (answers != null && answers.Count > 1)
			{
				imgUrl1 = answers[0].ImagePath;
				imgUrl2 = answers[1].ImagePath;
				imgUrl3 = answers[2].ImagePath;
				imgUrl4 = answers[3].ImagePath;
			}

			auditData.Add(new AuditData { Question = "Question1", ImageUrl = imgUrl1});
			auditData.Add(new AuditData { Question = "Question2", ImageUrl = imgUrl2});
			auditData.Add(new AuditData { Question = "Question3", ImageUrl = imgUrl3});
			auditData.Add(new AuditData { Question = "Question4", ImageUrl = imgUrl4});
			
			App.Current.Properties["AuditData"] = auditData;
			auditDatas = auditData;
			App.Current.Properties["AuditID"] = auditID;
		}

		private void OnSaveClick(object sender, EventArgs args)
        {

            List<AuditAnswers> userAnswers = new List<AuditAnswers>();
            AuditAnswers answer1 = new AuditAnswers();
            answer1.AuditId = Convert.ToInt32(auditID);
            answer1.Answer = txtEntry1.Text;
            answer1.QuestionId = 1;
            answer1.ImagePath = auditDatas[0].ImageUrl;
            userAnswers.Add(answer1);

            AuditAnswers answer2 = new AuditAnswers();
            answer2.AuditId = Convert.ToInt32(auditID);
            answer2.Answer = datePicker.Date.ToString();
            answer2.QuestionId = 2;
            answer2.ImagePath = auditDatas[1].ImageUrl;
            userAnswers.Add(answer2);

            AuditAnswers answer3 = new AuditAnswers();
            answer3.AuditId = Convert.ToInt32(auditID);
            answer3.Answer = chkFlag.IsToggled.ToString();
            answer3.QuestionId = 3;
            answer3.ImagePath = auditDatas[2].ImageUrl;
            userAnswers.Add(answer3);

            AuditAnswers answer4 = new AuditAnswers();
            answer4.AuditId = Convert.ToInt32(auditID);
            answer4.Answer = (string)ddlAnswer4.SelectedItem;
            answer4.QuestionId = 4;
            answer4.ImagePath = auditDatas[3].ImageUrl;
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