function StartLoadingImagesAsync() {
    //var allLoaded = false;
    var interval;
    var attemptsLimit = 100;
    var attemptsCount = 0;
    interval = setInterval(function () {
        attemptsCount++;
        LoadImagesAsync();
        if (!$("img").is(".async"))
            clearInterval(interval);
        if (attemptsLimit <= attemptsCount) {
            ShowImageErrorBanner();
            clearInterval(interval);
        }
    }, 3000);
}

function LoadImagesAsync() {
    $("img.async").each(function (i, elem) {
        if ($(elem).attr("finalSrc") != "") {
            var downloadingImage = new Image();
            downloadingImage.onload = function () {
                $(elem).attr("src", this.src);
                $(elem).removeClass("async");
                $(elem).removeAttr("finalSrc");
            };
            //downloadingImage.onerror = function () {
            //    alert("Error loading: " + $(elem).attr("finalSrc"));
            //    StartReloadingImage();
            //};
            downloadingImage.src = $(elem).attr("finalSrc");
        }
    });

    //$(".name").each(function () {
    //    if ($(this).height() > 25) {
    //        $(this).addClass('big_name');
    //    }
    //});
}

function ShowImageErrorBanner() {
    $("img.async").each(function (i, elem) {
        var downloadingImage = new Image();
        downloadingImage.onload = function () {
            $(elem).attr("src", this.src);
            $(elem).removeClass("async");
        };
        downloadingImage.src = $(elem).attr("@XCarsConfiguration.BucketEndpoint" + " " + "@XCarsConfiguration.BucketName" + "/" + "@XCarsConfiguration.AutoPhotosUploadUrl" + "@XCarsConfiguration.AutoNoPhotoName" + "@XCarsConfiguration.PhotoExtension");
    });
}