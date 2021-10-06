using System.Threading.Tasks;
using CameraTiltAngle.DataModels;

namespace CameraTiltAngle.Service
{
    public interface ICameraPositionService
    {
        Task<CameraPositionInfo> CalculateCameraPostionInfoAsync(int distanceFromObjectToWall, int cameraHeight);
    }
}