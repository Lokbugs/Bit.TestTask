using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CameraTiltAngle.DataModels;
using Dapper;

[assembly: InternalsVisibleTo("CameraTiltAngle.Tests")]
namespace CameraTiltAngle.Repository
{
    internal class CameraPositionRepository : ICameraPositionRepository
    {
        private string _connectionString;
        
        public CameraPositionRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public async Task SaveCameraTiltInfoAsync(IEnumerable<CameraPositionInfo> cameraTiltInfos)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync($@"
insert CameraTilt(tilt_angle, height_above_object)
values(@{nameof(CameraPositionInfo.CameraTiltAngle)}, @{nameof(CameraPositionInfo.CameraHeightAboveObject)})
", cameraTiltInfos);
        }
    }
}