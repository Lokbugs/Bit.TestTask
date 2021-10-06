using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CameraTiltAngle.DataModels;

[assembly: InternalsVisibleTo("CameraTiltAngle.Tests")]
namespace CameraTiltAngle.Repository
{
    public interface ICameraPositionRepository
    {
        Task SaveCameraTiltInfoAsync(IEnumerable<CameraPositionInfo> cameraTiltInfos);
    }
}