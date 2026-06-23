using Microsoft.AspNetCore.Mvc;
using WORKFLOW_TUBES_KPL_ERGOLAB.Config;
using WORKFLOW_TUBES_KPL_ERGOLAB.Core;

namespace WORKFLOW_TUBES_KPL_ERGOLAB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var data =
                ConfigLoader.Load<CategoryConfig>("categories.json");

            return Ok(data);
        }

        [HttpGet("sla")]
        public IActionResult GetSlaRules()
        {
            var data =
                ConfigLoader.Load<SlaConfig>("sla_rules.json");

            return Ok(data);
        }

        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var data =
                ConfigLoader.Load<RoleConfig>("role_permission.json");

            return Ok(data);
        }

        [HttpGet("notifications")]
        public IActionResult GetNotifications()
        {
            var data =
                ConfigLoader.Load<NotificationConfig>(
                    "notification_templates.json");

            return Ok(data);
        }
    }
}