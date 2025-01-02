var PageSize = 20;

/* 
* jQuery Ajax调用封装
* url:			调用地址
* data:			可选参数,表示Ajax调用参数
* onSuccess:	可选参数,成功回调函数,函数签名为  function(data), data参数为调用结果
* onError:		可选参数,失败回调函数,函数签名为  function (XMLHttpRequest, textStatus, errorThrown)
* dataType:		可选参数,Ajax返回数据类型,默认为 "json"
* cache:		可选参数,Ajax调用是否启用缓存,默认为 false
* onComplete:	可选参数,Ajax调用完成回调函数,函数签名为  function (XMLHttpRequest, textStatus)
* async:		可选参数,是否异步调用，默认为true
*/
function AjaxCall(url, data, onSuccess, onError, onComplete, dataType, cache, async) {

    for (var key in data) {
        if (typeof (data[key]) == "object" && typeof (data[key]) != null)
            data[key] = JSON.stringify(data[key]);
    }

    return $.ajax({
        url: url,
        data: data,
        type: "POST",
        dataType: dataType ? dataType : "json",
        cache: cache ? cache : false,
        success: onSuccess,
        async: (async == null) ? true : async,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (onError != null && onError(XMLHttpRequest, textStatus, errorThrown) == false) { return; }
            var error = JSON.parse(XMLHttpRequest.responseText);
            //统一的错误信息处理
            alert(error.ErrorMessage);
            //ErrorInfo(XMLHttpRequest, textStatus, errorThrown);
        },
        complete: onComplete
    });
}

/*数据绑定方法 Jbind
*template:绑定的模板
*container:要绑定数据的容器
*data:要绑定的数据
*/
function JBindData(template, container, data) {
    var temp = $(template).clone();
    temp.removeAttr("id"); //去除id属性，避免key="id"的情况下发生冲突
    temp[0].style.display = '';
    temp = $($('<p></p>').html(temp)).html();
    temp = '<!--data-->' + temp + '<!--data-->';

    return $(temp).bindTo(data, { appendTo: container });
}

//Json 日期转换 begin
//value:要转换的时间
//formatStr:时间格式
function formatDatebox(value, formatStr) {
    if (formatStr == "") {
        formatStr = "yyyy-MM-dd";
    }
    if (value == null || value == '' || value == "/Date(-62135596800000)/") {
        return '';
    }
    var dt;
    if (value instanceof Date) {
        dt = value;
    }
    else {
        dt = new Date(value);
        if (isNaN(dt)) {
            value = value.replace(/\/Date\((-?\d+)\)\//, '$1');
            dt = new Date();
            dt.setTime(value);
        }
    }

    return dt.format(formatStr);
}
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month 
        "d+": this.getDate(),    //day 
        "h+": this.getHours(),   //hour 
        "m+": this.getMinutes(), //minute 
        "s+": this.getSeconds(), //second 
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter 
        "S": this.getMilliseconds() //millisecond 
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
                (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
            RegExp.$1.length == 1 ? o[k] :
                ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}
//Json 日期转换 end

//jquery扩展 json填充表单  $("#form").populateForm(jsonData);
$.fn.populateForm = function (data) {
    return this.each(function () {
        var formElem, name;
        if (data == null) { this.reset(); return; }
        for (var i = 0; i < this.length; i++) {
            formElem = this.elements[i];
            //checkbox的name可能是name[]数组形式
            name = (formElem.type == "checkbox") ? formElem.name.replace(/(.+)\[\]$/, "$1") : formElem.name;
            if (data[name] == undefined) continue;
            switch (formElem.type) {
                case "checkbox":
                    if (data[name] == "") {
                        formElem.checked = false;
                    } else {
                        //数组查找元素
                        if (data[name].indexOf(formElem.value) > -1) {
                            formElem.checked = true;
                        } else {
                            formElem.checked = false;
                        }
                    }
                    break;
                case "radio":
                    if (data[name] == "") {
                        formElem.checked = false;
                    } else if (formElem.value == data[name]) {
                        formElem.checked = true;
                    }
                    break;
                case "button": break;
                default: formElem.value = data[name];
            }
        }
    });
};