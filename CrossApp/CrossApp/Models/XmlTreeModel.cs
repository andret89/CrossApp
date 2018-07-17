using System;
using System.Collections.Generic;
using System.Text;

namespace CrossApp.Models
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:ZIV-schema", IsNullable = false)]
    public partial class SingleResult
    {

        private decimal versionField;

        private SingleResultInstrument instrumentField;

        private SingleResultFireplace fireplaceField;

        private string transmissionStateField;

        private byte transmissionIdField;

        /// <remarks/>
        public decimal Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public SingleResultInstrument Instrument
        {
            get
            {
                return this.instrumentField;
            }
            set
            {
                this.instrumentField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplace Fireplace
        {
            get
            {
                return this.fireplaceField;
            }
            set
            {
                this.fireplaceField = value;
            }
        }

        /// <remarks/>
        public string TransmissionState
        {
            get
            {
                return this.transmissionStateField;
            }
            set
            {
                this.transmissionStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte TransmissionId
        {
            get
            {
                return this.transmissionIdField;
            }
            set
            {
                this.transmissionIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultInstrument
    {

        private string manufacturerField;

        private string typeField;

        private uint serialNumberField;

        /// <remarks/>
        public string Manufacturer
        {
            get
            {
                return this.manufacturerField;
            }
            set
            {
                this.manufacturerField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public uint SerialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplace
    {

        private SingleResultFireplaceFuel fuelField;

        private SingleResultFireplaceOilGasMeasurement oilGasMeasurementField;

        private byte fireplaceNumberField;

        /// <remarks/>
        public SingleResultFireplaceFuel Fuel
        {
            get
            {
                return this.fuelField;
            }
            set
            {
                this.fuelField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurement OilGasMeasurement
        {
            get
            {
                return this.oilGasMeasurementField;
            }
            set
            {
                this.oilGasMeasurementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte FireplaceNumber
        {
            get
            {
                return this.fireplaceNumberField;
            }
            set
            {
                this.fireplaceNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceFuel
    {

        private string nameField;

        private decimal cO2MaxField;

        private decimal o2RefField;

        private string fuelIdField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public decimal CO2Max
        {
            get
            {
                return this.cO2MaxField;
            }
            set
            {
                this.cO2MaxField = value;
            }
        }

        /// <remarks/>
        public decimal O2Ref
        {
            get
            {
                return this.o2RefField;
            }
            set
            {
                this.o2RefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FuelId
        {
            get
            {
                return this.fuelIdField;
            }
            set
            {
                this.fuelIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurement
    {

        private System.DateTime dateField;

        private System.DateTime timeField;

        private SingleResultFireplaceOilGasMeasurementO2 o2Field;

        private SingleResultFireplaceOilGasMeasurementCO_Dil cO_DilField;

        private SingleResultFireplaceOilGasMeasurementT_Flue t_FlueField;

        private SingleResultFireplaceOilGasMeasurementT_Air t_AirField;

        private SingleResultFireplaceOilGasMeasurementQ_Flue q_FlueField;

        private SingleResultFireplaceOilGasMeasurementEfficiency efficiencyField;

        private SingleResultFireplaceOilGasMeasurementP_Diff p_DiffField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time")]
        public System.DateTime Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementO2 O2
        {
            get
            {
                return this.o2Field;
            }
            set
            {
                this.o2Field = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementCO_Dil CO_Dil
        {
            get
            {
                return this.cO_DilField;
            }
            set
            {
                this.cO_DilField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementT_Flue T_Flue
        {
            get
            {
                return this.t_FlueField;
            }
            set
            {
                this.t_FlueField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementT_Air T_Air
        {
            get
            {
                return this.t_AirField;
            }
            set
            {
                this.t_AirField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementQ_Flue Q_Flue
        {
            get
            {
                return this.q_FlueField;
            }
            set
            {
                this.q_FlueField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementEfficiency Efficiency
        {
            get
            {
                return this.efficiencyField;
            }
            set
            {
                this.efficiencyField = value;
            }
        }

        /// <remarks/>
        public SingleResultFireplaceOilGasMeasurementP_Diff P_Diff
        {
            get
            {
                return this.p_DiffField;
            }
            set
            {
                this.p_DiffField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementO2
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementCO_Dil
    {

        private string unitField;

        private string errorField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementT_Flue
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementT_Air
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementQ_Flue
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementEfficiency
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:ZIV-schema")]
    public partial class SingleResultFireplaceOilGasMeasurementP_Diff
    {

        private string unitField;

        private string errorField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

}