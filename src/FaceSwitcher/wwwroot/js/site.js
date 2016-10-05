var ImageViewModel = function () {
    var self = this;

    self.isSwitchEnable = ko.observable(false);
    self.isLoadingInProgress = ko.observable(false);

    self.create = function (data, e) {
        self.isLoadingInProgress(true);

        var file = e.target.files[0];

        var reader = new FileReader();
        reader.onload = function () {
            setImageSource(reader.result);

            var formData = new FormData();
            formData.append("file", file);

            $.ajax({
                url: "api/images",
                type: "POST",
                processData: false,
                contentType: false,
                data: formData,
                async: true,
                success: function (response) {
                    self.id = response.id;
                    self.isSwitchEnable(true);
                    self.isLoadingInProgress(false);
                },
                error: function (errorMessage) {
                    console.log(errorMessage);
                }
            });
        };

        reader.readAsDataURL(file);
    };

    self.get = function () {
        $.ajax({
            url: "api/images/" + self.id,
            type: "GET",
            async: true,
            success: function (response) {
                setImageSource(response.url);
                self.isSwitchEnable(false);
            },
            error: function (errorMessage) {
                console.log(errorMessage);
            }
        });
    }
}

$(function () {
    var viewModel = new ImageViewModel();
    ko.applyBindings(viewModel);

    var dropElement = document.querySelector(".droppable");
    makeDroppable(dropElement, viewModel.create);
});

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
