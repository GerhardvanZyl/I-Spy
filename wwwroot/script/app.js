(function () {
    $("#upload").on("change", () => {
        let formData = new FormData();
        let file = $("#upload")[0].files[0];

        if (!file) return;

        $(".lds-circle")[0].style.display = "inline-block";
        $("#image-container").hide();
        $("#description").html("...");


        formData.append("files", file);

        $.ajax({
            url: "api/image",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false
        })
            .done((resultRaw) => {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#uploaded-image").attr("src", e.target.result);
                }

                reader.readAsDataURL(file);
                
                console.log("result: ", resultRaw);
                let result = JSON.parse(resultRaw);
                let tags = "";

                if (result.message) {
                    $("#description").html(result.message + " - Try a different image");
                } else {
                    let description = result.description.captions[0].text;
                    // let firstLetter = description.substr(0,1).toLocaleUpperCase();
                    // let sentenceDescription = firstLetter + description.substr(1);
                    $("#description").html("I see " + description + ".");

                    for (tag of result.description.tags){
                        tags += `<span>#${tag}</span> `;
                    }

                }

                $(".lds-circle")[0].style.display = "none";
                $("#image-container").show();
                $("#tags").html(tags);

            });

    });

})();