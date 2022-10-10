/**
 * Clase Text field
 */
class TextField {

  /**
   * Constructor de la clase TextField
   * @param editor, elemento sobre el que se añade la funcionalidad
   */
  constructor(editor) {
    let _self = this;
    this.editor = editor
    // const button = editor.querySelector('.editor-btn');
    this.toolbar = editor.getElementsByClassName('toolbar')[0];
    this.visuellView = editor.getElementsByClassName('visuell-view')[0];

    // Comprueba si existe un textArea
    if (editor.getElementsByTagName('textarea').length > 0) {

      this.textarea = editor.getElementsByTagName('textarea')[0];
      // Oculta el textarea para sustutir la edición por el div
      this.textarea.classList.add('d-none');
      // Oculta el textarea para sustutir la edición por el div
      this.toolbar.classList.remove('d-none');
      // Oculta el textarea para sustutir la edición por el div
      this.visuellView.classList.remove('d-none');
      this.visuellView.focus();

    }
	
  	editor.addEventListener("dragover", event => {
  	  // prevent default to allow drop
  	  event.preventDefault();
  	});
    $(editor).off("focusout").on("focusout",(e)=>{
      if($(editor).find(".visuell-view").html()=="<br>"){
        $(editor).find(".visuell-view").html("")
      }
     
    })

    if (editor.getElementsByTagName('content-area').length > 0) {
      const contentArea = editor.getElementsByClassName('content-area')[0];
      // add paragraph tag on new line
      contentArea.addEventListener('keypress', this.keyEvent);
    }

    // Add class inicilized to the element
    editor.classList.add("inicilized")

    this.buttons = this.toolbar.querySelectorAll('.editor-btn');
    //Evita que el tab salte a otro elemento
    this.visuellView.removeEventListener('keydown', this.keyEvent);
    this.visuellView.addEventListener('keydown', this.keyEvent);


    // add active tag event
    this.visuellView.addEventListener('selectionchange', this.selectionChange);

    // add paste event
    this.visuellView.removeEventListener('paste', this.pasteEvent);
    this.visuellView.addEventListener('paste', this.pasteEvent);


    // add toolbar button actions
    for(let i = 0; i < this.buttons.length; i++) {
      let button = this.buttons[i];
      
      // button.removeEventListener('click', this.eventListenerFn(button), false);
      // button.addEventListener('click', this.eventListenerFn(button), true);

      $(button).off('mousedown').on('mousedown', function(e) {
        e.preventDefault();
        let action = this.dataset.action;
        _self.execDefaultAction(action);

      });
    }

  }

  /**
   * Método que destruye el textArea
   */
  removeTextAreaOfertas() {
    // Comprueba si existe un textArea
    if (this.editor.getElementsByTagName('textarea').length > 0) {

      // Muestra el textarea
      this.textarea.classList.remove('d-none');
      // Oculta el toolbar
      this.toolbar.classList.add('d-none');
      // Oculta el editor
      this.visuellView.classList.add('d-none');
      // Delete the inicilized class to the element
      this.editor.classList.remove("inicilized")

    }
  }



  /**
   * Método para añadir los eventos de los botones del editor
   * (Actualmente no se usa)
   * @param button, botón sobre el que se ejecuta la acción
   */
  eventListenerFn(button) {
    let action = button.dataset.action;
    this.execDefaultAction(action);
  }

  /**
   * Método selectionChange
   * @param e, evento
   */
  selectionChange(e) {
  
    for(let i = 0; i < this.buttons.length; i++) {
      let button = this.buttons[i];
      
      // don't remove active class on code toggle button
      if(button.dataset.action === 'toggle-view') continue;
      
      button.classList.remove('active');
    }
 
    if(!childOf(window.getSelection().anchorNode.parentNode, editor)) return false;
    
    this.parentTagActive(window.getSelection().anchorNode.parentNode);
  }

  /**
   * Método parentTagActive
   * @param e, evento
   */
  parentTagActive(elem) {
    if(!elem ||!elem.classList || elem.classList.contains('visuell-view')) return false;
    
    let toolbarButton;
    
    // active by tag names
    let tagName = elem.tagName.toLowerCase();
    toolbarButton = this.editor.querySelectorAll(`.toolbar .editor-btn[data-tag-name="${tagName}"]`)[0];
    if(toolbarButton) {
      toolbarButton.classList.add('active');
    }
    
    // active by text-align
    let textAlign = elem.style.textAlign;
    toolbarButton = this.editor.querySelectorAll(`.toolbar .editor-btn[data-style="textAlign:${textAlign}"]`)[0];
    if(toolbarButton) {
      toolbarButton.classList.add('active');
    }
    
    return this.parentTagActive(elem.parentNode);
  }

  // boldButton(e) {
  //   document.execCommand("bold", false);
  //   let button = e.currentTarget;
  //   if (button.classList.contains('active')) {
  //     button.classList.remove('active');
  //   } else {
  //     button.classList.add('active');
  //   }
  // }

  /**
   * Este método ejecuta las acciones 'normales' de los botones
   * @param action, acción a ejecutar por el navegador
   */
  execDefaultAction(action) {
    document.execCommand(action, false);
  }


  // focusout(e) {
  //   let button = e.target.parentNode.querySelector('.editor-btn');
  //   let texto = e.target.innerHTML;
  //   if (texto.substr(texto.length - 4) == '<br>') {
  //     e.target.lastElementChild.remove();
  //   }
  //   button.classList.remove('active');
  // }

  /**
   * Método para el copiado de texto
   * @param e, evento
   */
  pasteEvent(e) {
    e.preventDefault();
    let text = (e.originalEvent || e).clipboardData.getData('text/plain');
    text = text.replaceAll("\n", "<br>");
    document.execCommand('insertHTML', false, text);
  }

  /**
   * Método keyEvent que deshabilita ciertas teclas
   * @param e, evento
   */
  keyEvent(e) {
    if (e.keyCode == 9) {
      e.preventDefault();
      document.execCommand('insertText', false, '    ');
    } 
    //if enter, disable bold 
    else if (e.keyCode == 13) {
      let button = e.currentTarget.parentNode.querySelector('.editor-btn');
      button.classList.remove('active');
    }
  }
}
