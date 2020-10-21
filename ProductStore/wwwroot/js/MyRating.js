
$("#ratingValue1").click(function () {
    var prod = '@Model.ProductName';
    var userIdentificator = '@user.Id';
    var ratingValue = "1";
    $.ajax({
        type: "get",
        url: "/Home/OnUpdateRating",
        success: function (responce) {
            $("#ratinglist").html(responce);
        },
        data: { productName: prod, userId: userIdentificator, value: ratingValue }
    });
});
