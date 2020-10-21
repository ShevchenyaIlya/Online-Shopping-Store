﻿var image_field = $('.profilePicture')[0]

$('.image-uploader').on('change', function (e) {
    var selectedFile = e.target.files[0];
    var reader = new FileReader();
    image_field.title = selectedFile.name;
    reader.onload = function (event) {
        image_field.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);
})
