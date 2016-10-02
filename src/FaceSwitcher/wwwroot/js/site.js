$(function () {
    $(":file").change(function () {
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function () {
                setImageSource(reader.result);
            };
            reader.readAsDataURL(this.files[0]);
        }
    });
});

$("#process").click(function () {
    var blobFile = $(":file")[0].files[0];
    var formData = new FormData();
    formData.append("file", blobFile);

    $.ajax({
        url: "api/images",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        async: true,
        success: function (response) {
            setImageSource(response);
        },
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
});

function setImageSource(source) {
    var img = new Image;
    img.onload = function () {
        resizeImage(img);
    };

    img.src = source;
    $("#uploadedImage").attr("src", source);
}

function resizeImage(img) {
    var newSize = scaleSize(500, 500, img.width, img.height);
    $("#uploadedImage").attr("width", newSize[0]);
    $("#uploadedImage").attr("height", newSize[1]);
}

function scaleSize(maxWidth, maxHeight, currWidth, currHeight) {
    var ratio = currHeight / currWidth;
    if (currWidth >= maxWidth && ratio <= 1) {
        currWidth = maxWidth;
        currHeight = currWidth * ratio;
    } else if (currHeight >= maxHeight) {
        currHeight = maxHeight;
        currWidth = currHeight / ratio;
    }
    return [currWidth, currHeight];
}
