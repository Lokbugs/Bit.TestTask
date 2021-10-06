using System.Collections.Generic;
using System.Threading.Tasks;
using CameraTiltAngle.DataModels;

namespace CameraTiltAngle.Repository
{
    internal interface ICameraPositionRepository
    {
        Task SaveCameraTiltInfoAsync(IEnumerable<CameraPositionInfo> cameraTiltInfos);
    }
}