// Toggle 'hide' class on click

var chat_box = document.getElementById("chatBtn");
var chat = document.getElementById("chat");

chat_box.onclick = function () {
    chat.classList.toggle("chat-hide");
}

var chat_header = document.getElementById("chatHeader");

chat_header.onclick = function () {
    chat.classList.toggle("chat-hide");
}