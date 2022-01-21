using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MrCMS.Models;
using MrCMS.Settings;
using Newtonsoft.Json;

namespace MrCMS.Website.Filters
{
    public class CheckGoogleRecaptcha : ICheckGoogleRecaptcha
    {
        private readonly GoogleRecaptchaSettings _settings;

        public CheckGoogleRecaptcha(GoogleRecaptchaSettings settings)
        {
            _settings = settings;
        }

        public async Task<GoogleRecaptchaCheckResult> CheckTokenAsync(string token)
        {
            if (!_settings.Enabled)
            {
                return GoogleRecaptchaCheckResult.NotEnabled;
            }


            if (string.IsNullOrWhiteSpace(token))
            {
                return GoogleRecaptchaCheckResult.Missing;
            }

            var json = JsonConvert.SerializeObject(new { @event = new { token = token, siteKey = _settings.SiteKey } });
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();
            var response = await client.PostAsync(new Uri($"https://recaptchaenterprise.googleapis.com/v1beta1/projects/{_settings.ProjectId}/assessments?key={_settings.ApiKey}"), data);
            string googleResponse = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<GoogleRecaptchaAssessmentModel>(googleResponse);

            //Check token is valid
            if (!(result?.TokenProperties?.Valid ?? false))
            {
                return GoogleRecaptchaCheckResult.Missing;
            }

            //Check score
            if (result.Score >= _settings.ByPassScore)
            {
                return GoogleRecaptchaCheckResult.Success;
            }
            return GoogleRecaptchaCheckResult.Failed;

        }
    }
}