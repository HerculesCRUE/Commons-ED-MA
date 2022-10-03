using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Load.Models.CVN
{
    // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cv.normalizado.org/referenceTables")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://cv.normalizado.org/referenceTables", IsNullable = false)]
    public partial class ReferenceTables
    {

        private Table[] tableField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Table", Namespace = "")]
        public Table[] Table
        {
            get
            {
                return this.tableField;
            }
            set
            {
                this.tableField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Table
    {

        private TableItem[] itemField;

        private string nameField;

        private string versionField;

        private string sourceField;

        private string xMLDataTypeField;

        private string xMLPropertyField;

        private string xMLIndicatorField;

        private string antecesorTableField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public TableItem[] Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XMLDataType
        {
            get
            {
                return this.xMLDataTypeField;
            }
            set
            {
                this.xMLDataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XMLProperty
        {
            get
            {
                return this.xMLPropertyField;
            }
            set
            {
                this.xMLPropertyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XMLIndicator
        {
            get
            {
                return this.xMLIndicatorField;
            }
            set
            {
                this.xMLIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string antecesorTable
        {
            get
            {
                return this.antecesorTableField;
            }
            set
            {
                this.antecesorTableField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TableItem
    {

        private string codeField;

        private ushort orderField;

        private TableItemNameDetail[] nameField;

        private string antecesorCodeField;

        private bool linkField;

        private string delegateField;

        /// <remarks/>
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public ushort Order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("NameDetail", IsNullable = false)]
        public TableItemNameDetail[] Name
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
        public string AntecesorCode
        {
            get
            {
                return this.antecesorCodeField;
            }
            set
            {
                this.antecesorCodeField = value;
            }
        }

        /// <remarks/>
        public bool Link
        {
            get
            {
                return this.linkField;
            }
            set
            {
                this.linkField = value;
            }
        }

        /// <remarks/>
        public string Delegate
        {
            get
            {
                return this.delegateField;
            }
            set
            {
                this.delegateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class TableItemNameDetail
    {

        private string nameField;

        private string shortNameField;

        private string langField;

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
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }
    }
}
