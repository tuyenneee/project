"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/foodStoreHub")
    .build();

connection.on("LoadCategory", function () {
    location.href = '/Category/Index'
    //location.reload();
});

connection.on("LoadProduct", function () {
    location.href = '/Product/Index'
    //location.reload();
});

connection.on("LoadAccount", function () {
    location.href = '/Account/Index'
    //location.reload();
});

connection.on("LoadSupplier", function () {
    location.href = '/Supplier/Index'
    //location.reload();
});

connection.on("LoadIndex", function () {
    location.href = '/Index'
    //location.reload();
});


connection.start().catch(function (err) {
    return console.error(err.toString());
});