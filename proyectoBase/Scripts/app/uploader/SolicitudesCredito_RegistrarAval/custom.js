$(document).ready(function () {

    // inicializacion
    $('#fileCedula').fileuploader({
        inputNameBrackets: false,
        changeInput: '<div class="fileuploader-input">' +
            '<button type="button" class="fileuploader-input-button btn-sm"><span>${captions.button}</span></button>' +
            '</div>',
        theme: 'dragdrop',
        limit: 5, //limite de archivos a subir
        maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, //peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos

        upload: {
            url: 'Aval_Registrar.aspx?type=upload&doc=7',
            data: null,
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: true,
            beforeSend: null,
            onSuccess: function (result, item) {
                var data = {};
                try {
                    data = JSON.parse(result);
                } catch (e) {
                    data.hasWarnings = true;
                }

                // validar exito
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                // validar si se produjo un error
                if (data.hasWarnings) {
                    for (var warning in data.warnings) {
                        alert(data.warnings);
                    }

                    item.html.removeClass('upload-successful').addClass('upload-failed');
                    return this.onError ? this.onError(item) : null;
                }

                item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);
            },
            onError: function (item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                ) : null;
            },
            onProgress: function (data, item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: null,
        },
        onRemove: function (item) {
            $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {
            /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

            //personalizar alertas
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            //personalizar confimaciones
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
    });

    $('#fileCompDomicilio').fileuploader({
        inputNameBrackets: false,
        changeInput: '<div class="fileuploader-input">' +
            '<button type="button" class="fileuploader-input-button btn-sm"><span>${captions.button}</span></button>' +
            '</div>',
        theme: 'dragdrop',
        limit: 5, //limite de archivos a subir
        maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, //peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos

        upload: {
            url: 'Aval_Registrar.aspx?type=upload&doc=8',
            data: null,
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: true,
            beforeSend: null,
            onSuccess: function (result, item) {
                var data = {};
                try {
                    data = JSON.parse(result);
                } catch (e) {
                    data.hasWarnings = true;
                }

                // validar exito
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                // validar si se produjo un error
                if (data.hasWarnings) {
                    for (var warning in data.warnings) {
                        alert(data.warnings);
                    }

                    item.html.removeClass('upload-successful').addClass('upload-failed');
                    return this.onError ? this.onError(item) : null;
                }

                item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);
            },
            onError: function (item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                ) : null;
            },
            onProgress: function (data, item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: null,
        },
        onRemove: function (item) {
            $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {
            /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

            //personalizar alertas
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            //personalizar confimaciones
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
    });

    $('#compIngresos').fileuploader({
        inputNameBrackets: false,
        changeInput: '<div class="fileuploader-input">' +
            '<button type="button" class="fileuploader-input-button btn-sm"><span>${captions.button}</span></button>' +
            '</div>',
        theme: 'dragdrop',
        limit: 5, //limite de archivos a subir
        maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, //peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos

        upload: {
            url: 'Aval_Registrar.aspx?type=upload&doc=9',
            data: null,
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: true,
            beforeSend: null,
            onSuccess: function (result, item) {
                var data = {};
                try {
                    data = JSON.parse(result);
                } catch (e) {
                    data.hasWarnings = true;
                }

                // validar exito
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                // validar si se produjo un error
                if (data.hasWarnings) {
                    for (var warning in data.warnings) {
                        alert(data.warnings);
                    }

                    item.html.removeClass('upload-successful').addClass('upload-failed');
                    return this.onError ? this.onError(item) : null;
                }

                item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);
            },
            onError: function (item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                ) : null;
            },
            onProgress: function (data, item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: null,
        },
        onRemove: function (item) {
            $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {
            /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

            //personalizar alertas
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            //personalizar confimaciones
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
    });

    $('#croquisDomicilio').fileuploader({
        inputNameBrackets: false,
        changeInput: '<div class="fileuploader-input">' +
            '<button type="button" class="fileuploader-input-button btn-sm"><span>${captions.button}</span></button>' +
            '</div>',
        theme: 'dragdrop',
        limit: 5, //limite de archivos a subir
        maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, //peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos

        upload: {
            url: 'Aval_Registrar.aspx?type=upload&doc=10',
            data: null,
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: true,
            beforeSend: null,
            onSuccess: function (result, item) {
                var data = {};
                try {
                    data = JSON.parse(result);
                } catch (e) {
                    data.hasWarnings = true;
                }

                // validar exito
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                // validar si se produjo un error
                if (data.hasWarnings) {
                    for (var warning in data.warnings) {
                        alert(data.warnings);
                    }

                    item.html.removeClass('upload-successful').addClass('upload-failed');
                    return this.onError ? this.onError(item) : null;
                }

                item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);
            },
            onError: function (item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                ) : null;
            },
            onProgress: function (data, item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: null,
        },
        onRemove: function (item) {
            $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {
            /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

            //personalizar alertas
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            //personalizar confimaciones
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
    });

    $('#croquisEmpleo').fileuploader({
        inputNameBrackets: false,
        changeInput: '<div class="fileuploader-input">' +
            '<button type="button" class="fileuploader-input-button btn-sm"><span>${captions.button}</span></button>' +
            '</div>',
        theme: 'dragdrop',
        limit: 5, //limite de archivos a subir
        maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, //peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos

        upload: {
            url: 'Aval_Registrar.aspx?type=upload&doc=11',
            data: null,
            type: 'POST',
            enctype: 'multipart/form-data',
            start: true,
            synchron: true,
            beforeSend: null,
            onSuccess: function (result, item) {
                var data = {};
                try {
                    data = JSON.parse(result);
                } catch (e) {
                    data.hasWarnings = true;
                }

                // validar exito
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                // validar si se produjo un error
                if (data.hasWarnings) {
                    for (var warning in data.warnings) {
                        alert(data.warnings);
                    }

                    item.html.removeClass('upload-successful').addClass('upload-failed');
                    return this.onError ? this.onError(item) : null;
                }

                item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                setTimeout(function () {
                    item.html.find('.progress-bar2').fadeOut(400);
                }, 400);
            },
            onError: function (item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length) {
                    progressBar.find('span').html(0 + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                    item.html.find('.progress-bar2').fadeOut(400);
                }

                item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                    '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                ) : null;
            },
            onProgress: function (data, item) {
                var progressBar = item.html.find('.progress-bar2');

                if (progressBar.length > 0) {
                    progressBar.show();
                    progressBar.find('span').html(data.percentage + "%");
                    progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                }
            },
            onComplete: null,
        },
        onRemove: function (item) {
            $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {
            /*Mensajes que se muestran al usuario, ya sean alertas o confirmaciones, se pueden adecuar a cualquier requerimiento*/

            //personalizar alertas
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            //personalizar confimaciones
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
    });
        
    $("#form").submit(function (e) {
        e.preventDefault();
    });
});

