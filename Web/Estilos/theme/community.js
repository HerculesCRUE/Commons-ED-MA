/*
    Theme Name: GNOSS Front - MyGnoss community theme
    Theme URI: http://dewenir.es

    Author: GNOSS Front
    Author URI: http://dewenir.es

    Description: Fichero community del tema de MyGNOSS.
    Version: 1.0
*/

var clonarMenuUsuario = {
    init: function () {
        this.config();
        this.copiarNavegacion();
        return;
    },
    config: function () {
        this.body = body;
        this.header = this.body.find('#header');
        this.usuario = this.header.find('.col.col03 .usuario');
        this.menuOriginal = this.body.find('#menuLateralUsuario');
        this.navegacion = this.menuOriginal.find('#navegacion');
        this.navegacionClonado = this.clonarNavegacion();
        return;
    },
    clonarNavegacion: function () {
        var navegacionClonadoAux = this.navegacion.clone();
        navegacionClonadoAux.attr('id', 'navegacionClonado');
        navegacionClonadoAux.attr('class', 'navegacion clonado');
        return navegacionClonadoAux;
    },
    copiarNavegacion: function () {
        if (this.navegacionClonado.length > 0) {
            this.usuario.prepend(this.navegacionClonado);
        }
        return;
    }
};

var accionesBuscadorCabecera = {
    init: function () {
        this.config();
        //this.mover();
        this.posicionar();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.header = this.body.find('#header');
        this.upperRow = this.header.find('.upper-row');
        this.col02 = this.header.find('.col.col02');
        this.col03 = this.header.find('.col.col03');
        this.buscador = this.body.find('.col-buscador');
        this.main = this.body.find('main');
        return;
    },
    mover: function () {
        this.buscador.removeClass('col col-12');
        this.col02.append(this.buscador);
        return;
    },
    posicionar: function () {
        var that = this;
        var container = that.main.find('.container');
        that.buscador.css('left', container.offset().left);

        $(window).resize(function () {
            that.buscador.css('left', container.offset().left);
        });
        return;
    },
    comportamiento: function () {
        var that = this;
        var buscador = this.col03.find('.buscar > a');
        var icono = buscador.find('span');

        buscador.off('click').on('click', function () {
            if (that.upperRow.hasClass('show')) {
                that.upperRow.removeClass('show');
                icono.text('search');
            } else {
                that.upperRow.addClass('show');
                icono.text('close');
            }
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
        this.header = this.body.find('#header');
        this.buscador = this.header.find('.col-buscador');
        this.main = this.body.find('main');
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
        var that = this;
        var container = that.main.find('.container');

        if (!$('#menuLateralMetabuscador').length > 0) return;

        $('#menuLateralMetabuscador').slideReveal({
            trigger: $('#txtBusquedaPrincipal'),
            width: 740,
            overlay: true,
            position: 'left',
            push: false,
            autoEscape: false,
            show: function (slider) {
                that.body.addClass('metabuscador-abierto');

                // Add position
                var position;
                setTimeout(() => {
                    if (container.offset().left < 177) {
                        position = that.buscador.offset().left;
                    } else {
                        position = container.offset().left;
                    }
                    slider.css('left', position);
                });

                // Click to close
                $(document).on('click', '#menuLateralMetabuscador, .row.upper-row, #txtBusquedaPrincipal', function(e) {
                    e.stopPropagation();
                    var $target = $(e.target);
                    if(!$target.closest('#menuLateralMetabuscador').length && $('#menuLateralMetabuscador').is(":visible")) {
                        $('#menuLateralMetabuscador').slideReveal('hide');
                    }
                });
                $('#txtBusquedaPrincipal').off('click');
                $('#txtBusquedaPrincipal').on('click', function(e){
                    e.stopPropagation();
                    $('#menuLateralMetabuscador').slideReveal('show');
                    $('.col-facetas .cerrar').trigger('click');
                });
            },
            hide: function () {
                that.body.removeClass('metabuscador-abierto');
            }
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
        this.metabuscadorTrigger = this.header.find('.col-buscador');
        this.metabuscador = this.body.find('#menuLateralMetabuscador');
        this.input = this.metabuscadorTrigger.find('#txtBusquedaPrincipal');
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

var communityMenuMovil = {
    init: function () {
        this.config();
        this.comportamiento();
        return;
    },
    config: function () {
        this.body = body;
        this.menu = this.body.find('#community-menu');
        return;
    },
    comportamiento: function () {
        var item = this.menu.find('li');

        item.off('click').on('click', function () {
            var that = $(this);
            that.parent().toggleClass('visible');
        });
        return;
    }
};

var accionesPlegarDesplegarModal = {
    init: function () {
        this.cambiarSeccion();
        this.montarTabla();
        this.expandAll();
        this.collapseAll();
        this.collapse();
        this.expandAllModal();
        this.collapseAllModal();
    },
    cambiarSeccion: function () {
        var cabecera = $('.cabecera-cv');
        var h1_container = cabecera.find('.h1-container');
        var enlace = h1_container.find('.dropdown-menu a');

        enlace.click(function (e) {
            var id = $(this).attr('href');
            enlace.removeClass('active');
            $(id).tab('show');
        });
    },
    montarTabla: function () {
        this.tabla = body.find('.tabla-curriculum');

        this.opciones = {
            lengthChange: false,
            info: false
        };

        this.tabla.DataTable(this.opciones);
    },
    expandAll: function () {
        var button = $('#expandAll');

        button.off('click').on('click', function (e) {
            var $this = $(e.target);
            var dataTarget = $this.attr('data-target');

            $(dataTarget + ' ' + 'a[data-toggle="collapse"]').each(function (i, event) {
                var $this = $(event);
                var objectID = $this.attr('href');
                if ($(objectID).hasClass('in') === false) {
                    $(objectID).collapse('show');
                    $(objectID).parent().addClass('active');
                }
            });
        });
    },
    collapseAll: function () {
        var button = $('#collapseAll');

        button.off('click').on('click', function (e) {
            var $this = $(e.target);
            var dataTarget = $this.attr('data-target');

            $(dataTarget + ' ' + 'a[data-toggle="collapse"]').each(function (i, event) {
                var $this = $(event);
                var objectID = $this.attr('href');
                $(objectID).collapse('hide');
                $(objectID).parent().removeClass('active');
            });
        });
    },
    collapse: function () {
        var button = $('.arrow');

        button.off('click').on('click', function () {
            var resource = $(this).parents('.resource');
            if (resource.hasClass('plegado')) {
                resource.removeClass('plegado');
            } else {
                resource.addClass('plegado');
            }
        });
    },
    expandAllModal: function () {
        var button = $('.expandAll');

        button.off('click').on('click', function(e) {
            var $this = $(e.target);
            var dataTarget = $this.attr('data-target');
            var form = button.parents('.formulario-edicion');

            form.find(dataTarget + '[data-toggle="collapse"]').each(function (i, event) {
                var $this = $(event);
                var objectID = $this.attr('href');
                $(objectID).collapse('hide');
            });
        });
    },
    collapseAllModal: function () {
        var button = $('.collapseAll');

        button.off('click').on('click', function (e) {
            var $this = $(e.target);
            var dataTarget = $this.attr('data-target');
            var form = button.parents('.formulario-edicion');

            form.find(dataTarget + '[data-toggle="collapse"]').each(function (i, event) {
                var $this = $(event);
                var objectID = $this.attr('href');
                $(objectID).collapse('show');
            });
        });
    }
};

var iniciarComportamientoImagenUsuario = {
    init: function () {
        this.config();
        this.montarPlugin();
    },
    config: function () {
        this.droparea = $('#foto-perfil-cv');
    },
    montarPlugin: function () {
        options = {
            sizeLimit: 250,
        };
        this.droparea.imageDropArea2(options);
    },
};

var iniciarDatepicker = {
    init: function() {
        this.montarPlugin();
    },
    montarPlugin: function() {
        $('.datepicker').datepicker({
            closeText: 'Cerrar',
            prevText: '<Ant',
            nextText: 'Sig>',
            currentText: 'Hoy',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        });
    }
};

var collapseResource = {
    init: function() {
        this.collapse();
    },
    collapse: function() {
        var boton = $('.arrow');

        boton.off('click').on('click', function() {
            var resource = $(this).parents('.resource');

            if (resource.hasClass('plegado')) {
                $(this).text('keyboard_arrow_down');
                resource.removeClass('plegado');
            } else {
                $(this).text('keyboard_arrow_up');
                resource.addClass('plegado');
            }
        });
    }
};

var comportamientoVerMasVerMenos = {
    init: function () {
        this.comportamiento();
    },
    comportamiento: function () {
        $('.description-wrap .moreResults .ver-mas').off('click').on('click', function () {
            var showMore = $(this).closest('.showMore');
            showMore.find('.ver-mas').css('display', 'none');
            showMore.find('.ver-menos').css('display', 'flex');
            var description_wrap = $(this).parents('.description-wrap');
            var desc = description_wrap.find('.desc');
            desc.css('display', 'block');
        });

        $('.description-wrap .moreResults .ver-menos').off('click').on('click', function () {
            var showMore = $(this).closest('.showMore');
            showMore.find('.ver-menos').css('display', 'none');
            showMore.find('.ver-mas').css('display', 'flex');
            var description_wrap = $(this).parents('.description-wrap');
            var desc = description_wrap.find('.desc');
            desc.css('display', '-webkit-box');
        });
    }
};

var comportamientoVerMasVerMenosTags = {
    init: function() {
        this.comportamiento();
    },
    comportamiento: function() {
        $('.list-wrap .moreResults .ver-mas').off('click').on('click', function () {
            var list = $(this).closest('.list-wrap');
            list.find('ul > .ocultar').show(200);
            list.find('.ver-mas').css('display', 'none');
            list.find('.ver-menos').css('display', 'flex');
        });

        $('.list-wrap .moreResults .ver-menos').off('click').on('click', function () {
            var list = $(this).closest('.list-wrap');
            list.find('ul > .ocultar').hide(200);
            list.find('.ver-menos').css('display', 'none');
            list.find('.ver-mas').css('display', 'flex');
        });
    }
};

var edicionListaAutorCV = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.formulario = $('.formulario-datos-experiencia');
        this.collapse_lista_autores = this.formulario.find('#collapse-lista-autores');
        this.collapse_autores = this.formulario.find('#collapse-autores');
    },
    comportamiento: function () {
        var that = this;
        this.collapse_lista_autores.find('.collapse-toggle').off('click').on('click', function () {
            if (that.collapse_lista_autores.hasClass('show')) {
                that.collapse_lista_autores.collapse('hide');
            }
        });

        this.collapse_autores.find('.collapse-toggle').off('click').on('click', function () {
            if (that.collapse_autores.hasClass('show')) {
                that.collapse_lista_autores.collapse('show');
            }
        });
    }
};

var operativaFormularioAutor = {
    init: function () {
        this.config();
        this.formPrincipal();
        this.formAutor();
        this.formCodigo();
    },
    config: function () {
        this.modal_autor = $('#modal-anadir-autor');
        this.formularioPrincipal = this.modal_autor.find('.formulario-principal');
        this.formularioAutor = this.modal_autor.find('.formulario-autor');
        this.formularioCodigo = this.modal_autor.find('.formulario-codigo');
    },
    formPrincipal: function () {
        var that = this;
        var btn = $('.form-principal');

        // Si se hace click en el enlace
        btn.off('click').on('click', function() {
            that.formularioPrincipal.show();
            that.formularioCodigo.hide();
            that.formularioAutor.hide();
        });

        this.modal();
    },
    formAutor: function () {
        var that = this;
        var btn = $('.form-autor');

        // Si se hace click en el enlace
        btn.off('click').on('click', function() {
            that.formularioPrincipal.hide();
            that.formularioCodigo.hide();
            that.formularioAutor.show();
        });

        this.modal();
    },
    formCodigo: function () {
        var that = this;
        var btn = $('.form-buscar');

        // Si se hace click en el enlace
        btn.off('click').on('click', function() {
            that.formularioPrincipal.hide();
            that.formularioCodigo.show();
            that.formularioAutor.hide();
        });

        this.modal();
    },
    modal: function () {
        var that = this;

        $('#modal-anadir-autor').on('show.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('hide');
        });

        $('#modal-editar-autor').on('show.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('hide');
        });

        $('#modal-anadir-autor').on('hide.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('show');
            $(this).find('.formulario-edicion').hide();
            that.formularioPrincipal.show();
        });

        $('#modal-editar-autor').on('hide.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('show');
        });
    }
};

var operativaFormularioTesauro = {
    init: function () {
        $('#modal-editar-entidad-0').on('show.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('hide');
            plegarSubFacetas.init();
        });

        $('#modal-editar-entidad-0').on('hide.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('show');
        });

        $('.js-desplegar-facetas-modal').on('click', function (){
            const button = $(this);
            const aux = button.closest('.entityaux');
            const action_buttons = button.closest('ul').find('li');
            const facetas_con_subfaceta = aux.find('.faceta.con-subfaceta.ocultarSubFaceta');
            const boton_desplegar_faceta = facetas_con_subfaceta.find('.desplegarSubFaceta span');
            boton_desplegar_faceta.trigger('click');
            action_buttons.removeClass('active');
            button.addClass('active');
        });

        $('.js-plegar-facetas-modal').on('click', function (){
            const button = $(this);
            const aux = button.closest('.entityaux');
            const action_buttons = button.closest('ul').find('li');
            const facetas_con_subfaceta = aux.find('.faceta.con-subfaceta:not(.ocultarSubFaceta)');
            const boton_desplegar_faceta = facetas_con_subfaceta.find('.desplegarSubFaceta span');
            boton_desplegar_faceta.trigger('click');
            action_buttons.removeClass('active');
            button.addClass('active');
        });
    }
};

var operativaFormularioProduccionCientifica = {
    init: function () {
        this.config();
        this.formPublicacion();
        this.formProyecto();
        this.modal();
    },
    config: function () {
        this.modal_prod_cientifica = $('#modal-enviar-produccion-cientifica');
        this.formularioPublicacion = this.modal_prod_cientifica.find('.formulario-publicacion');
        this.formularioProyecto = this.modal_prod_cientifica.find('.formulario-proyecto');
        this.resourceList = this.formularioProyecto.find('.resource-list');
    },
    formPublicacion: function () {
        var that = this;
        this.formularioPublicacion.find('.btn').off('click').on('click', function () {
            that.modal_prod_cientifica.find('.modal-body > .alert').hide();
            that.formularioPublicacion.hide();
            that.formularioProyecto.show();
        });
    },
    formProyecto: function () {
        var that = this;
        that.formularioProyecto.find('> .alert').hide();
        this.formularioProyecto.find('.btn').off('click').on('click', function () {
            if (that.resourceList.find('.resource .form-check-input').is(':checked')) {
                that.formularioProyecto.find('> .alert').hide();
                $(this).attr('data-dismiss', 'modal');
                mostrarNotificacion('success', 'La publicación permanecerá bloqueada hasta que se resuelva el procedimiento');
            } else {
                that.formularioProyecto.find('> .alert').show();
                $(this).addClass('disabled');
            }
        });

        this.resourceList.find('.resource .form-check-inline').on('change', function () {
            that.formularioProyecto.find('.btn').removeClass('disabled');
        });
    },
    modal: function () {
        var that = this;
        this.modal_prod_cientifica.on('hide.bs.modal', function () {
            // Status initial
            that.modal_prod_cientifica.find('.modal-body > .alert').show();
            that.formularioPublicacion.show();
            that.formularioPublicacion.find('.resource .form-check-input').prop('checked', false);
            that.formularioProyecto.hide();
            that.formularioProyecto.find('> .alert').hide();
            that.formularioProyecto.find('.btn').removeClass('disabled').attr('data-dismiss', '');
            that.formularioProyecto.find('.resource .form-check-input').prop('checked', false);
        });
    }
};

var comportamientoTopicosCV = {
    init: function () {
        $('#modal-anadir-topicos').on('show.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('hide');
        });

        $('#modal-anadir-topicos').on('hide.bs.modal', function () {
            $('#modal-anadir-datos-experiencia').modal('show');
        });
    }
};

var mostrarFichaCabeceraFixed = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.body = body;
        this.cabecera = this.body.find('.cabecera-ficha');
        this.contenido = this.body.find('.contenido-ficha');
    },
    comportamiento: function () {
        if (this.contenido.length < 1) return;
        const position = this.contenido.position().top;
        $(window).scroll(function (e) {
            var scroll = $(window).scrollTop() + 20;
            if(scroll >= position) {
                body.addClass('cabecera-ficha-fixed');
                return;
            } else {
                body.removeClass('cabecera-ficha-fixed');
                return;
            }
        });
    }
};

var clonarNombreFicha = {
    init: function () {
        this.config();
        this.copyName();
        this.copyIcon();
    },
    config: function () {
        this.body = body;
        this.cabeceraFicha = this.body.find('.cabecera-ficha');
        this.paneles = this.body.find('.tab-paneles-ficha');
        this.tabs = this.paneles.find('.tabs');
    },
    copyName: function () {
        if (body.hasClass('fichaRecurso-investigador')) {
            var nombre = this.cabeceraFicha.find('.nombre-usuario-wrap .nombre');
        } else {
            var nombre = this.cabeceraFicha.find('.ficha-title');
        }
        var divNombre = $('<div />').addClass('nombre');
        divNombre.append(nombre.text());
        this.tabs.prepend(divNombre);
    },
    copyIcon: function () {
        var icono = this.cabeceraFicha.find('.ficha-icon-wrap');
        this.tabs.find('.nombre').append(icono.html());
    }
};

var montarTooltip = {
    lanzar: function (elem, title, classes) {
        elem.tooltip({
            html: true,
            placement: 'bottom',
            template: '<div class="tooltip ' + classes + '" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
            title: title
        }).on('mouseenter', function () {
            // console.log('enter')
            var _this = this;
            // $(this).tooltip('show');
            $('.tooltip').on('mouseenter', function () {
                $(this).tooltip('show');
            }).on('mouseleave', function () {
                $(this).tooltip('hide')
            })
        }).on('mouseleave', function () {
            // console.log('leave')
            var _this = this;
            setTimeout(function () {
                if ($('.tooltip:hover').length < 0) {
                    // console.log('hover')
                    $(_this).tooltip("hide")
                }
                $('.tooltip').on('mouseleave', function () {
                    //console.log('leave')
                    $(_this).tooltip("hide");
                });
            });
        });

        this.comportamiento(elem);
    },
    comportamiento: function (elem) {
        var that = this;
        $(elem).on('shown.bs.tooltip', function () {
            that.cerrar();
        });
    },
    cerrar: function () {
        $('.tooltip').find('.cerrar').off('click').on('click', function() {
            $(this).parents('.tooltip').tooltip('hide');
        });
    }
};

var tooltipsAccionesRecursos = {
    init: function () {
        this.config();
        this.lanzar();
    },
    config: function () {
        this.body = body;
        this.listWrap = this.body.find('.list-wrap');
        this.info_resource = this.body.find('.info-resource');
        this.quotes = this.body.find('.quotes');
        this.link = this.listWrap.find('.link');
        this.block = this.body.find('.block-wrapper');
        this.visible = this.body.find('.eye');
        this.oculto = this.body.find('.visibility-activo');
    },
    lanzar: function () {
        montarTooltip.lanzar(this.info_resource, '', 'background-gris grupos');
        montarTooltip.lanzar(this.quotes, this.getTooltipQuotes(), 'background-blanco citas');
        montarTooltip.lanzar(this.link, this.getTooltipLink(), 'background-blanco link');
        montarTooltip.lanzar(this.block, 'Bloqueado', 'background-gris-oscuro');
        montarTooltip.lanzar(this.visible, 'Visible', 'background-gris-oscuro');
        montarTooltip.lanzar(this.oculto, 'Oculto', 'background-gris-oscuro');
    },
    getTooltipQuotes: function () {
        return `<p class="tooltip-title">Fuente de citas</p>
                <span class="material-icons cerrar">close</span>
                <ul class="no-list-style">
                    <li>
                        <span class="texto">SCOPUS</span>
                        <span class="num-resultado">144</span>
                    </li>
                    <li>
                        <span class="texto">WOS</span>
                        <span class="num-resultado">24</span>
                    </li>
                    <li>
                        <span class="texto">INRECS</span>
                        <span class="num-resultado">18</span>
                    </li>
                </ul>`;
    },
    getTooltipLink: function () {
        return `<p class="tooltip-title">Enlazado con</p>
                <span class="material-icons cerrar">close</span>
                <div class="tooltip-content">
                    <img src="./theme/resources/logotipos/logognossazul.png">
                    <p>National Center for Biotechnology Information Search database</p>
                </div>
                <a href="#" class="tooltip-link">Ir al enlace <span class="material-icons">keyboard_arrow_right</span></a>`
    }
};

var tooltipsCV = {
    init: function () {
        this.config();
        this.navegacion();
        this.traducciones();
    },
    config: function () {
        this.navegacionCV = $('#navegacion-cv');
        this.traduccionesWrap = $('.traducciones-wrapper');
    },
    navegacion: function () {
        var elemento = this.navegacionCV.find('li > a');

        $.each(elemento, function (i) {
            if ($(elemento[i]).find('.texto-tooltip').length > 0) {
                montarTooltip.lanzar($(elemento[i]), $(elemento[i]).find('.texto-tooltip'), 'background-gris-oscuro');
            } else {
                montarTooltip.lanzar($(elemento[i]), $(elemento[i]).text(), 'background-gris-oscuro');
            }
        });
    },
    traducciones: function () {
        var item = this.traduccionesWrap.find('.translation');

        $.each(item, function (i) {
            var circulo = $(item[i]).find('.circulo-color');
            var texto;

            if ($(item[i]).hasClass('translation-es')) {
                texto = 'Español (ES)';
            } else if ($(item[i]).hasClass('translation-en')) {
                texto = 'Inglés (EN)';
            }

            if (circulo.hasClass('circulo-verde')) {
                montarTooltip.lanzar($(item[i]), 'Con traducción: ' + texto, 'background-gris-oscuro');
            } else {
                montarTooltip.lanzar($(item[i]), 'Sin traducción: ' + texto, 'background-gris-oscuro');
            }
        });
    }
};

var cambioTraducciones = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.body = body;
    },
    comportamiento: function () {
        var accionesListado = this.body.find('.acciones-listado');
        var traducciones = accionesListado.find('.traducciones');
        var resourceList = this.body.find('.resource-list');
        var dropdownMenu = traducciones.find('.dropdown-menu');
        var dropdownToggle = traducciones.find('.dropdown-toggle');
        var dropdownToggleIcon = dropdownToggle.find('.material-icons');
        var modosTraducciones = dropdownMenu.find('a');

        modosTraducciones.on('click', function (e) {
            e.preventDefault();
            var item = $(this);
            var clase = item.data('class-resource-list');

            modosTraducciones.removeClass('activeView');
            item.addClass('activeView');

            if (clase != "") {
                var icon = item.find('.material-icons').text();
                dropdownToggleIcon.text(icon);
                resourceList.removeClass('translationEsView translationEnView');
                resourceList.addClass(clase);
            }
        });
    }
};

var contarLineasDescripcion = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.descriptionWrap = $('.description-wrap');
    },
    comportamiento: function () {
        this.descriptionWrap.each(function () {
            var descDiv = $(this).find('.desc');
            var divHeight = parseInt(descDiv.innerHeight());
            var lineHeight = parseInt(descDiv.css('line-height'));
            var lines = parseInt(divHeight / lineHeight);

            // Si no ha sido contado
            if(!$(this).hasClass('counted')) {
                // Do
                if (lines > 3) {
                    descDiv.css('overflow', 'hidden')
                        .css('display', '-webkit-box')
                        .css('-webkit-line-clamp', '3')
                        .css('-webkit-box-orient', 'vertical')
                        .css('text-overflow', 'ellipsis');
                } else {
                    $(this).closest('.description-wrap').find('.moreResults').hide();
                }
                $(this).addClass('counted');
            }
        });
    }
};

// var comportamientoSeleccionarHijo = {
//     init: function () {
//         this.config();
//         this.comportamiento();
//     },
//     config: function () {
//         this.arbol = body.find('.divTesArbol');
//     },
//     comportamiento: function () {
//         var categoria_padre = this.arbol.find('> .categoria-wrap');

//         $.each(categoria_padre, function (i, val) {
//             var panHijos = $(val).find('.panHijos');
//             var categorias_hijas = panHijos.find('> .categoria-wrap');

//             if (panHijos.length > 0) {
//                 // deshabilita las categorias padres
//                 $(val).find('> .categoria input').attr('disabled', true);
//             }

//             if (categorias_hijas.find('.panHijos').length) {
//                 // deshabilita las categorias hijas con hijos
//                 var categoria = categorias_hijas.find('.panHijos').closest('.categoria-wrap');
//                 categoria.find('> .categoria input').attr('disabled', true);
//             }
//         });
//     }
// };

var comportamientoAbrirArbol = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.arbol = body.find('.divTesArbol');
    },
    comportamiento: function () {
        var categoria_padre = this.arbol.find('> .categoria-wrap');
        var categoria_label = categoria_padre.find('.custom-control-label');

        categoria_label.off('click').on('click', function () {
            var padre = $(this).closest('.categoria-wrap');
            var desplegar = padre.find('> .boton-desplegar');
            if (desplegar.length > 0) {
                desplegar.trigger('click');
            }
        });
    }
};

var operativaModalSeleccionarTemas = {
    init: function () {
        $('#modal-seleccionar-area-tematica').on('show.bs.modal', function () {
            $('#modal-crear-cluster').modal('hide');
            comportamientoAbrirArbol.init();
        });

        $('#modal-seleccionar-area-tematica').on('hide.bs.modal', function () {
            $('#modal-crear-cluster').modal('show');

            // Close arbol
            $('.boton-desplegar').removeClass('mostrar-hijos');
        });
    }
};

var importarCVN = {
    init: function (){
        $('#file_cvn').GnossDragAndDrop({
            acceptedFiles: '*',
            onFileAdded: function (plugin, files) {
                $('.col-contenido .botonera').css('display', 'block');
            },
            onFileRemoved: function (plugin, files) {
                $('.col-contenido .botonera').css('display', 'none');
            }
        });
    }
};

var checkboxResources = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.checkboxResource = $('.custom-checkbox-resource');
    },
    comportamiento: function () {
        this.checkboxResource.off('click').on('click', function() {
            var icon = $(this).find('.material-icons');
            if ($(this).hasClass('add')) {
                icon.html('done');
                $(this).removeClass('add').addClass('done');
                $(this).parents('.resource').addClass('seleccionado');
            } else if ($(this).hasClass('done')) {
                icon.html('add');
                $(this).removeClass('done').addClass('add');
                $(this).parents('.resource').removeClass('seleccionado');
            }
        });
    }
};

/**
 * Este es un ejemplo de lanzamiento de gráficos de prueba
 */
var pintarGraficos = {
    init: function () {
        this.config();
        this.comportamiento();
    },
    config: function () {
        this.body = body;
        this.container = $('.graph-container');
    },
    comportamiento: function () {
        var that = this;
        var type;

        $.each(this.container, function() {
            var container = $(this);
            if (container.attr('data-type') == 'column') {
                that.initChartColumn(container);
            } else if (container.attr('data-type') == 'bar') {
                that.initChartBar(container);
            } else if (container.attr('data-type') == 'map') {
                that.initChartMap(container);
            } else if (container.attr('data-type') == 'pie') {
                that.initChartPie(container);
            }
        });
    },
    initChartColumn: function (container) {
        Highcharts.chart({
            chart: {
                type: 'column',
                renderTo: container[0],
            },
            title: {
                text: 'Expedientes por año'
            },
            xAxis: {
                categories: ['2014', '2015', '2016', '2017', '2018', '2019', '2020', '2021', '2022']
            },
            yAxis: {
                min: 0,
                title: {
                    text: ''
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: ( // theme
                            Highcharts.defaultOptions.title.style &&
                            Highcharts.defaultOptions.title.style.color
                        ) || 'gray'
                    }
                }
            },
            tooltip: {
                headerFormat: '<b>{point.x}</b><br/>',
                pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: false
                    },

                }
            },
            series: [ {
                name: 'Resto propuestas',
                data: [2, 2, 2, 1, 3, 2, 4, 7, 2],
                color: '#d3901c'
            },{
                name: 'Proyectos aprobados',
                data: [3, 7, 10, 12, 15, 15, 10, 7, 7],
                color: '#6cafe3'
            }],
            credits: {
                enabled: false,
            },
            exporting: {
                enabled: false,
            }
        });
    },
    initChartBar: function (container) {
        Highcharts.chart({
            chart: {
                type: 'bar',
                renderTo: container[0],
            },
            title: {
                text: 'Expedientes por'
            },
            xAxis: {
                categories: ['Neotec', 'ID', 'Innterconecta', 'Interempresas Int', 'Instrumentos']
            },
            yAxis: {
                min: 0,
                title: {
                    text: ''
                }
            },
            plotOptions: {
                series: {
                    stacking: 'normal'
                },
            },
            series: [{
                name: 'Proyectos aprobados',
                data: [22, 18, 7, 4, 6],
                color: '#6cafe3'
            }, {
                name: 'Resto de propuestas',
                data: [8, 2, 3, 0, 3],
                color: '#d3901c'
            }],
            credits: {
                enabled: false,
            },
            exporting: {
                enabled: false,
            }
        });
    },
    initChartPie: function (container) {
        Highcharts.chart({
            chart: {
                type: 'pie',
                renderTo: container[0],
            },
            title: {
                text: 'Expedientes por presupuesto'
            },
            accessibility: {
                announceNewData: {
                    enabled: true
                },
                point: {
                    valueSuffix: '%'
                }
            },
            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: false,
                        format: '{point.name}: {point.y:.1f}%'
                    }
                },
                pie: {
                    colorByPoint: true,
                    showInLegend: true
                }
            },
            colors: [
                '#d3901c',
                '#6cafe3',
                '#e5e5e5'
            ],
            tooltip: {
                headerFormat: '<span style="font-size:11px">{series.name}</span><br>',
                pointFormat: '<span style="color:{point.color}">{point.name}</span>: <b>{point.y:.2f}%</b> of total<br/>'
            },
            series: [
                {
                    name: "Presupuesto",
                    colorByPoint: true,
                    data: [
                        {
                            name: "< 400 K€",
                            y: 62.74,
                        },
                        {
                            name: "400 K€ - 1 M€",
                            y: 10.57,
                        },
                        {
                            name: "> 1 M€",
                            y: 7.23,
                        },
                    ]
                }
            ],
            credits: {
                enabled: false
            },
            exporting: {
                enabled: false,
            }
        });
    },
    initChartMap: function (container) {
        (async () => {
            const mapData = await fetch(
                'https://code.highcharts.com/mapdata/countries/es/es-all.topo.json'
            ).then(response => response.json());

            // New map-pie series type that also allows lat/lon as center option.
            // Also adds a sizeFormatter option to the series, to allow dynamic sizing
            // of the pies.
            Highcharts.seriesType('mappie', 'pie', {
                center: null, // Can't be array by default anymore
                states: {
                    hover: {
                        halo: {
                            size: 5
                        }
                    }
                },
                linkedMap: null, // id of linked map
                dataLabels: {
                    enabled: false
                }
            }, {
                init: function () {
                    Highcharts.Series.prototype.init.apply(this, arguments);
                    // Respond to zooming and dragging the base map
                    Highcharts.addEvent(this.chart.mapView, 'afterSetView', () => {
                        this.isDirty = true;
                    });
                },
                render: function () {
                    const series = this,
                        chart = series.chart,
                        linkedSeries = chart.get(series.options.linkedMap);
                    Highcharts.seriesTypes.pie.prototype.render.apply(
                        series,
                        arguments
                    );
                    if (series.group && linkedSeries && linkedSeries.is('map')) {
                        series.group.add(linkedSeries.group);
                    }
                },
                getCenter: function () {
                    const options = this.options,
                        chart = this.chart,
                        slicingRoom = 2 * (options.slicedOffset || 0);
                    if (!options.center) {
                        options.center = [null, null]; // Do the default here instead
                    }
                    // Handle lat/lon support
                    if (options.center.lat !== undefined) {
                        const projectedPos = chart.fromLatLonToPoint(options.center),
                            pixelPos = chart.mapView.projectedUnitsToPixels(
                                projectedPos
                            );

                        options.center = [pixelPos.x, pixelPos.y];
                    }
                    // Handle dynamic size
                    if (options.sizeFormatter) {
                        options.size = options.sizeFormatter.call(this);
                    }
                    // Call parent function
                    const result = Highcharts.seriesTypes.pie.prototype.getCenter
                        .call(this);
                    // Must correct for slicing room to get exact pixel pos
                    result[0] -= slicingRoom;
                    result[1] -= slicingRoom;
                    return result;
                },
                translate: function (p) {
                    this.options.center = this.userOptions.center;
                    this.center = this.getCenter();
                    return Highcharts.seriesTypes.pie.prototype.translate.call(this, p);
                }
            });

            const data = [
                // name, aprobados, resto, suma, value, offset (optional)
                ['La Rioja', 89283, 1318255, 2101660, 9391, 13705895],
                ['Madrid', 729547, 1318255, 2101660, 9391, 13705895],
                ['Barcelona', 729547, 89283, 2101660, 9391, 13705895],
                ['Málaga', 729547, 1318255, 2101660, 9391, 13705895],
                ['Pontevedra', 729547, 1318255, 2101660, 9391, 13705895],
                ['Asturias', 729547, 1318255, 2101660, 9391, 13705895],
                ['Las Palmas', 729547, 1318255, 2101660, 9391, 13705895],
            ],
            aproColor = 'rgba(108, 175, 227, 1)',
            resColor = 'rgba(211, 144, 28, 1)';

            // Compute max votes to find relative sizes of bubbles
            const maxVotes = data.reduce((max, row) => Math.max(max, row[5]), 0);

            // Build the chart
            const chart = Highcharts.mapChart({
                title: {
                    text: 'USA 2016 Presidential Election Results'
                },
                chart: {
                    animation: false, // Disable animation, especially for zooming
                    renderTo: container[0],
                },
                colorAxis: {
                    dataClasses: [{
                        from: -1,
                        to: 0,
                        color: resColor,
                        name: 'Resto de propuestas'
                    }, {
                        from: 0,
                        to: 1,
                        color: aproColor,
                        name: 'Proyectos aprobados'
                    }]
                },
                mapNavigation: {
                    enabled: true
                },
                // Limit zoom range
                yAxis: {
                    minRange: 2300
                },
                tooltip: {
                    useHTML: true
                },
                // Default options for the pies
                plotOptions: {
                    mappie: {
                        borderColor: 'rgba(255,255,255,0.4)',
                        borderWidth: 1,
                        tooltip: {
                            headerFormat: ''
                        }
                    }
                },
                series: [{
                    mapData,
                    data: data,
                    name: 'Comunidades',
                    borderColor: '#FFF',
                    showInLegend: false,
                    joinBy: ['name', 'id'],
                    keys: ['id', 'aprobados', 'resto',
                        'suma', 'value', 'pieOffset'],
                    tooltip: {
                        headerFormat: '',
                        pointFormatter: function () {
                            const hoverVotes = this.hoverVotes; // Used by pie only
                            return '<b>' + this.id + ' </b><br/>' +
                                [
                                    ['Proyectos aprobados', this.aprobados, aproColor],
                                    ['Resto de propuestas', this.resto, resColor],
                                ]
                                    .sort(function (a, b) {
                                        // Sort tooltip by most votes
                                        return b[1] - a[1];
                                    })
                                    .map(function (line) {
                                        return '<span style="color:' + line[2] +
                                            // Colorized bullet
                                            '">\u25CF</span> ' +
                                            // Party and votes
                                            (line[0] === hoverVotes ? '<b>' : '') +
                                            line[0] + ': ' +
                                            Highcharts.numberFormat(line[1], 0) +
                                            (line[0] === hoverVotes ? '</b>' : '') +
                                            '<br/>';
                                    })
                                    .join('') +
                                '<hr/>Total: ' + Highcharts.numberFormat(this.suma, 0);
                        }
                    }
                }, {
                    name: 'Connectors',
                    type: 'mapline',
                    color: 'rgba(130, 130, 130, 0.5)',
                    zIndex: 5,
                    showInLegend: false,
                    enableMouseTracking: false
                }],
                credits: {
                    enabled: false,
                },
                exporting: {
                    enabled: false,
                }
            });

            // Add the pies after chart load, optionally with offset and connectors
            chart.series[0].points.forEach(function (state) {
                if (!state.id) {
                    return; // Skip points with no data, if any
                }

                const pieOffset = state.pieOffset || {},
                    centerLat = parseFloat(state.properties.latitude),
                    centerLon = parseFloat(state.properties.longitude);

                // Add the pie for this state
                chart.addSeries({
                    type: 'mappie',
                    name: state.id,
                    linkedMap: 'es-all',
                    zIndex: 6, // Keep pies above connector lines
                    sizeFormatter: function () {
                        const zoomFactor = chart.mapView.zoom / chart.mapView.minZoom;

                        return Math.max(
                            this.chart.chartWidth / 45 * zoomFactor, // Min size
                            this.chart.chartWidth /
                                11 * zoomFactor * state.suma / maxVotes
                        );
                    },
                    tooltip: {
                        // Use the state tooltip for the pies as well
                        pointFormatter: function () {
                            return state.series.tooltipOptions.pointFormatter.call({
                                id: state.id,
                                hoverVotes: this.name,
                                aprobados: state.aprobados,
                                resto: state.resto,
                                suma: state.suma
                            });
                        }
                    },
                    data: [{
                        name: 'Proyectos aprobados',
                        y: state.aprobados,
                        color: aproColor
                    }, {
                        name: 'Resto de propuestas',
                        y: state.resto,
                        color: resColor
                    },],
                    center: {
                        lat: centerLat + (pieOffset.lat || 0),
                        lon: centerLon + (pieOffset.lon || 0)
                    }
                }, false);

                // Draw connector to state center if the pie has been offset
                if (pieOffset.drawConnector !== false) {
                    const centerPoint = chart.fromLatLonToPoint({
                            lat: centerLat,
                            lon: centerLon
                        }),
                        offsetPoint = chart.fromLatLonToPoint({
                            lat: centerLat + (pieOffset.lat || 0),
                            lon: centerLon + (pieOffset.lon || 0)
                        });
                    chart.series[1].addPoint({
                        name: state.id,
                        path: [
                            ['M', offsetPoint.x, offsetPoint.y],
                            ['L', centerPoint.x, centerPoint.y]
                        ]
                    }, false);
                }
            });

            // Only redraw once all pies and connectors have been added
            chart.redraw();
        })();
    }
};
/**/

function comportamientoCargaFacetasComunidad() {
    alturizarBodyTamanoFacetas.init();
    plegarFacetasCabecera.init();
    plegarSubFacetas.init();
    comportamientoVerMasVerMenos.init();
    comportamientoVerMasVerMenosTags.init();
    comportamientosModalFacetas.init();
};

$(function () {
    accionesBuscadorCabecera.init();
    communityMenuMovil.init();
    iniciarDatepicker.init();
    collapseResource.init();
    tooltipsAccionesRecursos.init();
    contarLineasDescripcion.init();
    cambioTraducciones.init();

    accionesPlegarDesplegarModal.init();

    operativaModalSeleccionarTemas.init();

    checkboxResources.init();

    $('.js-select2.disabled').prop('disabled', true);

    if (body.hasClass('fichaRecurso') || body.hasClass('edicionRecurso')) {
        comportamientoCargaFacetasComunidad();
    }

    if (body.hasClass('fichaRecurso')) {
        mostrarFichaCabeceraFixed.init();
        clonarNombreFicha.init();
    }

    if (body.hasClass('fichaRecurso-investigador')) {
        filtrarMovil.init();
        buscadorSeccion.init();
        cambioVistaListado.init();
    }

    if (body.hasClass('page-cv')) {
        iniciarComportamientoImagenUsuario.init();
        operativaFormularioAutor.init();
        operativaFormularioTesauro.init();
        operativaFormularioProduccionCientifica.init();
        comportamientoTopicosCV.init();
        tooltipsCV.init();
        edicionListaAutorCV.init();
    }

    if (body.hasClass('edicionCluster')) {
        comportamientoAbrirArbol.init();
    }

    if (body.hasClass('importar-cvn')) {
        importarCVN.init();
    }

    if (body.hasClass('vista-grafico')) {
        pintarGraficos.init();
    }
});

; (function ($) {

    /**
    Para hacer que la imagen se guarde directamente por ajax hay que configurar las siguientes opciones
    (Por defecto es "false" por lo que el File se guardará con el formulario al que pertenezca):

    options: {
        ajax: {
            url: (string) url a la que se quiere hacer la petición,
            param_name: (string) nombre del parámetro con el que se va a pasar el objeto File
        }
    }

    Se puede configurar también cual serán los selectores para cada elemento del droparea
    options: {
        inputSelector: ".image-uploader__input",
        dropAreaSelector: ".image-uploader__drop-area",
        preview: ".image-uploader__preview",
        previewImg: ".image-uploader__img",
        errorDisplay: ".image-uploader__error",
    }

    Configurar límite de tamaño en Kb (por defecto sin límite)
    options: {
        sizeLimit: 100
    }

    El html por defecto debería ser así:
        <div class="image-uploader js-image-uploader">
            <div class="image-uploader__preview">
                <!-- Si hay una imagen en el servidor pintarla en el src, si no dejarlo vacío  -->
                <img class="image-uploader__img" src="">
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
    */

    $.imageDropArea2 = function (element, options) {
        var defaults = {
            sizeLimit: false,
            ajax: false,
            inputSelector: ".image-uploader__input",
            dropAreaSelector: ".image-uploader__drop-area",
            preview: ".image-uploader__preview",
            previewImg: ".image-uploader__img",
            errorDisplay: ".image-uploader__error",
            // funcionPrueba: function() {}
        };
        var plugin = this;

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
            addInputChangeEvent();
            addDragAndDropEvents();
            initialImageCheck();
        };

        /**
         * Comprueba si en el inicio del plugin ya hay una imagen
         * para añadirla al input file
         */
        var initialImageCheck = async function () {
            const image_url = plugin.previewImg.attr("src");
            if (image_url && image_url !== "") {
                const dT = new ClipboardEvent('').clipboardData || new DataTransfer();
                const response = await fetch(image_url);
                const data = await response.blob();
                const metadata = {
                    type: 'image/jpeg'
                };
                const file = new File([data], image_url, metadata);
                dT.items.add(file);
                plugin.input.get(0).files = dT.files;
                plugin.input.trigger('change');
            };
        };

        var addInputChangeEvent = function () {
            plugin.input.change(function () {
                if (!isFileImage()) {
                    displayError('El archivo no es una imágen válida. Los formatos válidos son .png y .jpg.');
                    return;
                }

                if (!imageSizeAllowed()) {
                    displayError('El archivo pesa demasiado. El límite es ' + plugin.settings.sizeLimit + 'Kb');
                    return;
                }

                if (plugin.settings.ajax) {
                    uploadImageWithAjax();
                } else {
                    showImageTemporalPreview();
                }
            });
        };

        /**
         * Muestra la imagen que se ha añadido al input file
         */
        var showImageTemporalPreview = function () {

            const [file] = plugin.input.get(0).files
            if (file) {
                showImagePreview(URL.createObjectURL(file))
            }
        };

        /**
         * Incia lógica para llamada ajax
         */
        var uploadImageWithAjax = function () {

            if (checkAjaxSettings()) {
                return;
            };

            var data = new FormData();
            var files = plugin.input.get(0).files;
            if (files.length > 0) {
                data.append("ImagenRegistroUsuario", files[0]);
            }
            upload(data);
        };

        /**
         * Llamada ajax
         */
        var upload = function () {
            $.ajax({
                url: document.location.href,
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (response) {
                    onSuccesResponse(response);
                },
                error: function (er) {
                    displayError(er.statusText);
                },
            });
        }

        /**
         * @return {boolean}
         * true: Las opciones de ajax se han configurado
         * false: Las opciones de ajax se no han configurado
         */
        var checkAjaxSettings = function () {
            if (plugin.settings.ajax.hasProperty('param_name')) {
                console.log('La opción "ajax.param_name" no está configurada')
                return false;
            }
            if (plugin.settings.ajax.hasProperty('url')) {
                console.log('La opción de "ajax.url" no está configurada')
                return false;
            }
            return true;
        };

        /**
         * @return {string} error
         * Shows error message
         */
        var displayError = function (error) {
            plugin.errorDisplay.find(".ko").text(error);
            plugin.errorDisplay.find(".ko").show();
            plugin.preview.removeClass("show-preview");
        };

        /**
         * Hides error message
         */
        var hideError = function () {
            plugin.errorDisplay.find(".ko").hide();
        };

        /**
         * @param {string} response
         */
        var onSuccesResponse = function (response) {
            if (response.indexOf("imagenes/") === 0) {
                // Es una url de imagen
                showImagePreview(response);
                showLoadingImagePreview(false);
            } else {
                // No es una url de imagen
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
            } else {
                // Quitar loading
                $(".spinner-border").remove();
            }
        };

        /**
         * @param {string} : url de la imagen
         */
        var showImagePreview = function (url) {
            let image_url = url;
            if (plugin.settings.ajax) {
                image_url = "http://serviciospruebas.gnoss.net" + "/" + url;
            }
            hideError();
            plugin.previewImg.attr("src", image_url);
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

        /**
         * @return {boolean} return if file is a valid image
         */
        var isFileImage = function () {
            const acceptedImageTypes = ['image/jpeg', 'image/png'];
            const file = plugin.input.get(0).files[0];
            return file && acceptedImageTypes.includes(file['type'])
        }

        /**
         * @return {boolean} return if file is valid size
         */
        var imageSizeAllowed = function () {
            if (plugin.settings.sizeLimit) {
                const file = plugin.input.get(0).files[0];
                return (file.size / 1024) < plugin.settings.sizeLimit;
            } else {
                return true;
            }
        }

        plugin.init();
    };

    // add the plugin to the jQuery.fn object
    $.fn.imageDropArea2 = function (options) {
        return this.each(function () {
            if (undefined == $(this).data("imageDropArea2")) {
                var plugin = new $.imageDropArea2(this, options);
                $(this).data("imageDropArea2", plugin);
            }
        });
    };
})(jQuery);