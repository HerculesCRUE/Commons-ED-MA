![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 28/6/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Taxonomía unificada| 
|Descripción|Taxonomía y mapeos para la clasificación de resultados y actividades de investigación|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Introducción

La carpeta contiene la taxonomía con sus mapeos en [formato Excel en el documento Hércules-CommonsEDMA_Taxonomías_v1.36.xlsx](./Hércules-CommonsEDMA_Taxonomías_v1.36.xlsx); y también en triples en [Taxonomy_36.ttl](./Taxonomy_36.ttl). Este fichero se genera con el procedimiento descrito a continuación.

# Conversión del Excel de la taxonomía en un fichero .ttl.

La documentación acerca de la taxonomía unificada y sus mapeos se puede conultar en el análisis funcional del proyecto:
- [Análisis de taxonomías](https://confluence.um.es/confluence/pages/viewpage.action?pageId=397534598)
- [Taxonomía unificada de descriptores temáticos para Hércules](https://confluence.um.es/confluence/pages/viewpage.action?pageId=397534599)

## Definición de los ficheros 
En este fragmento de código se define de que Excel se van a extraer los datos (df) y en donde se van a almacenar (file). *Si se quiere cambiar el fichero Excel inicial o el .ttl final debe hacerse en estas líneas.* 
```
df = pd.read_excel("Hércules-CommonsEDMA_Taxonomías_v1.36.xlsx", engine='openpyxl', dtype=str).fillna('')
file = open("Taxonomy_36.ttl", "w")
```


## Función auxiliar 

Esta función auxiliar es necesaria para eliminar aquellos caracteres raros que dan fallos en él .ttl. 
```
eliminarAcentos = lambda str: str.translate(
    str.maketrans("áàäéèëíìïòóöùúüÀÁÄÈÉËÌÍÏÒÓÖÙÚÜ", "aaaeeeiiiooouuuAAAEEEIIIOOOUUU")).replace(" - ", "_").replace("- ", "_").replace("-", "_").replace("/", "_or_").replace("amp; ", "").replace(", ", "_").replace("(", "").replace(")", "").replace("&", "and").replace(" ", "_")
```

## Prefijos, Clases importadas y taxonomías. 

Se va a inicial un string donde se va a insertar todo el contenido que se va a ir generando en formato ttl. Cada vez que aparezca file.write(str) se escribirá en el fichero ttl previamente indicado. Para inicial el ttl con las declaración básicas se va a insertar en el str con 3 tipos distintos de declaraciones explicadas a continuación:
* Inicialmente se necesitan los prefijo de la ontología (líneas que empiezan por @prefix). Si se quieren añadir una nueva ontología se deberá insertar una línea similar a esta. 
```
@prefix roh: <http://w3id.org/roh#> .\n " \
      "@prefix owl: <http://www.w3.org/2002/07/owl#> .\n " \
      "@prefix rdfs: <http://www.w3.org/2000/01/rdf-schema#> .\n" \
      "@prefix rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .\n" \
      "@prefix : <http://w3id.org/roh/taxonomy#>.\n" \
      "@prefix vivo: <http://w3id.org/roh/mirror/vivo#> .\n" \
      "@prefix skos: <http://w3id.org/roh/mirror/skos#> .\n" \
```
* Las clases que se importan de otras ontologías para generar la nuestra. Si se quiere añadir una clase más de una ontología se deberá añadir el prefijo de esta e importar la clase con las líneas similares correspondientes a las externas a continuación. 
```
[ rdf:type owl:Ontology ].\n" \
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
```

* Definimos una clase de tipo KnowledgeArea por cada taxonomía de la que vamos a extraer datos de modo que cada una sea una clase distinta. Posteriormente leyendo el fichero Excel definiremos las instancias que pertenecen a cada una de estas clases.  A continuación se expone el ejemplo de como se ha creado la clase HerculesKA. Si se quiere añadir otra taxonomía se deben añadir dos líneas semejante a está cambiando el nombre por el correspondiente. 

```
 ":HerculesKA rdf:type owl:Class;\n" \
      "        rdfs:subClassOf roh:KnowledgeArea.\n" \
```


## Leer el Excel y pasar los datos la ontología. 

Se leerán cada una de las filas del fichero Excel. En los siguientes pasos se explicara el proceso realizado por cada una de las filas de este fichero. 

```
n = len(df.index)
for i in range(0, n):
```

### PASO 1 - crear la instancia de la taxonomía externa

Se leerán las columnas asociadas a las taxonomías que hemos usado para crear la nuestra. Por cada término de una taxonomía externa (puede estar compuesta por un descriptor solo, o solo por un descriptor y un código) se creara una instancia en el área de conocimiento de dicha taxonomía. De este modo estamos incorporar en nuestra enología las instancias de la taxonomía externa con los parámetros determinados. 

NOTA: Si una taxonomía tiene dos descriptores que se quieren enlazar con un mismo término de nuestra taxonomía, estos descriptores deben estar en dos columnas diferentes que se leerán en dos if diferentes y formaran instancias diferentes, ambas enlazadas a la clase definida anteriormente representante de la taxonomía que proviene. Ambas instancias se enlazaran con el término de nuestra taxonomía indicado después, ya que las representan boléanos diferentes. Lo que se está realizado hasta ahora es duplicar las columnas asociadas a dicha taxonomía externa y denominarlas igual con un _2 a fina del nombre. Se considera otra taxonomía pero en el fondo se instancia en la misma clase. 

Procedimiento: 
* se pone el booleano diseñado para decir que no hay nada de ese descriptor a false. 
* Se leen las filas (descriptor y código o solo descriptor)  correspondientes de ese término
    * En el caso en el que no sean vacías, el booleano se coloca a True y debemos declarar una instancia en la ontología, para ello:
    * Obtenemos el descriptor. Este siempre se le añadió una barra baja con el nombre de la taxonomía 
    * Obtenemos el código (en caso de tenerlo)
    * declaramos la instancia, (ejemplo a continuación)
        * la primera línea de este código define que es un concepto, la segunda el esquema en el que esta, la tercera el descriptor (nombre) que tiene y la ultima el código. En caso de que no tenga código se debe omitir la última file de este string. El siguiente texto es un ejemplo de Scopus, si fuera de otra taxonomía deberá cambiarse el nombre del área de conocimiento (definidos en el primer string) al que sea pertinente. También la variable name_scopus por la oportuna. 
```        
        str = ":" + name_scopus + " rdf:type roh:Concept; \n" \
                                  "skos:inScheme :Scopus;\n" \
                                  "skos:prefLabel \"" + df['ASJC-Scopus descriptor'][i] + "\";\n" \
                                  "skos:notation \"" + code + "\".\n"
```


### PASO 2: crear la instancia de nuestra taxonomía. 

Una vez creadas las instancia de la taxonomía externa. Vamos a ver el nivel y el descriptor de nuestra taxonomía. El nivel es importante para poder enlazarlo con el término del nivel anterior. Hay que tener en cuenta en nuestro Excel toda la taxonomías tienen un nivel superior excepto los de nivel 0. 

Por tanto se crean 4 if en el que averiguamos el nivel del término de esta fila que estamos recorriendo. Es importante tener en cuenta que si tenemos un nivel_2 siempre habrá declarado previamente un nivel_1 y un nivel_0 en caso contrario hay que verificar el Excel. 
 ```
 if df['Level 0'][i] == '':
        if df['Level 1'][i] == '':
            if df['Level 2'][i] == '':
                if df['Level 3'][i] == '':
                    print("error")
                    print(i)
                else:
                    ---
            else:
                --
        else:
            ---
 else: 
    ----
```

Por tanto en el siguiente paso  se explicara la estructura completa del código ocultado en el anterior ejemplo con ---, para ello supongamos que el nivel de esa fila es el nivel 3. En este siguiente paso se especificara que hay que cambiar si es otro nivel. 

### Paso 3: detectado el nivel, instanciación y enlace con las instancias de la taxonomía interna. 

Se definirá como nivel_3 el descriptor (nombre) de nuestra taxonomía. En caso de que hubiera sido un nivel 2 se denomina nivel_2, y etc... Nunca va a haber dos niveles a la vez en una única fila.


El siguiente paso es definir de un modo similar al que se han definido instancias anteriormente. Se inserta el código de ejemplo a continuación. La mayor diferencia es la línea 3 que define en enlace con el nivel_2 declarado. En caso de que fuera nivel 0 no se pone esa línea. En este caso la segunda línea siempre debe ser igual ya que forma parte de la misma taxonomía. 
 ```
file.write(":" + nivel_3 + "_AK rdf:type roh:Concept;" + "\n" 
            + "skos:inScheme :HerculesKA;\n" \
              "skos:narrower :" + nivel_2 + "_AK;\n" 
                           "skos:prefLabel \"" + df['Level 3'][i] + "\".\n")
```

Por último, hay que enlazar esta instancia con las externas. Como se han usado booleanos se sabe perfectamente cuales son estas instancias, ya que si por ejemple es el bool_scopus el nombre de la instancia es name_scopus. Además sabemos que es esta taxonomía por el boleado previamente declarado. Por tanto para relacionar estos términos se debe escribir lo siguiente:

```
if bool_scopus:
          file.write(":" + nivel_3 + "_AK skos:related :" + name_scopus + ".\n")
          bool_scopus = False
```
De este modo se recorren todas las posibles taxonomías y en caso de que en ella hubiera un booleano a true (en el paso1 1 se ha registrado dicha instancia). Se realiza automáticamente este enlace. 

NOTA: EL paso 3 se ha ejemplificado con el nivel_3 pero en caso de ser otro nivel el procedimiento es el mismo sustituyendo la variable por la correspondiente del nivel y enlazándola con la del nivel anterior. (Si es nivel_0 no se enlaza con ninguna). 

Si se añade una nueva taxonomía se han de modificar el PASO 1 y el paso 3 en el que por cada nivel se ha de añadir una nueva posible relación con la nueva instancia. 
