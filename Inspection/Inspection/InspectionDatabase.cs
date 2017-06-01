using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Inspection
{
    public class InspectionDatabase
    {
        private SQLiteConnection _connection;

        public InspectionDatabase()
        {
            _connection = DependencyService.Get<ISQLite>().GetConnection();
            _connection.CreateTable<AuditTemplate>();
            _connection.CreateTable<TemplateQuestions>();
            _connection.CreateTable<AuditDetails>();
            _connection.CreateTable<AuditAnswers>();
        }

        public IEnumerable<AuditTemplate> GetAllTemplates()
        {
            return (from t in _connection.Table<AuditTemplate>()
                    select t).ToList();
        }

        public int GetTemplateCount()
        {
            return (from t in _connection.Table<AuditTemplate>()
                    select t).Count();
        }
        public AuditTemplate GetTemplate(int id)
        {
            return _connection.Table<AuditTemplate>().FirstOrDefault(t => t.Id == id);
        }

        public void DeleteAuditTemplate(int id)
        {
            _connection.Delete<AuditTemplate>(id);
        }

        public int AddTemplate(AuditTemplate temp)
        {
           return _connection.Insert(temp);
        }

        public int AddQuestion(TemplateQuestions ques)
        {
            return _connection.Insert(ques);
        }

        public int SaveAnswer(AuditAnswers ans)
        {
            return _connection.Insert(ans);
        }

        public int SaveAudit(AuditDetails audit)
        {
            return _connection.Insert(audit);
        }

        public List<AuditDetails> GetAllAudit()
        {
            List<AuditDetails> audits = (from t in _connection.Table<AuditDetails>()
                    select t).ToList();
            foreach(AuditDetails a in audits)
            {
                a.AuditDisplayName = GetTemplateNameOnID(a.TemplateId) + "-" + a.Location;
            }
            return audits;
        }

        public void DeleteALLAuditDetails()
        {
            _connection.DeleteAll<AuditDetails>();
        }
        public void SetDefaultTemplate()
        {
           

            //List<TemplateQuestions> Questions = new List<TemplateQuestions>();
            //Questions = dbUpdates.GetTemplateQuestions();
            //AuditTemplate temp1 = dbUpdates.GetAuditTemplate("Food");

            if (GetTemplateCount() == 0)
            {
                AuditTemplate foodAudit = new AuditTemplate();
                foodAudit.Name = "Appearance Care";
                foodAudit.Type = "Food";
                int id = AddTemplate(foodAudit);

                TemplateQuestions question1 = new TemplateQuestions();
                question1.TemplateId = id;
                question1.QuestionDescription = "Is there sufficient storage space? ";
                AddQuestion(question1);
                

                TemplateQuestions question2 = new TemplateQuestions();
                question2.TemplateId = id;
                question2.QuestionDescription = "Are chemicals and cleaning products stored away from food storage areas?";
                AddQuestion(question2);
                

                TemplateQuestions question3 = new TemplateQuestions();
                question3.TemplateId = id;
                question3.QuestionDescription = "Is all packaging in good condition?";
                AddQuestion(question3);

                TemplateQuestions question4 = new TemplateQuestions();
                question4.TemplateId = id;
                question4.QuestionDescription = "Have appropriate corrective actions been taken and recorded?";
                AddQuestion(question4);

                AuditTemplate foodAudit4 = new AuditTemplate();
                foodAudit4.Name = "Pest Inspection";
                foodAudit4.Type = "Food";
                AddTemplate(foodAudit4);

                AuditTemplate foodAudit5 = new AuditTemplate();
                foodAudit5.Name = "RestRoom Inspection";
                foodAudit5.Type = "Food";
                AddTemplate(foodAudit5);

                AuditTemplate foodAudit1 = new AuditTemplate();
                foodAudit1.Name = "QA Audio Visual Inspection";
                foodAudit1.Type = "Food";
                AddTemplate(foodAudit1);

                AuditTemplate foodAudit2 = new AuditTemplate();
                foodAudit2.Name = "Chevron Janitorial Services Survey Form v8";
                foodAudit2.Type = "Food";
                AddTemplate(foodAudit2);

                AuditTemplate foodAudit3 = new AuditTemplate();
                foodAudit3.Name = "Chevron Site Services - Service Quality Inspection";
                foodAudit3.Type = "Food";
                AddTemplate(foodAudit3);


                AuditTemplate foodAudit6 = new AuditTemplate();
                foodAudit6.Name = "Meeting Room Inspection ";
                foodAudit6.Type = "Food";
                AddTemplate(foodAudit6);
            }
        }

        public void SetAuditDetails()
        {
            int count = (from t in _connection.Table<AuditDetails>()
                         select t).ToList().Count;
            if(count == 0)
            {
                AuditDetails audit = new AuditDetails();
                audit.CreatedOn = DateTime.Now.Date;
                audit.Location = "CGI Mumbai";
                audit.TemplateId = 1;
                audit.UserId = "Kenjale,Pooja";
                SaveAudit(audit);

                AuditDetails audit1 = new AuditDetails();
                audit1.CreatedOn = DateTime.Now.Date;

                audit1.Location = "CGI Mumbai seepz";
                audit1.TemplateId = 2;
                audit1.UserId = "Kenjale,Pooja";
                SaveAudit(audit1);
            }
        }
        public List<TemplateQuestions> GetTemplateQuestions()
        {
            return (from t in _connection.Table<TemplateQuestions>()
                    select t).ToList();
        }
        public List<AuditAnswers> GetAuditAnswersByID(int id)
        {
            return _connection.Table<AuditAnswers>().Where(t => t.AuditId == id).ToList();
        }
        public string GetTemplateNameOnID(int id)
        {
            return _connection.Table<AuditTemplate>().FirstOrDefault(t => t.Id == id).Name;
        }

        public void SaveAnswers(List<AuditAnswers> questionAnswers)
        {
            foreach (AuditAnswers ans in questionAnswers)
            {
                SaveAnswer(ans);
            }
        }

       
        //public int SaveDefaultAudit()
        //{
        //    AuditDetails audit1 = new AuditDetails();
        //    //audit.Id = 1;
        //    audit1.TemplateId = 1;
        //    audit1.Location = "CGI Mumbai";
        //    audit1.UserId = "CGI Admin";
        //    audit1.GPSCoordinate = "20,20";
        //    int audit1Id = dbUpdates.SaveAuditDetail(audit1);
        //    //AuditDetails audit = new AuditDetails();
        //    ////audit.Id = 1;
        //    //audit.TemplateId = 1;
        //    //audit.Location = "Seepz Mumbai";
        //    //audit.UserId = "Seepz Admin";
        //    //audit.GPSCoordinate = "20,20";
        //    //int idtest= dbUpdates.SaveAuditDetail(audit);
        //    return audit1Id;
        //}

       
    }
}
