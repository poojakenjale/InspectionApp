using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Inspection
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            InspectionDatabase db = new InspectionDatabase();
            List<AuditDetails> audits = db.GetAllAudit();
            listViewAudits.ItemsSource = audits;
            
        }
        void OnAddAuditClick(object sender, EventArgs args)
        {
            Navigation.PushModalAsync(new AuditCreation());
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            listViewAudits.BeginRefresh();
            InspectionDatabase db = new InspectionDatabase();
            List<AuditDetails> audits = db.GetAllAudit();
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                listViewAudits.ItemsSource = audits;
            else
            {
                List<AuditDetails> auditseach = audits.Where(i => i.AuditDisplayName.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                listViewAudits.ItemsSource = auditseach;
            }

            listViewAudits.EndRefresh();
        }     

        private void listViewAudits_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Navigation.PushModalAsync(new AuditQuestionAnswers(((AuditDetails)e.SelectedItem).Id, false));
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
