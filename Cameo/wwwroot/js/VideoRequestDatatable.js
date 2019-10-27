﻿function initVideoRequestsDatatable() {
    //console.log(VideoRequestStatusEnum);

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
                "data": null, "title": "Действия", "autoWidth": true,
                "render": function (data, type, full, meta) {
                    var html = "";

                    if (full.status.id == VideoRequestStatusEnum.waitingForResponse)
                        html += "<a href='#' class='btn btn-danger btn-sm' onclick='CancelRequest(" + full.id + ");' >Отменить</a>";

                    return html;
                }
            },
            //{
            //    "render": function (data, type, full, meta) { return '<a class="btn btn-info" href="/Demo/Edit/' + full.CustomerID + '">Edit</a>'; }
            //},
            

        ],
        fnDrawCallback: function (oSettings) {
            //alert("fff");
            //console.log(oSettings.json.recordsFiltered);//do whatever with your custom response
            //$("#recordsFiltered").text(oSettings.json.recordsFiltered);

            $('[data-toggle="tooltip"]').tooltip();
        }
    });
}

function CancelRequest(requestID)
{
    $(".btn").attr("disabled", "disabled");

    $.ajax({
        type: "POST",
        url: "/VideoRequest/CancelRequest?id=" + requestID,
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