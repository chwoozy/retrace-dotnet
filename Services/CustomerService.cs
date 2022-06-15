using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test_WebApplication.Models.DomainModel;
using Test_WebApplication.Models.ViewModel;

namespace Test_WebApplication.Services
{
    public class CustomerService
    {
        TestApplicationEntities db = new TestApplicationEntities();

        public int SaveCustomer(RegistrationModel registration)
        {
            try
            {
                var checkemail = db.Customers.FirstOrDefault(x => x.Email.ToLower() == registration.Email.ToLower().Trim() && !x.IsArchive.Value);
                if (checkemail != null)
                    return 3;

                var customer = new Customer();
                customer.CreatedDate = DateTime.UtcNow;
                customer.Name = registration.Name;
                customer.Email = registration.Email;
                customer.Password = Encrypt(registration.Password);
                customer.IsArchive = false;
                db.Customers.Add(customer);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 2;
            }


        }
        public List<ToDoc> LoadTODo()
        {
            return db.ToDocs.ToList();
        }
        public RegistrationModel Login(LoginModel login)
        {
            var model = new RegistrationModel();

            var data = db.Customers.Where(x => x.Email.ToLower() == login.Email.ToLower().Trim() && !x.IsArchive.Value).FirstOrDefault();
            if (data != null)
            {
                string pass = Decrypt(data.Password);
                if (login.Password == pass)
                {
                    model.Email = data.Email;
                    model.Name = data.Name;

                }
            }
            return model;
        }


        public bool SaveToDoc(ToDoc todoc)
        {
            try
            {
                var data = new ToDoc();
                data.CreatedDate = DateTime.UtcNow;
                data.Name = todoc.Name;
                //data.Description = todoc.Description;
                db.ToDocs.Add(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool RemoveToDoc(int id)
        {
            try
            {
                var data = db.ToDocs.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    db.ToDocs.Remove(data);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string Encrypt(string str)
        {
            try
            {
                byte[] encData_byte = new byte[str.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(str);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public string Decrypt(string str)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(str);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
    }
}