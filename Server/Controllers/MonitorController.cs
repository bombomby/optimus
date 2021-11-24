using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Optimus.Server.Hubs;
using Optimus.Tracing;

namespace Optimus.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        PerformanceTracer _tracer;
        IHubContext<PerformanceHub> _hub;
        ILogger<MonitorController> _logger;


        public MonitorController(ILogger<MonitorController> logger, PerformanceTracer tracer, IHubContext<PerformanceHub> hub)
        {
            _logger = logger;
            _tracer = tracer;
            _hub = hub;

            _tracer.OnProcessChanged += _tracer_OnProcessChanged;
        }

        private void _tracer_OnProcessChanged(ProcessEvent ev)
        {
            _hub.Clients.All.SendAsync("OnProcess", ev);
        }

        [HttpPost("Start")]
        public IActionResult Start()
        {
            _logger.LogInformation("Start");
            _tracer.Start();
            return Ok();
        }

        [HttpPost("Stop")]
        public IActionResult Stop()
        {
            _logger.LogInformation("Stop");
            _tracer.Stop();
            return Ok();
        }

        [HttpGet("Processes")]
        public List<ProcessEvent> GetProcesses()
        {
            return _tracer.ProcessEvents;
        }
    }
}
