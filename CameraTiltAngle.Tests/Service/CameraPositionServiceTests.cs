using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CameraTiltAngle.DataModels;
using CameraTiltAngle.Repository;
using CameraTiltAngle.Service;
using NSubstitute;
using NUnit.Framework;

namespace CameraTiltAngleTests.Service
{
    [TestFixture]
    public class CameraPositionServiceTests
    {
        private ICameraPositionService _service;

        private ICameraPositionRepository _cameraPositionRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _cameraPositionRepository = Substitute.For<ICameraPositionRepository>();
            _service = new CameraPositionService(_cameraPositionRepository);
        }

        [SetUp]
        public void SetUp()
            => _cameraPositionRepository.ClearReceivedCalls();

        [TestCase(160, 320, 45, 160)]
        [TestCase(300, 160, 0, 0)]
        [TestCase(0, 350, 90, 190)]
        [TestCase(300, 260, 18.43, 100)]
        [TestCase(700, 350, 15.19, 190)]
        public async Task CorrectInputData_CorrectCalculateAngle(int distanceFromObjectToWall, int cameraHeight, double angle, int cameraHeightAboveObject)
        {
            var result = await _service.CalculateCameraPostionInfoAsync(distanceFromObjectToWall, cameraHeight);

            _cameraPositionRepository.Received().SaveCameraTiltInfoAsync(
                Arg.Is<IEnumerable<CameraPositionInfo>>(
                    x => x.Single().CameraTiltAngle == angle && x.Single().CameraHeightAboveObject == cameraHeightAboveObject));
            
            Assert.AreEqual(angle, result.CameraTiltAngle);
            Assert.AreEqual(cameraHeightAboveObject, result.CameraHeightAboveObject);
        }

        [Test]
        public void NegativeDistanceFromObjectToWall_ThrowException()
            => Assert.ThrowsAsync<Exception>(() => _service.CalculateCameraPostionInfoAsync(-1, 160));

        [Test]
        public void NegativeCameraHeight_ThrowException()
            => Assert.ThrowsAsync<Exception>(() => _service.CalculateCameraPostionInfoAsync(700, -1));

        [Test]
        public void CameraHeightLessThanAverageObjectHeight_ThrowException()
            => Assert.ThrowsAsync<Exception>(() => _service.CalculateCameraPostionInfoAsync(700, 159));
    }
}