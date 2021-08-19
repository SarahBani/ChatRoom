'use strict';

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/notification").build();

connection.on('SendAll', ({ userId, header, content }) => {
    //var header = document.createElement('h3');
    //header.textContent = header + ': ';
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

connection.start().catch(function (err) {
    console.error('errorrrrrrrr');
    console.error(err.toString());
});

function notifyAll(data, callback) {
    connection.invoke("NotifyAll", data).then(function () {
        if (typeof callback == 'function') {
            callback();
        }
    })
}