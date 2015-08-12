using System;
using System.Web;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.AspNet.Membership.OpenAuth;

namespace OmgLolz.Account
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        protected string ProviderName
        {
            get { return (string)ViewState["ProviderName"] ?? String.Empty; }
            private set { ViewState["ProviderName"] = value; }
        }

        protected string ProviderDisplayName
        {
            get { return (string)ViewState["ProviderDisplayName"] ?? String.Empty; }
            private set { ViewState["ProviderDisplayName"] = value; }
        }

        protected string ProviderUserId
        {
            get { return (string)ViewState["ProviderUserId"] ?? String.Empty; }
            private set { ViewState["ProviderUserId"] = value; }
        }

        protected string ProviderUserName
        {
            get { return (string)ViewState["ProviderUserName"] ?? String.Empty; }
            private set { ViewState["ProviderUserName"] = value; }
        }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                ProcessProviderResult();
            }
        }

        protected void logIn_Click(object sender, EventArgs e)
        {
            CreateAndLoginUser();
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            RedirectToReturnUrl();
        }

        private void ProcessProviderResult()
        {
            // Ergebnis von einem Authentifizierungsanbieter in der Anforderung verarbeiten
            ProviderName = OpenAuth.GetProviderNameFromCurrentRequest();

            if (String.IsNullOrEmpty(ProviderName))
            {
                Response.Redirect(FormsAuthentication.LoginUrl);
            }

            // Weiterleitungs-URL für OpenAuth-Überprüfung erstellen
            var redirectUrl = "~/Account/RegisterExternalLogin";
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(returnUrl))
            {
                redirectUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl);
            }

            // OpenAuth-Nutzlast überprüfen
            var authResult = OpenAuth.VerifyAuthentication(redirectUrl);
            ProviderDisplayName = OpenAuth.GetProviderDisplayName(ProviderName);
            if (!authResult.IsSuccessful)
            {
                Title = "Fehler bei der externen Anmeldung.";
                userNameForm.Visible = false;

                providerMessage.Text = String.Format("Fehler bei der externen Anmeldung {0},", ProviderDisplayName);

                // Wenn Sie diesen Fehler anzeigen möchten, aktivieren Sie Seitenablaufverfolgung in der Datei "web.config" (<system.web><trace enabled="true"/></system.web>), und besuchen Sie "~/Trace.axd".
                Trace.Warn("OpenAuth", String.Format("Fehler beim Überprüfen der Authentifizierung mit \"{0}\")", ProviderDisplayName), authResult.Error);
                return;
            }

            // Der Benutzer hat sich mit dem Anbieter erfolgreich angemeldet.
            // Überprüfen, ob der Benutzer bereits lokal registriert ist
            if (OpenAuth.Login(authResult.Provider, authResult.ProviderUserId, createPersistentCookie: false))
            {
                RedirectToReturnUrl();
            }

            // Anbieterdetails in "ViewState" speichern
            ProviderName = authResult.Provider;
            ProviderUserId = authResult.ProviderUserId;
            ProviderUserName = authResult.UserName;

            // Abfragezeichenfolge aus der Aktion entfernen
            Form.Action = ResolveUrl(redirectUrl);

            if (User.Identity.IsAuthenticated)
            {
                // Der Benutzer ist bereits authentifiziert. Externe Anmeldung hinzufügen und an Rückgabe-URL weiterleiten.
                OpenAuth.AddAccountToExistingUser(ProviderName, ProviderUserId, ProviderUserName, User.Identity.Name);
                RedirectToReturnUrl();
            }
            else
            {
                // Der Benutzer ist neu. Nach dem gewünschten Mitgliedschaftsnamen fragen.
                userName.Text = authResult.UserName;
            }
        }

        private void CreateAndLoginUser()
        {
            if (!IsValid)
            {
                return;
            }

            var createResult = OpenAuth.CreateUser(ProviderName, ProviderUserId, ProviderUserName, userName.Text);
            if (!createResult.IsSuccessful)
            {

                userNameMessage.Text = createResult.ErrorMessage;

            }
            else
            {
                // Der Benutzer wurde erstellt und erfolgreich zugeordnet.
                if (OpenAuth.Login(ProviderName, ProviderUserId, createPersistentCookie: false))
                {
                    RedirectToReturnUrl();
                }
            }
        }

        private void RedirectToReturnUrl()
        {
            var returnUrl = Request.QueryString["ReturnUrl"];
            if (!String.IsNullOrEmpty(returnUrl) && OpenAuth.IsLocalUrl(returnUrl))
            {
                Response.Redirect(returnUrl);
            }
            else
            {
                Response.Redirect("~/");
            }
        }
    }
}