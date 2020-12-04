"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/commenthub").build();

//Disable send button until connection is established
var postId = document.getElementById("productName").value
document.getElementById("sendButton1").disabled = true;

connection.on("ReceiveMessage", function (title, body, postDate, image, username, productName) {
    var li = document.createElement("li");
    var m = new Date();
    var dateString = m.getUTCFullYear() + "-" + (m.getUTCMonth() + 1) + "-" + m.getUTCDate() + " " + m.getHours() + ":" + m.getUTCMinutes() + ":" + m.getUTCSeconds();
    li.className = "list-group-item";
    li.style.marginBottom = "6px";

    var text = "<div class=\"media\"><div></div><div class=\"media-body\"><div class=\"media\" style=\"overflow:visible;\">" +
        "<div><img class=\"mr-3\" style=\"width: 40px; height:40px;\" src=" + image + ">" + 
        "</div><div class=\"media-body\" style=\"overflow:visible;\"><div class=\"row\"><div class=\"col-md-12\"><p><a href=\"#\">" + username + ":</a>" + title + "<br>" +
        body + "<br/><small class=\"text-muted\">" + dateString + "</small></p></div></div></div></div></div>";
    li.innerHTML = text;
    document.getElementById("userComments1").prepend(li);
});

connection.start().then(function () {
    document.getElementById("sendButton1").disabled = false;
    connection.invoke("JoinPostGroup", postId);
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton1").addEventListener("click", function (event) {
    var title = document.getElementById("commentTitle").value;
    var body = document.getElementById("commentBody").value;
    var image = document.getElementById("image").value;
    var username = document.getElementById("username").value;
    var productName = document.getElementById("productName").value;
    connection.invoke("SendMessage", title, body, image, username, productName).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});