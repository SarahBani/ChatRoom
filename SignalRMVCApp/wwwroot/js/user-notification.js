'use strict';

var connection = new signalR.HubConnectionBuilder().withUrl(`/hubs/user-notification?userId=` + userId).build();

connection.on('SendToUser', ({ userId, header, content }) => {
    var b = document.createElement('b');
    b.innerText = userId + ': ';
    var i = document.createElement('i');
    i.innerText = '(' + header + ') ';
    var span = document.createElement('span');
    span.innerText = content;
    var hr = document.createElement('hr');

    var p = document.createElement('p');
    p.appendChild(b);
    p.appendChild(i);
    p.appendChild(span);
    p.appendChild(hr);

    document.getElementById('chat').appendChild(p);
});

connection.start().then(function () {
    connection.invoke("GetConnectionId").then(function (connectionId) {
        document.getElementById("signalRConnectionId").innerHTML = connectionId;
    })
})
    .catch(function (err) {
        console.error('errorrrrrrrr');
        console.error(err.toString());
    });