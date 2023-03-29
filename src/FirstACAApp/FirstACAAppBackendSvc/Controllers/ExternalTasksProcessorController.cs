using Microsoft.AspNetCore.Mvc;
using FirstACAAppBackendSvc.Models;
using Dapr.Client;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FirstACAAppBackendSvc.Controllers
{
    [Route("ExternalTasksProcessor")]
    [ApiController]
    public class ExternalTasksProcessorController : ControllerBase
    {
        private const string OUTPUT_BINDING_NAME = "externaltasksblobstore";
        private readonly ILogger<ExternalTasksProcessorController> _logger;
        private readonly DaprClient _daprClient;

        public ExternalTasksProcessorController(ILogger<ExternalTasksProcessorController> logger,
                                                DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcesseTaskAndStore([FromBody] TaskModel taskModel)
        {
            try
            {
                _logger.LogInformation("Started processing external task message from storage queue. Task Name: '{0}'", taskModel.TaskName);

                taskModel.TaskId = Guid.NewGuid();
                taskModel.TaskCreatedOn = DateTime.UtcNow;

                //Dapr SideCar Invocation (save task to a state store)
                await _daprClient.InvokeMethodAsync(HttpMethod.Post, "tasksmanager-backend-api", $"api/tasks", taskModel);

                _logger.LogInformation("Saved external task to the state store successfuly. Task name: '{0}', Task Id: '{1}'", taskModel.TaskName, taskModel.TaskId);

                //code to invoke external binding and store queue message content into blob file in auzre storage
                IReadOnlyDictionary<string, string> metaData = new Dictionary<string, string>()
                    {
                        { "blobName", $"{taskModel.TaskId}.json" },
                    };

                await _daprClient.InvokeBindingAsync(OUTPUT_BINDING_NAME, "create", taskModel, metaData);

                _logger.LogInformation("Invoked output binding '{0}' for external task. Task name: '{1}', Task Id: '{2}'", OUTPUT_BINDING_NAME, taskModel.TaskName, taskModel.TaskId);

                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}