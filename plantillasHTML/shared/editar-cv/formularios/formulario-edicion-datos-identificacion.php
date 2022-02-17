<form class="formulario-edicion formulario-datos-identificacion">
    <!-- DATOS PERSONALES -->
    <div class="simple-collapse">
        <a class="collapse-toggle" data-toggle="collapse" href="#collapse-datos-personales" role="button" aria-expanded="true" aria-controls="collapse-datos-personales">
            Datos personales
        </a>
        <div class="collapse show" id="collapse-datos-personales">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Primer apellido</label>
                        <input placeholder="Introduce tu primer apellido" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Segundo apellido</label>
                        <input placeholder="Introduce tu segundo apellido" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Nombre</label>
                        <input placeholder="Introduce tu nombre" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
                <div class="custom-form-row">
                    <div class="form-group ">
                        <label class="control-label d-block">Sexo</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="hombre">Hombre</option>
                            <option value="mujer">Mujer</option>
                        </select>
                    </div>
                    <div class="form-group form-group-date">
                        <label class="control-label d-block">Fecha de nacimiento</label>
                        <input title="Fecha de nacimiento" type="text" class="ac_input datepicker form-control not-outline" placeholder="Introduce tu fecha de nacimiento" name="" id="" autocomplete="off">
                        <span class="material-icons">today</span>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Correo electrónico</label>
                        <input placeholder="Introduce tu correo electrónico" type="email" name="" id="" class="form-control not-outline">
                    </div>
                </div>
                <div class="custom-form-row">
                    <div class="form-group ">
                        <label class="control-label d-block">Tipo de documento</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="Tipo de documento 1">Tipo de documento 1</option>
                            <option value="Tipo de documento 2">Tipo de documento 2</option>
                            <option value="Tipo de documento 3">Tipo de documento 3</option>
                            <option value="Tipo de documento 4">Tipo de documento 4</option>
                            <option value="Tipo de documento 5">Tipo de documento 5</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Número de documento</label>
                        <input placeholder="Introduce tu número de documento" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END DATOS PERSONALES -->

    <!-- TELÉFONO FIJO -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-tel-fijo" role="button" aria-expanded="false" aria-controls="collapse-tel-fijo">
            Teléfono fijo
        </a>
        <div class="collapse" id="collapse-tel-fijo">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Código internacional</label>
                        <input placeholder="Introduce tu código internacional" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Número</label>
                        <input placeholder="Introduce tu número" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Extensión</label>
                        <input placeholder="Introduce tu extensión" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END TELÉFONO FIJO -->

    <!-- FAX -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-fax" role="button" aria-expanded="false" aria-controls="collapse-fax">
            Fax
        </a>
        <div class="collapse" id="collapse-fax">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Código internacional</label>
                        <input placeholder="Introduce tu código internacional" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Número</label>
                        <input placeholder="Introduce tu número" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Extensión</label>
                        <input placeholder="Introduce tu extensión" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END FAX -->

    <!-- MÁS DATOS -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-mas-datos" role="button" aria-expanded="false" aria-controls="collapse-mas-datos">
            Más datos
        </a>
        <div class="collapse" id="collapse-mas-datos">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group expand-2">
                        <label class="control-label d-block">Foto de perfil</label>
                        <div class="image-uploader js-image-uploader" id="foto-perfil-cv">
                            <div class="image-uploader__preview">
                                <!-- Si hay una imagen en el servidor pintarla en el src, si no dejarlo vacío  -->
                                <img class="image-uploader__img" src=./theme/resources/imagenes-pre/foto-usuario.jpg">
                            </div>
                            <div class="image-uploader__drop-area">
                                <div class="image-uploader__icon">
                                    <span class="material-icons">backup</span>
                                </div>
                                <div class="image-uploader__info">
                                    <p><strong>Arrastra y suelta en la zona punteada una foto para tu perfil</strong></p>
                                    <p>Imágenes en formato .PNG o .JPG</p>
                                    <p>Peso máximo de las imágenes 250 kb</p>
                                </div>
                            </div>
                            <div class="image-uploader__error">
                                <p class="ko"></p>
                            </div>
                            <input type="file" class="image-uploader__input">
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Página web personal</label>
                        <input placeholder="Introduce tu página web personal" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END MÁS DATOS -->

    <!-- INFORMACIÓN CONTACTO -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-contacto" role="button" aria-expanded="false" aria-controls="collapse-contacto">
            Información de contacto
        </a>
        <div class="collapse" id="collapse-contacto">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group expand-2">
                        <label class="control-label d-block">Dirección</label>
                        <input placeholder="Introduce tu dirección" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Código postal</label>
                        <input placeholder="Introduce tu código postal" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Ciudad</label>
                        <input placeholder="Introduce tu ciudad" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">País</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                            <option value="78118e78-47fd-490f-92c4-e34797c9b087">Angola</option>
                            <option value="8c4dc66e-faf0-45f1-937c-a87891a7e395">Anguilla</option>
                            <option value="24ef5244-c8a6-4c9e-aba8-566826531ca9">Antártida</option>
                            <option value="8ca92982-f3d0-4df8-8dee-0ef19fbf98be">Antigua y Barbuda</option>
                            <option value="6052a938-abca-45c4-9541-75a1b4a1983e">Antillas Holandesas</option>
                            <option value="6e3b2135-2fda-45ca-96e3-c57ea6729022">Arabia Saudí</option>
                            <option value="cd0d1a97-f03c-447e-99bf-7cb27928cc6e">Argelia</option>
                            <option value="044eee21-2364-41c8-bc52-1ff3d1033b88">Argentina</option>
                            <option value="05daaed4-ef44-48d0-ae82-1743528398c0">Armenia</option>
                            <option value="b581b0bb-1686-4d8b-b4c5-1d1be561ff0b">Aruba</option>
                            <option value="87baf63b-24f3-4e61-b73e-b2226cf3b5cc">ARY Macedonia</option>
                            <option value="4662f2c9-da06-4759-8a3e-a5df2c9ace61">Australia</option>
                            <option value="657c5a7e-8302-43d8-b025-3523b8104aa1">Austria</option>
                            <option value="280a936e-9ce6-47a1-99ce-af47bb9df61b">Azerbaiyán</option>
                            <option value="43c981cf-c034-4df7-a0a6-5329a7eb7a24">Bahamas</option>
                            <option value="fa9dbe2f-0df2-401a-ad59-9a6557b88e08">Bahréin</option>
                            <option value="26ae08d2-d610-4d23-8d02-3d0fcb02584c">Bangladesh</option>
                            <option value="bca5cc89-0211-4696-8baa-b2c6d7090feb">Barbados</option>
                            <option value="e54f14b3-05a6-47cf-8335-0f2d02769a31">Bélgica</option>
                            <option value="49c02869-b7a2-4a70-9f75-caad5363f130">Belice</option>
                            <option value="8071dd96-3aa5-408f-b690-cf97724909d9">Benin</option>
                            <option value="270cac43-20b9-4e56-91a7-8d23a1a688de">Bermudas</option>
                            <option value="92d18cac-dfe0-42fa-9180-24c078c3c371">Bhután</option>
                            <option value="57a54be6-d64d-4488-a4de-1f3267df2492">Bielorrusia</option>
                            <option value="9755acf5-77a7-43d7-aeac-692c12ce6801">Bolivia</option>
                            <option value="32d75163-1baa-43c3-975a-af84f3bcd33a">Bosnia y Herzegovina</option>
                            <option value="26cd9574-c586-4fc4-9aff-ed8d298a569b">Botsuana</option>
                            <option value="8e38f790-77e8-47d1-956c-a64a89103f63">Brasil</option>
                            <option value="ec78af5e-678e-4292-92e9-e5841bc41bb7">Brunéi</option>
                            <option value="71207a55-f06e-4891-964c-c3481ed20aa9">Bulgaria</option>
                            <option value="83804114-9bfc-4ddf-bda5-c86638508a62">Burkina Faso</option>
                            <option value="f1f7a863-11bc-4992-acce-c9b99487bb4f">Burundi</option>
                            <option value="5370664e-af56-42f5-9b33-ab0f0417bdb4">Cabo Verde</option>
                            <option value="bd504159-e1dd-49e5-8e40-f105068f3d9c">Camboya</option>
                            <option value="6ac3a29c-8248-4085-892f-dac3ff03b7bf">Camerún</option>
                            <option value="7a3cd989-ece8-4fd1-ac90-ee19be2f2b5e">Canadá</option>
                            <option value="c0ca8475-23ac-4f4f-93ba-c28d06f69b67">Chad</option>
                            <option value="56da1c0b-772c-4ed6-bf8a-c61c80c8caff">Chile</option>
                            <option value="7bec9170-f338-467d-bd93-6113d8168439">China</option>
                            <option value="8dcc7c84-f4d5-41f4-8981-5844bdf02112">Chipre</option>
                            <option value="7a26e83b-e956-4ce4-a797-b6326f370374">Ciudad del Vaticano</option>
                            <option value="cb1dc8fc-5fbb-4b96-a07e-d6d2286d53f1">Colombia</option>
                            <option value="46c079d5-0a29-4920-ae9d-2a21c1a3ed98">Comoras</option>
                            <option value="cda90c82-da96-4c12-89c9-383a4d3a900b">Congo</option>
                            <option value="73384b52-18ce-497e-b175-7037e2245757">Corea del Norte</option>
                            <option value="95976c3c-ecf7-41df-ae23-6fd4aab0b969">Corea del Sur</option>
                            <option value="a4c5274d-9b1d-4306-8725-2f2876523b4c">Costa de Marfil</option>
                            <option value="95294524-fd46-4635-8971-701a63949804">Costa Rica</option>
                            <option value="d470aed3-74c7-4d31-b5c6-dce4d7322f61">Croacia</option>
                            <option value="d4d6380d-e650-4089-a924-71a0c546fb03">Cuba</option>
                            <option value="e6167713-acc0-49ff-9e5a-c48aab15e5b4">Dinamarca</option>
                            <option value="00d4f2af-5051-4f01-bf50-dca6f0840a69">Dominica</option>
                            <option value="3d8fe44c-bf9e-4a2a-afe8-5b856d9ab0c4">Ecuador</option>
                            <option value="d14adac1-d306-4c68-9f8d-458726cc286c">Egipto</option>
                            <option value="67f17331-4e1c-46e5-9761-2a6f0be92056">El Salvador</option>
                            <option value="f63077d5-4c6e-4d5a-ad73-799dea0dca9d">Emiratos Árabes Unidos</option>
                            <option value="2319b8b1-8211-4994-8b35-c2c04e9066e5">Eritrea</option>
                            <option value="6fa9b8c0-ecb9-4e63-a1ac-6bef0299e56a">Eslovaquia</option>
                            <option value="a2cdba20-ce03-4ff8-8ca7-77f1a5c99ca6">Eslovenia</option>
                            <option value="98d604b4-3141-4499-bde1-c320f09ef45c" selected="selected">España</option>
                            <option value="3ea91f4c-293d-4e29-ab64-9293f28a12eb">Estados Unidos</option>
                            <option value="f8a8580c-8b4b-4d67-b487-3612e0716b69">Estonia</option>
                            <option value="bcd71081-47ca-4a53-9b52-fba56545f2a5">Etiopía</option>
                            <option value="2ebd2f60-bac0-4532-81ef-45dc1f45c6e2">Filipinas</option>
                            <option value="9fcfa3c2-9d7b-45ff-97f2-ad2097aff07f">Finlandia</option>
                            <option value="e5a21c0b-98d2-4ea1-a074-05544c803f77">Fiyi</option>
                            <option value="b8b70243-eb59-4a88-aaca-cb367c9d0b5f">Francia</option>
                            <option value="0e8cf3f7-154b-4fdd-98df-04cb26786af1">Gabón</option>
                            <option value="87db8f6e-27bc-4443-9fcc-9795c44e8dc0">Gambia</option>
                            <option value="d314d230-03a9-4d7c-adaf-ce8404e02d8d">Georgia</option>
                            <option value="c8cf89da-5f47-4028-b999-b7970aa5e776">Ghana</option>
                            <option value="64021223-79e6-45c6-b318-32f5b15ace77">Gibraltar</option>
                            <option value="b7988bb0-540d-4ad7-a734-2df234fa6820">Granada</option>
                            <option value="afb5eb1a-5334-40d2-b428-0b9577ce54e4">Grecia</option>
                            <option value="21e35659-1839-4877-9705-64b8b4c3c01a">Groenlandia</option>
                            <option value="5b8dc635-aef1-4be7-aac5-cbb0261c725e">Guadalupe</option>
                            <option value="2e88249a-68bb-4d88-844c-687c3fa9b8b5">Guam</option>
                            <option value="4a838ef9-a921-429e-9949-6edca59a5967">Guatemala</option>
                            <option value="c2a79452-430d-47d0-a523-b549a33f2449">Guayana Francesa</option>
                            <option value="60fb826a-8c83-49ba-9cbb-64679aeb3b3c">Guinea</option>
                            <option value="7752dbfb-9df9-4f1c-95d5-8233229cd982">Guinea Ecuatorial</option>
                            <option value="ea751ba1-263e-4084-b1aa-1f3edc8e4945">Guinea-Bissau</option>
                            <option value="ba1d53c6-3d37-48fc-8de7-ce2e88379dfb">Guyana</option>
                            <option value="2cb6cd9a-e2f6-4373-bd3e-9259756bf099">Haití</option>
                            <option value="3153d9a2-c982-40bd-a3b9-f4c97c0dc285">Honduras</option>
                            <option value="520c6fd4-e8b4-4f5a-84dc-800a340ab103">Hong Kong</option>
                            <option value="51e0fddf-b7e9-4cf1-838d-15cf9b481589">Hungría</option>
                            <option value="eb16b4af-09e1-4fcd-8f6a-1da31fd1bb73">India</option>
                            <option value="1b2c093a-e529-4832-901b-799f038e2498">Indonesia</option>
                            <option value="19a39a10-c0a9-4d1d-9b48-9d83cd0289d3">Irán</option>
                            <option value="e67dbb4c-d5fe-4d70-b999-6794bfe9acf0">Iraq</option>
                            <option value="685fe6ac-6910-4c4d-a352-26fb65d1d7a9">Irlanda</option>
                            <option value="cf84aea1-8e7f-49b1-afbd-16ac350b96fd">Isla Bouvet</option>
                            <option value="63975fce-9e3e-478c-a476-dd4511c8b02c">Isla de Navidad</option>
                            <option value="afa22ac6-7a06-49af-930e-0c7cabaacd21">Isla Norfolk</option>
                            <option value="e4a818bf-7aec-4af0-b7c6-91979b7ab7af">Islandia</option>
                            <option value="61516a48-678a-45a0-a9e8-ec3506d74df0">Islas Caimán</option>
                            <option value="7cd3aa30-51f3-401e-9fb7-09522b4c91fc">Islas Cocos</option>
                            <option value="24ccc5d2-63a9-48f8-84e0-1b33b84a0349">Islas Cook</option>
                            <option value="b59c8ef0-42c6-47f4-a358-3f08c288e835">Islas Feroe</option>
                            <option value="8cd5e8d8-ed74-4c6b-88d6-07cbe01f9c6f">Islas Georgias del Sur y Sandwich del Sur</option>
                            <option value="afc2b9a4-5e22-4658-bfbc-f3e55c268e93">Islas Gland</option>
                            <option value="ea5f2f5e-6886-4c6f-b289-1f06a1d60aea">Islas Heard y McDonald</option>
                            <option value="015dcea1-94a6-4937-98b5-639145104650">Islas Malvinas</option>
                            <option value="41db9c4a-9c39-47cd-8b61-e30b38eb4b1b">Islas Marianas del Norte</option>
                            <option value="5e225226-546f-43b2-9cff-26367fd4c501">Islas Marshall</option>
                            <option value="03fac88c-540f-425e-9513-32d25c1200a7">Islas Pitcairn</option>
                            <option value="d0ee3c84-e674-4957-b192-e6bb9e841ceb">Islas Salomón</option>
                            <option value="297a00b2-0ed0-4968-842a-51ef46899462">Islas Turcas y Caicos</option>
                            <option value="a6266c9e-1a52-44ec-b193-8d0e89e07693">Islas ultramarinas de Estados Unidos</option>
                            <option value="7fe46ca5-6440-4f3c-8330-0584ee70ba12">Islas Vírgenes Británicas</option>
                            <option value="8c7a5fe2-0c83-459a-9f77-071afb1d9d85">Islas Vírgenes de los Estados Unidos</option>
                            <option value="b1226777-3c2c-4bbe-bba5-e61cf2c4bfb2">Israel</option>
                            <option value="0a677a04-7634-4dac-ba85-06cee1901067">Italia</option>
                            <option value="7eb6c619-2d9b-42f8-a032-16bc1fd9b870">Jamaica</option>
                            <option value="760c2143-b7ac-4c2e-a873-8478f1eff570">Japón</option>
                            <option value="317ad2ae-6a28-4f83-ad92-5dd055d0cdb5">Jordania</option>
                            <option value="c4f59c7d-c53d-4665-b5d2-f1e3c5bc40bf">Kazajstán</option>
                            <option value="b1bf3e85-c4c3-4d31-919e-e23e7dc9e785">Kenia</option>
                            <option value="72e572e1-22a0-4ed0-9ec3-130f89487287">Kirguistán</option>
                            <option value="12efc107-5c37-4269-8bf8-019f2e9a2d02">Kiribati</option>
                            <option value="942528a6-562f-4cdc-99f1-3591e10f7d6b">Kuwait</option>
                            <option value="882a5515-ab26-42e4-90a7-a98028bfe048">Laos</option>
                            <option value="c7bd6189-ac9a-496a-8f2f-1404abfab7d8">Lesotho</option>
                            <option value="6fc67913-e8b1-49ca-8f47-954659cda946">Letonia</option>
                            <option value="79c1c7b7-be1b-405e-a199-c080d44d239f">Líbano</option>
                            <option value="1b3daf07-0f70-4720-970a-5ad785696a4c">Liberia</option>
                            <option value="547960fd-f814-4a6a-b34e-9a3113d3b846">Libia</option>
                            <option value="4398976a-0e34-4ecc-a6b1-e2965e34eebe">Liechtenstein</option>
                            <option value="7ddb9175-581f-4bc6-a243-0e9a0028f5d3">Lituania</option>
                            <option value="20c8d61c-dad3-447c-b5a4-40859e139f15">Luxemburgo</option>
                            <option value="97dd87a9-d2dc-4bc4-8d0c-6e4661333f35">Macao</option>
                            <option value="6bec1ee9-b43b-42bc-9614-750c2facfe3e">Madagascar</option>
                            <option value="654bdc4d-51a5-4e54-ba20-ccf985d16c2a">Malasia</option>
                            <option value="cf6a44f4-4e2b-4413-8847-8d325bf806b2">Malawi</option>
                            <option value="ab8e2dda-342c-498a-aba3-f58ca6ea46ee">Maldivas</option>
                            <option value="315e7cda-fa28-494f-b236-1265483beafe">Malí</option>
                            <option value="b304198b-41f9-431d-bba4-e7f37ec85497">Malta</option>
                            <option value="794570dc-c095-4120-9567-ead857fcf99c">Marruecos</option>
                            <option value="924a97f1-8b84-448a-ad34-7f1799d66535">Martinica</option>
                            <option value="79c4d559-32aa-4496-b261-d9501a1dde0f">Mauricio</option>
                            <option value="64e2a05b-e512-469f-be11-89437407e414">Mauritania</option>
                            <option value="3f3d8b37-945e-4e40-bf20-623c94137bd9">Mayotte</option>
                            <option value="edffeb80-4c64-4108-9525-7ab0a06c5e36">México</option>
                            <option value="f813d598-c3ab-4b44-af4f-69e3524b8702">Micronesia</option>
                            <option value="c696b0f2-31d7-4ed6-ba6e-7fbf8f3db20a">Moldavia</option>
                            <option value="6f1a0587-28c1-42ec-947f-10912ba38994">Mónaco</option>
                            <option value="6be45d1c-e633-4304-add2-c610964b463c">Mongolia</option>
                            <option value="fc0e494f-1877-4e07-af61-17bce27e0aa0">Montserrat</option>
                            <option value="ae13deb0-3902-41b2-b92a-302b17f0af8d">Mozambique</option>
                            <option value="ed884584-6b87-431d-937e-fab9e12b58b5">Myanmar</option>
                            <option value="0232be8b-5b11-4a46-8c8c-2537f56489ee">Namibia</option>
                            <option value="8bb98281-3e48-4716-8af8-26da64a8bad0">Nauru</option>
                            <option value="a3592a08-c393-4269-8368-975181dcedc9">Nepal</option>
                            <option value="dd3b07ee-d4a8-4e77-af6d-ad75b184c643">Nicaragua</option>
                            <option value="07d60550-ca00-4821-8805-cbe1764f4e0a">Níger</option>
                            <option value="5001bd1d-1cd4-4951-86a8-a1256e83663b">Nigeria</option>
                            <option value="3b54fa6a-f198-4beb-9c3d-c6d8d51276e4">Niue</option>
                            <option value="7c7168dd-9aae-4ba2-8427-e8cbf39980a6">Noruega</option>
                            <option value="2d86bd90-0323-441d-aa8f-e1acc751480a">Nueva Caledonia</option>
                            <option value="0253b357-b461-4434-a232-bde61cd90a3d">Nueva Zelanda</option>
                            <option value="c3f4ce9a-502c-4a99-8ede-77395d9ab982">Omán</option>
                            <option value="93d8a0b5-a872-4ccb-b108-946cff331500">Países Bajos</option>
                            <option value="fac21a46-bac9-49b7-887c-2686542763ba">Pakistán</option>
                            <option value="304360b5-a29d-4f52-8ec1-ddfd76901686">Palau</option>
                            <option value="52a06a97-7755-45fd-a6f9-9e0e2fb00cf1">Palestina</option>
                            <option value="8bbe3531-be80-4cf8-a21f-1f9e543f7702">Panamá</option>
                            <option value="a8c97b98-5ad5-4136-96df-9de741459a66">Papúa Nueva Guinea</option>
                            <option value="5503fe7f-a472-443d-a3bf-a758b539a2b9">Paraguay</option>
                            <option value="16be46e9-8972-437e-ae6e-d555c0abbf4f">Perú</option>
                            <option value="81c8804d-e778-41e9-8ec6-a9a4e3adb8b2">Polinesia Francesa</option>
                            <option value="025d6684-17bb-44a9-932b-cc4d03a3b026">Polonia</option>
                            <option value="09e2dd95-df7e-428b-bec6-408ee8e6d5e5">Portugal</option>
                            <option value="c809de3f-ab27-40f6-a4de-d3da4322c68b">Puerto Rico</option>
                            <option value="ad155b9d-844f-47ff-85dd-2919ffbd9c34">Qatar</option>
                            <option value="d3aafab1-6cec-435d-a650-768adb3d0015">Reino Unido</option>
                            <option value="1b532a34-6eb2-457e-a541-396b9691a4c7">República Centroafricana</option>
                            <option value="83cbcb7f-ce6e-4d9e-95ec-343e8ffbd352">República Checa</option>
                            <option value="fe377788-5208-4f75-8eb0-7c16c2f5a51c">República Democrática del Congo</option>
                            <option value="fbfe38af-94da-4732-97a6-729f173d9937">República Dominicana</option>
                            <option value="0b5792ac-5686-43fc-8efd-e9cd8fafa27a">Reunión</option>
                            <option value="fd578788-4f8d-47ee-b7ca-9e9e07836bfa">Ruanda</option>
                            <option value="5e73af74-90d9-4e0b-9296-9a63f134d074">Rumania</option>
                            <option value="e45a2d54-1f59-4a05-990e-76677c9d2673">Rusia</option>
                            <option value="be5e4964-514c-422f-8720-b4f20c68c24c">Sahara Occidental</option>
                            <option value="99a26e3a-16b5-4321-8ef3-016dbd925c59">Samoa</option>
                            <option value="ddef150e-27fe-47d5-baa3-6d5f8a2d2836">Samoa Americana</option>
                            <option value="93565305-46bd-4e7d-8b97-3d0ccae40d52">San Cristóbal y Nevis</option>
                            <option value="a35dbf71-8d84-465e-b57f-3ea0fd8351e8">San Marino</option>
                            <option value="065e7341-7ea2-4c65-844d-a837c2ea77e4">San Pedro y Miquelón</option>
                            <option value="a0d1b2d8-1f65-4d96-95cf-6f17fdd706a3">San Vicente y las Granadinas</option>
                            <option value="8ed3739d-7e2a-41ef-9adb-b597efac8321">Santa Helena</option>
                            <option value="c47720d3-d14a-4d95-921c-eb6d00f138ca">Santa Lucía</option>
                            <option value="5e87c50d-f2b5-42b8-b151-a139f32317fe">Santo Tomé y Príncipe</option>
                            <option value="bc37fee4-dbc4-4a5e-8038-8605ae37a593">Senegal</option>
                            <option value="86353b55-cd76-4e70-9e32-9ba81c20971e">Serbia y Montenegro</option>
                            <option value="466928af-e871-48df-92b8-8c9b59c75580">Seychelles</option>
                            <option value="94828e17-ee44-4d2b-afd5-11dec394266c">Sierra Leona</option>
                            <option value="ab346e08-ba22-417c-8e2d-aea64e152ec4">Singapur</option>
                            <option value="2297bd2f-9651-4fe6-bd89-c77a9ee0b27a">Siria</option>
                            <option value="38b708f7-0317-41cb-963b-f0a943999ae0">Somalia</option>
                            <option value="7c2ce468-bd8a-4608-84bc-caeb9a452f6c">Sri Lanka</option>
                            <option value="a9676459-4629-4457-8cc2-0e86c8d305b7">Suazilandia</option>
                            <option value="6a96a7cf-446c-4e2d-993d-0b216f5eab80">Sudáfrica</option>
                            <option value="6796f86e-b6e0-4f6b-9536-531a3639108f">Sudán</option>
                            <option value="53c9fdf7-5056-47ac-9ef3-1b094631d900">Suecia</option>
                            <option value="0387af4e-2fc9-46d5-a724-529f255ad279">Suiza</option>
                            <option value="fc832f2a-6ddc-4785-bee6-aca9411dfd39">Surinam</option>
                            <option value="943b22ae-6f70-4dc9-8c6d-8932053c4347">Svalbard y Jan Mayen</option>
                            <option value="3d3c6a4c-3bf3-4007-ad3a-6985757cb9ba">Tailandia</option>
                            <option value="82797782-e7ee-4679-b5bd-cd968ca428bb">Taiwán</option>
                            <option value="220abc71-5ae0-4396-b432-9d33337a0a4d">Tanzania</option>
                            <option value="51fca73f-9155-4743-9b20-fc5f729a18f6">Tayikistán</option>
                            <option value="919d02da-41a2-4643-81b0-370c7834bf1e">Territorio Británico del Océano Índico</option>
                            <option value="8ced5fb7-2c66-4f2d-9021-833331dc8048">Territorios Australes Franceses</option>
                            <option value="bc43c198-6c24-4648-b4c0-598e38e49e21">Timor Oriental</option>
                            <option value="3f22970c-993d-4821-8d29-7516b3ea7928">Togo</option>
                            <option value="fb3ca633-9774-496e-a506-c82bf00404f3">Tokelau</option>
                            <option value="8e807af8-494b-42f8-bb8a-4690c23f6d98">Tonga</option>
                            <option value="be5ec95d-9b08-4dce-be63-729d025f044b">Trinidad y Tobago</option>
                            <option value="c59cae79-b17f-4b1a-8211-1a44eb8a6ee6">Túnez</option>
                            <option value="ff2503c9-8027-40bd-bec9-113bbf41b141">Turkmenistán</option>
                            <option value="4054f64b-5ddc-4d9f-a2a3-4e1748736ded">Turquía</option>
                            <option value="9f368812-06ca-4a9c-83b6-08491582fa74">Tuvalu</option>
                            <option value="22347fe8-42ed-43e9-b6f4-481b991f24ee">Ucrania</option>
                            <option value="22465abb-4021-468f-957a-ab9d6c9471bc">Uganda</option>
                            <option value="447f4fc1-aff3-475c-ab11-9b8934118b89">Uruguay</option>
                            <option value="184d656e-d4f0-43e0-a01c-b961d08a4c63">Uzbekistán</option>
                            <option value="e05d983f-4dfc-4d65-9349-3e6334910270">Vanuatu</option>
                            <option value="825d47c4-1dc4-4658-b65a-da53e1f282a2">Venezuela</option>
                            <option value="c9f5e9ed-b1a6-42ea-ba8e-76add79a87ea">Vietnam</option>
                            <option value="e017edab-4adf-4db3-9f28-d3558a6079d9">Wallis y Futuna</option>
                            <option value="845edad6-f29c-4b49-9ae1-118b7754aedc">Yemen</option>
                            <option value="9ad6f6b9-6d5b-4462-99eb-aa73c8f3ec99">Yibuti</option>
                            <option value="4d870382-9b08-4e21-8450-70db2eda4cea">Zambia</option>
                            <option value="d25041ff-640c-4134-af17-babfea4a4f52">Zimbabwe</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Comunidad autónoma / Región</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                        </select>
                    </div>
                </div>
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Provincia</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Nacionalidad</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END INFORMACIÓN CONTACTO -->

    <!-- DATOS DE NACIMIENTO -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-nacimiento" role="button" aria-expanded="false" aria-controls="collapse-nacimiento">
            Datos de nacimiento
        </a>
        <div class="collapse" id="collapse-nacimiento">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Ciudad</label>
                        <input placeholder="Introduce tu ciudad" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">País</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                            <option value="78118e78-47fd-490f-92c4-e34797c9b087">Angola</option>
                            <option value="8c4dc66e-faf0-45f1-937c-a87891a7e395">Anguilla</option>
                            <option value="24ef5244-c8a6-4c9e-aba8-566826531ca9">Antártida</option>
                            <option value="8ca92982-f3d0-4df8-8dee-0ef19fbf98be">Antigua y Barbuda</option>
                            <option value="6052a938-abca-45c4-9541-75a1b4a1983e">Antillas Holandesas</option>
                            <option value="6e3b2135-2fda-45ca-96e3-c57ea6729022">Arabia Saudí</option>
                            <option value="cd0d1a97-f03c-447e-99bf-7cb27928cc6e">Argelia</option>
                            <option value="044eee21-2364-41c8-bc52-1ff3d1033b88">Argentina</option>
                            <option value="05daaed4-ef44-48d0-ae82-1743528398c0">Armenia</option>
                            <option value="b581b0bb-1686-4d8b-b4c5-1d1be561ff0b">Aruba</option>
                            <option value="87baf63b-24f3-4e61-b73e-b2226cf3b5cc">ARY Macedonia</option>
                            <option value="4662f2c9-da06-4759-8a3e-a5df2c9ace61">Australia</option>
                            <option value="657c5a7e-8302-43d8-b025-3523b8104aa1">Austria</option>
                            <option value="280a936e-9ce6-47a1-99ce-af47bb9df61b">Azerbaiyán</option>
                            <option value="43c981cf-c034-4df7-a0a6-5329a7eb7a24">Bahamas</option>
                            <option value="fa9dbe2f-0df2-401a-ad59-9a6557b88e08">Bahréin</option>
                            <option value="26ae08d2-d610-4d23-8d02-3d0fcb02584c">Bangladesh</option>
                            <option value="bca5cc89-0211-4696-8baa-b2c6d7090feb">Barbados</option>
                            <option value="e54f14b3-05a6-47cf-8335-0f2d02769a31">Bélgica</option>
                            <option value="49c02869-b7a2-4a70-9f75-caad5363f130">Belice</option>
                            <option value="8071dd96-3aa5-408f-b690-cf97724909d9">Benin</option>
                            <option value="270cac43-20b9-4e56-91a7-8d23a1a688de">Bermudas</option>
                            <option value="92d18cac-dfe0-42fa-9180-24c078c3c371">Bhután</option>
                            <option value="57a54be6-d64d-4488-a4de-1f3267df2492">Bielorrusia</option>
                            <option value="9755acf5-77a7-43d7-aeac-692c12ce6801">Bolivia</option>
                            <option value="32d75163-1baa-43c3-975a-af84f3bcd33a">Bosnia y Herzegovina</option>
                            <option value="26cd9574-c586-4fc4-9aff-ed8d298a569b">Botsuana</option>
                            <option value="8e38f790-77e8-47d1-956c-a64a89103f63">Brasil</option>
                            <option value="ec78af5e-678e-4292-92e9-e5841bc41bb7">Brunéi</option>
                            <option value="71207a55-f06e-4891-964c-c3481ed20aa9">Bulgaria</option>
                            <option value="83804114-9bfc-4ddf-bda5-c86638508a62">Burkina Faso</option>
                            <option value="f1f7a863-11bc-4992-acce-c9b99487bb4f">Burundi</option>
                            <option value="5370664e-af56-42f5-9b33-ab0f0417bdb4">Cabo Verde</option>
                            <option value="bd504159-e1dd-49e5-8e40-f105068f3d9c">Camboya</option>
                            <option value="6ac3a29c-8248-4085-892f-dac3ff03b7bf">Camerún</option>
                            <option value="7a3cd989-ece8-4fd1-ac90-ee19be2f2b5e">Canadá</option>
                            <option value="c0ca8475-23ac-4f4f-93ba-c28d06f69b67">Chad</option>
                            <option value="56da1c0b-772c-4ed6-bf8a-c61c80c8caff">Chile</option>
                            <option value="7bec9170-f338-467d-bd93-6113d8168439">China</option>
                            <option value="8dcc7c84-f4d5-41f4-8981-5844bdf02112">Chipre</option>
                            <option value="7a26e83b-e956-4ce4-a797-b6326f370374">Ciudad del Vaticano</option>
                            <option value="cb1dc8fc-5fbb-4b96-a07e-d6d2286d53f1">Colombia</option>
                            <option value="46c079d5-0a29-4920-ae9d-2a21c1a3ed98">Comoras</option>
                            <option value="cda90c82-da96-4c12-89c9-383a4d3a900b">Congo</option>
                            <option value="73384b52-18ce-497e-b175-7037e2245757">Corea del Norte</option>
                            <option value="95976c3c-ecf7-41df-ae23-6fd4aab0b969">Corea del Sur</option>
                            <option value="a4c5274d-9b1d-4306-8725-2f2876523b4c">Costa de Marfil</option>
                            <option value="95294524-fd46-4635-8971-701a63949804">Costa Rica</option>
                            <option value="d470aed3-74c7-4d31-b5c6-dce4d7322f61">Croacia</option>
                            <option value="d4d6380d-e650-4089-a924-71a0c546fb03">Cuba</option>
                            <option value="e6167713-acc0-49ff-9e5a-c48aab15e5b4">Dinamarca</option>
                            <option value="00d4f2af-5051-4f01-bf50-dca6f0840a69">Dominica</option>
                            <option value="3d8fe44c-bf9e-4a2a-afe8-5b856d9ab0c4">Ecuador</option>
                            <option value="d14adac1-d306-4c68-9f8d-458726cc286c">Egipto</option>
                            <option value="67f17331-4e1c-46e5-9761-2a6f0be92056">El Salvador</option>
                            <option value="f63077d5-4c6e-4d5a-ad73-799dea0dca9d">Emiratos Árabes Unidos</option>
                            <option value="2319b8b1-8211-4994-8b35-c2c04e9066e5">Eritrea</option>
                            <option value="6fa9b8c0-ecb9-4e63-a1ac-6bef0299e56a">Eslovaquia</option>
                            <option value="a2cdba20-ce03-4ff8-8ca7-77f1a5c99ca6">Eslovenia</option>
                            <option value="98d604b4-3141-4499-bde1-c320f09ef45c" selected="selected">España</option>
                            <option value="3ea91f4c-293d-4e29-ab64-9293f28a12eb">Estados Unidos</option>
                            <option value="f8a8580c-8b4b-4d67-b487-3612e0716b69">Estonia</option>
                            <option value="bcd71081-47ca-4a53-9b52-fba56545f2a5">Etiopía</option>
                            <option value="2ebd2f60-bac0-4532-81ef-45dc1f45c6e2">Filipinas</option>
                            <option value="9fcfa3c2-9d7b-45ff-97f2-ad2097aff07f">Finlandia</option>
                            <option value="e5a21c0b-98d2-4ea1-a074-05544c803f77">Fiyi</option>
                            <option value="b8b70243-eb59-4a88-aaca-cb367c9d0b5f">Francia</option>
                            <option value="0e8cf3f7-154b-4fdd-98df-04cb26786af1">Gabón</option>
                            <option value="87db8f6e-27bc-4443-9fcc-9795c44e8dc0">Gambia</option>
                            <option value="d314d230-03a9-4d7c-adaf-ce8404e02d8d">Georgia</option>
                            <option value="c8cf89da-5f47-4028-b999-b7970aa5e776">Ghana</option>
                            <option value="64021223-79e6-45c6-b318-32f5b15ace77">Gibraltar</option>
                            <option value="b7988bb0-540d-4ad7-a734-2df234fa6820">Granada</option>
                            <option value="afb5eb1a-5334-40d2-b428-0b9577ce54e4">Grecia</option>
                            <option value="21e35659-1839-4877-9705-64b8b4c3c01a">Groenlandia</option>
                            <option value="5b8dc635-aef1-4be7-aac5-cbb0261c725e">Guadalupe</option>
                            <option value="2e88249a-68bb-4d88-844c-687c3fa9b8b5">Guam</option>
                            <option value="4a838ef9-a921-429e-9949-6edca59a5967">Guatemala</option>
                            <option value="c2a79452-430d-47d0-a523-b549a33f2449">Guayana Francesa</option>
                            <option value="60fb826a-8c83-49ba-9cbb-64679aeb3b3c">Guinea</option>
                            <option value="7752dbfb-9df9-4f1c-95d5-8233229cd982">Guinea Ecuatorial</option>
                            <option value="ea751ba1-263e-4084-b1aa-1f3edc8e4945">Guinea-Bissau</option>
                            <option value="ba1d53c6-3d37-48fc-8de7-ce2e88379dfb">Guyana</option>
                            <option value="2cb6cd9a-e2f6-4373-bd3e-9259756bf099">Haití</option>
                            <option value="3153d9a2-c982-40bd-a3b9-f4c97c0dc285">Honduras</option>
                            <option value="520c6fd4-e8b4-4f5a-84dc-800a340ab103">Hong Kong</option>
                            <option value="51e0fddf-b7e9-4cf1-838d-15cf9b481589">Hungría</option>
                            <option value="eb16b4af-09e1-4fcd-8f6a-1da31fd1bb73">India</option>
                            <option value="1b2c093a-e529-4832-901b-799f038e2498">Indonesia</option>
                            <option value="19a39a10-c0a9-4d1d-9b48-9d83cd0289d3">Irán</option>
                            <option value="e67dbb4c-d5fe-4d70-b999-6794bfe9acf0">Iraq</option>
                            <option value="685fe6ac-6910-4c4d-a352-26fb65d1d7a9">Irlanda</option>
                            <option value="cf84aea1-8e7f-49b1-afbd-16ac350b96fd">Isla Bouvet</option>
                            <option value="63975fce-9e3e-478c-a476-dd4511c8b02c">Isla de Navidad</option>
                            <option value="afa22ac6-7a06-49af-930e-0c7cabaacd21">Isla Norfolk</option>
                            <option value="e4a818bf-7aec-4af0-b7c6-91979b7ab7af">Islandia</option>
                            <option value="61516a48-678a-45a0-a9e8-ec3506d74df0">Islas Caimán</option>
                            <option value="7cd3aa30-51f3-401e-9fb7-09522b4c91fc">Islas Cocos</option>
                            <option value="24ccc5d2-63a9-48f8-84e0-1b33b84a0349">Islas Cook</option>
                            <option value="b59c8ef0-42c6-47f4-a358-3f08c288e835">Islas Feroe</option>
                            <option value="8cd5e8d8-ed74-4c6b-88d6-07cbe01f9c6f">Islas Georgias del Sur y Sandwich del Sur</option>
                            <option value="afc2b9a4-5e22-4658-bfbc-f3e55c268e93">Islas Gland</option>
                            <option value="ea5f2f5e-6886-4c6f-b289-1f06a1d60aea">Islas Heard y McDonald</option>
                            <option value="015dcea1-94a6-4937-98b5-639145104650">Islas Malvinas</option>
                            <option value="41db9c4a-9c39-47cd-8b61-e30b38eb4b1b">Islas Marianas del Norte</option>
                            <option value="5e225226-546f-43b2-9cff-26367fd4c501">Islas Marshall</option>
                            <option value="03fac88c-540f-425e-9513-32d25c1200a7">Islas Pitcairn</option>
                            <option value="d0ee3c84-e674-4957-b192-e6bb9e841ceb">Islas Salomón</option>
                            <option value="297a00b2-0ed0-4968-842a-51ef46899462">Islas Turcas y Caicos</option>
                            <option value="a6266c9e-1a52-44ec-b193-8d0e89e07693">Islas ultramarinas de Estados Unidos</option>
                            <option value="7fe46ca5-6440-4f3c-8330-0584ee70ba12">Islas Vírgenes Británicas</option>
                            <option value="8c7a5fe2-0c83-459a-9f77-071afb1d9d85">Islas Vírgenes de los Estados Unidos</option>
                            <option value="b1226777-3c2c-4bbe-bba5-e61cf2c4bfb2">Israel</option>
                            <option value="0a677a04-7634-4dac-ba85-06cee1901067">Italia</option>
                            <option value="7eb6c619-2d9b-42f8-a032-16bc1fd9b870">Jamaica</option>
                            <option value="760c2143-b7ac-4c2e-a873-8478f1eff570">Japón</option>
                            <option value="317ad2ae-6a28-4f83-ad92-5dd055d0cdb5">Jordania</option>
                            <option value="c4f59c7d-c53d-4665-b5d2-f1e3c5bc40bf">Kazajstán</option>
                            <option value="b1bf3e85-c4c3-4d31-919e-e23e7dc9e785">Kenia</option>
                            <option value="72e572e1-22a0-4ed0-9ec3-130f89487287">Kirguistán</option>
                            <option value="12efc107-5c37-4269-8bf8-019f2e9a2d02">Kiribati</option>
                            <option value="942528a6-562f-4cdc-99f1-3591e10f7d6b">Kuwait</option>
                            <option value="882a5515-ab26-42e4-90a7-a98028bfe048">Laos</option>
                            <option value="c7bd6189-ac9a-496a-8f2f-1404abfab7d8">Lesotho</option>
                            <option value="6fc67913-e8b1-49ca-8f47-954659cda946">Letonia</option>
                            <option value="79c1c7b7-be1b-405e-a199-c080d44d239f">Líbano</option>
                            <option value="1b3daf07-0f70-4720-970a-5ad785696a4c">Liberia</option>
                            <option value="547960fd-f814-4a6a-b34e-9a3113d3b846">Libia</option>
                            <option value="4398976a-0e34-4ecc-a6b1-e2965e34eebe">Liechtenstein</option>
                            <option value="7ddb9175-581f-4bc6-a243-0e9a0028f5d3">Lituania</option>
                            <option value="20c8d61c-dad3-447c-b5a4-40859e139f15">Luxemburgo</option>
                            <option value="97dd87a9-d2dc-4bc4-8d0c-6e4661333f35">Macao</option>
                            <option value="6bec1ee9-b43b-42bc-9614-750c2facfe3e">Madagascar</option>
                            <option value="654bdc4d-51a5-4e54-ba20-ccf985d16c2a">Malasia</option>
                            <option value="cf6a44f4-4e2b-4413-8847-8d325bf806b2">Malawi</option>
                            <option value="ab8e2dda-342c-498a-aba3-f58ca6ea46ee">Maldivas</option>
                            <option value="315e7cda-fa28-494f-b236-1265483beafe">Malí</option>
                            <option value="b304198b-41f9-431d-bba4-e7f37ec85497">Malta</option>
                            <option value="794570dc-c095-4120-9567-ead857fcf99c">Marruecos</option>
                            <option value="924a97f1-8b84-448a-ad34-7f1799d66535">Martinica</option>
                            <option value="79c4d559-32aa-4496-b261-d9501a1dde0f">Mauricio</option>
                            <option value="64e2a05b-e512-469f-be11-89437407e414">Mauritania</option>
                            <option value="3f3d8b37-945e-4e40-bf20-623c94137bd9">Mayotte</option>
                            <option value="edffeb80-4c64-4108-9525-7ab0a06c5e36">México</option>
                            <option value="f813d598-c3ab-4b44-af4f-69e3524b8702">Micronesia</option>
                            <option value="c696b0f2-31d7-4ed6-ba6e-7fbf8f3db20a">Moldavia</option>
                            <option value="6f1a0587-28c1-42ec-947f-10912ba38994">Mónaco</option>
                            <option value="6be45d1c-e633-4304-add2-c610964b463c">Mongolia</option>
                            <option value="fc0e494f-1877-4e07-af61-17bce27e0aa0">Montserrat</option>
                            <option value="ae13deb0-3902-41b2-b92a-302b17f0af8d">Mozambique</option>
                            <option value="ed884584-6b87-431d-937e-fab9e12b58b5">Myanmar</option>
                            <option value="0232be8b-5b11-4a46-8c8c-2537f56489ee">Namibia</option>
                            <option value="8bb98281-3e48-4716-8af8-26da64a8bad0">Nauru</option>
                            <option value="a3592a08-c393-4269-8368-975181dcedc9">Nepal</option>
                            <option value="dd3b07ee-d4a8-4e77-af6d-ad75b184c643">Nicaragua</option>
                            <option value="07d60550-ca00-4821-8805-cbe1764f4e0a">Níger</option>
                            <option value="5001bd1d-1cd4-4951-86a8-a1256e83663b">Nigeria</option>
                            <option value="3b54fa6a-f198-4beb-9c3d-c6d8d51276e4">Niue</option>
                            <option value="7c7168dd-9aae-4ba2-8427-e8cbf39980a6">Noruega</option>
                            <option value="2d86bd90-0323-441d-aa8f-e1acc751480a">Nueva Caledonia</option>
                            <option value="0253b357-b461-4434-a232-bde61cd90a3d">Nueva Zelanda</option>
                            <option value="c3f4ce9a-502c-4a99-8ede-77395d9ab982">Omán</option>
                            <option value="93d8a0b5-a872-4ccb-b108-946cff331500">Países Bajos</option>
                            <option value="fac21a46-bac9-49b7-887c-2686542763ba">Pakistán</option>
                            <option value="304360b5-a29d-4f52-8ec1-ddfd76901686">Palau</option>
                            <option value="52a06a97-7755-45fd-a6f9-9e0e2fb00cf1">Palestina</option>
                            <option value="8bbe3531-be80-4cf8-a21f-1f9e543f7702">Panamá</option>
                            <option value="a8c97b98-5ad5-4136-96df-9de741459a66">Papúa Nueva Guinea</option>
                            <option value="5503fe7f-a472-443d-a3bf-a758b539a2b9">Paraguay</option>
                            <option value="16be46e9-8972-437e-ae6e-d555c0abbf4f">Perú</option>
                            <option value="81c8804d-e778-41e9-8ec6-a9a4e3adb8b2">Polinesia Francesa</option>
                            <option value="025d6684-17bb-44a9-932b-cc4d03a3b026">Polonia</option>
                            <option value="09e2dd95-df7e-428b-bec6-408ee8e6d5e5">Portugal</option>
                            <option value="c809de3f-ab27-40f6-a4de-d3da4322c68b">Puerto Rico</option>
                            <option value="ad155b9d-844f-47ff-85dd-2919ffbd9c34">Qatar</option>
                            <option value="d3aafab1-6cec-435d-a650-768adb3d0015">Reino Unido</option>
                            <option value="1b532a34-6eb2-457e-a541-396b9691a4c7">República Centroafricana</option>
                            <option value="83cbcb7f-ce6e-4d9e-95ec-343e8ffbd352">República Checa</option>
                            <option value="fe377788-5208-4f75-8eb0-7c16c2f5a51c">República Democrática del Congo</option>
                            <option value="fbfe38af-94da-4732-97a6-729f173d9937">República Dominicana</option>
                            <option value="0b5792ac-5686-43fc-8efd-e9cd8fafa27a">Reunión</option>
                            <option value="fd578788-4f8d-47ee-b7ca-9e9e07836bfa">Ruanda</option>
                            <option value="5e73af74-90d9-4e0b-9296-9a63f134d074">Rumania</option>
                            <option value="e45a2d54-1f59-4a05-990e-76677c9d2673">Rusia</option>
                            <option value="be5e4964-514c-422f-8720-b4f20c68c24c">Sahara Occidental</option>
                            <option value="99a26e3a-16b5-4321-8ef3-016dbd925c59">Samoa</option>
                            <option value="ddef150e-27fe-47d5-baa3-6d5f8a2d2836">Samoa Americana</option>
                            <option value="93565305-46bd-4e7d-8b97-3d0ccae40d52">San Cristóbal y Nevis</option>
                            <option value="a35dbf71-8d84-465e-b57f-3ea0fd8351e8">San Marino</option>
                            <option value="065e7341-7ea2-4c65-844d-a837c2ea77e4">San Pedro y Miquelón</option>
                            <option value="a0d1b2d8-1f65-4d96-95cf-6f17fdd706a3">San Vicente y las Granadinas</option>
                            <option value="8ed3739d-7e2a-41ef-9adb-b597efac8321">Santa Helena</option>
                            <option value="c47720d3-d14a-4d95-921c-eb6d00f138ca">Santa Lucía</option>
                            <option value="5e87c50d-f2b5-42b8-b151-a139f32317fe">Santo Tomé y Príncipe</option>
                            <option value="bc37fee4-dbc4-4a5e-8038-8605ae37a593">Senegal</option>
                            <option value="86353b55-cd76-4e70-9e32-9ba81c20971e">Serbia y Montenegro</option>
                            <option value="466928af-e871-48df-92b8-8c9b59c75580">Seychelles</option>
                            <option value="94828e17-ee44-4d2b-afd5-11dec394266c">Sierra Leona</option>
                            <option value="ab346e08-ba22-417c-8e2d-aea64e152ec4">Singapur</option>
                            <option value="2297bd2f-9651-4fe6-bd89-c77a9ee0b27a">Siria</option>
                            <option value="38b708f7-0317-41cb-963b-f0a943999ae0">Somalia</option>
                            <option value="7c2ce468-bd8a-4608-84bc-caeb9a452f6c">Sri Lanka</option>
                            <option value="a9676459-4629-4457-8cc2-0e86c8d305b7">Suazilandia</option>
                            <option value="6a96a7cf-446c-4e2d-993d-0b216f5eab80">Sudáfrica</option>
                            <option value="6796f86e-b6e0-4f6b-9536-531a3639108f">Sudán</option>
                            <option value="53c9fdf7-5056-47ac-9ef3-1b094631d900">Suecia</option>
                            <option value="0387af4e-2fc9-46d5-a724-529f255ad279">Suiza</option>
                            <option value="fc832f2a-6ddc-4785-bee6-aca9411dfd39">Surinam</option>
                            <option value="943b22ae-6f70-4dc9-8c6d-8932053c4347">Svalbard y Jan Mayen</option>
                            <option value="3d3c6a4c-3bf3-4007-ad3a-6985757cb9ba">Tailandia</option>
                            <option value="82797782-e7ee-4679-b5bd-cd968ca428bb">Taiwán</option>
                            <option value="220abc71-5ae0-4396-b432-9d33337a0a4d">Tanzania</option>
                            <option value="51fca73f-9155-4743-9b20-fc5f729a18f6">Tayikistán</option>
                            <option value="919d02da-41a2-4643-81b0-370c7834bf1e">Territorio Británico del Océano Índico</option>
                            <option value="8ced5fb7-2c66-4f2d-9021-833331dc8048">Territorios Australes Franceses</option>
                            <option value="bc43c198-6c24-4648-b4c0-598e38e49e21">Timor Oriental</option>
                            <option value="3f22970c-993d-4821-8d29-7516b3ea7928">Togo</option>
                            <option value="fb3ca633-9774-496e-a506-c82bf00404f3">Tokelau</option>
                            <option value="8e807af8-494b-42f8-bb8a-4690c23f6d98">Tonga</option>
                            <option value="be5ec95d-9b08-4dce-be63-729d025f044b">Trinidad y Tobago</option>
                            <option value="c59cae79-b17f-4b1a-8211-1a44eb8a6ee6">Túnez</option>
                            <option value="ff2503c9-8027-40bd-bec9-113bbf41b141">Turkmenistán</option>
                            <option value="4054f64b-5ddc-4d9f-a2a3-4e1748736ded">Turquía</option>
                            <option value="9f368812-06ca-4a9c-83b6-08491582fa74">Tuvalu</option>
                            <option value="22347fe8-42ed-43e9-b6f4-481b991f24ee">Ucrania</option>
                            <option value="22465abb-4021-468f-957a-ab9d6c9471bc">Uganda</option>
                            <option value="447f4fc1-aff3-475c-ab11-9b8934118b89">Uruguay</option>
                            <option value="184d656e-d4f0-43e0-a01c-b961d08a4c63">Uzbekistán</option>
                            <option value="e05d983f-4dfc-4d65-9349-3e6334910270">Vanuatu</option>
                            <option value="825d47c4-1dc4-4658-b65a-da53e1f282a2">Venezuela</option>
                            <option value="c9f5e9ed-b1a6-42ea-ba8e-76add79a87ea">Vietnam</option>
                            <option value="e017edab-4adf-4db3-9f28-d3558a6079d9">Wallis y Futuna</option>
                            <option value="845edad6-f29c-4b49-9ae1-118b7754aedc">Yemen</option>
                            <option value="9ad6f6b9-6d5b-4462-99eb-aa73c8f3ec99">Yibuti</option>
                            <option value="4d870382-9b08-4e21-8450-70db2eda4cea">Zambia</option>
                            <option value="d25041ff-640c-4134-af17-babfea4a4f52">Zimbabwe</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Comunidad autónoma / Región</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="00000000-0000-0000-0000-000000000000">Sin especificar</option>
                            <option value="01fd88b8-9f7a-4e8b-9507-c6f64f0678c8">Afganistán</option>
                            <option value="1cb67d02-77ca-4659-bde5-493a9f044603">Albania</option>
                            <option value="4ce4d4ef-f984-475d-b3b5-7094fb776c8b">Alemania</option>
                            <option value="ff0c120f-8a81-442b-b43d-4b21e60de364">Andorra</option>
                        </select>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END DATOS DE NACIMIENTO -->

    <!-- IDENTIFICADORES -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-identificadores" role="button" aria-expanded="false" aria-controls="collapse-identificadores">
            Identificadores
        </a>
        <div class="collapse" id="collapse-identificadores">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group ">
                        <label class="control-label d-block">Identificadores</label>
                        <select id="" name="" class="js-select2" data-select-search="true">
                            <option value="Identificador 1">Identificador 1</option>
                            <option value="Identificador 2">Identificador 2</option>
                            <option value="Identificador 3">Identificador 3</option>
                            <option value="Identificador 4">Identificador 4</option>
                            <option value="Identificador 5">Identificador 5</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Número</label>
                        <input placeholder="Introduce el número de identificador" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END IDENTIFICADORES -->

    <!-- MÓVIL -->
    <div class="simple-collapse">
        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-movil" role="button" aria-expanded="false" aria-controls="collapse-movil">
            Móvil
        </a>
        <div class="collapse" id="collapse-movil">
            <div class="simple-collapse-content">
                <div class="custom-form-row">
                    <div class="form-group">
                        <label class="control-label d-block">Código internacional</label>
                        <input placeholder="Introduce tu código internacional" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Número</label>
                        <input placeholder="Introduce tu número" type="text" name="" id="" class="form-control not-outline">
                    </div>
                    <div class="form-group">
                        <label class="control-label d-block">Extensión</label>
                        <input placeholder="Introduce tu extensión" type="text" name="" id="" class="form-control not-outline">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END MÓVIL -->

</form>