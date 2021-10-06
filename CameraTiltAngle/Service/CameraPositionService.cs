using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CameraTiltAngle.DataModels;
using CameraTiltAngle.Repository;
using NLog;

[assembly: InternalsVisibleTo("CameraTiltAngle.Tests")]
namespace CameraTiltAngle.Service
{
    internal class CameraPositionService : ICameraPositionService
    {
        internal const int AVERAGE_OBJECT_HEIGHT_SM = 160;
        
        private ICameraPositionRepository _cameraPositionRepository;

        private ILogger _logger;
        
        public CameraPositionService(ICameraPositionRepository cameraPositionRepository)
        {
            _cameraPositionRepository = cameraPositionRepository;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public async Task<CameraPositionInfo> CalculateCameraPostionInfoAsync(int distanceFromObjectToWall, int cameraHeight)
        {
            ValidateInputData(distanceFromObjectToWall, cameraHeight);
            
            var result = new CameraPositionInfo()
            {
                DistanceFromObjectToWall = distanceFromObjectToWall, 
                CameraHeight = cameraHeight, 
                CameraHeightAboveObject = cameraHeight - AVERAGE_OBJECT_HEIGHT_SM
            };

            var tanTiltAngle = (double) result.CameraHeightAboveObject / result.DistanceFromObjectToWall;
            result.CameraTiltAngle = Math.Round(Math.Atan(tanTiltAngle) * 180 / Math.PI, 2);

            await _cameraPositionRepository.SaveCameraTiltInfoAsync(new[] { result });
            
            return result;
        }

        private void ValidateInputData(int distanceFromObjectToWall, int cameraHeight)
        {
            Action<string> logAndThrowEx = errorMessage => 
            {
                _logger.Error($"CameraPositionService.ValidateInputData.{errorMessage}");
                throw new Exception(errorMessage);
            };

            if (distanceFromObjectToWall < 0)
                logAndThrowEx("The distance from object to wall cannot be negative.");
            
            if (cameraHeight < 0)
                logAndThrowEx("The camera`s height cannot be negative.");

            if (cameraHeight < AVERAGE_OBJECT_HEIGHT_SM)
                logAndThrowEx("The camera`s height cannot be less than average object height.");
        }
        
    }
}