var ImageViewModel = function () {
    var self = this;

    self.isSwitchEnable = ko.observable(false);
    self.isLoadingInProgress = ko.observable(false);

    var dropElement = document.querySelector(".droppable");

    self.dragover = function () {
        dropElement.classList.add("dragover");
    }

    self.dragleave = function () {
        dropElement.classList.remove("dragover");
    }

    self.drop = function (data, e) {
        dropElement.classList.remove("dragover");
        self.create(data, e.originalEvent);
    }

    self.inputClick = function () {
        $("#imageInput").click();
    }

    self.uploadImage = function (data, e) {
        self.isLoadingInProgress(true);

        var files = e.dataTransfer
            ? e.dataTransfer.files
            : e.target.files;

        var file = files[0];
        var reader = new FileReader();
        reader.onload = function () {
            setImageSource(reader.result);

            var formData = new FormData();
            formData.append("file", file);

            postImage(formData, function (response) {
                self.id = response.id;
                self.isSwitchEnable(true);
                self.isLoadingInProgress(false);
            });
        };

        reader.readAsDataURL(file);
    };

    self.performSwitch = function () {
        getUrl(self.id,
            function (response) {
                setImageSource(response.url);
                self.isSwitchEnable(false);
            });
    };
}

$(function () {
    var viewModel = new ImageViewModel();
    ko.applyBindings(viewModel);
});

function setImageSource(source) {
    var img = new Image();
    img.src = source;

    $("#uploadedImage").attr("src", source);
    $("#imageDrop").hide();
}

function getUrl(id, successHandler) {
    $.ajax({
        url: "api/images/" + id,
        type: "GET",
        async: true,
        success: successHandler,
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
}

function postImage(data, successHandler) {
    $.ajax({
        url: "api/images",
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        async: true,
        success: successHandler,
        error: function (errorMessage) {
            console.log(errorMessage);
        }
    });
}
