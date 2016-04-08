$(document).ready(function () {

    var MAX_FILE_SIZE = 0.5 * 1024 * 1024;

    $('#attention').hide();

    // Create DOM Elements
    var tableCell=$('<td></td>');
    var tableRow=$('<tr></tr>');
    var anchor=$('<a href=""></a>');
    var paragr=$('<p></p>');
    var image = $('<img src="" alt="">');

    // Create RegExp for file types with picture
    var extensionArray = ['ai', 'avi', 'css', 'dll', 'doc', 'gif', 'html', 'jpg', 'js', 'mp3', 'pdf', 'png', 'ppt', 'psd', 'txt', 'xls', 'zip'];
    var stringReg = '';
    for (var i = 0; i < extensionArray.length; i++) {
        stringReg += extensionArray[i] + '|';
    }
    stringReg = stringReg.substring(0, (stringReg.length - 1));
    var RV_TYPE = new RegExp(stringReg, 'i');

    // Set handler for File Input Change
    $('#newFile').change(function () {
        var files = this.files;
        if (files[0].size > MAX_FILE_SIZE) {
            $('#attention').show();
        } else {
            $('#attention').hide();
        }
    });

    // Set handler for Files Form submit
    $('form[name="traineeFile"]').on('submit', function(e) {
        e.preventDefault;
        var $form = $('form[name="traineeFile"]');
        var currentForm = {};
        currentForm['fileName'] = $form.find('#fileName').val();
        currentForm['newFile'] = $form.find('#newFile').val();
        console.log('currentForm Out= ' + JSON.stringify(currentForm));

        var newFile = $('#newFile')[0].files;
        /*
        console.log('newFile.name ' + newFile[0].name);
        console.log('newFile.size ' + newFile[0].size);
        console.log('newFile.type ' + newFile[0].type);
        */
       
        if (newFile[0].size > MAX_FILE_SIZE) {
           return false;
        };
        
        // --------------- UPLOADING FILE AJAX ---------------

        var fileData = $('#newFile').prop('files')[0];
        var fileDescription = $('#fileDescription').val();
        
        var form = $('form[name="traineeFile"]'); // getting FORM from page
        var formData = new FormData(form); // creating instanse of FormData
        
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
                
                /******** Create new DOM elements *****/

                //Create image
                var newImg = image.clone();
                //Work with Image extension
                var nameFile = data.name;
                var nameArr = nameFile.split('.');
                var extension = nameArr[nameArr.length - 1].substring(0, 3);

                if (extension == 'jep') { extension = 'jpg' };
                if (extension == '7z') { extension = 'zip' };

                if (RV_TYPE.test(extension) == false) {
                    extension = 'other';
                }
                var downloadImg = '/images/file-' + extension + '.png';
                newImg.attr('src', downloadImg);

                //Create Cell 1
                var newCell1 = tableCell.clone();
                var newAnchor = anchor.clone();
                var download = '/Home/Download/' + data.id;
                newAnchor.attr('href', download).text(data.name);
                newCell1.append(newAnchor.prepend(newImg));

                //Create Cell 2
                var newCell2 = tableCell.clone();
                var newParagr = paragr.clone();
                newParagr.text(data.description);
                newCell2.append(newParagr);

                //Create Cell 3
                var newCell3 = tableCell.clone();
                var newAnchor = anchor.clone();
                var download = '/Home/RemoveFile/' + data.id;
                newAnchor.attr('href', download).text('Delete');
                newCell3.append(newAnchor);

                var newRow = tableRow.clone();
                newRow.append(newCell1).append(newCell2).append(newCell3);

                /*****************************/

                // Insert new Row in Table
                $('#listTable').find('tr:first').after(newRow);

            },
            error: function () {
                alert("error");
            },
            beforeSend: function () {
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

    $('#listTable').find('a[href^="/Home/Download"]').each(function () {
        //Create image
        var newImg = image.clone();
        //Work with Image extension
        var nameFile = $(this).text();
        var nameArr = nameFile.split('.');
        var extension = nameArr[nameArr.length - 1].substring(0, 3);

        if (extension == 'jep') { extension = 'jpg' };
        if (extension == '7z') { extension = 'zip' };

        if (RV_TYPE.test(extension) == false) {
            extension = 'other';
        }
        var downloadImg = '/images/file-' + extension + '.png';
        newImg.attr('src', downloadImg);
        $(this).prepend(newImg);
    });
    

});