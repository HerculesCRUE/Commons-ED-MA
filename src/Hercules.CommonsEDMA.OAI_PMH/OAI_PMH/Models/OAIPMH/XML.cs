namespace OAI_PMH.Models.OAIPMH
{
    class XML
    {
        public string Id { get; }
        public string Set { get; }
        public string Xml { get; }

        public XML(string pXML, string pId, string pSet)
        {
            Set = pSet;
            Xml = pXML;
            Id = pId.ToString();
        }
    }
}
