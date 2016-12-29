using System.Linq;
using Abp.Configuration;
using Abp.Localization;
using Abp.Net.Mail;
using LearningMpaAbp.EntityFramework;

namespace LearningMpaAbp.Migrations.SeedData
{
    public class DefaultSettingsCreator
    {
        private readonly LearningMpaAbpDbContext _context;

        public DefaultSettingsCreator(LearningMpaAbpDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            //Emailing
            AddSettingIfNotExists(EmailSettingNames.DefaultFromAddress, "admin@mydomain.com");
            AddSettingIfNotExists(EmailSettingNames.DefaultFromDisplayName, "mydomain.com mailer");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Port, "587");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Host, "smtp.qq.com");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UserName, "ysjshengjie@qq.com");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Password, "123456");
            AddSettingIfNotExists(EmailSettingNames.Smtp.Domain, "");
            AddSettingIfNotExists(EmailSettingNames.Smtp.EnableSsl, "true");
            AddSettingIfNotExists(EmailSettingNames.Smtp.UseDefaultCredentials, "false");

            //Languages
            AddSettingIfNotExists(LocalizationSettingNames.DefaultLanguage, "en");
        }

        private void AddSettingIfNotExists(string name, string value, int? tenantId = null)
        {
            if (_context.Settings.Any(s => s.Name == name && s.TenantId == tenantId && s.UserId == null))
            {
                return;
            }

            _context.Settings.Add(new Setting(tenantId, null, name, value));
            _context.SaveChanges();
        }
    }
}