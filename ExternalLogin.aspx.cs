using System;
using System.Collections;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebMail
{
    public partial class ExternalLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            if (!string.IsNullOrEmpty(Request.QueryString["linfo"]))
            {

                var arr = new ArrayList();

                // string xmlText = Request.QueryString["linfo"];//Request["xml"];
                var credentialData = new CredentialEncrypter().Dycrypt(Request.QueryString["linfo"]);

                var userName = credentialData.UserName;//"Donotreply.ShopingCart@gmail.com";
                var password = credentialData.Password; //"15935789";

                string xmlText = "<?xml version=\"1.0\" encoding=\"utf-8\"?><webmail><param name=\"action\" value=\"login\" /><param name=\"request\" value=\"\" /><param name=\"email\"><![CDATA[" + userName + "]]></param><param name=\"mail_inc_login\"><![CDATA[]]></param><param name=\"mail_inc_pass\"><![CDATA[" + password + "]]></param><param name=\"mail_inc_host\"><![CDATA[imap.gmail.com]]></param><param name=\"mail_inc_port\" value=\"993\"/><param name=\"mail_protocol\" value=\"1\"/><param name=\"mail_out_host\"><![CDATA[smtp.gmail.com]]></param><param name=\"mail_out_port\" value=\"587\"/><param name=\"mail_out_auth\" value=\"1\"/><param name=\"sign_me\" value=\"0\"/><param name=\"language\"><![CDATA[]]></param><param name=\"advanced_login\" value=\"0\"/></webmail>";
                if (arr.Count > 0) xmlText = (string)arr[0];
                if (xmlText != null)
                {
                    Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");
                    Log.WriteLine("", xmlText);
                    Log.WriteLine("", ">>>>>>>>>>>>>>>>  IN  >>>>>>>>>>>>>>>>");

                    Account acct = Session[Constants.sessionAccount] as Account;

                    XmlPacketManager manager = new XmlPacketManager(acct, this);
                    XmlPacket packet = manager.ParseClientXmlText(xmlText);

                    if (Session[Constants.sessionErrorText] != null)
                    {
                        manager.ErrorFromSession = Session[Constants.sessionErrorText] as String;
                        Session.Remove(Constants.sessionErrorText);
                    }

                    XmlDocument doc = manager.CreateServerXmlDocumentResponse(packet);

                    if (Session[Constants.sessionAccount] == null)
                    {
                        if (manager.CurrentAccount != null)
                        {
                            Session.Add(Constants.sessionAccount, manager.CurrentAccount);
                            int idUser = manager.CurrentAccount.IDUser;
                            if (Session[Constants.sessionUserID] == null)
                            {
                                Session.Add(Constants.sessionUserID, idUser);
                            }
                            else
                            {
                                Session[Constants.sessionUserID] = idUser;
                            }
                        }
                    }
                    else
                    {
                        if (manager.CurrentAccount != null)
                        {
                            if (!manager.CurrentAccount.Equals(Session[Constants.sessionAccount]))
                            {
                                Session[Constants.sessionAccount] = manager.CurrentAccount;
                                if (Session[Constants.sessionUserID] == null)
                                {
                                    Session.Add(Constants.sessionUserID, manager.CurrentAccount.IDUser);
                                }
                                else
                                {
                                    Session[Constants.sessionUserID] = manager.CurrentAccount.IDUser;
                                }
                            }
                        }
                        else
                        {
                            Session[Constants.sessionAccount] = null;
                        }
                    }

                    //Response.Clear();
                    //Response.ContentType = @"text/xml";
                    //Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
                    //Log.WriteLine("", doc.OuterXml);
                    //Log.WriteLine("", "<<<<<<<<<<<<<<<<<  OUT  <<<<<<<<<<<<<<<");
                    //Response.Write(doc.OuterXml);
                    //  Response.End(); 
                    Response.Redirect("~/webmail.aspx");

                }
            }
            else
            {
                Response.Redirect("~/default.aspx");
            }
        }
    }
}