$(function () {
    var dropElement = document.querySelector(".droppable");
    makeDroppable(dropElement, uploadImage);

    var uploadElement = document.getElementById("imageInput");
    makeChanging(uploadElement, uploadImage);
});

$("#switch").click(function () {
    var blob = base64toBlob(localStorage.currentImage);
    var formData = new FormData();

    formData.append("file", blob);

    $.ajax({
        url: "api/images",
        type: "POST",
        processData: false,
        contentType: false,
        data: formData,
        async: true,
        success: function (response) {
            setImageSource(response);
            $("#switch").attr("disabled", true);
        },
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
});

function uploadImage(e) {
    var files = e.dataTransfer
        ? e.dataTransfer.files
        : e.target.files;

    $.each(files, function (index, file) {
        var reader = new FileReader();
        reader.onload = function () {
            setImageSource(reader.result);
            localStorage.currentImage = reader.result;
        };

        reader.readAsDataURL(file);
    });

    $("#switch").attr("disabled", false);
}

function makeDroppable(element, callback) {
    var input = document.createElement("input");
    input.setAttribute("type", "file");
    input.style.display = "none";

    input.addEventListener("change", callback);
    element.appendChild(input);

    element.addEventListener("dragover", function (e) {
        e.preventDefault();
        e.stopPropagation();
        element.classList.add("dragover");
    });

    element.addEventListener("dragleave", function (e) {
        e.preventDefault();
        e.stopPropagation();
        element.classList.remove("dragover");
    });

    element.addEventListener("drop", function (e) {
        e.preventDefault();
        e.stopPropagation();
        element.classList.remove("dragover");
        callback(e);
    });

    element.addEventListener("click", function () {
        input.value = null;
        input.click();
    });
}

function makeChanging(element, callback) {
    element.addEventListener("change", callback);
}

function setImageSource(source) {
    var img = new Image();
    img.src = source;

    $("#uploadedImage").attr("src", source);
    $("#imageDrop").hide();
}

function base64toBlob(data) {
    var byteString = atob(data.split(",")[1]);
    var contentType = data.substring(data.lastIndexOf(":") + 1, data.lastIndexOf(";"));

    var arrayBuffer = new ArrayBuffer(byteString.length);
    var intArray = new Uint8Array(arrayBuffer);

    for (var i = 0; i < byteString.length; i++) {
        intArray[i] = byteString.charCodeAt(i);
    }

    return new Blob([arrayBuffer], { type: contentType });
}
