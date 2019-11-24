function FileAjaxUpload(input, modelID, fileType, spinnerTagID, imgPreviewContainerTagID) {
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
                //$("#imgInp").attr("disabled", "disabled");
                $(input).attr("disabled", "disabled");
            },
            success: function (data) {
                if (imgPreviewContainerTagID != undefined
                    || imgPreviewContainerTagID != null
                    || imgPreviewContainerTagID != "") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("#" + imgPreviewContainerTagID).attr('src', e.target.result);
                    }
                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    alert("File uploaded!");
                }
            },
            complete: function () {
                //alert("completed");
                $("#" + spinnerTagID).hide();
                //$("#imgInp").removeAttr("disabled");
                $(input).removeAttr("disabled");
            }
        });
    }
}

function FileAjaxDelete(fileID, modelID, fileType, spinnerTagID, imgPreviewContainerTagID, deleteBtnID) {
    if (fileID != undefined)
    {
        var formData = new FormData();
        formData.append("fileID", "" + fileID);
        formData.append("objID", "" + modelID);
        formData.append("fileType", fileType);

        $.ajax({
            url: "/Attachment/Delete",
            data: formData,
            processData: false,
            contentType: false,
            type: "POST",
            beforeSend: function () {
                //alert("beforeSend");
                $("#" + spinnerTagID).show();
                //$("#imgInp").attr("disabled", "disabled");
                $("#" + deleteBtnID).attr("disabled", "disabled");
            },
            success: function (data) {
                if (imgPreviewContainerTagID != undefined
                    || imgPreviewContainerTagID != null
                    || imgPreviewContainerTagID != "")

                {
                    $("#" + imgPreviewContainerTagID).attr('src', '/Content/Images/nophoto.png');
                }
                else {
                    alert("File Deleted!");
                }
            },
            complete: function () {
                //alert("completed");
                $("#" + spinnerTagID).hide();
                //$("#imgInp").removeAttr("disabled");
                $("#" + deleteBtnID).removeAttr("disabled");
            }
        });
    }
}