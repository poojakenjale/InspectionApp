using System;
using SQLite.Net.Attributes;
namespace Inspection
{
   public class AuditTemplate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public String Name { get; set; }
        public String Type { get; set; }


        public override String ToString()
        {
            return Name;
        }
    }
}
