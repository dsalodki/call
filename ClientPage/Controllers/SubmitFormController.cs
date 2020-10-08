using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using ZaprosBy.Feedback.Api.Models;

namespace ZaprosBy.Feedback.Api.Controllers
{
    [AllowAnonymous]
    public class SubmitFormController : ApiController
    {

        [HttpPost]
        public bool Post([FromBody]SubmitFormModel model)
        {
            if (ModelState.IsValid)
            {
                //todo validate call time (one per x minutes)
                var connStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                using (var conn = new SqlConnection(connStr))
                {
                    using (var cmd = new SqlCommand("", conn))
                    {
                        //looking for provider

                        //save in db

                    }
                }

                return true;
            }
            return false;
        }
    }
}
