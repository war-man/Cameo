function initVideoRequestsDatatable()
{
    $("#videoRequests").DataTable({
        searching: false,
        ordering: false,
        processing: true,
        serverSide: true,
        scrollX: true,
        //lengthChange: false,
        //language: {
        //    infoFiltered: " ",
        //    info: " ",
        //    processing: Resource.Loading + "...",
        //    zeroRecords: Resource.ZeroRecords,
        //    infoEmpty: "empty",
        //    paginate: {
        //        previous: " ",
        //        next: " ",
        //    }
        //},
        ajax:
        {
            url: "/VideoRequestSearch/",
            type: "POST",
            //contentType: "application/json",
            //datatype: "json",
            //data: function (d) {
            //    d.modelVM = searchVM;
            //    return JSON.stringify(d);
            //},
            //complete: function () {
            //    StartLoadingImagesAsync();
            //}
        },
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
            },
            {
                "targets": [3],
                "visible": false
            }
        ],
        columns: [
            { "data": "id", "name": "id", "autoWidth": true },
            {
                "data": null, "name": "toFrom", "title": "Для кого", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    var html = "";
                    html += "<p>Для " + full.to;
                    if (full.from != null)
                        html += " от " + full.from;
                    html += ' <i class="glyphicon glyphicon-info-sign" data-toggle="tooltip" data-placement="right" title="' + full.instructions + '"></i>';
                    html += "</p>";

                    return html;
                }
            },
            {
                "data": "person", /*"name": "person",*/ "title": "Пользователь", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    var html = '<img src="' + full.person.avatar.url + '" alt="Avatar" class="img-circle img-thumbnail" height="50" width="50" />'
                    if (userType == talentType)
                        html += "<br /><a href='/Customer/Details/" + full.person.id + "'>";
                    else
                        html += "<br /><a href='/Talent/Details/" + full.person.id + "'>";
                    html += full.person.fullName;
                    html += "</a>";
                    return html;
                }
            },
            {
                //"data": "statusId", "name": "statusId", "autoWidth": true,
                "render": function (data, type, full, meta) { return '' + full.status.id + ''; }
            },
            {
                "data": null, "name": "statusName", "title": "Статус", "autoWidth": true,
                "render": function (data, type, full, meta) { return '' + full.status.name + ''; }
            },
            { "data": "dateCreated", "name": "dateCreated", "title": "Дата создания", "autoWidth": true },
            {
                "data": null, /*"name": "deadline", */"title": "Дедлайн", "autoWidth": true,
                "render": function (data, type, full, meta) { return '' + full.deadlineText + ''; }
            },
            {
                "data": null, /*"name": "deadline", */"title": "Видео", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    var html = "";
                    
                    if (userType == talentType)
                    {
                        //in future this will be a video player but not an image
                        if (full.video.id > 0)
                        {
                            html += "<video id='imgContainer_" + full.id + "' width='240' height='320' controls><source src='" + full.video.url + "' type='video/mp4'></video>"
                        }
                        //html += '<img id="imgContainer_' + full.id + '" src="' + full.video.url + '" alt="Video" class="img-thumbnail" height="150" width="50" />';
                        html += "<br />";

                        if (full.videoConfirmed == true)
                            html += "<small><div class='alert alert-success'>Видео подтверждено</div></small>";
                        else
                        {
                            if (full.uploadVideoBtnIsAvailable == true)
                            {
                                if (full.video.id > 0)
                                {
                                    html += "<button type='button' class='btn btn-danger btn-sm' id='deleteBtn_" + full.id + "' onclick='DeleteVideo(" + full.video.id + ", " + full.id + ");' title='Удалить Видео' ><i class='glyphicon glyphicon-remove-sign'></i></button>";;
                                    html += "<small><div class='alert alert-warning'>Видео еще не подтверждено. Для завершения запроса, пожалуйста, подтвердите</div></small>";
                                    //html += "<br />";
                                    //if (full.videoConfirmed == false)
                                    //{
                                        html += "<button type='button' class='btn btn-success btn-sm' onclick='ConfirmVideo(" + full.id + ");' >Подтвердить</button>";
                                    //}    
                                }

                                //in future jquery-file-upload.js will be used
                                //html += "<div id='fileuploader_" + full.id + "' class='fileuploader'>Upload</div>";
                                html += '<img id="spinner_' + full.id + '" src="/Content/Images/spinner.gif" style="display: none; height:20px;" />';
                                html += "<input type='file' id='imgInp_" + full.id + "' onchange = 'VideoAjaxUpload(this, " + full.id + ", \"" + videoRequestVideoFileType + "\", \"spinner_" + full.id + "\", \"imgContainer_" + full.id + "\");' />";
                            }
                            else
                                html += "<small><i>Текущий статус запроса не позволяет загрузить видео</i></small>";
                        }
                    }
                    else
                    {
                        if (full.videoPaid == true)
                            html += "<a href='/Video/Details/" + full.id + "'>Перейти к видео</a>";
                        else if (full.videoConfirmed == true)
                        {
                            html += "<div class='alert alert-success'>Видео готово!</div>";
                            html += "<button type='button' class='btn btn-success' onclick='MakePayment(" + full.id + ");'>Оплатить!</button>";
                        }
                        else
                            html += "<small><i>Видео не готово</i></small>";
                    }

                    return html;
                }
            },
            {
                "data": null, "title": "Действия", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    var html = "";

                    if (full.cancelBtnIsAvailable == true)
                    {
                        html += "<button class='btn btn-danger btn-sm' onclick='CancelRequest(" + full.id + ");' >Отменить</button>";
                    }

                    if (full.acceptBtnIsAvailable == true) {
                        html += "<button class='btn btn-success btn-sm' onclick='AcceptRequest(" + full.id + ");' >Принять</button>";
                    }

                    return html;
                }
            }
        ],
        fnDrawCallback: function (oSettings) {
            //alert("fff");
            //console.log(oSettings.json.recordsFiltered);//do whatever with your custom response
            //$("#recordsFiltered").text(oSettings.json.recordsFiltered);

            $('[data-toggle="tooltip"]').tooltip();

        }
    });
}

function VideoAjaxUpload(input, modelID, fileType, spinnerTagID, videoPreviewContainerTagID) {
    if (input.files && input.files[0]) {
        var files = input.files;
        var formData = new FormData();

        for (var i = 0; i != files.length; i++) {
            formData.append("files", files[i]);
        }
        formData.append("id", "" + modelID);
        formData.append("fileType", fileType);

        $.ajax({
            url: "/Attachment/Upload",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            beforeSend: function () {
                //alert("beforeSend");
                $("#" + spinnerTagID).show();
                $(input).attr("disabled", "disabled");
            },
            success: function (data) {
                //alert("success");
                if (videoPreviewContainerTagID != undefined
                    || videoPreviewContainerTagID != null
                    || videoPreviewContainerTagID != "")
                {

                    $("#" + videoPreviewContainerTagID).attr('src', data.url);
                }
                else {
                    alert("File uploaded!");
                }
            },
            error: function (data) {
                //alert("error");
            },
            complete: function () {
                //alert("complete");
                $("#" + spinnerTagID).hide();
                $(input).removeAttr("disabled");
            }
        });
    }
}

function CancelRequest(requestID)
{
    $(".btn").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/VideoRequest/Cancel?id=" + requestID,
        //data: {
        //    id: requestID
        //},
        //data: JSON.stringify({
        //    id: requestID
        //}),
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert("success");
            //alert(data);
        },
        error: function (data) {
            //alert("error");
            console.log(data);
        },
        complete: function (data) {
            //alert("completed");
            $(".btn").removeAttr("disabled");
        }
    });
}

function AcceptRequest(requestID) {
    $(".btn").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/VideoRequest/Accept?id=" + requestID,
        //data: {
        //    id: requestID
        //},
        //data: JSON.stringify({
        //    id: requestID
        //}),
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert("success");
            //alert(data);
        },
        error: function (data) {
            //alert("error");
            console.log(data);
        },
        complete: function (data) {
            //alert("completed");
            $(".btn").removeAttr("disabled");
        }
    });
}

function DeleteVideo(videoID, requestID) {
    FileAjaxDelete(videoID, requestID, videoRequestVideoFileType, "spinner_" + requestID, "imgContainer_" + requestID, "deleteBtn_" + requestID);
}

function ConfirmVideo(requestID) {
    $(".btn").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/VideoRequest/ConfirmVideo?id=" + requestID,
        //data: {
        //    id: requestID
        //},
        //data: JSON.stringify({
        //    id: requestID
        //}),
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert("success");
            //alert(data);
        },
        error: function (data) {
            //alert("error");
            console.log(data);
        },
        complete: function (data) {
            //alert("completed");
            $(".btn").removeAttr("disabled");
        }
    });
}

function MakePayment(requestID) {
    $(".btn").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/VideoRequest/MakePayment?id=" + requestID,
        //data: {
        //    id: requestID
        //},
        //data: JSON.stringify({
        //    id: requestID
        //}),
        contentType: "application/json; charset=utf-8",
        //dataType: "json",
        success: function (data) {
            //alert("success");
            //alert(data);
        },
        error: function (data) {
            //alert("error");
            console.log(data);
        },
        complete: function (data) {
            //alert("completed");
            $(".btn").removeAttr("disabled");
        }
    });
}