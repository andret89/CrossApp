using System;
using System.Collections.Generic;
using System.Text;

namespace CrossApp.Models
{

    public class Type
    {
        public string name { get; set; }
        public string description { get; set; }
        public string xmlid { get; set; }
    }

    public class Unit
    {
        public string name { get; set; }
        public string xmlid { get; set; }
    }

    public class Value
    {
        public string description { get; set; }
        public DateTime timeStamp { get; set; }
        public double value { get; set; }
    }

    public class Channel
    {
        public Type type { get; set; }
        public Unit unit { get; set; }
        public List<Value> values { get; set; }
    }

    public class Device
    {
        public string name { get; set; }
        public string serial { get; set; }
    }

    public class Value2
    {
        public string description { get; set; }
        public string name { get; set; }
        public double value { get; set; }
    }

    public class Property
    {
        public string description { get; set; }
        public string name { get; set; }
        public List<Value2> values { get; set; }
    }

    public class Type2
    {
        public string name { get; set; }
    }

    public class JsonTreeModel
    {
        public List<object> additionalMeasurementInformation { get; set; }
        public List<Channel> channels { get; set; }
        public Device device { get; set; }
        public List<Property> properties { get; set; }
        public string schemaVersion { get; set; }
        public DateTime timeStamp { get; set; }
        public Type2 type { get; set; }
    }
}