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
    public partial class AuditCreation : ContentPage
    {
        InspectionDatabase db = new InspectionDatabase();
        public AuditCreation()
        {
            InitializeComponent();
            BindingContext = new AuditCreationViewModel();
            List<AuditTemplate> templates = new List<AuditTemplate>();
            templates = db.GetAllTemplates();
            ddlTemplate.ItemsSource = templates;
        }

        private void OnNextClick(object sender, EventArgs args)
        {
            AuditDetails audit = new AuditDetails();
            audit.TemplateId =  ((AuditTemplate)ddlTemplate.SelectedItem).Id;
            audit.Location =  txtLocation.Text;            
            audit.UserId = "Kenjale,Pooja";
            audit.CreatedOn = DateTime.Now.Date;
            int auditID = db.SaveAudit(audit);
			Navigation.PushModalAsync(new AuditQuestionAnswers(auditID,true));
        }
    }

    class AuditCreationViewModel : INotifyPropertyChanged
    {

        public AuditCreationViewModel()
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