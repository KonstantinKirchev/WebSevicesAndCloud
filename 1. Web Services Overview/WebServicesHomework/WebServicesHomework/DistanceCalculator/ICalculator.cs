namespace DistanceCalculator
{
    using System.ServiceModel;
    using System.Runtime.Serialization;

    [ServiceContract]
    public interface ICalculator
    {
        [OperationContract]
        double CalculateDistance(Point startPoint, Point endPoint);
    }

    [DataContract]
    public class Point
    {
        [DataMember]
        public int X { get; set; }

        [DataMember]
        public int Y { get; set; }
    }
}
