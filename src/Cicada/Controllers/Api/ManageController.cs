using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Cicada.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [EnableCors("CicadaApi")]
    public class ManageController : ControllerBase
    {
        [HttpGet]
        public string HeartBeat(int id)
        {
            return "Ack: " + id.ToString();
        }

    }
}
