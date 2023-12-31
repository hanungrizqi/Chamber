﻿using FormsAuth;
using restApi.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace restApi.ViewModel
{
    public class ClsLogin
    {
        CFMDataContext db = new CFMDataContext();

        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRespon Login()
        {
            var respon = new LoginRespon();

            //bool status = false;
            bool status_login = false;
            string nrp = "";

            if (Username.Count() > 7)
            {
                nrp = Username.Substring(Username.Length - 7);
            }
            else
            {
                nrp = Username;
            }

            status_login = CheckValidLogin();
            if (status_login == false)
            {
                status_login = OpenLdap(Username, Password);
            }

            if (status_login == true)
            {

                var data_user = db.TBL_M_USERs.Where(x => x.Username == nrp).SingleOrDefault();
                if (data_user != null)
                {
                    //status = true;
                    respon.Status = true;
                }
                else
                {
                    //status = false;
                    respon.Status = false;
                    respon.Message = "Maaf anda tidak memiliki akses ke DCFC";
                }
            }
            else
            {
                respon.Status = false;
                respon.Message = "Username or Password incorrect.";
            }

            //return status;
            return respon;
        }


        public bool CheckValidLogin()
        {
            bool stat = false;

            try
            {
                var ldap = new LdapAuthentication("LDAP://KPPMINING.NET:389");
                stat = ldap.IsAuthenticated("KPPMINING.NET", Username, Password);
                stat = true;
            }
            catch (Exception)
            {
                stat = false;
            }

            return stat;
        }

        public bool OpenLdap(string username = "", string password = "")
        {
            bool status = true;
            String uid = "cn=" + username + ",ou=Users,dc=kpp,dc=net";

            DirectoryEntry root = new DirectoryEntry("LDAP://10.12.101.102", uid, password, AuthenticationTypes.None);

            try
            {
                object connected = root.NativeObject;
                status = true;

            }
            catch (Exception)
            {
                status = false;
            }

            return status;
        }

        public class LoginRespon
        {
            public string Message { get; set; }
            public bool Status { get; set; }
        }

    }
}