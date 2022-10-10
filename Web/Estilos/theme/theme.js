/*
    Theme Name: GNOSS Front - MyGnoss base theme
    Theme URI: http://dewenir.es

    Author: GNOSS Front
    Author URI: http://dewenir.es

    Description: Fichero base de customización del tema de MyGNOSS.
    Version: 1.0
*/

var bodyScrolling = {
    obj_top: 0,
    init: function () {
        this.config();
        this.scroll();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    scroll: function () {
        var that = this;
        $(window).scroll(function () {
            that.lanzar();
        });
        return;
    },
    lanzar: function () {
        var obj = $(window);
        this.obj_top = obj.scrollTop();
        if (this.obj_top <= 10) {
            this.body.removeClass("scrolling");
        } else {
            this.body.addClass("scrolling");
        }
        return;
    }
};

var operativaFullWidth = {
    init: function () {
        this.config();
        this.lanzar();
        this.escalado();

        return;
    },
    config: function () {
        this.body = body;
        this.fullwidthrow = this.body.find('.fullwidthrow');
        this.main = this.body.find('main[role="main"] > .container');
        return;
    },
    lanzar: function () {
        var windows_width = $(window).width();
        var container_width = this.main.width();
        var anchoScrollbar = (this.body.width() > 767) ? 31.5 : 0;
        var margen = (windows_width - container_width + anchoScrollbar) / 2;

        var margenNegativo = parseFloat('-' + margen);

        this.fullwidthrow.each(function () {
            var item = $(this);
            item.css({
                "transform": "translateX(" + margenNegativo + "px)",
                "width": "100vw"
            });
        });

        return;
    },
    escalado: function () {
        var that = this;

        $(window).resize(function () {
            that.lanzar();
        });

        return;
    }
};

var menusLateralesManagement = {
    init: function () {
        this.config();
        this.montarMenusLaterales();
        this.comportamientoBotonCerrar();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    montarMenusLaterales: function () {
        this.montarMenuLateral();
        this.montarMenuLateralComunidad();
        this.montarMenuLateralMetabuscador();
        this.montarMenuLateralUsuario();
        // this.onResize();
    },
    montarMenuLateralUsuario: function () {
        if (!$('#menuLateralUsuario').length > 0) return;

        $('#menuLateralUsuario').slideReveal({
            trigger: $("#menuLateralUsuarioTrigger"),
            width: 320,
            overlay: true,
            position: 'right',
            push: false,
            autoEscape: false,
        });
    },
    montarMenuLateralMetabuscador: function () {
        if (!$('#menuLateralMetabuscador').length > 0) return;

        $('#menuLateralMetabuscador').slideReveal({
            trigger: $("#menuLateralMetabuscadorTrigger"),
            width: 820,
            overlay: true,
            position: 'right',
            push: false,
            autoEscape: false,
            show: function (slider, trigger) {
                var width = 820
                var windowWidth = $(window).width();
                var width = windowWidth < width ? windowWidth : width;
                slider.css('width', width + 'px');
            },
        });
    },
    montarMenuLateral: function () {
        if (!$('#menuLateral').length > 0) return;

        $('#menuLateral').slideReveal({
            trigger: $("#menuLateralTrigger"),
            width: 320,
            overlay: true,
            position: 'left',
            push: false,
            autoEscape: false,
        });
    },
    montarMenuLateralComunidad: function () {
        if (!$('#menuLateralComunidad').length > 0) return;

        body.append($('#menuLateralComunidad'))

        $('#menuLateralComunidad').slideReveal({
            trigger: $("#menuLateralComunidadTrigger"),
            width: 320,
            overlay: true,
            position: 'left',
            push: false,
            autoEscape: false,
        });
    },
    comportamientoBotonCerrar: function () {
        var localBody = this.body;
        var menus = localBody.find('.menuLateral');
        var cerrar = menus.find('.header .cerrar');

        cerrar.on('click', function () {

            var item = $(this);
            var menu = item.closest('.menuLateral');
            menu.slideReveal("hide");

        });

        return;
    },
    onResize: function () {
        var that = this;
        var width = $(window).width();
        $(window).on('resize', function () {
            if ($(this).width() !== width) {
                width = $(this).width();
                that.montarMenuLateralMetabuscador()
            }
        });
    }

};

var clonarMenuUsuario = {
    init: function () {
        this.config();
        this.copiar();
        return;
    },
    config: function () {
        this.body = body;
        this.main = body.find('main[role="main"]');
        this.menuOriginal = this.body.find('#menuLateralUsuario');
        this.menuClonado = this.clonar();
        return;
    },
    clonar: function () {
        var menuClonadoAux = this.menuOriginal.clone();
        menuClonadoAux.attr("id", "menuLateralUsuarioClonado");
        menuClonadoAux.attr("class", "menuLateral usuario clonado");
        return menuClonadoAux;
    },
    copiar: function () {
        if (this.menuClonado.length > 0) {
            this.main.prepend(this.menuClonado);
        }
    }
};

var sacarPrimerasLetrasNombre = {
    init: function (numLetras, nombre) {
        var resul = this.sacar(numLetras, nombre);
        return resul;
    },
    sacar: function (numLetras, nombre) {
        var resul = "";
        if (nombre == undefined) return;
        var partes = nombre.split(' ');
        $.each(partes, function (c, v) {
            if (c > numLetras - 1) return false;
            var primera = v.substring(0, 1);
            resul = resul + primera;
        });

        return this.sustituirAcentos(resul);
    },
    sustituirAcentos: function (text) {
        var acentos = "ÃÀÁÄÂÈÉËÊÌÍÏÎÒÓÖÔÙÚÜÛãàáäâèéëêìíïîòóöôùúüûÑñÇç";
        var original = "AAAAAEEEEIIIIOOOOUUUUaaaaaeeeeiiiioooouuuunncc";
        for (var i = 0; i < acentos.length; i++) {
            text = text.replace(acentos.charAt(i), original.charAt(i));
        }
        return text;
    }
};

var obtenerClaseBackgroundColor = {
    init: function (nombre) {
        var resul = this.obtener(nombre);
        return resul;
    },
    obtener: function (nombre) {
        //var number = Math.floor(Math.random() * maximo) + 1;
        if (nombre == undefined) return;
        var letra = sacarPrimerasLetrasNombre.init(1, this.sustituirAcentos(nombre)).toLowerCase();
        return 'color-' + letra;
    },
    sustituirAcentos: function (text) {
        if (text == null) return;
        var acentos = "ÃÀÁÄÂÈÉËÊÌÍÏÎÒÓÖÔÙÚÜÛãàáäâèéëêìíïîòóöôùúüûÑñÇç";
        var original = "AAAAAEEEEIIIIOOOOUUUUaaaaaeeeeiiiioooouuuunncc";
        for (var i = 0; i < acentos.length; i++) {
            text = text.replace(acentos.charAt(i), original.charAt(i));
        }
        return text;
    }
};

var circulosPersona = {
    init: function () {
        this.config();
        this.circulos();
        return;
    },
    config: function () {
        this.body = body;
        this.headerResource = this.body.find('.header-resource');
        return;
    },
    circulos: function () {

        var h1Container = this.headerResource.find('.h1-container');

        var titulo = h1Container.find('h1');

        if (titulo.text() != undefined) {
            var iniciales = sacarPrimerasLetrasNombre.init(2, titulo.text());
            var clase = obtenerClaseBackgroundColor.init(titulo.text());
            var spanCirculo = $('<span />').addClass('circuloPersona ' + clase).text(iniciales);

            h1Container.append(spanCirculo);
        }

        return;
    }
};

var filtrarMovil = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.filtrarMovil = this.body.find('.btn-filtrar-movil');
        this.colFacetas = this.body.find('.col-facetas');
        return;
    },
    comportamiento: function () {
        var that = this;

        this.filtrarMovil.off('click').on('click', function (e) {
            if (that.body.hasClass('facetas-abiertas')) {
                that.cerrarPanelLateralFacetas();
            } else {
                that.abrirPanelLateralFacetas();
                that.body.addClass('facetas-abiertas');
            }
        });

        this.colFacetas.find('.cerrar').off('click').on('click', function (e) {
            that.cerrarPanelLateralFacetas();
        });

        return;
    },
    abrirPanelLateralFacetas: function () {
        var bloqueFacetas = this.colFacetas.find('#panFacetas');
        bloqueFacetas.addClass('pmd-scrollbar mCustomScrollbar');
        bloqueFacetas.attr('data-mcs-theme', "minimal-dark");
        this.body.addClass('facetas-abiertas');
    },
    cerrarPanelLateralFacetas: function () {
        var bloqueFacetas = this.colFacetas.find('#panFacetas');
        bloqueFacetas.removeClass('pmd-scrollbar mCustomScrollbar');
        this.body.removeClass('facetas-abiertas');

    }
};

var metabuscador = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.header = this.body.find('#header');
        this.metabuscadorTrigger = this.header.find('a[data-target="menuLateralMetabuscador"]');
        this.metabuscador = this.body.find('#menuLateralMetabuscador');
        this.input = this.metabuscador.find('#txtBusquedaPrincipal');
        this.resultadosMetabuscador = this.body.find('#resultadosMetabuscador');
        this.verMasEcosistema = this.body.find('#verMasEcosistema');
        return;
    },
    comportamiento: function () {
        var that = this;

        that.metabuscadorTrigger.on('click', function (e) {
            that.input.focus();
        });

        that.input.on('keyup', function () {
            var val = that.input.val();
            if (val.length > 0) {
                that.cargarResultados();
            } else {
                that.ocultarResultados();
            }
        });

        return;
    },
    ocultarResultados: function () {
        this.metabuscador.removeClass('mostrarResultados');
    },
    cargarResultados: function () {
        var that = this;
        this.metabuscador.addClass('mostrarResultados');
        // simular la carga de cada sección
        that.cargarRecursos();
        that.cargarDebates();
        that.cargarPreguntas();
        that.cargarPersonas();
        that.linkVerEnElEcosistema();
    },
    showLoader: function (loader_div, time) {
        var loaderBar = loader_div.progressBarTimer({
            autostart: false,
            timeLimit: time,
            warningThreshold: 0,
            baseStyle: '',
            warningStyle: '',
            completeStyle: '',
            smooth: true,
            striped: false,
            animated: false,
            height: 12
        });
        return loaderBar;

    },
    cargar: function (tipo_recurso, tiempo) {
        var loader_container = $('#loader-' + tipo_recurso + '-wrap');
        loader_container.find('.progress-bar').remove();

        var loader_div = $('<div id="#loader-' + tipo_recurso + '" class="progress-bar"></div>');
        loader_container.append(loader_div);
        loader_container.show();

        var loader = this.showLoader(loader_div, tiempo);
        loader.start();

        return new Promise((resolve, reject) => {
            setTimeout(function () {
                loader.stop();
                loader_div.remove();
                loader_container.hide();
                resolve();
            }, tiempo * 1000);
        });
    },
    cargarRecursos: function () {
        var that = this;
        var bloque = that.resultadosMetabuscador.find('.bloque.recursos');
        bloque.hide();

        var procesoCarga = this.cargar('recursos', 5);
        procesoCarga.then(function () {
            bloque.show();
        });
    },
    cargarDebates: function () {
        var that = this;
        var bloque = that.resultadosMetabuscador.find('.bloque.debates');
        bloque.hide();

        var procesoCarga = this.cargar('debates', 4);
        procesoCarga.then(function () {
            bloque.show();
        });
    },
    cargarPreguntas: function () {
        var that = this;
        var bloque = that.resultadosMetabuscador.find('.bloque.preguntas');
        bloque.hide();

        var procesoCarga = this.cargar('preguntas', 3);
        procesoCarga.then(function () {
            bloque.show();
        });
    },
    cargarPersonas: function () {
        var that = this;
        var bloque = that.resultadosMetabuscador.find('.bloque.personas');
        bloque.hide();

        var procesoCarga = this.cargar('personas', 2);
        procesoCarga.then(function () {
            bloque.show();
        });
    },
    linkVerEnElEcosistema: function () {
        var that = this;
        that.verMasEcosistema.hide();
        setTimeout(() => {
            that.verMasEcosistema.show()
        }, 5000);
    }
};

var buscadorSeccion = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    comportamiento: function () {

        var headerListado = this.body.find('#headerListado');
        var input = headerListado.find('#finderSection');

        input.focusin(function () {
            headerListado.addClass('sugerencias');
        }).focusout(function () {
            headerListado.removeClass('sugerencias buscando');
        });

        input.on('keydown', function (e) {
            setTimeout(function () {
                var val = input.val();
                if (val.length > 0) {
                    headerListado.removeClass('sugerencias').addClass('buscando');
                } else {
                    headerListado.removeClass('buscando').addClass('sugerencias');
                }
            }, 100);
        });

        return;
    }
};

var listadoMensajesAcciones = {
    init: function () {
        this.config();
        this.comportamientoCheck();
        this.comportamientosDropdown();
        this.comportamientoRecargar();
        return;
    },
    config: function () {
        this.body = body;
        this.checkAllMesages = this.body.find('#checkAllMesages');
        this.checkActions = this.body.find('#checkActions');
        this.reloadButton = this.body.find('#reloadMensajes');
        return;
    },
    comportamientoRecargar: function () {
        var that = this;

        this.reloadButton.off('click').on('click', function (e) {
            // SIMULACION DE PETICIÓN DE PRUEBAS
            var resourceList = that.body.find('.col-contenido .resource-list');
            resourceList.hide();
            MostrarUpdateProgress()
            setTimeout(function () {
                resourceList.show();
                OcultarUpdateProgress()
            }, 1000);

        });

        return;
    },
    comportamientoCheck: function () {
        var resourceList = this.body.find('.col-contenido .resource-list');
        var resources = resourceList.find('.resource');
        var resourcesCheck = resources.find('.check-wrapper input[type="checkbox"]');

        this.checkAllMesages.off('change').on('change', function (e) {
            if ($(this).is(':checked')) {
                resourcesCheck.prop('checked', true)
            } else {
                resourcesCheck.prop('checked', false)
            }
        });

        return;
    },
    comportamientosDropdown: function () {
        var that = this;
        this.checkActions.on('click', '.item-dropdown', function () {
            var action = $(this);
            var resourceList = that.body.find('.col-contenido .resource-list');
            var resources = resourceList.find('.resource');

            if (action.hasClass('checkall')) {
                that.seleccionarTodos();
            }
            if (action.hasClass('decheckall')) {
                that.deSeleccionarTodos();
            }
            if (action.hasClass('checkAllRead')) {
                that.seleccionarTodosLeidos(resources);
            }
            if (action.hasClass('checkAllNonRead')) {
                that.seleccionarTodosNoLeidos(resources);
            }
        });

        return;
    },
    seleccionarTodos: function () {
        this.checkAllMesages.prop('checked', true);
        this.checkAllMesages.trigger('change');
    },
    deSeleccionarTodos: function () {
        this.checkAllMesages.prop('checked', false);
        this.checkAllMesages.trigger('change');
    },
    seleccionarTodosLeidos: function (resources) {
        this.deSeleccionarTodos()
        var checks = resources.filter('.no-leido').find('.check-wrapper input[type="checkbox"]');;
        checks.prop('checked', true).trigger('change');
    },
    seleccionarTodosNoLeidos: function (resources) {
        this.deSeleccionarTodos()
        var checks = resources.not('.no-leido').find('.check-wrapper input[type="checkbox"]');
        checks.prop('checked', true).trigger('change');
    }
};

var accionDropdownSelect = {
    init: function () {
        if ($('.dropdown-select').length > 0) this.comportamiento();
    },
    comportamiento: function () {
        var select = $('.dropdown-select');
        var menu = select.find('.dropdown-menu');
        var item = menu.find('.item-dropdown');

        item.off('click').on('click', function () {
            var parent = $(this).parents('.dropdown-select');
            var toggle = parent.find('.dropdown-toggle');
            var items = parent.find('.item-dropdown');
            toggle.html($(this).html());
            toggle.addClass('active');
            items.removeClass('activeView');
            $(this).addClass('activeView');
        });
    }
};


// Añadir la clase .dropdown-autofocus (al padre del .dropdown-menu) para que cuando se despligue el dropdown
// se haga autofocus en el primer input que haya en el dropdown
var accionDropdownAutofocus = {
    init: function () {
        $('.dropdown-autofocus').on('shown.bs.dropdown', function () {
            $dropdown = $(this);
            setTimeout(function () {
                $dropdown.find('input').first().focus();
            });
        });
    }
};

var cambioVistaListado = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    comportamiento: function () {

        var accionesListado = this.body.find('.acciones-listado');
        var visualizacion = accionesListado.find('.visualizacion');
        var resourceList = this.body.find('.resource-list');
        var dropdownMenu = visualizacion.find('.dropdown-menu');
        var dropdownToggle = visualizacion.find('.dropdown-toggle');
        var dropdownToggleIcon = dropdownToggle.find('.material-icons');
        var modosVisualizacion = dropdownMenu.find('a');

        modosVisualizacion.on('click', function (e) {
            e.preventDefault();
            var item = $(this);
            var clase = item.data('class-resource-list');

            modosVisualizacion.removeClass('activeView');
            item.addClass('activeView');

            if (clase != "") {
                var icon = item.find('.material-icons').text();
                dropdownToggleIcon.text(icon);
                resourceList.removeClass('compacView listView mosaicView mapView graphView grafoView');
                resourceList.addClass(clase);
            }
        });

        return;
    }
};

var masHeaderMensaje = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.headerMensaje = this.body.find('.header-mensaje');
        return;
    },
    comportamiento: function () {

        this.headerMensaje.off('click', '.ver-mas').on('click', '.ver-mas', function (e) {
            e.preventDefault();
            var verMas = $(this);
            var padre = verMas.parent();
            var ul = padre.find('ul');

            ul.children().each(function (i) {
                var li = $(this);
                if (padre.hasClass('abierto') && i > 1) {
                    li.addClass('oculto');
                    padre.removeClass('abierto');
                    verMas.text('más');
                } else if (!padre.hasClass('abierto') && i > 1) {
                    li.removeClass('oculto');
                    padre.addClass('abierto');
                    verMas.text('menos');
                }
            });

        });

        return;
    }
};

var plegarFacetasCabecera = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    comportamiento: function () {
        var that = this;
        this.facetas = this.body.find('#panFacetas');
        var facetasTitle = this.facetas.find('.faceta-title');

        facetasTitle.off('click').on('click', function (e) {
            var title = $(this);
            var target = $(e.target);
            var box = title.parents('.box');

            if (target.hasClass('search-icon')) {
                e.preventDefault();
                e.stopPropagation();
            } else {
                that.mostrarOcultarFaceta(box);
            }

            alturizarBodyTamanoFacetas.init();
        });
    },
    mostrarOcultarFaceta: function (box) {
        box.toggleClass('plegado');
    },
    mostrarFaceta: function (box) {
        box.removeClass('plegado');
    },


};

var facetasVerMasVerTodos = {
    init: function () {
        this.config();
        this.comportamientoVerMas();
        this.comportamientoVerTodos();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    comportamientoVerTodos: function () { },
    comportamientoVerMas: function () {

        $('.moreResults .ver-mas').off('click').on('click', function () {
            var facetaContainer = $(this).closest('.facetedSearch');
            facetaContainer.find('ul.listadoFacetas > .ocultar').show(200);
            facetaContainer.find('.ver-mas').css('display', 'none');
            facetaContainer.find('.ver-menos').css('display', 'flex');
            alturizarBodyTamanoFacetas.init();
        });

        $('.moreResults .ver-menos').off('click').on('click', function () {
            var facetaContainer = $(this).closest('.facetedSearch');
            facetaContainer.find('ul.listadoFacetas > .ocultar').hide(200);
            facetaContainer.find('.ver-menos').css('display', 'none');
            facetaContainer.find('.ver-mas').css('display', 'flex');
            alturizarBodyTamanoFacetas.init();
            return false;
        });
    }
};

var comportamientosModalFacetas = {
    init: function(){
        $('.js-desplegar-facetas-modal').on('click', function(){
            const button = $(this);
            const faceta_wrap = button.closest('.facetas-wrap');
            const action_buttons = button.closest('ul').find('li');
            const facetas_con_subfaceta = faceta_wrap.find('.faceta.con-subfaceta.ocultarSubFaceta');
            const boton_desplegar_faceta = facetas_con_subfaceta.find('.desplegarSubFaceta span');
            boton_desplegar_faceta.trigger('click');
            action_buttons.removeClass('active');
            button.addClass('active');
        });
        $('.js-plegar-facetas-modal').on('click', function(){
            const button = $(this);
            const faceta_wrap = button.closest('.facetas-wrap');
            const action_buttons = button.closest('ul').find('li');
            const facetas_con_subfaceta = faceta_wrap.find('.faceta.con-subfaceta:not(.ocultarSubFaceta)');
            const boton_desplegar_faceta = facetas_con_subfaceta.find('.desplegarSubFaceta span');
            boton_desplegar_faceta.trigger('click');
            action_buttons.removeClass('active');
            button.addClass('active');
        });
        $('.js-anterior-facetas-modal').on('click', function(){
            $('.resultados-wrap .listadoFacetas').animate({
                marginLeft: 30,
                opacity: 0
            }, 200, function () {
                $('.resultados-wrap .listadoFacetas').css({ marginLeft: -30 });
                $('.resultados-wrap .listadoFacetas').animate({
                    marginLeft: 30,
                    opacity: 1
                }, 200);
            });
        });
        $('.js-siguiente-facetas-modal').on('click', function(){
            $('.resultados-wrap .listadoFacetas').animate({
                marginLeft: -30,
                opacity: 0
            }, 200, function () {
                $('.resultados-wrap .listadoFacetas').css({ marginLeft: 30 });
                $('.resultados-wrap .listadoFacetas').animate({
                    marginLeft: 30,
                    opacity: 1
                }, 200);
            });
        });
    }
}

var plegarSubFacetas = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        return;
    },
    comportamiento: function () {
        $('.desplegarSubFaceta .material-icons').unbind().click(function () {
            var padre = $(this).closest('a');
            var icono = $(this);
            var icono_texto = icono.text().trim();
            if (icono_texto == 'add' || icono_texto == 'remove') {
                if (icono_texto == 'add') {
                    padre.removeClass('ocultarSubFaceta');
                    icono.text('remove');
                } else {
                    padre.addClass('ocultarSubFaceta');
                    icono.text('add');
                }
            }
            if (icono_texto == 'expand_more' || icono_texto == 'expand_less') {
                if (icono_texto == 'expand_more') {
                    padre.removeClass('ocultarSubFaceta');
                    icono.text('expand_less');
                } else {
                    padre.addClass('ocultarSubFaceta');
                    icono.text('expand_more');
                }
            }
            alturizarBodyTamanoFacetas.init();
            return false;
        });
    }
};

var alturizarBodyTamanoFacetas = {
    init: function () {
        this.config();
        this.calcularAltoFacetas();
        return;
    },
    config: function () {
        this.body = body;
        this.panFacetas = this.body.find('#panFacetas');
        this.container = this.body.find('main[role="main"] > .container');
        return;
    },
    calcularAltoFacetas: function () {
        var altoFacetas = this.panFacetas.height();

        this.container.css({
            'min-height': altoFacetas
        });

        return;
    }
};

var mensajeAnyadirRegistro = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.contenido = this.body.find('.contenido');
        return;
    },
    comportamiento: function () {

        var that = this;
        var btn = this.contenido.find('.anyadir-registro');
        var divMensaje = $('<div></div>').addClass('mensaje-anyadir-registro');
        var tituloMensaje = $('<h3></h3>').text('Miembro añadido correctamente');
        var mensaje = $('<p></p>').text('Mensaje visible durante 10 segundos....');
        divMensaje.append(tituloMensaje);
        divMensaje.append(mensaje);

        btn.off('click').on('click', function () {
            that.contenido.append(divMensaje);
            setTimeout(function () {
                divMensaje.remove();
            }, 10000);
        });

        return;
    }
};

// calcular en la carga de la página cuanto padding-top tiene que tener el contenido
// según la altura de la cabecera fixed
var calcularPaddingTopMain = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.header = this.body.find('#header');
        this.main = this.body.find('main[role="main"]');
        return;
    },
    comportamiento: function () {
        var headerHeight = this.header.innerHeight();
        this.main.css('padding-top', headerHeight + 'px');
    }
};

var desplegarDestinatarios = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.destinatarios = this.body.find('.destinatarios-mensaje');
        this.lista = this.destinatarios.find('.lista-destinatarios');
        this.listaExtra = this.lista.filter('.extra');
        this.desplegarDestinatarios = this.body.find('.desplegar-destinatarios');
        return;
    },
    initialCheck: function () {
        if (this.listaExtra.length > 0) {
            this.comportamiento()
        }
    },
    comportamiento: function () {
        var that = this;
        this.desplegarDestinatarios.click(function () {
            that.destinatarios.toggleClass('show');
        });
    }
};

var addCustomScrollBar = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.header = this.body.find('.mensaje-principal');
        return;
    },
    comportamiento: function () {
    }
};

var accionesRecurso = {
    init: function () {
        accionHistorial.init();
    }
}

var accionHistorial = {
    init: function () {
        this.config();
    },
    config: function () {
        this.opciones = {
            paging: false,
            ordering: false,
            select: false,
            searching: false,
            info: false,
            responsive: true,
            autoWidth: false,
            columnDefs: [
                { responsivePriority: 1, targets: 0 },
                { responsivePriority: 1, targets: 1 },
                { responsivePriority: 1, targets: 2 },
                { responsivePriority: 2, targets: 4 },
                { responsivePriority: 1, targets: 5 },
                { responsivePriority: 3, targets: 3 }
            ]
        }
    },
    montarTabla: function () {
        this.modal = $('#modal-container');
        this.tabla = body.find('.tabla-versiones');
        this.hay_tabla = this.tabla.length > 0 ? true : false;

        if (!this.hay_tabla) return;
        this.tabla_montada = this.tabla.DataTable(this.opciones);
        this.comportamientosChecks();
        this.observarModal();

    },
    comportamientosChecks: function () {
        var checks = this.tabla.find("input[type='checkbox']");
        checks.on('change', function () {
            var check = $(this);
            var tr = check.closest('tr');
            if (check.is(':checked')) {
                tr.addClass('activo');
            } else {
                tr.removeClass('activo');
            }
        });
    },
    observarModal: function () {
        var that = this;

        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                if (mutation.attributeName === "class") {
                    that.tabla_montada.responsive.rebuild();
                    that.tabla_montada.responsive.recalc();
                }
            });
        });

        observer.observe(this.modal[0], {
            attributes: true
        });
    }
}

var accionDesplegarCategorias = {
    init: function () {
        this.config();
        this.mostrarOcultarCategoriasHijas();
    },
    config: function () {
        this.pan_categorias = $('.divTesArbol.divCategorias');
        this.desplegables = this.pan_categorias.find('.boton-desplegar')
    },
    mostrarOcultarCategoriasHijas: function () {
        if (this.desplegables.length > 0) {
            this.desplegables.off('click').on('click', function () {
                $(this).toggleClass('mostrar-hijos');
            });
        }
    },
}

var iniciarSelects2 = {
    init: function () {
        this.config();
        this.montarSelects();
    },
    config: function () {
        this.select2 = body.find('.js-select2');
        this.defaultOptions = {
            minimumResultsForSearch: 10
        };
    },
    montarSelects: function () {
        this.select2.select2(this.defaultOptions);
    }
}

var iniciarDropAreaImagenPerfil = {
    init: function () {
        this.config();
        this.montar();
    },
    config: function () {
        this.droparea = body.find('.js-image-uploader');
    },
    montar: function () {
        this.droparea.imageDropArea({});
    }
}

var customizarAvisoCookies = {
    init: function () {
        this.config();
        this.comportamientoAceptar();
        if (this.avisoCookies.length < 1) return;
        return;
    },
    config: function () {
        this.body = body;
        this.avisoCookies = this.body.find('#aviso-cookies');
        return;
    },
    comportamientoAceptar: function () {
        var that = this;
        var aceptar = $('#aceptarCookies');

        aceptar.on('click', function (e) {
            e.preventDefault();
            that.guardarAceptarCookies();
        });
        return;
    },
    guardarAceptarCookies: function () {
        var that = this;
        /*GnossPeticionAjax(
            $('#inpt_baseUrlBusqueda').val() + '/aceptar-cookies',
            null,
            true
        ).fail(function (data) {
            console.log('error al guardar la cookie');
        }).done(function (data) {
            that.avisoCookies.animate( {opacity:'0'}, 400, 'swing', function(){
                that.avisoCookies.remove();
            });
        });*/

        $.ajax({
            url: $('#inpt_baseUrlBusqueda').val() + '/aceptar-cookies'
        }).done(function () {
            that.avisoCookies.animate({ opacity: '0' }, 400, 'swing', function () {
                that.avisoCookies.remove();
            });
        });
        return;
    }
};

var modificarCabeceraOnScrolling = {
    init: function () {
        this.config();
        this.observarBody();
        this.observerScroll();
    },
    config: function () {
        this.body = body;
        this.buscador = body.find(".col-buscador");
        this.colContenido = body.find(".col-contenido");
        this.accionesListado = body.find(".header-listado .acciones-listado");
        this.filtros = body.find("#panFiltros");
        this.resultados = body.find(
            ".header-listado .h1-container .numResultados"
        );
        this.fakeResultados = body.find(
            ".col-buscador .btn-filtrar-movil .resultados"
        );
    },
    observarBody: function () {
        const that = this;
        const target = this.body.get(0);

        var observer = new MutationObserver(function (mutations) {
            mutations.forEach(function (mutation) {
                if (mutation.attributeName !== "class") return;

                if (that.isScrolling()) {
                    that.ajustarElementos();
                } else {
                    that.volverElementosAlEstadoInicial();
                }
            });
        });

        var config = {
            attributes: true,
        };

        observer.observe(target, config);
    },
    isScrolling: function () {
        return this.body.hasClass("scrolling");
    },
    ajustarElementos: function () {
        this.ajustarBuscador();
        this.ajustarNombreComunidad();
    },
    ajustarBuscador: function () {
        const width = this.colContenido.innerWidth();

        if ($(window).width() > 1199) {
            this.buscador.css("width", width - 25 + "px");
        } else {
            this.buscador.css("width", "auto");
        }
    },
    volverBuscadoralEstadoInicial: function () {
        this.buscador.css("width", "100%");
    },
    ajustarNombreComunidad: function () {
        $('.community-menu-wrapper .page-name-wrapper').css("width", this.buscador.offset().left - 100 + "px");
    },
    volverNombreComunidadEstadoInicial: function () {
        $('.community-menu-wrapper .page-name-wrapper').css("width", "auto");
    },
    volverElementosAlEstadoInicial: function () {
        this.volverBuscadoralEstadoInicial();
        this.volverNombreComunidadEstadoInicial();
    },
    observerScroll: function () {
        $(window).on("scrolling-down", this.onScrollDown.bind(this));
        $(window).on("scrolling-up", this.onScrollUp.bind(this));
    },
    onScrollDown: function () {
        this.copiarResultados();
        this.aplicarSombra();
    },
    onScrollUp: function () {
        this.aplicarSombra()
    },
    copiarResultados: function () {
        if (this.resultados.length === 0 || this.fakeResultados.length === 0)
            return;

        this.fakeResultados.text(this.resultados.text());
    },
    aplicarSombra: function () {
        // depende de si hay filtros o no hay que aplicar la sombra a
        // los filtros o a las acciones para que no se superponga una sombra con otra
        if(this.filtros.length === 0 || this.filtros.is(':hidden')){
            this.filtros.removeClass('add-shadow')
            this.accionesListado.addClass('add-shadow');
        }else{
            this.filtros.addClass('add-shadow');
            this.accionesListado.removeClass('add-shadow');
        }
    },
};

/**
 * [mostrarNotificacion]
 * @param  {string} tipo Puede ser 'info', 'success', 'warning', 'error'
 * @param  {string} contenido 'Mensaje que se quiere mostrar'
 */
var mostrarNotificacion = function (tipo, contenido) {
    toastr[tipo](contenido, 'Mensaje de la plataforma', {
        toastClass: 'toast themed',
        positionClass: "toast-bottom-center",
        target: 'body',
        closeHtml: '<span class="material-icons">close</span>',
        showMethod: 'slideDown',
        timeOut: 5000,
        escapeHtml: false,
        closeButton: true,
    });
};

function comportamientoCargaFacetasComunidad() {
    alturizarBodyTamanoFacetas.init();
    plegarFacetasCabecera.init();
    plegarSubFacetas.init();
    facetasVerMasVerTodos.init();
    comportamientosModalFacetas.init();
};

var timeoutUpdateProgres;
function MostrarUpdateProgress() {
    MostrarUpdateProgressTime(15000)
}

function MostrarUpdateProgressTime(time) {
    if ($('#mascaraBlanca').length > 0) {
        $('body').addClass('mascaraBlancaActiva');

        if (time > 0) {
            timeoutUpdateProgress = setTimeout("OcultarUpdateProgress()", time);
        }
    }
}

function OcultarUpdateProgress() {
    if ($('#mascaraBlanca').length > 0) {
        clearTimeout(timeoutUpdateProgress);
        $('body').removeClass('mascaraBlancaActiva');
    }
}


; (function ($) {

    $.imageDropArea = function (element, options) {
        var defaults = {
            inputSelector: ".image-uploader__input",
            dropAreaSelector: ".image-uploader__drop-area",
            preview: ".image-uploader__preview",
            previewImg: ".image-uploader__img",
            errorDisplay: ".image-uploader__error",
            // funcionPrueba: function() {}
        };
        var plugin = this;

        // Objeto HTML del Loading que se mostraría mientras se esté realizando la carga de la imagen
        var loadingSpinnerHtml = "";
        loadingSpinnerHtml +=
            '<div class="spinner-border texto-primario" role="status" style="position: absolute; top: 45%; left:40%">';
        loadingSpinnerHtml += '<span class="sr-only">Cargando...</span>';
        loadingSpinnerHtml += "</div>";

        plugin.settings = {};

        var $element = $(element);
        var element = element;

        plugin.init = function () {
            plugin.settings = $.extend({}, defaults, options);
            plugin["input"] = $(plugin.settings.inputSelector);
            plugin["dropAreaSelector"] = $(plugin.settings.dropAreaSelector);
            plugin["preview"] = $(plugin.settings.preview);
            plugin["previewImg"] = $(plugin.settings.previewImg);
            plugin["errorDisplay"] = $(plugin.settings.errorDisplay);
            onInputChange();
            addDragAndDropEvents();
        };

        var onInputChange = function () {
            plugin.input.change(function () {
                // Mostrar spinner de carga de imagen
                showLoadingImagePreview(true);
                var data = new FormData();
                var files = plugin.input.get(0).files;
                if (files.length > 0) {
                    data.append("ImagenRegistroUsuario", files[0]);
                }
                $.ajax({
                    url: document.location.href,
                    type: "POST",
                    processData: false,
                    contentType: false,
                    data: data,
                    success: function (response) {
                        hideError();
                        onSuccesResponse(response);
                    },
                    error: function (er) {
                        displayError(er.statusText);
                    },
                });
            });
        };

        var displayError = function (error) {
            plugin.errorDisplay.find(".ko").text(error);
            plugin.errorDisplay.find(".ko").show();
            plugin.preview.removeClass("show-preview");
        };
        var hideError = function () {
            plugin.errorDisplay.find(".ko").hide();
        };

        var onSuccesResponse = function (response) {
            if (response.indexOf("imagenes/") === 0) {
                showImagePreview(response);
                showLoadingImagePreview(false);
            } else {
                displayError(response);
                showLoadingImagePreview(false);
            }
        };

        /**
         * @param {boolean} showLoading: Indicar si ha iniciado la carga y por lo tanto, es necesario mostrar un "loading".
         * true: Mostrará el "loading"
         * false: Quitar ese "loading" -> Fin carga de imagen
         */
        var showLoadingImagePreview = function (showLoading) {
            // Mostrar loading
            if (showLoading) {
                // Quitar la imagen actual del preview
                plugin.preview.attr("src", "");
                // Mostrar un spinner dentro del preview.
                plugin.preview.append(loadingSpinnerHtml);
            } else {
                // Quitar loading
                $(".spinner-border").remove();
            }
        };

        var showImagePreview = function (response) {
            plugin.previewImg.attr(
                "src",
                "http://serviciospruebas.gnoss.net" + "/" + response
            );
            plugin.preview.addClass("show-preview");
        };

        var addDragAndDropEvents = function () {
            plugin.dropAreaSelector
                .off("dragenter dragover")
                .on("dragenter dragover", function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                });

            plugin.dropAreaSelector.off("click").on("click", function (e) {
                e.preventDefault();
                e.stopPropagation();
                plugin.input.trigger("click");
            });

            plugin.dropAreaSelector.off("dragleave").on("dragleave", function (e) {
                e.preventDefault();
                e.stopPropagation();
            });

            plugin.dropAreaSelector.off("drop").on("drop", function (e) {
                e.preventDefault();
                e.stopPropagation();
                let dt = e.originalEvent.dataTransfer;
                let files = dt.files;
                plugin.input.get(0).files = files;
                plugin.input.trigger("change");
            });
        };
        plugin.init();
    };

    // add the plugin to the jQuery.fn object
    $.fn.imageDropArea = function (options) {
        return this.each(function () {
            if (undefined == $(this).data("imageDropArea")) {
                var plugin = new $.imageDropArea(this, options);
                $(this).data("imageDropArea", plugin);
            }
        });
    };
})(jQuery);


$.fn.reverse = [].reverse;

var body;

$(function () {
    body = $('body');

    bodyScrolling.init();
    calcularPaddingTopMain.init();
    clonarMenuUsuario.init();
    menusLateralesManagement.init();
    metabuscador.init();
    iniciarSelects2.init();
    accionDesplegarCategorias.init();
    //circulosPersona.init();
    customizarAvisoCookies.init();
    accionDropdownSelect.init();
    accionDropdownAutofocus.init();

    if (body.hasClass('fichaRecurso') || body.hasClass('edicionRecurso')) {
        accionesRecurso.init();
    }

    if (body.hasClass('fichaMensaje')) {
        desplegarDestinatarios.init();
    }

    if (body.hasClass('listado')) {
        filtrarMovil.init();
        buscadorSeccion.init();
        cambioVistaListado.init();
        modificarCabeceraOnScrolling.init();
        comportamientoCargaFacetasComunidad();

        if (body.hasClass('mensajes')) {
            listadoMensajesAcciones.init();
        }
    }

    if (body.hasClass('registro')) {
        iniciarDropAreaImagenPerfil.init();
    }
});