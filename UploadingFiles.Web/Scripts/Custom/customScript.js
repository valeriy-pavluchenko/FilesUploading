$(document).ready(function () {

    var MAX_FILE_SIZE = 0.5 * 1024 * 1024;
    var $messageTooBig = '<div><h4>Слишком большой файл</h4></div>';

/*
    var files;

    // Вешаем функцию на событие
    // Получим данные файлов и добавим их в переменную

    $('#newFile').change(function () {
        files = this.files;
        console.log('files.name ' + files[0].name);
        console.log('files.size ' + files[0].size);
        console.log('files.type ' + files[0].type);

    });
*/
    $('body').on('click', '#saveFile', function () {
        var $form = $('form[name="traineeFile"]');
        var currentForm = {};
        currentForm['fileName'] = $form.find('#fileName').val();
        currentForm['newFile'] = $form.find('#newFile').val();
        console.log('currentForm Out= ' + JSON.stringify(currentForm));

      var newFile = $('#newFile')[0].files;
        
        console.log('newFile.name ' + newFile[0].name);
        console.log('newFile.size ' + newFile[0].size);
        console.log('newFile.type ' + newFile[0].type);


        if (newFile[0].size > MAX_FILE_SIZE) {
            //$('#loadFileModal').appendTo($messageTooBig);
            alert($messageTooBig);
        }

        var formData = new FormData($('form[name="traineeFile"]'));

        $.ajax({
            url: '/AjaxQueryController/AddFile',
            type: 'POST',
            data: formData,
            cache: false,
            dataType: 'json',
            processData: false, // Не обрабатываем файлы (Don't process the files)
            contentType: false, // Так jQuery скажет серверу что это строковой запрос
            //dataType: 'json',
            success: function (data) {
                alert('Перегрузи страницу');
            },
            error: function () {
                alert('Извините на сервере произошла ошибка\n имя файла ' + newFile[0].name + '\n размер файла ' + newFile[0].size + 'байт');
                // test
                
                //////////
            },
            beforeSend: function () {
                //$formButtons.attr("disabled", "disabled");
                //$submitMessage.text('Сохраняется').addClass('prf-blink');
            },
            complete: function () {
                //$formButtons.removeAttr("disabled");
                //$submitMessage.text('Сохранить').removeClass('prf-blink');
                $('#loadFileModal').modal('hide');
                $('#fileName').val('');
                $('#newFile').val('');
            }
        });
    });


    $('body').on('click', '#saveLink', function () {
        var $form = $('form[name="traineeLink"]');
        var currentForm = {};
        currentForm['linkName'] = $form.find('#linkName').val();
        currentForm['linkUrl'] = $form.find('#linkURL').val();
        console.log('currentForm Out= ' + JSON.stringify(currentForm));

        $.ajax({
            url: 'action',
            method: 'POST',
            async: true,
            contentType: 'application/json',
            data: JSON.stringify(currentForm),
            dataType: 'json',
            success: function (data) {

            },
            error: function () {
                alert('Извините на сервере произошла ошибка\n' + JSON.stringify(currentForm));
                // test

                //////////
            },
            beforeSend: function () {
                //$formButtons.attr("disabled", "disabled");
                //$submitMessage.text('Сохраняется').addClass('prf-blink');
            },
            complete: function () {
                //$formButtons.removeAttr("disabled");
                //$submitMessage.text('Сохранить').removeClass('prf-blink');
                $('#loadLinkModal').modal('hide');
                $('#linkName').val('');
                $('#linkURL').val('');                
            }
        });
    });



});