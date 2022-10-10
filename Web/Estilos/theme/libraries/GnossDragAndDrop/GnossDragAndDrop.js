/*!
 * GnossDragAndDrop 0.0.0
 */
; (function ($) {

    const plugin_name = "GnossDragAndDrop"

    $[plugin_name] = function (element, options) {
        var plugin = this;

        plugin["file"] = null;

        plugin["$input"] = $(element);
        plugin["input"] = element;

        plugin.init = function (options) {

            plugin.defaultOptions = {
                dropAreaSelector: ".gdd-area",
                preview: ".gdd-preview-wrap",
                previewImg: ".gdd-preview",
                previewExtension: ".gdd-extension",
                errorDisplay: ".gdd-error",
                nameDisplay: ".gdd-name",
                maxSize: 2000,
                spinner: plugin.getLoaderSpinnerHtml(),
                onFileAdded: function (plugin, files) { },
                onFileRemoved: function (plugin) { }
            };

            plugin["settings"] = { ...plugin.defaultOptions, ...options };

            plugin.settings.texts = { ...plugin.getDefaultTexts(), ...plugin.getCustomTexts() };

            plugin["$component"] = plugin.addHtml(plugin.settings.texts);

            plugin["dropAreaSelector"] = plugin.$component.find(plugin.settings.dropAreaSelector);
            plugin["preview"] = plugin.$component.find(plugin.settings.preview);
            plugin["previewImg"] = plugin.$component.find(plugin.settings.previewImg);
            plugin["errorDisplay"] = plugin.$component.find(plugin.settings.errorDisplay);
            plugin["nameDisplay"] = plugin.$component.find(plugin.settings.nameDisplay);
            plugin["previewExtension"] = plugin.$component.find(plugin.settings.previewExtension);

            plugin.addFormat();
            plugin._addEventOnInputChange();
            plugin.addDragAndDropEvents();
            plugin.loadFileFromData();
        };


        /**
         * Returns default texts
         * @returns {Object}
         */
        plugin.getDefaultTexts = function () {
            const formats = plugin.getAllowedFormatsString();
            const size = plugin.getMaximumSizeString();
            return {
                title: `Arrastra y suelta en la línea de puntos o haz clic aquí para añadir el archivo`,
                format: `Formatos admitidos: ${formats}`,
                size: `Máximo peso: ${size}`,
                formatError: `Formato no permitido. Archivos permitidos: ${formats}`,
                sizeError: `Archivo demasiado pesado. Máximo peso: ${size}`,
            }
        };

        /**
         * Returns custom texts
         * @returns {Object}
         */
        plugin.getCustomTexts = function () {
            const customText = {};
            if (plugin.input.hasAttribute('data-title-text')) {
                customText.title = plugin.input.getAttribute('data-title-text');
            }
            if (plugin.input.hasAttribute('data-size-text')) {
                customText.size = plugin.input.getAttribute('data-size-text');
            }
            if (plugin.input.hasAttribute('data-format-text')) {
                customText.format = plugin.input.getAttribute('data-format-text');
            }
            if (plugin.input.hasAttribute('data-format-error')) {
                customText.formatError = plugin.input.getAttribute('data-format-error');
            }
            if (plugin.input.hasAttribute('data-size-error')) {
                customText.sizeError = plugin.input.getAttribute('data-size-error');
            }
            return customText;
        };

        /**
         * @returns {String}
         */
        plugin.getAllowedFormatsString = function () {
            let formats = "todos los archivos";
            const acceptedFiles = plugin.settings.acceptedFiles;
            switch (acceptedFiles) {
                case 'image':
                    formats = 'archivos de tipo Imagen';
                    break;
                case 'audio':
                    formats = 'archivos de tipo Audio';
                    break;
                case 'video':
                    formats = 'archivos de tipo Vídeo';
                    break;
            }
            if (Array.isArray(acceptedFiles) && acceptedFiles.length) {
                formats = "";
                for (let index = 0; index < acceptedFiles.length; index++) {
                    const extension = acceptedFiles[index];
                    if (index != 0) {
                        formats += ", "
                    }
                    formats += '.' + extension;
                }
            }
            return formats;
        };

        /**
         * @returns {String}
         */
        plugin.getMaximumSizeString = function () {
            return `${plugin.settings.maxSize} KB`
        };

        /**
         * @returns {String}
         */
        plugin.getLoaderSpinnerHtml = function () {
            return `
            <div class="spinner-border texto-primario" role="status" style="position: absolute; top: 45%; left:40%">
                <span class="sr-only">
                    Cargando...
                </span>
            </div>`;
        };

        /**
         * Add the plugin html components to the DOM
         * @param {Object} texts
         * @returns {JQuery}
         */
        plugin.addHtml = function (texts) {
            const $component = $(plugin._getHtml(texts));
            $component.insertBefore(plugin.$input);
            plugin.$input.hide();
            return $component;
        };

        /**
         * Returns the html plugin components as a string
         * @param {Object} texts
         * @returns {String}
         */
        plugin._getHtml = function (texts) {
            return `<div class="gdd-wrap">
                <div class="gdd-area">
                    <div class="gdd-loader-wrapper">
                        <div class="gdd-loader">
                        </div>
                    </div>
                    <div class="gdd-extension-wrap">
                        <div class="gdd-extension"></div>
                    </div>
                    <div class="gdd-preview-wrap">
                        <img class="gdd-preview" src="">
                    </div>
                    <div class="gdd-name">
                    </div>
                    <div class="gdd-info">
                        <p class="gdd-title">${texts.title}</p>
                        <p class="gdd-format">${texts.format}</p>
                        <p class="gdd-size">${texts.size}</p>
                    </div>
                    <div class="gdd-actions">
                        <div class="gdd-edit">
                        </div>
                        <div class="gdd-delete">
                        </div>
                    </div>
                </div>
                <div class="gdd-error">
                    <p class="ko"></p>
                </div>
            </div>`
        };

        /**
         * Adds the required format limitations to the input
         */
        plugin.addFormat = function () {
            const acceptedFiles = plugin.settings.acceptedFiles
            let format = "*";

            switch (acceptedFiles) {
                case '*':
                    format = '*';
                    break;
                case 'image':
                    format = 'image/*';
                    break;
                case 'audio':
                    format = 'audio/*';
                    break;
                case 'video':
                    format = 'video/*';
                    break;
            }
            if (Array.isArray(acceptedFiles) && acceptedFiles.length) {
                format = "";
                for (let index = 0; index < acceptedFiles.length; index++) {
                    const extension = acceptedFiles[index];
                    if (index != 0) {
                        format += ", "
                    }
                    format += '.' + extension;
                }
            }
            plugin.$input.attr('accept', format);
        };

        plugin._addEventOnInputChange = function () {
            plugin.$input.change(plugin._onInputChange)
        };

        /**
         * add file to plugin and style
         * @param {File} file
         */

        plugin._onInputChange = function () {
            plugin.hideError();

            const files = plugin.input.files;

            if(typeof plugin.settings.beforeValidation === "function"){
                plugin.settings.beforeValidation(plugin, files);
            }

            // no files in the input (it has been removed)
            if (!plugin._isAnyFile(files)) {
                plugin.resetPlugin(true);
                return;
            }

            const file = files[0];

            // file not valid
            if (!plugin._validateFile(file)) {
                plugin.resetPlugin(false);
                return;
            };

            plugin._setFile(file);

            if (plugin._isImage(file)) {
                plugin.showImagePreview(file);
                plugin._hideFileIcon();
            } else {
                plugin._showFileIcon(file);
                plugin._deleteImagePreview();
            }

            if(typeof plugin.settings.onFileAdded === "function"){
                plugin.settings.onFileAdded(plugin, files);
            }
        }

        /**
         * Show file icon
         */
        plugin._showFileIcon = function () {
            plugin.$component.addClass('show-file-icon');
        };

        /**
         * Hide file icon
         */
        plugin._hideFileIcon = function () {
            plugin.$component.removeClass('show-file-icon');
        };

        /**
         * Show loading
         */
        plugin._showLoading = function () {
            plugin.$component.addClass('show-loading');
        };

        /**
         * Hide loading
         */
        plugin._hideLoading = function () {
            plugin.$component.removeClass('show-loading');
        };

        /**
         * add file to plugin and style
         * @param {File} file
         */
        plugin._setFile = function (file) {
            plugin.file = file;
            plugin._addFileName(file);
            plugin.$component.addClass('file-added');
            plugin.$component.removeClass('dragover');
        };

        /**
         * reset plugin to no file style
         */
        plugin.resetPlugin = function (hideError = true) {
            plugin.file = null;
            plugin.input.value = "";
            plugin.input.removeAttribute('data-file-url');
            plugin.input.removeAttribute('data-image-url');
            plugin.$component.removeClass('file-added');
            plugin._deleteImagePreview();
            plugin._hideFileIcon();
            if(hideError){
                plugin.hideError();
            }
            // call to custom function
            if(typeof plugin.settings.onFileRemoved === "function"){
                plugin.settings.onFileRemoved(plugin);
            }
        };

        /**
         * Destroys plugin
         */
        plugin.destroy = function () {

            // remove errors, files...
            plugin.resetPlugin();

            // remove added html
            plugin.$component.remove()

            // const $gdd_wrap = plugin.$input.siblings('.gdd-wrap');

            // unbind events
            plugin.$input.unbind();

            // remove data from the input
            plugin.$input.removeData();
        };

        /**
         * add file to plugin and style
         * @param {File} file
         */
        plugin._addFileName = function (file) {
            const url = file.name;
            const splitUrl = url.split('/');
            let lastFilePartName = splitUrl[splitUrl.length -1];

            // remove the uuid of the file loaded in gnoss server
            // maybe not the best solution, plugin check if file name has more than 4 '-' so
            // plugin understands that the file name has uuid
            const fileNameSplitted = lastFilePartName.split('-')
            if(fileNameSplitted.length > 4){
                lastFilePartName = fileNameSplitted[fileNameSplitted.length -1]
            }

            plugin.nameDisplay.html(lastFilePartName);
        };

        /**
         * Returns if there is a file
         * @param {File[]} files
         * @returns {bool}
         */
        plugin._isAnyFile = function (files) {
            return typeof files[0] !== 'undefined';
        };

        /**
         * Returns if the file is an image
         * @param {File} file
         * @returns {bool}
         */
        plugin._isImage = function (file) {
            return file.type.includes('image') || 
                plugin._hasImagePreloaded() || 
                file.name.includes('data:image') || 
                file.name.includes('.jpg') ||
                file.name.includes('.png') ||
                file.name.includes('.gif');
        };

        /**
         * Returns if the file is an image url
         * @param {File} file
         * @returns {bool}
         */
        plugin._isImageUrl = function (file) {
            return file.name.includes('http') && (file.name.includes('.jpg') ||
                file.name.includes('.png') ||
                file.name.includes('.gif'));
        };

        /**
         * Returns if the file is an audio
         * @param {File} file
         * @returns {bool}
         */
        plugin._isAudio = function (file) {
            return file.type.includes('audio');
        };

        /**
         * Returns if the file is a video
         * @param {File} file
         * @returns {bool}
         */
        plugin._isVideo = function (file) {
            return file.type.includes('video');
        };

        /**
         * Returns if file is valid
         * @param {File} file
         * @returns {bool}
         */
        plugin._validateFile = function (file) {
            return plugin._validateFormat(file) && plugin._validateSize(file);
        };

        /**
         * Returns if file is valid size
         * @param {File} file
         * @returns {bool}
         */
        plugin._validateSize = function (file) {
            const fileSizeInKB = file.size / 1000
            if (plugin.settings.maxSize > fileSizeInKB) return true;

            plugin.displaySizeError();
            return false;
        };

        /**
         * Returns if file is valid format
         * @param {File} file
         * @returns {bool}
         */
        plugin._validateFormat = function (file) {
            const acceptedFiles = plugin.settings.acceptedFiles;
            let fileAccepted = false

            if (acceptedFiles === "*") {
                fileAccepted = true;
            }

            if (acceptedFiles === "image" || acceptedFiles === "image/*" ) {
                fileAccepted = plugin._isImage(file);
            }

            if (acceptedFiles === "video" || acceptedFiles === "video/*" ) {
                fileAccepted = plugin._isVideo(file);
            }

            if (acceptedFiles === "audio" || acceptedFiles === "audio/*" ) {
                fileAccepted = plugin._isAudio(file);
            }

            if (Array.isArray(acceptedFiles) && acceptedFiles.length) {
                const extension = plugin.getFileExtension();

                if (acceptedFiles.includes(extension.toLowerCase())) {
                    fileAccepted = true;
                }
            }
            if (fileAccepted) {
                return true;
            } else {
                plugin.displayFormatError();
                return false;
            }
        };

        /**
         * Shows an error
         * @param {String} error
         */
        plugin.displayError = function (error) {
            plugin.errorDisplay.find(".ko").html(error);
            plugin.errorDisplay.find(".ko").show();
        };

        /**
         * Add a file to the input
         * @param {String} url of file
         * @param {bool} isImage of file
         */
        plugin.addFile = function (url, isImage = false) {
            const dT = new ClipboardEvent('').clipboardData || new DataTransfer();
            $.get(url).then(function(data) {
                if(isImage){
                    dT.items.add(new File([url], url, {'type':'image/*'}));

                    // add the attr in order to show the preview
                    const att = document.createAttribute("data-image-url");
                    att.value = url;
                    plugin.input.setAttributeNode(att);
                }else{
                    dT.items.add(new File([url], url));
                }
                plugin.input.files = dT.files;
                plugin.$input.trigger("change");
            });
        };

        /**
         * Set file if data-file-url or data-image-url is present in
         * the input when plugin is initialited
         */
        plugin.loadFileFromData = function () {
            if(!plugin.input.hasAttribute('data-file-url') && !plugin.input.hasAttribute('data-image-url') ) return;

            let fileUrl = "";
            let isImage = false;

            if(plugin.input.hasAttribute('data-file-url')){

                fileUrl = plugin.input.getAttribute('data-file-url');

            }else if(plugin.input.hasAttribute('data-image-url')){

                fileUrl = plugin.input.getAttribute('data-image-url');
                isImage = true;
            };

            plugin.addFile(fileUrl, isImage);
        };

        /**
         * @returns {bool} return true if there is an image to be preloaded
         */
        plugin._hasImagePreloaded = function () {
            return plugin.input.hasAttribute('data-image-url');
        };

        /**
         * Shows an error
         * @param {String} error
         */
        plugin.displaySizeError = function () {
            plugin.displayError(plugin.settings.texts.sizeError)
        };

        /**
         * Shows an error
         * @param {String} error
         */
        plugin.displayFormatError = function () {
            plugin.displayError(plugin.settings.texts.formatError)
        };

        /**
         * Hide error message
         */
        plugin.hideError = function () {
            plugin.errorDisplay.find(".ko").hide();
        };

        /**
         * @returns {File} file or null
         */
        plugin.getFile = function () {
            const files = plugin.input.files;
            // no files in the input (it has been removed)
            if (!plugin._isAnyFile(files)) {
                return null;
            }
            return files[0];
        };

        /**
         * @returns {FileList}
         */
        plugin.getFilesList = function () {
            return plugin.input.files;
        };

        /**
         * @returns {String} file extension
         */
        plugin.getFileExtension = function () {
            return plugin.getFile().name.split('.').pop();
        };

        /**
         * Shows a preview of the image
         */
        plugin.showImagePreview = function (file) {
            plugin.$component.addClass('image-added');
            let src = "";

            // if image has been set by js or html with the data-image-url
            if(plugin.input.hasAttribute('data-image-url')){
                src = plugin.input.getAttribute('data-image-url')
            } if(plugin._isImageUrl(file) || file.name.includes('data:image')){
                // if image has been set by url but without the data-image-url
                src = file.name;
            }else {
                // if image has set normally by the user
                src = URL.createObjectURL(plugin.getFile());
            }

            plugin.previewImg.attr("src", src);
        };

        /**
         * Deletes image preview
         */
        plugin._deleteImagePreview = function () {
            plugin.$component.removeClass('image-added');
            plugin.previewImg.attr(
                "src",
                ""
            );
        };

        /**
         * Add drags and drop events to the div.gdd-area
         */
        plugin.addDragAndDropEvents = function () {
            plugin.dropAreaSelector.off("dragenter").on("dragenter", function (e) {
                e.preventDefault();
                e.stopPropagation();
            });

            plugin.dropAreaSelector.off("dragover").on("dragover", function (e) {
                e.preventDefault();
                e.stopPropagation();
                plugin.$component.addClass('dragover')
            });

            plugin.dropAreaSelector.off("click").on("click", function (e) {
                e.preventDefault();
                e.stopPropagation();
                const target = $(e.target);

                if (target.hasClass('gdd-delete')) {
                    plugin.resetPlugin(true);
                    return;
                }

                if (target.hasClass('gdd-edit')) {
                    plugin.resetPlugin(true);
                }

                plugin.$input.trigger("click");
            });

            plugin.dropAreaSelector.off("dragleave").on("dragleave", function (e) {
                e.preventDefault();
                e.stopPropagation();
                plugin.$component.removeClass('dragover')
            });

            plugin.dropAreaSelector.off("drop").on("drop", function (e) {
                e.preventDefault();
                e.stopPropagation();
                let dt = e.originalEvent.dataTransfer;
                let files = dt.files;
                plugin.input.files = files;
                plugin.$input.trigger("change");
            });
        };

        plugin.init(options);
    };

    // add the plugin to the jQuery.fn object
    $.fn[plugin_name] = function (options, ...args) {
        return this.each(function () {
            const node = this;
            const plugin_instance = $(node).data(plugin_name);

            if (undefined == plugin_instance) {
                // plugin instance does not exist, create a new one
                $(node).data(plugin_name, new $[plugin_name](node, options));
            } else {
                // plugin instance exists, check if the method is beeing called on the same instace
                const stored_input = $(node).data(plugin_name).input;
                if (stored_input === undefined || stored_input != this) return;

                // check if method exists
                if (plugin_instance[options]) {
                    return plugin_instance[options].apply(plugin_instance, args);
                } else {
                    console.log('Method does not exist on ' + plugin_name);
                }
            }
        });
    };
})(jQuery);

