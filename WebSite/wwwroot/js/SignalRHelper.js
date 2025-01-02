"use strict";



const connection = new signalR.HubConnectionBuilder()
    .withUrl("/rebateHub")  // 替换为你的 SignalR Hub 地址
    .withAutomaticReconnect()  // 自动重连
    .build();


//connection.on("ReceiveMessage", (user, message) => {
//    console.log(`Received message from server: ${user}-${message}`);



//});

connection.on("Layout", (message) => {


    new Vue().$notify({
        title: '全局通知',
        message: message,
        type: 'info'  // 可选的通知类型，例如 success、warning、info、error
    });



});


connection.start()
    .then(() => {
        console.log('signalR连接成功');
        // 可以在这里执行其他初始化操作
    })
    .catch(err => console.error('连接错误：', err));


//connection.onclose(() => {
//    console.log('SignalR 连接断开.');
//});

//connection.onreconnecting(() => {
//    console.log('SignalR 正在重连');
//});

//connection.onreconnected(() => {
//    console.log('SignalR 重连成功');
//});
