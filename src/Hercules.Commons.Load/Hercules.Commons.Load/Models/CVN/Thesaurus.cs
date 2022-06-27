using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.Commons.Load.Models.CVN
{

    // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://cv.normalizado.org/thesaurus")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://cv.normalizado.org/thesaurus", IsNullable = false)]
    public partial class Thesaurus
    {

        private item[] itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item", Namespace = "")]
        public item[] item
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class item
    {

        private string itemIdField;

        private byte itemOrderField;

        private string itemAncestorIdField;

        private itemItemDescription[] itemDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string itemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public byte itemOrder
        {
            get
            {
                return this.itemOrderField;
            }
            set
            {
                this.itemOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string itemAncestorId
        {
            get
            {
                return this.itemAncestorIdField;
            }
            set
            {
                this.itemAncestorIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("itemDescription")]
        public itemItemDescription[] itemDescription
        {
            get
            {
                return this.itemDescriptionField;
            }
            set
            {
                this.itemDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class itemItemDescription
    {

        private itemItemDescriptionNameDetail nameDetailField;

        /// <remarks/>
        public itemItemDescriptionNameDetail NameDetail
        {
            get
            {
                return this.nameDetailField;
            }
            set
            {
                this.nameDetailField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class itemItemDescriptionNameDetail
    {

        private string nameField;

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
