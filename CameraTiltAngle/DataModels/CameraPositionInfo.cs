namespace CameraTiltAngle.DataModels
{
    public class CameraPositionInfo
    {
        public int CameraHeight { get; set; }

        public int CameraHeightAboveObject { get; set; }
        
        public int DistanceFromObjectToWall { get; set; }
        
        public double CameraTiltAngle { get; set; }
    }
}