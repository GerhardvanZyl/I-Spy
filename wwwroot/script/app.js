(function () {
    $("#upload").on("change", () => {
        $("#status-container").html("Uploading");
        $("#description").html("");

        let formData = new FormData();
        let file = $("#upload")[0].files[0];
        formData.append("files", file);

        $.ajax({
            url: "api/image",
            type: "POST",
            data: formData,
            //dataType: "json",
            processData: false,
            contentType: false
        })
            .done((resultRaw) => {
                console.log("result: ", resultRaw);
                let result = JSON.parse(resultRaw);

                $("#status-container").html("");

                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#uploaded-image").attr("src", e.target.result)
                }

                reader.readAsDataURL(file);

                if (result.message) {
                    $("#status-container").html(result.message + " - Try a different image");
                } else {
                    let description = result.description.captions[0].text;
                    $("#description").html(description);
                }
            });

    });

})();