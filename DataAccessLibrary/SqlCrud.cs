using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqlCrud
    {
        private readonly string _connectionString;
        private SqlDataAccess db = new SqlDataAccess();

        public SqlCrud(string connectionString)
        {
            _connectionString=connectionString;
        }


        public List<BasicContactModel> GetAllContacts()
        {
            string sql = "SELECT Id, FirstName, LastName FROM dbo.Contacts";

            return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
        }


        public FullContactModel GetFullContactById(int id)
        {

            string sql = "select Id, FirstName, LastName from dbo.Contacts where Id = @Id ";

            FullContactModel output = new FullContactModel();

            output.BasicInfo =  db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id}, _connectionString).FirstOrDefault();
            if (output.BasicInfo == null) 
            {

                throw new Exception("User Not Found");
            }

            sql = @"select EmailAddresses.Id, EmailAddress from dbo.EmailAddresses
                  inner join dbo.ContactEmail on EmailAddresses.Id = ContactEmail.EmailAddressId
                  where ContactEmail.ContactId = @Id";
            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);



            sql = @"select ContactPhoneNumbers.Id, PhoneNumber from dbo.ContactPhoneNumbers
                inner join PhoneNumbers on PhoneNumbers.Id = ContactPhoneNumbers.PhoneNumberId
                where ContactPhoneNumbers.ContactId = @Id ";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);



            return output;


        }
    }
}
