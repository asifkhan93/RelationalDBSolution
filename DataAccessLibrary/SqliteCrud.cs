using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString=connectionString;
        }


        public List<BasicContactModel> GetAllContacts()
        {
            string sql = "SELECT Id, FirstName, LastName FROM Contacts";

            return db.LoadData<BasicContactModel, dynamic>(sql, new { }, _connectionString);
        }


        public FullContactModel GetFullContactById(int id)
        {

            string sql = "select Id, FirstName, LastName from Contacts where Id = @Id ";

            FullContactModel output = new FullContactModel();

            output.BasicInfo =  db.LoadData<BasicContactModel, dynamic>(sql, new { Id = id }, _connectionString).FirstOrDefault();
            if (output.BasicInfo == null)
            {

                throw new Exception("User Not Found");
            }

            sql = @"select EmailAddresses.Id, EmailAddress from EmailAddresses
                  inner join ContactEmail on EmailAddresses.Id = ContactEmail.EmailAddressId
                  where ContactEmail.ContactId = @Id";
            output.EmailAddresses = db.LoadData<EmailAddressModel, dynamic>(sql, new { Id = id }, _connectionString);



            sql = @"select ContactPhoneNumbers.Id, PhoneNumber from ContactPhoneNumbers
                inner join PhoneNumbers on PhoneNumbers.Id = ContactPhoneNumbers.PhoneNumberId
                where ContactPhoneNumbers.ContactId = @Id ";

            output.PhoneNumbers = db.LoadData<PhoneNumberModel, dynamic>(sql, new { Id = id }, _connectionString);



            return output;


        }

        public void CreateContact(FullContactModel contact)
        {
            String sql = "Insert into Contacts (FirstName, LastName) values (@FirstName, @LastName);";
            // Save the basic Contact
            db.SaveData(sql,
                        new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                        _connectionString);

            //Get the ID number of the contact 
            sql = "select Id from Contacts where FirstName = @FirstName and LastName = @LastName;";
            int id = db.LoadData<BasicContactModel, dynamic>(sql,
                                                new { contact.BasicInfo.FirstName, contact.BasicInfo.LastName },
                                                _connectionString).First().Id;
            foreach (var phoneNumber in contact.PhoneNumbers)
            {
                if (phoneNumber.Id == 0)
                {
                    sql = "Insert into PhoneNumbers (PhoneNumber) values (@PhoneNumber);";
                    db.SaveData(sql,
                                new { phoneNumber.PhoneNumber },
                                _connectionString);

                    phoneNumber.Id = db.LoadData<IdLookupModel, dynamic>(@"Select Id from PhoneNumbers 
                                                                where PhoneNumber = @PhoneNumber;",
                                                                new { phoneNumber.PhoneNumber },
                                                                _connectionString).First().Id;
                }
                else
                {

                    sql = "Insert into ContactPhoneNumbers (ContactId, PhoneNumberId) values (@ContactId, @PhoneNumberId);";
                    db.SaveData(sql,
                                new { ContactId = id, PhoneNumberId = phoneNumber.Id },
                                _connectionString);
                }
            }



            foreach (var email in contact.EmailAddresses)
            {

                if (email.Id == 0)
                {
                    sql = "Insert into EmailAddresses (EmailAddress) values (@EmailAddress);";
                    db.SaveData(sql,
                                new { email.EmailAddress },
                                _connectionString);
                    email.Id = db.LoadData<IdLookupModel, dynamic>(@"Select Id from EmailAddresses 
                                                                where EmailAddress = @EmailAddress;",
                                                                new { email.EmailAddress },
                                                                _connectionString).First().Id;
                }
                else
                {
                    sql = "Insert into ContactEmail (ContactId, EmailAddressId) values (@ContactId, @EmailAddressId);";
                    db.SaveData(sql,
                                new { ContactId = id, EmailAddressId = email.Id },
                                _connectionString);

                }



            }
        }
    }
}
