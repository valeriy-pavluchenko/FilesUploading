$(document).ready(function () {

    var MAX_FILE_SIZE = 0.5 * 1024 * 1024;

    $('#attention').hide();

    var tableCell=$('<td></td>');
    var tableRow=$('<tr></tr>');
    var anchor=$('<a href=""></a>');
    var paragr=$('<p></p>');

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

    $('#newFile').change(function () {
        var files = this.files;
        console.log('files.name ' + files[0].name);
        console.log('files.size ' + files[0].size);
        console.log('files.type ' + files[0].type);
        if (files[0].size > MAX_FILE_SIZE) {
            $('#attention').show();
        } else {
            $('#attention').hide();
        }
    });


    $('form[name="traineeFile"]').on('submit', function(e) {
        e.preventDefault;
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
           return false;
        };
        
        // --------------- UPLOADING FILE AJAX ---------------

        var fileData = $('#newFile').prop('files')[0];
        var fileDescription = $('#fileDescription').val();
        
        var form = $('form[name="traineeFile"]'); // getting FORM from page
        var formData = new FormData(form); // creating instanse of FormData
        
        //var formData = new FormData(); // creating instanse of FormData

        // filling FormData with data for sending to server
        formData.append("file", fileData); // adding file to form data
        formData.append("description", fileDescription); // adding description of form data

        console.log('formData= ' + JSON.stringify(formData));

        $.ajax({
            url: "/AjaxQuery/AddFile",
            type: 'POST',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                //alert("works" + JSON.stringify(data));
                var newRow = tableRow.clone();

                var newCell1 = tableCell.clone();
                var newAnchor = anchor.clone();
                var download = '/Home/Download/' + data.id;
                newAnchor.attr('href', download).text(data.name);
                newCell1.append(newAnchor);

                var newCell2 = tableCell.clone();
                var newParagr = paragr.clone();
                newParagr.text(data.description);
                newCell2.append(newParagr);

                var newCell3 = tableCell.clone();
                var newAnchor = anchor.clone();
                var download = '/Home/RemoveFile/' + data.id;
                newAnchor.attr('href', download).text('Delete');
                newCell3.append(newAnchor);

                newRow.append(newCell1).append(newCell2).append(newCell3);

                $('#listTable').append(newRow);
            },
            error: function () {
                alert("error");
            },
            before: function () {
                console.log('Ajax!!!');
            },
            complete: function () {
                $('#loadFileModal').modal('hide');
                $('#fileDescription').val('');
                $('#newFile').val('');
                $('#newFileName').val('');
            }
        });

        return false;
    });



    $('form[name="traineeLink"]').on('submit', function (e) {
        e.preventDefault;
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

        return false;
    });
    

});