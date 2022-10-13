import pandas as pd

#defincion del fichero de donde sacamos los datos, debe ser un excel, y dicheros donde almacenar la taxonomia en formato .ttl
df = pd.read_excel("Hércules-CommonsEDMA_Taxonomías_v1.36.xlsx", engine='openpyxl', dtype=str).fillna('')
file = open("Taxonomy_36.ttl", "w")

#funcionar para eliminar caracteres raros que pueda tener el nombre del campo
eliminarAcentos = lambda str: str.translate(
    str.maketrans("áàäéèëíìïòóöùúüÀÁÄÈÉËÌÍÏÒÓÖÙÚÜ", "aaaeeeiiiooouuuAAAEEEIIIOOOUUU")).replace(" - ", "_").replace("- ", "_").replace(
            "-", "_").replace("/", "_or_").replace("amp; ", "").replace(", ", "_").replace("(", "").replace(")",
                                                                                                            "").replace(
            "&", "and").replace(" ", "_")

# declaracion de los prefijos de la aontologia,
# de los clases importadas necesarias
# y las areas temticas ene ste caso son los nombres de la taxonomias que se quiere incorportar.
str = "@prefix roh: <http://w3id.org/roh#> .\n " \
      "@prefix owl: <http://www.w3.org/2002/07/owl#> .\n " \
      "@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .\n" \
      "@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .\n" \
      "@prefix : <http://w3id.org/roh/taxonomy#>.\n" \
      "@prefix vivo: <http://w3id.org/roh/mirror/vivo#> .\n" \
      "@prefix skos: <http://w3id.org/roh/mirror/skos#> .\n" \
      "[ rdf:type owl:Ontology ].\n" \
      "###  http://w3id.org/roh/mirror/skos#Concept\n" \
      "roh:Concept rdf:type owl:Class .\n" \
      "###  http://w3id.org/roh/mirror/skos#ConceptScheme\n" \
      "roh:ConceptScheme rdf:type owl:Class.\n" \
      "###  http://www.w3.org/2004/02/skos/core#prefLabel\n" \
      "skos:prefLabel rdf:type owl:AnnotationProperty .\n" \
      "###  http://www.w3.org/2004/02/skos/core#narrower\n" \
      "skos:narrower rdf:type owl:AnnotationProperty .\n" \
      "###  http://www.w3.org/2004/02/skos/core#related\n" \
      "skos:related rdf:type owl:AnnotationProperty .\n" \
      "###  http://www.w3.org/2004/02/skos/core#narrower\n" \
      "skos:narrower rdf:type owl:AnnotationProperty .\n" \
      "###  http://w3id.org/roh#KnowledgeArea\n" \
      "roh:KnowledgeArea rdf:type owl:Class ;\n" \
      "              rdfs:subClassOf roh:ConceptScheme.\n" \
      "#<http://w3id.org/roh/taxonomy#> a  vivo:Dataset.\n" \
      "#### ESQUEMAS - TAXONOMIAS USADAS Y CREADA #######\n" \
      ":HerculesKA rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":Scopus rdf:type owl:Class;\n" \
      "       rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":arXiv  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":MESH  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":Hercules  rdf:type owl:Class;" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":SourceForge  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":Bio  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":WoS  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":UNESCO  rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":FECYT   rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
      ":SGI    rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n"
file.write(str)


n = len(df.index)
for i in range(0, n):

    bool_scopus=False
    if df['ASJC Scopus Code'][i] != '' or df['ASJC-Scopus descriptor'][i] != '':
        bool_scopus = True
        name_scopus = eliminarAcentos(df['ASJC-Scopus descriptor'][i]) + "_scopus"
        code = df['ASJC Scopus Code'][i]

        str = ":" + name_scopus + " rdf:type roh:Concept; \n" \
                                  "skos:inScheme :Scopus;\n" \
                                  "skos:prefLabel \"" + df['ASJC-Scopus descriptor'][i] + "\";\n" \
                                                                                          "skos:notation \"" + code + "\".\n"

        file.write(str)
    bool_scopus_2=False
    if df['ASJC Scopus Code 2'][i] != '' or df['ASJC-Scopus descriptor 2'][i] != '':
        bool_scopus_2 = True
        name_scopus_2 = eliminarAcentos(df['ASJC-Scopus descriptor 2'][i]) + "_scopus"
        code = df['ASJC Scopus Code 2'][i]

        str = ":" + name_scopus_2 + " rdf:type roh:Concept; \n" \
                                  "skos:inScheme :Scopus;\n" \
                                  "skos:prefLabel \"" + df['ASJC-Scopus descriptor 2'][i] + "\";\n" \
                                                                                          "skos:notation \"" + code + "\".\n"

        file.write(str)
    bool_arxiv=False
    if df['arXiv Code'][i] != '':
        bool_arxiv = True
        name_arxiv = eliminarAcentos(df['arXiv Code'][i]) + "_arxiv"

        str = ":" + name_arxiv + " rdf:type roh:Concept; \n" \
                                 "skos:inScheme :arXiv;\n" \
                                 "skos:prefLabel \"" + df['arXiv Code'][i] + "\".\n"
        file.write(str)
    bool_mesh=False
    if df['MESH Code'][i] != '':
        bool_mesh = True
        name_mesh = eliminarAcentos(df['MESH Code'][i]) + "_MESH"

        file.write(":" + name_mesh + " rdf:type roh:Concept; \n" \
                                     "skos:inScheme :MESH;\n" \
                                     "skos:prefLabel \"" + df['MESH Code'][i] + "\".\n")
        file.write(str)
    bool_SourceForge=False
    if (df['SourceForge'][i] != ''):
        bool_SourceForge = True
        name_SourceForge = eliminarAcentos(df['SourceForge'][i]) + "_SourceForge"

        file.write(":" + name_SourceForge + " rdf:type roh:Concept; \n" \
                                            "skos:inScheme :SourceForge;\n" \
                                            "skos:prefLabel \"" + df['SourceForge'][i] + "\".\n")
        file.write(str)
    bool_Bio=False
    if (df['Bio-Protocols'][i] != ''):
        bool_Bio = True
        name_Bio = eliminarAcentos(df['Bio-Protocols'][i]) + "_Bio"

        file.write(":" + name_Bio + " rdf:type roh:Concept; \n" \
                                    "skos:inScheme :Bio;\n" \
                                    "skos:prefLabel \"" + df['Bio-Protocols'][i] + "\".\n")
        file.write(str)
    bool_WoS=False
    if (df['WoS-JCR code'][i] != '' or df['WoS-JCR descriptor'][i] != ''):
        bool_WoS = True
        name_WoS = eliminarAcentos(df['WoS-JCR descriptor'][i]) + "_WoS"
        code_WoS = df['WoS-JCR code'][i]
        file.write(":" + name_WoS + " rdf:type roh:Concept; \n" \
                                    "skos:inScheme :WoS;\n" \
                                    "skos:prefLabel \"" + df['WoS-JCR descriptor'][i] + "\";\n" \
                                                                                        "skos:notation \"" + code_WoS + "\".\n")
        file.write(str)
    bool_WoS_2=False
    if (df['WoS-JCR code 2'][i] != '' or df['WoS-JCR descriptor 2'][i] != ''):
        bool_WoS_2 = True
        name_WoS_2 = eliminarAcentos(df['WoS-JCR descriptor 2'][i])+ "_WoS"
        code_WoS = df['WoS-JCR code 2'][i]
        file.write(":" + name_WoS_2 + " rdf:type roh:Concept; \n" \
                                      "skos:inScheme :WoS;\n" \
                                      "skos:prefLabel \"" + df['WoS-JCR descriptor 2'][i] + "\";\n" \
                                                                                            "skos:notation \"" + code_WoS + "\".\n")

        file.write(str)
    bool_unesko=False
    if (df['UNESCO code'][i] != '' or df['UNESCO descriptor'][i] != ''):
        bool_unesko = True
        name_unesko = eliminarAcentos(df['UNESCO descriptor'][i]) + "_unesco"
        code_unesko = df['UNESCO code'][i]
        file.write(":" + name_unesko + " rdf:type roh:Concept; \n" \
                                       "skos:inScheme :UNESCO;\n" \
                                       "skos:prefLabel \"" + df['UNESCO descriptor'][i] + "\";\n" \
                                                                                          "skos:notation \"" + code_unesko + "\".\n")

        file.write(str)
    bool_FECYT=False
    if (df['FECYT CVN code'][i] != '' or df['FECYT CVN descriptor'][i] != ''):
        bool_FECYT = True
        name_FECYT = eliminarAcentos(df['FECYT CVN descriptor'][i]) + "_FECYT"
        code = df['FECYT CVN code'][i]
        file.write(":" + name_FECYT + " rdf:type roh:Concept; \n" \
                                      "skos:inScheme :FECYT;\n" \
                                      "skos:prefLabel \"" + df['FECYT CVN descriptor'][i] + "\";\n" \
                                                                                            "skos:notation \"" + code + "\".\n")
        file.write(str)
    bool_FECYT_2=False
    if df['FECYT CVN code 2'][i] != '' or df['FECYT CVN descriptor 2'][i] != '':
        bool_FECYT_2 = True
        name_FECYT_2 = eliminarAcentos(df['FECYT CVN descriptor 2'][i]) + "_FECYT"
        code = df['FECYT CVN code 2'][i]
        file.write(":" + name_FECYT_2 + " rdf:type roh:Concept; \n" \
                                        "skos:inScheme :FECYT;\n" \
                                        "skos:prefLabel \"" + df['FECYT CVN descriptor 2'][i] + "\";\n" \
                                                                                                "skos:notation \"" + code + "\".\n")
        file.write(str)
    bool_sgi=False
    if df['SGI descriptor'][i] != '' or df['SGI code'][i] != '':
        bool_sgi = True
        name_sgi = eliminarAcentos(df['SGI descriptor'][i]) + "_SGI"
        code = df['SGI code'][i]
        file.write(":" + name_sgi + " rdf:type roh:Concept; \n" \
                                    "skos:inScheme :SGI;\n" \
                                    "skos:prefLabel \"" + df['SGI descriptor'][i] + "\";\n" \
                                                                                    "skos:notation \"" + code + "\".\n")
        file.write(str)
    bool_sgi_2=False
    if df['SGI descriptor 2'][i] != '' or df['SGI code 2'][i] != '':
        bool_sgi_2 = True
        name_sgi_2 = eliminarAcentos(df['SGI descriptor 2'][i]) + "_SGI"
        code = df['SGI code 2'][i]
        file.write(":" + name_sgi_2 + " rdf:type roh:Concept; \n" \
                                      "skos:inScheme :SGI;\n" \
                                      "skos:prefLabel \"" + df['SGI descriptor 2'][i] + "\";\n" \
                                                                                        "skos:notation \"" + code + "\".\n")
        file.write(str)
    bool_arxiv_2=False
    if df['arXiv Code DUP'][i] != '':
        bool_arxiv_2 = True
        name_arxiv_2 = eliminarAcentos(df['arXiv Code DUP'][i]) + "_arxiv"

        file.write(":" + name_arxiv_2 + " rdf:type roh:Concept; \n" \
                                        "skos:inScheme :arXiv;\n" \
                                        "skos:prefLabel \"" + df['arXiv Code DUP'][i] + "\".\n")

        file.write(str)
    bool_hercules=False
    if df['Hercules'][i] != '':
        bool_hercules = True
        name_hercules = eliminarAcentos(df['Hercules'][i])+ "_hercules"

        file.write(":" + name_hercules + " rdf:type roh:Concept; \n" \
                                         "skos:inScheme :Hercules;\n" \
                                         "skos:prefLabel \"" + df['Hercules'][i] + "\".\n")


    if df['Level 0'][i] == '':
        if df['Level 1'][i] == '':
            if df['Level 2'][i] == '':
                if df['Level 3'][i] == '':
                    print("error")
                    print(i)
                else:
                    nivel_3 = eliminarAcentos(df['Level 3'][i])
                    # declaracion de existencia de ese nivel.#
                    file.write(":" + nivel_3 + "_AK rdf:type roh:Concept;" + "\n" + "skos:inScheme :HerculesKA;\n" \
                                                                                    "skos:narrower :" + nivel_2 + "_AK;\n" \
                                                                                                                  "skos:prefLabel \"" + \
                               df['Level 3'][i] + "\".\n")

                    if bool_scopus:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_scopus + ".\n")
                        bool_scopus = False
                    if bool_scopus_2:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_scopus_2 + ".\n")
                        bool_scopus_2 = False
                    if bool_arxiv:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_arxiv + ".\n")
                        bool_arxiv = False
                    if bool_mesh:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_mesh + ".\n")
                        bool_mesh = False
                    if bool_SourceForge:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_SourceForge + ".\n")
                        bool_SourceForge = False
                    if bool_Bio:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_Bio + ".\n")
                        bool_Bio = False
                    if bool_WoS:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_WoS + ".\n")
                        bool_WoS = False
                    if bool_WoS_2:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_WoS_2 + ".\n")
                        bool_WoS_2 = False
                    if bool_unesko:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_unesko + ".\n")
                        bool_unesko = False
                    if bool_FECYT:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_FECYT + ".\n")
                        bool_FECYT = False
                    if bool_FECYT_2:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_FECYT_2 + ".\n")
                        bool_FECYT_2 = False
                    if bool_sgi:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_sgi + ".\n")
                        bool_sgi = False
                    if bool_sgi_2:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_sgi_2 + ".\n")
                        bool_sgi_2 = False
                    if bool_arxiv_2:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_arxiv_2 + ".\n")
                        bool_arxiv_2 = False
                    if bool_hercules:
                        file.write(":" + nivel_3 + "_AK skos:related :" + name_hercules + ".\n")
                        bool_hercules = False

            else:
                nivel_2 = df['Level 2'][i].replace(" - ", "_").replace("- ", "_").replace("-", "_").replace("/",
                                                                                                            "_or_").replace(
                    "amp; ", "").replace(", ", "_").replace("(", "").replace(")", "").replace("&", "and").replace(" ",
                                                                                                                  "_")
                file.write(":" + nivel_2 + "_AK rdf:type roh:Concept;" + "\n" + "skos:inScheme :HerculesKA;\n" \
                                                                                "skos:narrower :" + nivel_1 + "_AK;\n" \
                                                                                                              "skos:prefLabel \"" +
                           df['Level 2'][i] + "\".\n")
                if bool_scopus:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_scopus + ".\n")
                    bool_scopus = False
                if bool_scopus_2:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_scopus_2 + ".\n")
                    bool_scopus = False
                if bool_arxiv:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_arxiv + ".\n")
                    bool_arxiv = False
                if bool_mesh:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_mesh + ".\n")
                    bool_mesh = False
                if bool_SourceForge:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_SourceForge + ".\n")
                    bool_SourceForge = False
                if bool_Bio:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_Bio + ".\n")
                    bool_Bio = False
                if bool_WoS:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_WoS + ".\n")
                    bool_WoS = False
                if bool_WoS_2:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_WoS_2 + ".\n")
                    bool_WoS_2 = False
                if bool_unesko:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_unesko + ".\n")
                    bool_unesko = False
                if bool_FECYT:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_FECYT + ".\n")
                    bool_FECYT = False
                if bool_FECYT_2:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_FECYT_2 + ".\n")
                    bool_FECYT_2 = False
                if bool_SGI:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_sgi + ".\n")
                    bool_SGI = False
                if bool_sgi_2:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_sgi_2 + ".\n")
                    bool_sgi_2 = False
                if bool_arxiv_2:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_arxiv_2 + ".\n")
                    bool_arxiv_2 = False
                if bool_hercules:
                    file.write(":" + nivel_2 + "_AK skos:related :" + name_hercules + ".\n")
                    bool_hercules = False

        else:
            nivel_1 = eliminarAcentos(df['Level 1'][i])
            file.write(":" + nivel_1 + "_AK rdf:type roh:Concept;" + "\n" + "skos:inScheme :HerculesKA;\n" \
                                                                            "skos:narrower :" + nivel_0 + "_AK;\n" \
                                                                                                          "skos:prefLabel \"" +
                       df['Level 1'][i] + "\".\n")
            # declaracion de existencia de ese nivel
            if bool_scopus:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_scopus + ".\n")
                bool_scopus = False
            if bool_scopus_2:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_scopus_2 + ".\n")
                bool_scopus = False
            if bool_arxiv:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_arxiv + ".\n")
                bool_arxiv = False
            if bool_mesh:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_mesh + ".\n")
                bool_mesh = False
            if bool_SourceForge:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_SourceForge + ".\n")
                bool_SourceForge = False
            if bool_Bio:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_Bio + ".\n")
                bool_Bio = False
            if bool_WoS:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_WoS + ".\n")
                bool_WoS = False
            if bool_WoS_2:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_WoS_2 + ".\n")
                bool_WoS_2 = False
            if bool_unesko:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_unesko + ".\n")
                bool_unesko = False
            if bool_FECYT:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_FECYT + ".\n")
                bool_FECYT = False
            if bool_FECYT_2:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_FECYT_2 + ".\n")
                bool_FECYT_2 = False
            if bool_sgi:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_sgi + ".\n")
                bool_SGI = False
            if bool_sgi_2:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_sgi_2 + ".\n")
                bool_sgi_2 = False
            if bool_arxiv_2:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_arxiv_2 + ".\n")
                bool_arxiv_2 = False
            if bool_hercules:
                file.write(":" + nivel_1 + "_AK skos:related :" + name_hercules + ".\n")
                bool_hercules = False
    else:
        nivel_0 = df['Level 0'][i].replace(" - ", "_").replace("- ", "_").replace("-", "_").replace("/",
                                                                                                    "_or_").replace(
            "amp; ", "").replace(", ", "_").replace("(", "").replace(")", "").replace("&", "and").replace(" ", "_")
        file.write(":" + nivel_0 + "_AK rdf:type roh:Concept;" + "\n" + "skos:inScheme :HerculesKA;\n" \
                                                                        "skos:prefLabel \"" + df['Level 0'][
                       i] + "\".\n")

        # declaracion de exitencia de ese nivel,
        if bool_scopus:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_scopus + ".\n")
            bool_scopus = False
        if bool_arxiv:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_arxiv + ".\n")
            bool_arxiv = False
        if bool_mesh:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_mesh + ".\n")
            bool_mesh = False
        if bool_SourceForge:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_SourceForge + ".\n")
            bool_SourceForge = False
        if bool_Bio:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_Bio + ".\n")
            bool_Bio = False
        if bool_WoS:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_WoS + ".\n")
            bool_WoS = False
        if bool_WoS_2:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_WoS_2 + ".\n")
            bool_WoS_2 = False
        if bool_unesko:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_unesko + ".\n")
            bool_unesko = False
        if bool_FECYT:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_FECYT + ".\n")
            bool_FECYT = False
        if bool_FECYT_2:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_FECYT_2 + ".\n")
            bool_FECYT_2 = False
        if bool_sgi:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_sgi + ".\n")
            bool_SGI = False
        if bool_sgi_2:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_sgi_2 + ".\n")
            bool_sgi_2 = False
        if bool_arxiv_2:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_arxiv_2 + ".\n")
            bool_arxiv_2 = False
        if bool_hercules:
            file.write(":" + nivel_0 + "_AK skos:related :" + name_hercules + ".\n")
            bool_hercules = False
    # llegados a este punto tenemos los niveles siempre.
file.close()
