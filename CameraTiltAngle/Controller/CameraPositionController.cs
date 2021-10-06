using System;
using System.Threading.Tasks;
using CameraTiltAngle.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CameraTiltAngle.Controllers
{
    [Route("[controller]/[action]")]
    public class CameraTiltAngleController : Controller
    {
        private ICameraPositionService _cameraPositionService;
        
        public CameraTiltAngleController(ICameraPositionService cameraPositionService)
        {
            _cameraPositionService = cameraPositionService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCameraTiltAngle(int distanceFromObjectToWall, int cameraHeight)
        {
            try
            {
                var result = await _cameraPositionService.CalculateCameraPostionInfoAsync(distanceFromObjectToWall, cameraHeight);
                
                return new OkObjectResult(
                    new JsonResult(new 
                        {
                            CameraTiltAngle = result.CameraTiltAngle, 
                            CameraHeightAboveObject = result.CameraHeightAboveObject
                        }));
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}