using System;
using System.Collections.Generic;

using System.Text;


namespace WebMail
{
    public class CredentialEncrypter
    {
        /// <summary>
        /// Used to encrypt Password and userName
        /// </summary>
        /// <param name="credential"></param>
        /// <returns></returns>
        public string Encrypt(Credential credential)
        {
            try
            {
                var dataString = credential.UserName + "AMP&(o98)" + credential.Password;
                var encrptedString = new StringBuilder();

                int second = 0;
                while (second < 5)
                    second = DateTime.Now.Second;

                for (int i = 0; i < dataString.Length; i++)
                    encrptedString.Append((char)(dataString[i] + second));

                return (99 - second) + Convert.ToString(encrptedString);
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        /// <summary>
        /// Used to Dycrypt string to password 
        /// </summary>
        /// <param name="credentialUrl"></param>
        /// <returns></returns>
        public Credential Dycrypt(string credentialUrl)
        {
            try
            {
                var secondValue = 99 - Convert.ToInt32(Convert.ToString(credentialUrl[0]) + Convert.ToString(credentialUrl[1]));
                var mainDataString = new StringBuilder();

                for (int i = 2; i < credentialUrl.Length; i++)
                    mainDataString.Append((char)(credentialUrl[i] - secondValue));

                var data = Convert.ToString(mainDataString).Split(new string[] { "AMP&(o98)" }, StringSplitOptions.None);
                if (data.Length < 2)
                    return null;

                return new Credential
                {
                    UserName = data[0],
                    Password = data[1]

                };

            }
            catch (Exception ex)
            {

                return null;
            }
        }



    }

    public class Credential
    {
        public string UserName { set; get; }
        public string Password { set; get; }
    }
}
